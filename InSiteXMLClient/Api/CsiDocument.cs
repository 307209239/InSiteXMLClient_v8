using Camstar.Util;
using Camstar.XMLClient.Interface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace Camstar.XMLClient.API
{
    internal class CsiDocument : ICsiDocument
    {
        protected XmlDocument mRequestDocument;
        protected ICsiDocument mResponseDocument;
        protected ICsiSession mSession;
        protected ICsiXmlElement mRootElement;
        protected List<ICsiExceptionData> mExceptions = new List<ICsiExceptionData>();
        private const string mkXMLVersion = "1.1";

        public CsiDocument(ICsiSession session)
        {
            this.mSession = session;
            this.mRequestDocument = new XmlDocument();
            this.mResponseDocument = null;
            this.mRootElement = (ICsiXmlElement)new CsiXmlElement(this, this.mRequestDocument.CreateElement("__InSite"));
            this.mRequestDocument.AppendChild((XmlNode)(this.mRootElement as CsiXmlElement).GetDomElement());
            this.mRootElement.SetAttribute("__version", "1.1");
            this.mRootElement.SetAttribute("__encryption", "2");
        }

        public CsiDocument(ICsiSession session, string xml)
        {
            this.mSession = session;
            this.BuildFromString(xml);
            this.mResponseDocument = null;
        }

        public ICsiDocument Submit()
        {
            this.mResponseDocument = null;
            try
            {
                this.mResponseDocument = (this.mSession as CsiSession).Submit(this);
            }
            catch (Exception ex)
            {
                throw new CsiClientException(-1L, ex, this.GetType().FullName + ".submit()");
            }
            return this.mResponseDocument;
        }

        public void BuildFromString(string xml)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.PreserveWhitespace = false;
                doc.LoadXml(xml);
                this.SetDocument(doc);
            }
            catch (XmlException ex)
            {
                throw new CsiClientException(-1L, (Exception)ex, this.GetType().FullName + ".buildFromString()");
            }
        }

        public string AsXml()
        {
            StringBuilder sb = new StringBuilder();
            this.mRequestDocument.Save((XmlWriter)new XmlTextWriter((TextWriter)new StringWriter(sb)));
            return sb.ToString();
        }

        public ICsiService CreateService(string serviceType) => !StringUtil.IsEmptyString(serviceType) ? (ICsiService)new CsiService(this, serviceType, this.mRootElement) : throw new CsiClientException(-1L, "Can not create Service with an empty name", this.GetType().FullName + ".createService()");

        public virtual bool GetAlwaysReturnSelectionValues()
        {
            string name1 = "__session.__connect.__processingDefaults.__alwaysGetSelectionValues";
            ICsiXmlElement rootElement = this.GetRootElement();
            ICsiXmlElement childByName = rootElement.FindChildByName(name1);
            if (childByName == null)
            {
                string name2 = "__session.__useSession.__processingDefaults.__alwaysGetSelectionValues";
                childByName = rootElement.FindChildByName(name2);
            }
            return childByName != null;
        }

        public virtual void SetAlwaysReturnSelectionValues(bool val)
        {
            if (val == this.GetAlwaysReturnSelectionValues())
                return;
            ICsiXmlElement childByName1 = this.GetRootElement().FindChildByName("__session");
            if (childByName1 == null)
                throw new CsiClientException(-1L, "__session does not exist", this.GetType().FullName + ".setAlwaysReturnSelectionValues()");
            ICsiXmlElement parentElement1 = childByName1.FindChildByName("__connect") ?? childByName1.FindChildByName("__useSession");
            if (parentElement1 == null)
                throw new CsiClientException(-1L, "__connect does not exist", this.GetType().FullName + ".setAlwaysReturnSelectionValues()");
            ICsiXmlElement parentElement2 = parentElement1.FindChildByName("__processingDefaults") ?? (ICsiXmlElement)new CsiXmlElement(this, "__processingDefaults", parentElement1);
            ICsiXmlElement childByName2 = parentElement2.FindChildByName("__alwaysGetSelectionValues");
            if (childByName2 == null)
            {
                ICsiXmlElement csiXmlElement = (ICsiXmlElement)new CsiXmlElement(this, "__alwaysGetSelectionValues", parentElement2);
            }
            else
                parentElement2.RemoveChild(childByName2);
        }

        public virtual ICsiService GetService()
        {
            ICsiService csiService = null;
            IEnumerator enumerator = this.GetServices().GetEnumerator();
            if (enumerator.MoveNext())
                csiService = enumerator.Current as ICsiService;
            return csiService;
        }

        public virtual ICsiService[] GetServices()
        {
            XmlNodeList elementsByTagName = this.mRequestDocument.GetElementsByTagName("__service");
            ICsiService[] csiServiceArray = new CsiService[elementsByTagName.Count];
            for (int i = 0; i < elementsByTagName.Count; ++i)
            {
                XmlNode xmlNode = elementsByTagName[i];
                csiServiceArray[i] = new CsiService(this, xmlNode as XmlElement);
            }
            return csiServiceArray;
        }

        public bool CheckErrors()
        {
            if (this.mExceptions.Count == 0)
            {
                foreach (XmlNode xmlNode in this.mRequestDocument.GetElementsByTagName("__exceptionData"))
                    this.mExceptions.Add(new CsiExceptionData(this, xmlNode as XmlElement));
            }
            return this.mExceptions.Count > 0;
        }

        public virtual ICsiExceptionData[] GetExceptions() => this.mExceptions.Count == 0 && !this.CheckErrors() ? null : this.mExceptions.ToArray();

        public ICsiResponseData ResponseData() => this.GetRootElement().FindChildByName("__responseData") as ICsiResponseData;

        public ICsiExceptionData ExceptionData() => this.mExceptions.Count == 0 && !this.CheckErrors() ? (ICsiExceptionData)null : this.mExceptions[0] as ICsiExceptionData;

        public ICsiRequestData RequestData()
        {
            if (!(this.GetRootElement().FindChildByName("__requestData") is CsiRequestData csiRequestDataImpl))
                csiRequestDataImpl = new CsiRequestData(this, this.GetRootElement());
            return (ICsiRequestData)csiRequestDataImpl;
        }

        public ICsiQuery CreateQuery() => new CsiQuery(this, this.mRootElement);

        public virtual ICsiQuery GetQuery()
        {
            ICsiQuery csiQuery = null;
            XmlNodeList elementsByTagName = this.mRequestDocument.GetElementsByTagName("__query");
            if (elementsByTagName.Count > 0)
                csiQuery = new CsiQuery(this, elementsByTagName[0] as XmlElement);
            return csiQuery;
        }

        public virtual ICsiQuery[] GetQueries()
        {
            XmlNodeList elementsByTagName = this.mRequestDocument.GetElementsByTagName("__query");
            ICsiQuery[] csiQueryArray = new CsiQuery[elementsByTagName.Count];
            for (int i = 0; i < elementsByTagName.Count; ++i)
            {
                XmlNode xmlNode = elementsByTagName[i];
                csiQueryArray[i] = new CsiQuery(this, xmlNode as XmlElement);
            }
            return csiQueryArray;
        }

        public string SaveRequestData(string filename, bool append)
        {
            FileMode mode = append ? FileMode.Append : FileMode.Create;
            FileStream fileStream = new FileStream(filename, mode);
            XmlTextWriter xmlTextWriter = new XmlTextWriter((Stream)fileStream, (Encoding)null);
            xmlTextWriter.Formatting = Formatting.Indented;
            this.mRequestDocument.Save((XmlWriter)xmlTextWriter);
            xmlTextWriter.Close();
            fileStream.Close();
            return this.AsXml();
        }

        public string SaveResponseData(string filename, bool append)
        {
            if (this.mResponseDocument != null)
                this.mResponseDocument.SaveRequestData(filename, append);
            return this.mResponseDocument.AsXml();
        }

        public string GetTxnGuid()
        {
            string str = string.Empty;
            ICsiService service = this.GetService();
            if (service != null)
            {
                IEnumerator enumerator = service.GetChildrenByName("__txnGUID").GetEnumerator();
                if (enumerator.MoveNext())
                    str = (enumerator.Current as CsiXmlElement).GetElementValue();
            }
            return str;
        }

        protected internal XmlElement CreateDomElement(string tagName) => this.mRequestDocument.CreateElement(tagName);

        protected internal virtual ICsiXmlElement GetRootElement() => this.mRootElement;

        private void SetDocument(XmlDocument doc)
        {
            this.mRequestDocument = doc;
            this.mRootElement = (ICsiXmlElement)new CsiXmlElement(this, doc.DocumentElement);
            this.mExceptions.Clear();
        }
    }
}
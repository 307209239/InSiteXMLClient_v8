using Camstar.Util;
using Camstar.XMLClient.API.Utilities;
using Camstar.XMLClient.Interface;
using System;
using System.Collections;

namespace Camstar.XMLClient.API
{
    internal class CsiSession : ICsiSession
    {
        private Hashtable mDocuments;
        private string mPassword;
        private string mSessionID;
        private ICsiConnection mConnection;

        public CsiSession(string name, string password, ICsiConnection connection)
        {
            this.mDocuments = new Hashtable();
            this.UserName = name;
            this.Password = password;
            this.mPassword = password;
            this.mConnection = connection;
        }

        public string UserName { get; private set; }

        public string Password { get; private set; }

        public string Host { get; set; }

        public int Port { get; set; }

        public string SessionId
        {
            get => this.mSessionID;
            set
            {
                if (string.IsNullOrEmpty(value))
                    return;
                this.mSessionID = value;
                if (Port == 80 || Port == 443)
                 this.mPassword =string.Empty;
            }
        }

        public ICsiDocument CreateDocument(string name)
        {
            lock (this)
            {
                if (StringUtil.IsEmptyString(name))
                    throw new CsiClientException(3014678L, this.GetType().FullName + ".createDocument()");
                try
                {
                    if (this.FindDocument(name) != null)
                        throw new CsiClientException(3014677L, this.GetType().FullName + ".createDocument()");
                    ICsiDocument document = new CsiDocument((ICsiSession)this);
                    this.mDocuments[name] = document;
                    ICsiXmlElement parent = (ICsiXmlElement)new CsiXmlElement(document, "__session", (document as CsiDocument).GetRootElement());
                    if (string.IsNullOrEmpty(this.mPassword))
                        this.CreateConnectWithoutPassword(this.UserName, this.mSessionID, parent);
                    else
                        this.CreateConnect(this.UserName, this.mPassword, parent);
                    return document;
                }
                catch (Exception ex)
                {
                    throw new CsiClientException(-1L, ex, this.GetType().FullName + ".createDocument()");
                }
            }
        }

        public ICsiDocument FindDocument(string name) => this.mDocuments[name] as ICsiDocument;

        public void RemoveDocument(string name)
        {
            lock (this)
                this.mDocuments.Remove(name);
        }

        protected internal virtual void ChangeAllDocumentsSessionId(string newKey)
        {
            if (this.mDocuments == null)
                return;
            lock (this)
            {
                foreach (object key in (IEnumerable)this.mDocuments.Keys)
                    this.ChangeDocumentSessionId(this.mDocuments[key] as ICsiDocument, newKey);
            }
        }

        protected virtual void ChangeDocumentSessionId(ICsiDocument document, string newKey)
        {
            if (!(document is CsiDocument))
                return;
            ICsiXmlElement childByName1 = ((CsiDocument)document).GetRootElement().FindChildByName("__session");
            if (childByName1 != null)
            {
                string name = "__useSession.sessionId";
                if (childByName1.FindChildByName(name) is CsiXmlElement childByName2 && childByName2.GetDomElement() != null)
                    CsiXmlHelper.SetTextNode(childByName2.GetDomElement(), newKey);
            }
        }

        private void CreateConnectWithoutPassword(
          string userName,
          string sessionID,
          ICsiXmlElement parent)
        {
            ICsiDocument ownerDocument = parent.GetOwnerDocument();
            ICsiXmlElement parent1 = (ICsiXmlElement)new CsiXmlElement(ownerDocument, "__useSession", parent);
            new CsiNamedObject(ownerDocument, "user", parent1).SetRef(userName);
            ICsiDataField csiDataField = (ICsiDataField)new CsiDataField(ownerDocument, "sessionId", parent1);
            csiDataField.SetAttribute("__encrypted", "no");
            csiDataField.SetValue(sessionID);
        }

        private void CheckSessionId(ICsiDocument document, ref string xmlRequest)
        {
            if (string.IsNullOrEmpty(this.SessionId))
                return;
            string str = CsiSessionManager.CheckSessionKey(this.Host, this.Port, this.UserName, this.Password);
            if (!string.IsNullOrEmpty(str))
            {
                string sessionId = this.SessionId;
                if (this.mConnection is CsiConnection mConnection)
                    mConnection.ChangeExpiredSessionId(sessionId, str);
                this.ChangeDocumentSessionId(document, str);
                xmlRequest = document.AsXml();
            }
        }

        private void CreateConnect(string userName, string password, ICsiXmlElement parent)
        {
            ICsiDocument ownerDocument = parent.GetOwnerDocument();
            ICsiXmlElement parent1 = (ICsiXmlElement)new CsiXmlElement(ownerDocument, "__connect", parent);
            new CsiNamedObject(ownerDocument, "user", parent1).SetRef(userName);
            new CsiDataField(ownerDocument, nameof(password), parent1).SetEncryptedValue(password);
        }

        internal ICsiDocument Submit(ICsiDocument document)
        {
            string xmlRequest = document.AsXml();
            string empty = string.Empty;
            string xml;
            try
            {
                this.CheckSessionId(document, ref xmlRequest);
                xml = this.mConnection.Submit(xmlRequest);
            }
            catch (Exception ex)
            {
                throw new CsiClientException(-1L, ex, this.GetType().FullName + ".submit()");
            }
            return new CsiDocument((ICsiSession)this, xml);
        }
    }
}
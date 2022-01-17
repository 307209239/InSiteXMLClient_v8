using Camstar.XMLClient.Interface;
using System.Xml;

namespace Camstar.XMLClient.API
{
    internal class CsiExceptionData : CsiXmlElement, ICsiExceptionData, ICsiXmlElement
    {
        public CsiExceptionData(ICsiDocument doc, XmlElement domElement)
          : base(doc, domElement)
        {
        }

        public CsiExceptionData(ICsiDocument doc, string name, ICsiXmlElement parent)
          : base(doc, name, parent)
        {
        }

        public virtual int GetErrorCode() => int.Parse(this.GetDomElement().GetElementsByTagName("__errorCode")[0].FirstChild.Value);

        public virtual int GetSeverity() => int.Parse(this.GetDomElement().GetElementsByTagName("__severity")[0].FirstChild.Value);

        public virtual string GetDescription() => this.GetNodeValue("__errorDescription");

        public virtual string GetSource() => this.GetNodeValue("__errorSource");

        public virtual string GetSystemMessage() => this.GetNodeValue("__errorSystemMessage");

        public virtual string GetFailureContext() => this.GetNodeValue("__failureContext");

        public virtual XmlNodeList GetExceptionParameters()
        {
            XmlNodeList elementsByTagName = this.GetDomElement().GetElementsByTagName("__exceptionParameters");
            return elementsByTagName.Count <= 0 ? elementsByTagName : elementsByTagName[0].ChildNodes;
        }

        public virtual string GetExceptionParameter(string tagName)
        {
            string empty = string.Empty;
            foreach (XmlNode exceptionParameter in this.GetExceptionParameters())
            {
                if (exceptionParameter.Name == tagName)
                {
                    empty = exceptionParameter.FirstChild.Value;
                    break;
                }
            }
            return empty;
        }

        private string GetNodeValue(string tagName)
        {
            string empty = string.Empty;
            XmlNodeList elementsByTagName = this.GetDomElement().GetElementsByTagName(tagName);
            if (elementsByTagName.Count > 0)
            {
                XmlNode firstChild = elementsByTagName[0].FirstChild;
                if (firstChild != null)
                    empty = firstChild.Value;
            }
            return empty;
        }
    }
}
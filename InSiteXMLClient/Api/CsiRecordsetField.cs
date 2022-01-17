using Camstar.XMLClient.Interface;
using System;
using System.Xml;

namespace Camstar.XMLClient.API
{
    internal class CsiRecordsetField : CsiXmlElement, ICsiRecordsetField, ICsiXmlElement
    {
        public CsiRecordsetField(ICsiDocument doc, XmlElement element)
          : base(doc, element)
        {
        }

        public CsiRecordsetField(ICsiDocument doc, string name, ICsiXmlElement parent)
          : base(doc, name, parent)
        {
        }

        public virtual string GetName() => this.GetElementName();

        public virtual string GetValue()
        {
            try
            {
                return this.GetDomElement().FirstChild?.Value ?? "";
            }
            catch 
            {
                return "";
            }
        }
    }
}
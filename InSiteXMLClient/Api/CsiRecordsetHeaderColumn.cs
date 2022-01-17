using Camstar.XMLClient.Interface;
using System.Xml;

namespace Camstar.XMLClient.API
{
    internal class CsiRecordsetHeaderColumn :
      CsiXmlElement,
      ICsiRecordsetHeaderColumn,
      ICsiXmlElement
    {
        public CsiRecordsetHeaderColumn(ICsiDocument doc, ICsiXmlElement element)
          : base(doc, "__column", element)
        {
        }

        public CsiRecordsetHeaderColumn(ICsiDocument doc, XmlElement domElement)
          : base(doc, domElement)
        {
        }

        public virtual string GetName() => this.FindChildByName("__name") is CsiXmlElement childByName ? CsiXmlHelper.GetFirstTextNodeValue(childByName) : null;

        public virtual ICsiLabel GetLabel() => this.FindChildByName("__label") as ICsiLabel;

        public virtual ICsiFieldType GetFieldType() => this.FindChildByName("__fieldType") as ICsiFieldType;
    }
}
using Camstar.XMLClient.Interface;
using System.Xml;

namespace Camstar.XMLClient.API
{
    internal class CsiSelectionValue : CsiXmlElement, ICsiSelectionValue, ICsiXmlElement
    {
        public CsiSelectionValue(ICsiDocument doc, XmlElement domElement)
          : base(doc, domElement)
        {
        }

        public virtual string GetDisplayName() => this.GetData("__displayName");

        public virtual string GetValue() => this.GetData("__value");

        private string GetData(string tagName) => (this.FindChildByName(tagName) as CsiXmlElement).GetDomElement().FirstChild.Value;
    }
}
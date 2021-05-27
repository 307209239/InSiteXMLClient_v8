using System;
using System.Xml;
using Camstar.XMLClient.Interface;

namespace Camstar.XMLClient.API
{
  internal class CsiSelectionValues : CsiXmlElement, ICsiSelectionValues, ICsiXmlElement
    {
    public CsiSelectionValues(ICsiDocument doc, ICsiXmlElement parent)
      : base(doc, "__selectionValues", parent)
    {
    }

    public CsiSelectionValues(ICsiDocument doc, XmlElement element)
      : base(doc, element)
    {
    }

    public virtual Array GetAllSelectionValues() => this.GetAllChildren();

    public ICsiSelectionValue GetSelectionValueByName(string name) => this.FindChildByName("__selectionValue", "__displayName", name) as ICsiSelectionValue;
  }
}

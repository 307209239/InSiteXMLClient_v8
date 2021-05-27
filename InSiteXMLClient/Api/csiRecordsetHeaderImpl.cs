using System;
using System.Xml;
using Camstar.XMLClient.Interface;

namespace Camstar.XMLClient.API
{
  internal class CsiRecordsetHeader : CsiXmlElement, ICsiRecordsetHeader, ICsiXmlElement
  {
    public CsiRecordsetHeader(ICsiDocument doc, ICsiXmlElement element)
      : base(doc, "__recordSetHeader", element)
    {
    }

    public CsiRecordsetHeader(ICsiDocument doc, XmlElement domElement)
      : base(doc, domElement)
    {
    }

    public virtual long GetCount()
    {
      try
      {
        return long.Parse(this.GetAttribute("__columnCount"));
      }
      catch (Exception ex)
      {
        return CsiXmlHelper.GetChildCount(this.GetDomElement());
      }
    }

    public virtual Array GetColumns() => this.GetChildrenByName("__column");
  }
}

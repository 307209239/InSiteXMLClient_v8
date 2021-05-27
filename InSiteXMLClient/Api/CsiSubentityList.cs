using System;
using System.Xml;
using Camstar.XMLClient.Interface;

namespace Camstar.XMLClient.API
{
  internal class CsiSubentityList: 
    CsiObjectList,
    ICsiSubentityList,
    ICsiObjectList,
    ICsiList,
    ICsiField,
    ICsiXmlElement
  {
    public CsiSubentityList(ICsiDocument doc, XmlElement domElement)
      : base(doc, domElement)
    {
    }

    public CsiSubentityList(ICsiDocument doc, string name, ICsiXmlElement parent)
      : base(doc, name, parent)
    {
    }

    public override bool IsSubentityList() => true;

    public ICsiSubentity AppendItem()
    {
      ICsiSubentity ICsiSubentity = (ICsiSubentity) new CsiSubentity(this.GetOwnerDocument(), "__listItem", (ICsiXmlElement) this);
      ICsiSubentity.SetAttribute("__listItemAction", "add");
      return ICsiSubentity;
    }

    public ICsiSubentity ChangeItemByIndex(int index)
    {
      ICsiSubentity ICsiSubentity = (ICsiSubentity) new CsiSubentity(this.GetOwnerDocument(), "__listItem", (ICsiXmlElement) this);
      ICsiSubentity.SetAttribute("__listItemAction", "change");
      CsiXmlHelper.FindCreateSetValue((ICsiXmlElement) ICsiSubentity, "__index", Convert.ToString(index));
      return ICsiSubentity;
    }

    public ICsiSubentity GetItemByIndex(int index)
    {
      CsiXmlElement csiXmlElementImpl = this.GetItem(index);
      return csiXmlElementImpl == null ? (ICsiSubentity) null : (ICsiSubentity) new CsiSubentity(this.GetOwnerDocument(), csiXmlElementImpl.GetDomElement());
    }
  }
}

using System;
using System.Xml;
using Camstar.XMLClient.Interface;

namespace Camstar.XMLClient.API
{
  internal class CsiNamedSubentityList : 
    CsiObjectList,
    ICsiNamedSubentityList,
    ICsiObjectList,
    ICsiList,
    ICsiField,
    ICsiXmlElement
  {
    public CsiNamedSubentityList(ICsiDocument doc, XmlElement domElement)
      : base(doc, domElement)
    {
    }

    public CsiNamedSubentityList(ICsiDocument doc, string name, ICsiXmlElement parent)
      : base(doc, name, parent)
    {
    }

    public override bool IsNamedSubentityList() => true;

    public ICsiNamedSubentity AppendItem(string name)
    {
      ICsiNamedSubentity ICsiNamedSubentity = (ICsiNamedSubentity) new CsiNamedSubentity(this.GetOwnerDocument(), "__listItem", (ICsiXmlElement) this);
      ICsiNamedSubentity.SetAttribute("__listItemAction", "add");
      ICsiNamedSubentity.SetName(name);
      return ICsiNamedSubentity;
    }

    public ICsiNamedSubentity DeleteItemByName(string itemName)
    {
      ICsiNamedSubentity ICsiNamedSubentity = (ICsiNamedSubentity) new CsiNamedSubentity(this.GetOwnerDocument(), "__listItem", (ICsiXmlElement) this);
      ICsiNamedSubentity.SetAttribute("__listItemAction", "delete");
      CsiXmlHelper.FindCreateSetValue2((ICsiXmlElement) ICsiNamedSubentity, "__key", "__name", itemName, true);
      return ICsiNamedSubentity;
    }

    public ICsiNamedSubentity ChangeItemByName(string itemName)
    {
      ICsiNamedSubentity ICsiNamedSubentity = (ICsiNamedSubentity) new CsiNamedSubentity(this.GetOwnerDocument(), "__listItem", (ICsiXmlElement) this);
      ICsiNamedSubentity.SetAttribute("__listItemAction", "change");
      CsiXmlHelper.FindCreateSetValue2((ICsiXmlElement) ICsiNamedSubentity, "__key", "__name", itemName, true);
      return ICsiNamedSubentity;
    }

    public ICsiNamedSubentity ChangeItemByIndex(int index)
    {
      ICsiNamedSubentity ICsiNamedSubentity = (ICsiNamedSubentity) new CsiNamedSubentity(this.GetOwnerDocument(), "__listItem", (ICsiXmlElement) this);
      ICsiNamedSubentity.SetAttribute("__listItemAction", "change");
      CsiXmlHelper.FindCreateSetValue((ICsiXmlElement) ICsiNamedSubentity, "__index", Convert.ToString(index));
      return ICsiNamedSubentity;
    }

    public ICsiNamedSubentity GetItemByIndex(int index)
    {
      CsiXmlElement csiXmlElementImpl = this.GetItem(index);
      return csiXmlElementImpl == null ? (ICsiNamedSubentity) null : (ICsiNamedSubentity) new CsiNamedSubentity(this.GetOwnerDocument(), csiXmlElementImpl.GetDomElement());
    }

    public ICsiNamedSubentity GetItemByName(string name)
    {
      CsiXmlElement csiXmlElementImpl = this.GetItem(name);
      return csiXmlElementImpl == null ? (ICsiNamedSubentity) null : (ICsiNamedSubentity) new CsiNamedSubentity(this.GetOwnerDocument(), csiXmlElementImpl.GetDomElement());
    }
  }
}

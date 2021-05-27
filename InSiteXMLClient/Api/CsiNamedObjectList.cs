using System;
using System.Xml;
using Camstar.XMLClient.Interface;

namespace Camstar.XMLClient.API
{
  internal class CsiNamedObjectList : 
    CsiObjectList,
    ICsiNamedObjectList,
    ICsiObjectList,
    ICsiList,
    ICsiField,
    ICsiXmlElement
  {
    public CsiNamedObjectList(ICsiDocument doc, XmlElement domElement)
      : base(doc, domElement)
    {
    }

    public CsiNamedObjectList(ICsiDocument doc, string name, ICsiXmlElement parent)
      : base(doc, name, parent)
    {
    }

    public override bool IsNamedObjectList() => true;

    public ICsiNamedObject AppendItem(string itemName)
    {
        ICsiNamedObject csiNamedObject = (ICsiNamedObject) new CsiNamedObject(this.GetOwnerDocument(), "__listItem", (ICsiXmlElement) this);
      csiNamedObject.SetAttribute("__listItemAction", "add");
      csiNamedObject.SetRef(itemName);
      return csiNamedObject;
    }

    public void DeleteItemByName(string itemName)
    {
        ICsiNamedObject csiNamedObject = (ICsiNamedObject) new CsiNamedObject(this.GetOwnerDocument(), "__listItem", (ICsiXmlElement) this);
      csiNamedObject.SetAttribute("__listItemAction", "delete");
      CsiXmlHelper.FindCreateSetValue2((ICsiXmlElement) csiNamedObject, "__key", "__name", itemName, true);
    }

    public ICsiNamedObject ChangeItemByName(string itemName)
    {
        ICsiNamedObject csiNamedObject = (ICsiNamedObject) new CsiNamedObject(this.GetOwnerDocument(), "__listItem", (ICsiXmlElement) this);
      csiNamedObject.SetAttribute("__listItemAction", "change");
      CsiXmlHelper.FindCreateSetValue2((ICsiXmlElement) csiNamedObject, "__key", "__name", itemName, true);
      return csiNamedObject;
    }

    public ICsiNamedObject ChangeItemByIndex(int index)
    {
        ICsiNamedObject csiNamedObject = (ICsiNamedObject) new CsiNamedObject(this.GetOwnerDocument(), "__listItem", (ICsiXmlElement) this);
      csiNamedObject.SetAttribute("__listItemAction", "change");
      CsiXmlHelper.FindCreateSetValue((ICsiXmlElement) csiNamedObject, "__index", Convert.ToString(index));
      return csiNamedObject;
    }

    public ICsiNamedObject GetItemByIndex(int index)
    {
      CsiXmlElement csiXmlElementImpl = this.GetItem(index);
      return csiXmlElementImpl == null ? (ICsiNamedObject) null : (ICsiNamedObject) new CsiNamedObject(this.GetOwnerDocument(), csiXmlElementImpl.GetDomElement());
    }

    public ICsiNamedObject GetItemByName(string name)
    {
      CsiXmlElement csiXmlElementImpl = this.GetItem(name);
      return csiXmlElementImpl == null ? (ICsiNamedObject) null : (ICsiNamedObject) new CsiNamedObject(this.GetOwnerDocument(), csiXmlElementImpl.GetDomElement());
    }
  }
}

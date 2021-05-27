using System;
using System.Xml;
using Camstar.XMLClient.Interface;

namespace Camstar.XMLClient.API
{
  internal class CsiContainerList : 
    CsiObjectList,
    ICsiContainerList,
    ICsiObjectList,
    ICsiList,
    ICsiField,
    ICsiXmlElement
  {
    public CsiContainerList(ICsiDocument doc, XmlElement domElement)
      : base(doc, domElement)
    {
    }

    public CsiContainerList(ICsiDocument doc, string name, ICsiXmlElement parent)
      : base(doc, name, parent)
    {
    }

    public override bool IsContainerList() => true;

    public ICsiContainer AppendItem(string name, string level)
    {
        ICsiContainer ICsiContainer = (ICsiContainer) new CsiContainer(this.GetOwnerDocument(), "__listItem", (ICsiXmlElement) this);
      ICsiContainer.SetAttribute("__listItemAction", "add");
      ICsiContainer.SetRef(name, level);
      return ICsiContainer;
    }

    public void DeleteItemByRef(string name, string level)
    {
      ICsiContainer ICsiContainer = (ICsiContainer) new CsiContainer(this.GetOwnerDocument(), "__listItem", (ICsiXmlElement) this);
      ICsiContainer.SetAttribute("__listItemAction", "delete");
      CsiXmlHelper.FindCreateSetValue2((ICsiXmlElement) ICsiContainer, "__key", "__name", name, true);
      CsiXmlHelper.FindCreateSetValue3((ICsiXmlElement) ICsiContainer, "__key", "__level", "__name", level, true);
    }

    public ICsiContainer ChangeItemByRef(string name, string level)
    {
      ICsiContainer ICsiContainer = (ICsiContainer) new CsiContainer(this.GetOwnerDocument(), "__listItem", (ICsiXmlElement) this);
      ICsiContainer.SetAttribute("__listItemAction", "change");
      CsiXmlHelper.FindCreateSetValue2((ICsiXmlElement) ICsiContainer, "__key", "__name", name, true);
      CsiXmlHelper.FindCreateSetValue3((ICsiXmlElement) ICsiContainer, "__key", "__level", "__name", level, true);
      return ICsiContainer;
    }

    public ICsiContainer ChangeItemByIndex(int index)
    {
      ICsiContainer ICsiContainer = (ICsiContainer) new CsiContainer(this.GetOwnerDocument(), "__listItem", (ICsiXmlElement) this);
      ICsiContainer.SetAttribute("__listItemAction", "change");
      CsiXmlHelper.FindCreateSetValue((ICsiXmlElement) ICsiContainer, "__index", Convert.ToString(index));
      return ICsiContainer;
    }

    public ICsiContainer GetItemByIndex(int index)
    {
      CsiXmlElement csiXmlElementImpl = this.GetItem(index);
      return csiXmlElementImpl == null ? (ICsiContainer) null : (ICsiContainer) new CsiContainer(this.GetOwnerDocument(), csiXmlElementImpl.GetDomElement());
    }

    public ICsiContainer GetItemByRef(string name, string level)
    {
      CsiContainer csiContainerImpl =  null;
      foreach (object allChild in this.GetAllChildren())
      {
        CsiObject csiObjectImpl = allChild as CsiObject;
        csiContainerImpl = new CsiContainer(this.GetOwnerDocument(), csiObjectImpl.GetDomElement());
        if (level != null && level.Length == 0)
          level = (string) null;
        if (!csiContainerImpl.Equals(name, level))
          csiContainerImpl = null;
        else
          break;
      }
      return (ICsiContainer) csiContainerImpl;
    }
  }
}

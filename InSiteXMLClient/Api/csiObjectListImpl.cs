using System.Xml;
using Camstar.XMLClient.Interface;

namespace Camstar.XMLClient.API
{
  internal class CsiObjectList : CsiList, ICsiObjectList, ICsiList, ICsiField, ICsiXmlElement
  {
    public CsiObjectList(ICsiDocument doc, XmlElement domElement)
      : base(doc, domElement)
    {
    }

    public CsiObjectList(ICsiDocument doc, string name, ICsiXmlElement parent)
      : base(doc, name, parent)
    {
    }

    public override bool IsObjectList() => true;

    public virtual void AppendItemById(string istanceID) => new CsiObject(this.GetOwnerDocument(), "__listItem", (ICsiXmlElement) this).SetObjectId(istanceID);

    protected virtual CsiXmlElement GetItem(string name)
    {
      foreach (CsiXmlElement listItem in this.GetListItems())
      {
        CsiXmlElement childByName = (CsiXmlElement) listItem.FindChildByName("__name");
        if (childByName != null && name.Equals(childByName.GetElementValue()))
          return listItem;
      }
      return (CsiXmlElement) null;
    }
  }
}

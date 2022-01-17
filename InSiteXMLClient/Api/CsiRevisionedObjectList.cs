using Camstar.Util;
using Camstar.XMLClient.Interface;
using System;
using System.Xml;

namespace Camstar.XMLClient.API
{
    internal class CsiRevisionedObjectList :
      CsiObjectList,
      ICsiRevisionedObjectList,
      ICsiObjectList,
      ICsiList,
      ICsiField,
      ICsiXmlElement
    {
        public CsiRevisionedObjectList(ICsiDocument doc, XmlElement domElement)
          : base(doc, domElement)
        {
        }

        public CsiRevisionedObjectList(ICsiDocument doc, string name, ICsiXmlElement parent)
          : base(doc, name, parent)
        {
        }

        public override bool IsRevisionedObjectList() => true;

        public ICsiRevisionedObject AppendItem(
          string itemName,
          string revision,
          bool useROR)
        {
            ICsiRevisionedObject revisionedObject = (ICsiRevisionedObject)new CsiRevisionedObject(this.GetOwnerDocument(), "__listItem", (ICsiXmlElement)this);
            revisionedObject.SetAttribute("__listItemAction", "add");
            revisionedObject.SetRef(itemName, revision, useROR);
            return revisionedObject;
        }

        public void DeleteItemByRef(string itemName, string revision, bool useROR)
        {
            ICsiRevisionedObject revisionedObject = (ICsiRevisionedObject)new CsiRevisionedObject(this.GetOwnerDocument(), "__listItem", (ICsiXmlElement)this);
            revisionedObject.SetAttribute("__listItemAction", "delete");
            CsiXmlHelper.FindCreateSetValue2((ICsiXmlElement)revisionedObject, "__key", "__name", itemName, true);
            CsiXmlHelper.FindCreateSetValue2((ICsiXmlElement)revisionedObject, "__key", "__rev", revision, true);
            CsiXmlHelper.FindCreateSetValue2((ICsiXmlElement)revisionedObject, "__key", "__useROR", useROR ? "true" : "false");
        }

        public ICsiRevisionedObject ChangeItemByRef(
          string itemName,
          string revision,
          bool useROR)
        {
            ICsiRevisionedObject revisionedObject = (ICsiRevisionedObject)new CsiRevisionedObject(this.GetOwnerDocument(), "__listItem", (ICsiXmlElement)this);
            revisionedObject.SetAttribute("__listItemAction", "change");
            CsiXmlHelper.FindCreateSetValue2((ICsiXmlElement)revisionedObject, "__key", "__name", itemName, true);
            CsiXmlHelper.FindCreateSetValue2((ICsiXmlElement)revisionedObject, "__key", "__rev", revision, true);
            CsiXmlHelper.FindCreateSetValue2((ICsiXmlElement)revisionedObject, "__key", "__useROR", useROR ? "true" : "false");
            return revisionedObject;
        }

        public ICsiRevisionedObject ChangeItemByIndex(int index)
        {
            ICsiRevisionedObject revisionedObject = (ICsiRevisionedObject)new CsiRevisionedObject(this.GetOwnerDocument(), "__listItem", (ICsiXmlElement)this);
            revisionedObject.SetAttribute("__listItemAction", "change");
            CsiXmlHelper.FindCreateSetValue((ICsiXmlElement)revisionedObject, "__index", Convert.ToString(index));
            return revisionedObject;
        }

        public ICsiRevisionedObject GetItemByIndex(int index)
        {
            CsiXmlElement csiXmlElementImpl = this.GetItem(index);
            return csiXmlElementImpl == null ? (ICsiRevisionedObject)null : (ICsiRevisionedObject)new CsiRevisionedObject(this.GetOwnerDocument(), csiXmlElementImpl.GetDomElement());
        }

        public ICsiRevisionedObject GetItemByRef(
          string itemName,
          string revision,
          bool useROR)
        {
            foreach (object allChild in this.GetAllChildren())
            {
                CsiXmlElement csiXmlElementImpl = allChild as CsiXmlElement;
                if (csiXmlElementImpl.FindChildByName("__name") is CsiXmlElement name && itemName.Equals(CsiXmlHelper.GetFirstTextNodeValue(name)) && (csiXmlElementImpl.FindChildByName("__useROR") is CsiXmlElement ror && Convert.ToBoolean(CsiXmlHelper.GetFirstTextNodeValue(ror)) == useROR) && (useROR || StringUtil.IsEmptyString(revision) || csiXmlElementImpl.FindChildByName("__rev") is CsiXmlElement rev && revision.Equals(CsiXmlHelper.GetFirstTextNodeValue(rev))))
                    return (ICsiRevisionedObject)new CsiRevisionedObject(this.GetOwnerDocument(), csiXmlElementImpl.GetDomElement());
            }
            return null;
        }
    }
}
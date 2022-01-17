using Camstar.XMLClient.Interface;
using System;
using System.Xml;

namespace Camstar.XMLClient.API
{
    internal class CsiDataList : CsiList, ICsiDataList, ICsiList, ICsiField, ICsiXmlElement
    {
        public CsiDataList(ICsiDocument doc, XmlElement domElement)
          : base(doc, domElement)
        {
        }

        public CsiDataList(ICsiDocument doc, string name, ICsiXmlElement parent)
          : base(doc, name, parent)
        {
        }

        public override bool IsDataList() => true;

        public ICsiDataField AppendItem(string val)
        {
            CsiDataField csiDataFieldImpl = new CsiDataField(this.GetOwnerDocument(), "__listItem", (ICsiXmlElement)this);
            csiDataFieldImpl.SetAttribute("__listItemAction", "add");
            csiDataFieldImpl.SetValue(val);
            return (ICsiDataField)csiDataFieldImpl;
        }

        public ICsiDataField DeleteItemByValue(string val)
        {
            CsiDataField csiDataFieldImpl = new CsiDataField(this.GetOwnerDocument(), "__listItem", (ICsiXmlElement)this);
            csiDataFieldImpl.SetAttribute("__listItemAction", "delete");
            ICsiXmlElement createSetValue2 = CsiXmlHelper.FindCreateSetValue2((ICsiXmlElement)csiDataFieldImpl, "__key", "__value", val, true);
            return (ICsiDataField)new CsiDataField(this.GetOwnerDocument(), (createSetValue2 as CsiXmlElement).GetDomElement());
        }

        public ICsiDataField ChangeItemByIndex(int index, string val)
        {
            CsiDataField csiDataFieldImpl = new CsiDataField(this.GetOwnerDocument(), "__listItem", (ICsiXmlElement)this);
            csiDataFieldImpl.SetAttribute("__listItemAction", "change");
            CsiXmlHelper.FindCreateSetValue((ICsiXmlElement)csiDataFieldImpl, "__index", Convert.ToString(index));
            ICsiXmlElement createSetValue = CsiXmlHelper.FindCreateSetValue((ICsiXmlElement)csiDataFieldImpl, "__value", val, true);
            return (ICsiDataField)new CsiDataField(this.GetOwnerDocument(), (createSetValue as CsiXmlElement).GetDomElement());
        }

        public ICsiDataField ChangeItemByValue(string oldValue, string newValue)
        {
            CsiDataField csiDataFieldImpl = new CsiDataField(this.GetOwnerDocument(), "__listItem", (ICsiXmlElement)this);
            csiDataFieldImpl.SetAttribute("__listItemAction", "change");
            CsiXmlHelper.FindCreateSetValue2((ICsiXmlElement)csiDataFieldImpl, "__key", "__value", oldValue, true);
            ICsiXmlElement createSetValue = CsiXmlHelper.FindCreateSetValue((ICsiXmlElement)csiDataFieldImpl, "__value", newValue, true);
            return (ICsiDataField)new CsiDataField(this.GetOwnerDocument(), (createSetValue as CsiXmlElement).GetDomElement());
        }

        public ICsiDataField GetItemByIndex(int index)
        {
            CsiXmlElement csiXmlElementImpl = this.GetItem(index);
            return csiXmlElementImpl == null ? (ICsiDataField)null : (ICsiDataField)new CsiDataField(this.GetOwnerDocument(), csiXmlElementImpl.GetDomElement());
        }
    }
}
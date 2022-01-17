using Camstar.Util;
using Camstar.XMLClient.Enum;
using Camstar.XMLClient.Interface;
using System;
using System.Xml;

namespace Camstar.XMLClient.API
{
    internal class CsiRequestField : CsiField, ICsiRequestField, ICsiField, ICsiXmlElement
    {
        public CsiRequestField(ICsiDocument doc, XmlElement requestField)
          : base(doc, requestField)
        {
        }

        public CsiRequestField(ICsiDocument doc, string name, ICsiXmlElement parent)
          : base(doc, name, parent)
        {
        }

        public virtual void SetSerializationMode(SerializationModes mode)
        {
            string str = "";
            switch (mode)
            {
                case SerializationModes.SerializationModeDeep:
                    str = "deep";
                    break;

                case SerializationModes.SerializationModeShallow:
                    str = "shallow";
                    break;

                case SerializationModes.SerializationModeDefault:
                    str = "default";
                    break;
            }
            if (StringUtil.IsEmptyString(str))
                return;
            this.SetAttribute("__serialization", str);
        }

        public void RequestSelectionValues()
        {
            if (this.GetSelectionValues() != null)
                return;
            CsiSelectionValues selectionValuesImpl = new CsiSelectionValues(this.GetOwnerDocument(), (ICsiXmlElement)this);
        }

        public void RequestAllFields() => this.GetAllFieldElement();

        public ICsiRequestField RequestField(string fieldName)
        {
            if (!(this.FindChildByName(fieldName) is CsiRequestField requestFieldImpl))
                requestFieldImpl = new CsiRequestField(this.GetOwnerDocument(), fieldName, (ICsiXmlElement)this);
            return (ICsiRequestField)requestFieldImpl;
        }

        public void RequestAllFieldsRecursive() => this.GetAllFieldElement().SetAttribute("__recursive", "true");

        public void RequestCaption()
        {
            if (!(this.FindChildByName("__metadata") is ICsiMetaData csiMetaData))
                csiMetaData = (ICsiMetaData)new CsiMetaData(this.GetOwnerDocument(), (ICsiXmlElement)this);
            csiMetaData.RequestFieldLabel();
        }

        public void RequestUserDefinedFields() => CsiXmlHelper.FindCreateSetValue((ICsiXmlElement)this, "__userDefinedFields", null);

        public void RequestFieldDefinition()
        {
            if (!(this.FindChildByName("__metadata") is CsiMetaData csiMetaDataImpl))
                csiMetaDataImpl = new CsiMetaData(this.GetOwnerDocument(), (ICsiXmlElement)this);
            CsiXmlHelper.FindCreateSetValue((ICsiXmlElement)csiMetaDataImpl, "__fieldDef", null);
        }

        public void RequestCdoDefinition()
        {
            if (!(this.FindChildByName("__metadata") is ICsiMetaData csiMetaData))
                csiMetaData = (ICsiMetaData)new CsiMetaData(this.GetOwnerDocument(), (ICsiXmlElement)this);
            csiMetaData.RequestCdoDefinition();
        }

        public void RequestUserDefinedFieldDefinitions()
        {
            if (!(this.FindChildByName("__metadata") is ICsiMetaData csiMetaData))
                csiMetaData = (ICsiMetaData)new CsiMetaData(this.GetOwnerDocument(), (ICsiXmlElement)this);
            csiMetaData.RequestUserDefinedFields();
        }

        public void RequestDefaultValue() => CsiXmlHelper.FindCreateSetValue((ICsiXmlElement)this, "__defaultValue", null);

        public ICsiRequestSelectionValuesEx RequestSelectionValuesEx()
        {
            if (!(this.FindChildByName("__requestSelectionValuesEx") is CsiRequestSelectionValuesEx selectionValuesExImpl))
                selectionValuesExImpl = new CsiRequestSelectionValuesEx(this.GetOwnerDocument(), (ICsiXmlElement)this);
            return (ICsiRequestSelectionValuesEx)selectionValuesExImpl;
        }

        public ICsiRequestField RequestListItemByIndex(
          int index,
          string fieldName,
          string cdoTypeName)
        {
            if (index < 0)
                throw new CsiClientException(3014682L, this.GetType().FullName + ".requestListItemByIndex()");
            CsiXmlElement csiXmlElementImpl1 = (CsiXmlElement)null;
            foreach (object obj in this.GetChildrenByName("__listItem"))
            {
                csiXmlElementImpl1 = obj as CsiXmlElement;
                if (int.Parse(csiXmlElementImpl1.GetAttribute("__index")) != index)
                    csiXmlElementImpl1 = (CsiXmlElement)null;
                else
                    break;
            }
            if (csiXmlElementImpl1 == null)
            {
                csiXmlElementImpl1 = new CsiXmlElement(this.GetOwnerDocument(), "__listItem", (ICsiXmlElement)this);
                csiXmlElementImpl1.SetAttribute("__index", Convert.ToString(index));
            }
            CsiXmlElement csiXmlElementImpl2 = csiXmlElementImpl1;
            if (!StringUtil.IsEmptyString(fieldName) && fieldName != null)
            {
                if (this.FindChildByName(fieldName) is CsiXmlElement childByName)
                    this.RemoveChild((ICsiXmlElement)childByName);
                csiXmlElementImpl2 = (CsiXmlElement)new CsiRequestField(this.GetOwnerDocument(), fieldName, (ICsiXmlElement)csiXmlElementImpl1);
                csiXmlElementImpl1.AppendChild((ICsiXmlElement)csiXmlElementImpl2);
            }
            return csiXmlElementImpl2 as ICsiRequestField;
        }

        public ICsiRequestField RequestListItemByName(
          string name,
          string fieldName,
          string cdoTypeName)
        {
            if (name == null)
                throw new CsiClientException(3014682L, this.GetType().FullName + ".requestListItemByName()");
            CsiXmlElement csiXmlElementImpl1 = (CsiXmlElement)null;
            foreach (object obj in this.GetChildrenByName("__listItem"))
            {
                csiXmlElementImpl1 = obj as CsiXmlElement;
                if (!name.Equals(csiXmlElementImpl1.GetAttribute("__name")))
                    csiXmlElementImpl1 = (CsiXmlElement)null;
                else
                    break;
            }
            if (csiXmlElementImpl1 == null)
            {
                csiXmlElementImpl1 = new CsiXmlElement(this.GetOwnerDocument(), "__listItem", (ICsiXmlElement)this);
                csiXmlElementImpl1.SetAttribute("__name", name);
            }
            CsiXmlElement csiXmlElementImpl2 = csiXmlElementImpl1;
            if (!StringUtil.IsEmptyString(fieldName))
            {
                if (this.FindChildByName(fieldName) is CsiXmlElement childByName)
                    this.RemoveChild((ICsiXmlElement)childByName);
                csiXmlElementImpl2 = (CsiXmlElement)new CsiRequestField(this.GetOwnerDocument(), fieldName, (ICsiXmlElement)csiXmlElementImpl1);
                csiXmlElementImpl1.AppendChild((ICsiXmlElement)csiXmlElementImpl2);
            }
            return csiXmlElementImpl2 as ICsiRequestField;
        }

        private ICsiXmlElement GetAllFieldElement() => this.FindChildByName("__allFields") ?? (ICsiXmlElement)new CsiXmlElement(this.GetOwnerDocument(), "__allFields", (ICsiXmlElement)this);
    }
}
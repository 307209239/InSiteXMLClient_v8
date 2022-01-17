using Camstar.Util;
using Camstar.XMLClient.Enum;
using Camstar.XMLClient.Interface;
using System.Xml;

namespace Camstar.XMLClient.API
{
    internal class CsiField : CsiXmlElement, ICsiField, ICsiXmlElement
    {
        public CsiField(ICsiDocument doc, XmlElement domElement) : base(doc, domElement)
        {
        }

        public CsiField(ICsiDocument doc, string name, ICsiXmlElement parent) : base(doc, name, parent)
        {
        }

        public override bool IsField() => true;

        public virtual ICsiSelectionValues GetSelectionValues() => this.FindChildByName("__selectionValues") as ICsiSelectionValues;

        public virtual ICsiLabel GetCaption() => this.FindChildByName("__metadata") is ICsiMetaData childByName ? childByName.GetFieldLabel() : null;

        public virtual ICsiFieldDefinition GetFieldDefinition() => this.FindChildByName("__metadata") is ICsiMetaData childByName ? childByName.GetCdoField() : null;

        public virtual ICsiCdoDefinition GetCdoDefinition() => this.FindChildByName("__metadata") is ICsiMetaData childByName ? childByName.GetCdoDefinition() : null;

        public virtual ICsiField GetDefaultValue() => (ICsiField)(this.FindChildByName("__defaultValue") as CsiField);

        public virtual ICsiSelectionValuesEx GetSelectionValuesEx() => this.FindChildByName("__selectionValuesEx") as ICsiSelectionValuesEx;

        public virtual string GetSpecificType()
        {
            string attribute = this.GetAttribute("__specificType");
            if (StringUtil.IsEmptyString(attribute) && (this.GetElementName().Equals("__listItem") && this.GetParentElement() != null))
                attribute = this.GetParentElement().GetAttribute("__specificType");
            return attribute;
        }

        public virtual CsiGenericTypes GetGenericType()
        {
            string attribute = this.GetAttribute("__genericType");
            if (StringUtil.IsEmptyString(attribute) && (this.GetElementName().Equals("__listItem") && this.GetParentElement() != null))
                attribute = this.GetParentElement().GetAttribute("__genericType");
            if (!StringUtil.IsEmptyString(attribute))
            {
                if (attribute.Equals("Boolean"))
                    return CsiGenericTypes.GenericTypeBoolean;
                if (attribute.Equals("Decimal"))
                    return CsiGenericTypes.GenericTypeDecimal;
                if (attribute.Equals("Fixed"))
                    return CsiGenericTypes.GenericTypeFixed;
                if (attribute.Equals("Float"))
                    return CsiGenericTypes.GenericTypeFloat;
                if (attribute.Equals("Integer"))
                    return CsiGenericTypes.GenericTypeInteger;
                if (attribute.Equals("Object"))
                    return CsiGenericTypes.GenericTypeObject;
                if (attribute.Equals("String"))
                    return CsiGenericTypes.GenericTypeString;
                if (attribute.Equals("TimeStamp"))
                    return CsiGenericTypes.GenericTypeTimestamp;
            }
            return CsiGenericTypes.GenericTypeNone;
        }

        public virtual CsiReferenceTypes GetReferenceType()
        {
            string attribute = this.GetAttribute("__referenceType");
            if (StringUtil.IsEmptyString(attribute) && (this.GetElementName().Equals("__listItem") && this.GetParentElement() != null))
                attribute = this.GetParentElement().GetAttribute("__referenceType");
            if (!StringUtil.IsEmptyString(attribute))
            {
                if (attribute.Equals("Container"))
                    return CsiReferenceTypes.ReferenceTypeContainer;
                if (attribute.Equals("NamedDataObject"))
                    return CsiReferenceTypes.ReferenceTypeNamedDataObject;
                if (attribute.Equals("RevisionedObject"))
                    return CsiReferenceTypes.ReferenceTypeRevisionedObject;
                if (attribute.Equals("Subentity"))
                    return CsiReferenceTypes.ReferenceTypeSubEntity;
                if (attribute.Equals("NamedSubentity"))
                    return CsiReferenceTypes.ReferenceTypeNamedSubEntity;
                if (attribute.Equals("Object"))
                    return CsiReferenceTypes.ReferenceTypeObject;
                if (attribute.Equals(""))
                    return CsiReferenceTypes.ReferenceTypeNone;
            }
            return CsiReferenceTypes.ReferenceTypeNone;
        }

        public virtual ICsiField[] GetFields() => this.GetFields();

        protected virtual ICsiMetaData GetMetaData() => this.FindChildByName("__metadata") as ICsiMetaData;
    }
}
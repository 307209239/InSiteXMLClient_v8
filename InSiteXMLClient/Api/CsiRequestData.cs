using Camstar.Util;
using Camstar.XMLClient.Enum;
using Camstar.XMLClient.Interface;
using System;
using System.Xml;

namespace Camstar.XMLClient.API
{
    internal class CsiRequestData : CsiXmlElement, ICsiRequestData, ICsiXmlElement
    {
        public CsiRequestData(ICsiDocument doc, XmlElement domElement)
          : base(doc, domElement)
        {
        }

        public CsiRequestData(ICsiDocument doc, ICsiXmlElement parent)
          : base(doc, "__requestData", parent)
        {
        }

        public override bool IsRequestData() => true;

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

        public virtual ICsiRequestField RequestField(string fieldName) => this.RequestForField(fieldName);

        public virtual void RequestSessionValues()
        {
            CsiXmlElement csiXmlElementImpl = new CsiXmlElement(this.GetOwnerDocument(), "__sessionValues", (ICsiXmlElement)this);
        }

        public virtual void RequestAllFields()
        {
            CsiXmlElement csiXmlElementImpl = new CsiXmlElement(this.GetOwnerDocument(), "__allFields", (ICsiXmlElement)this);
        }

        public virtual void RequestAllFieldsRecursive() => new CsiXmlElement(this.GetOwnerDocument(), "__allFields", (ICsiXmlElement)this).SetAttribute("__recursive", "true");

        public virtual void RequestFields(Array fields)
        {
            foreach (string field in fields)
                this.RequestField(field);
        }
    }
}
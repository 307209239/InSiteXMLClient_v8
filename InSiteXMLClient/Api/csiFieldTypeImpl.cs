using Camstar.Util;
using System.Xml;
using Camstar.XMLClient.Enum;
using Camstar.XMLClient.Interface;

namespace Camstar.XMLClient.API
{
  internal class CsiFieldType : CsiXmlElement, ICsiFieldType, ICsiXmlElement
  {
    public CsiFieldType(ICsiDocument doc, ICsiXmlElement element)
      : base(doc, "__fieldType", element)
    {
    }

    public CsiFieldType(ICsiDocument doc, XmlElement domElement)
      : base(doc, domElement)
    {
    }

    public virtual string GetSpecificType() => this.FindChildByName("__specificType") is CsiXmlElement childByName ? CsiXmlHelper.GetFirstTextNodeValue(childByName) : (string) null;

    public virtual string GetNodeType() => this.FindChildByName("__nodeType") is CsiXmlElement childByName ? CsiXmlHelper.GetFirstTextNodeValue(childByName) : (string) null;

    public virtual CsiGenericTypes GetGenericType()
    {
      if (this.FindChildByName("__genericType") is CsiXmlElement childByName)
      {
        string firstTextNodeValue = CsiXmlHelper.GetFirstTextNodeValue(childByName);
        if (!StringUtil.IsEmptyString(firstTextNodeValue))
        {
          if (firstTextNodeValue.Equals("Boolean"))
            return CsiGenericTypes.GenericTypeBoolean;
          if (firstTextNodeValue.Equals("Decimal"))
            return CsiGenericTypes.GenericTypeDecimal;
          if (firstTextNodeValue.Equals("Fixed"))
            return CsiGenericTypes.GenericTypeFixed;
          if (firstTextNodeValue.Equals("Float"))
            return CsiGenericTypes.GenericTypeFloat;
          if (firstTextNodeValue.Equals("Integer"))
            return CsiGenericTypes.GenericTypeInteger;
          if (firstTextNodeValue.Equals("Object"))
            return CsiGenericTypes.GenericTypeObject;
          if (firstTextNodeValue.Equals("String"))
            return CsiGenericTypes.GenericTypeString;
          if (firstTextNodeValue.Equals("TimeStamp"))
            return CsiGenericTypes.GenericTypeTimestamp;
        }
      }
      return CsiGenericTypes.GenericTypeNone;
    }

    public virtual CsiReferenceTypes GetReferenceType()
    {
      if (this.FindChildByName("__referenceType") is CsiXmlElement childByName)
      {
        string firstTextNodeValue = CsiXmlHelper.GetFirstTextNodeValue(childByName);
        if (!StringUtil.IsEmptyString(firstTextNodeValue))
        {
          if (firstTextNodeValue.Equals("Container"))
            return CsiReferenceTypes.ReferenceTypeContainer;
          if (firstTextNodeValue.Equals("NamedDataObject"))
            return CsiReferenceTypes.ReferenceTypeNamedDataObject;
          if (firstTextNodeValue.Equals("RevisionedObject"))
            return CsiReferenceTypes.ReferenceTypeRevisionedObject;
          if (firstTextNodeValue.Equals("Subentity"))
            return CsiReferenceTypes.ReferenceTypeSubEntity;
          if (firstTextNodeValue.Equals("NamedSubentity"))
            return CsiReferenceTypes.ReferenceTypeNamedSubEntity;
          if (firstTextNodeValue.Equals("Object"))
            return CsiReferenceTypes.ReferenceTypeObject;
          if (firstTextNodeValue.Equals(""))
            return CsiReferenceTypes.ReferenceTypeNone;
        }
      }
      return CsiReferenceTypes.ReferenceTypeNone;
    }
  }
}

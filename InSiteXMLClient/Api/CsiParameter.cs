using System;
using System.Xml;
using Camstar.XMLClient.Interface;

namespace Camstar.XMLClient.API
{
  internal class CsiParameter : CsiXmlElement, ICsiParameter, ICsiXmlElement
  {
    private XmlElement mElementName;
    private XmlElement mElementValue;

    public CsiParameter(ICsiDocument doc, string paramName, ICsiXmlElement parent)
      : base(doc, "__parameter", parent)
    {
      this.mElementName = new CsiXmlElement(doc, "__name", (ICsiXmlElement) this).GetDomElement();
      this.mElementValue = new CsiXmlElement(doc, "__value", (ICsiXmlElement) this).GetDomElement();
      CsiXmlHelper.SetCdataNode(this.mElementName, paramName);
      CsiXmlHelper.SetCdataNode(this.mElementValue, "");
    }

    public CsiParameter(ICsiDocument doc, XmlElement element)
      : base(doc, element)
    {
      this.mElementName = ((CsiXmlElement) this.FindChildByName("__name")).GetDomElement();
      this.mElementValue = ((CsiXmlElement) this.FindChildByName("__value")).GetDomElement();
    }

    public virtual string GetValue()
    {
      try
      {
        return (this.mElementValue.FirstChild as XmlCDataSection).Data;
      }
      catch (XmlException ex)
      {
        throw new CsiClientException(-1L, (Exception) ex, this.GetType().FullName + ".getValue()");
      }
    }

    public virtual void SetValue(string val)
    {
      try
      {
        CsiXmlHelper.SetCdataNode(this.mElementValue, val);
      }
      catch (XmlException ex)
      {
        throw new CsiClientException(-1L, (Exception) ex, this.GetType().FullName + ".setValue()");
      }
    }
  }
}

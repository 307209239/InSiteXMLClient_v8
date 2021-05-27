using Camstar.Util;
using System;
using System.Xml;
using Camstar.XMLClient.Enum;
using Camstar.XMLClient.Interface;

namespace Camstar.XMLClient.API
{
  internal class CsiDataField : CsiField, ICsiDataField, ICsiField, ICsiXmlElement
  {
    private XmlNode mData;
    private const string mkUID = "{8700F239-6C00-43e9-BA57-F2393B34D1DA}";

    public CsiDataField(ICsiDocument doc, XmlElement domElement)
      : base(doc, domElement)
    {
      try
      {
        this.mData = this.GetDomElement().FirstChild;
        if (this.mData != null)
          return;
        this.mData = (XmlNode) this.GetDomElement();
      }
      catch (Exception ex)
      {
        this.mData = (XmlNode) null;
      }
    }

    public CsiDataField(ICsiDocument doc, string name, ICsiXmlElement parent)
      : base(doc, name, parent)
    {
      this.mData = (XmlNode) this.GetDomElement().OwnerDocument.CreateCDataSection("");
      this.GetDomElement().AppendChild(this.mData);
    }

    public CsiDataField(ICsiDocument doc, string name, ICsiXmlElement parent, string val)
      : this(doc, name, parent)
      => this.SetValue(val);

    private CsiDataField(
        ICsiDocument doc,
      string name,
        ICsiXmlElement parent,
      string val,
      bool ignoreContraint)
      : base(doc, name, parent)
    {
      this.mData = (XmlNode) this.GetDomElement().OwnerDocument.CreateCDataSection(val);
      this.GetDomElement().AppendChild(this.mData);
    }

    public override bool IsDataField() => true;

    public virtual string GetValue()
    {
      if (StringUtil.IsEmptyString(this.mData.Value))
        return string.Empty;
      string strMessage = this.mData.Value;
      if (this.GetAttribute("__encrypted") == "yes")
        strMessage = new RC2StringProvider().Decrypt("{8700F239-6C00-43e9-BA57-F2393B34D1DA}", strMessage);
      return strMessage;
    }

    public virtual void SetValue(string val) => this.mData.Value = val;

    public virtual bool IsEmptyValue() => "yes".Equals(this.GetAttribute("__empty"));

    public void SetEmptyValue() => this.SetAttribute("__empty", "yes");

    public void SetFormattedValue(string val, DataFormats format)
    {
      string val1;
      try
      {
        switch (format)
        {
          case DataFormats.FormatDateAndTime:
            val1 = csiXMLDataFormat.locale2lexicalDateTime(val);
            break;
          case DataFormats.FormatDate:
            val1 = csiXMLDataFormat.locale2lexicalDate(val);
            break;
          case DataFormats.FormatTime:
            val1 = csiXMLDataFormat.locale2lexicalTime(val);
            break;
          case DataFormats.FormatDecimal:
            val1 = csiXMLDataFormat.locale2lexicalDecimal(val);
            break;
          case DataFormats.FormatFloat:
            val1 = csiXMLDataFormat.locale2lexicalFloat(val);
            break;
          default:
            throw new CsiClientException(-1L, "Wrong format", this.GetType().FullName + ".setFormattedValue()");
        }
      }
      catch (Exception ex)
      {
        string src = this.GetType().FullName + "#setFormattedValue()";
        throw new CsiClientException(-1L, "Can not convert '" + val + "' with format '" + (object) format + "'. " + ex.Message, src);
      }
      if (val1 == null)
        return;
      this.SetValue(val1);
    }

    public string GetFormattedValue(DataFormats format)
    {
      string val = this.GetValue();
      if (val == null || val.Length == 0)
        return val;
      string str;
      try
      {
        switch (format)
        {
          case DataFormats.FormatDateAndTime:
            str = csiXMLDataFormat.lexical2localeDateTime(val);
            break;
          case DataFormats.FormatDate:
            str = csiXMLDataFormat.lexical2localeDate(val);
            break;
          case DataFormats.FormatTime:
            str = csiXMLDataFormat.lexical2localeTime(val);
            break;
          case DataFormats.FormatDecimal:
            str = csiXMLDataFormat.lexical2localeDecimal(val);
            break;
          case DataFormats.FormatFloat:
            str = csiXMLDataFormat.lexical2localeFloat(val);
            break;
          default:
            throw new CsiClientException(-1L, "Wrong format", this.GetType().FullName + ".getFormattedValue()");
        }
      }
      catch (Exception ex)
      {
        string src = this.GetType().FullName + ".getFormattedValue()";
        throw new CsiClientException(-1L, "Cannot convert '" + val + "' with format '" + format.GetType().Name + "'. " + ex.Message, src);
      }
      return str;
    }

    public void SetEncryptedValue(string val)
    {
      this.SetAttribute("__encrypted", "yes");
      this.SetValue(new RC2StringProvider().Encrypt("{8700F239-6C00-43e9-BA57-F2393B34D1DA}", val));
    }
  }
}

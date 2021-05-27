using System;
using System.Xml;
using Camstar.XMLClient.Interface;

namespace Camstar.XMLClient.API
{
  internal class CsiService : CsiObject, ICsiService, ICsiObject, ICsiField, ICsiXmlElement
  {
    public CsiService(ICsiDocument doc, string serviceName, ICsiXmlElement parent)
      : base(doc, "__service", parent)
    {
      this.SetAttribute("__serviceType", serviceName);
      CsiXmlHelper.FindCreateSetValue((ICsiXmlElement) this, "__txnGUID", CsiXmlHelper.GenerateGuid());
      CsiXmlHelper.FindCreateSetValue((ICsiXmlElement) this, "__utcOffset", csiXMLDataFormat.GetUTCOffset());
    }

    public CsiService(ICsiDocument doc, XmlElement domElement)
      : base(doc, domElement)
    {
    }

    public override bool IsService() => true;

    public string ServiceTypeName() => this.GetDomElement().GetAttribute("__serviceType");

    public ICsiObject InputData() => (ICsiObject) new CsiObject(this.GetOwnerDocument(), (ICsiXmlElement) this);

    public ICsiRequestData RequestData()
    {
      if (!(this.FindChildByName("__requestData") is CsiRequestData csiRequestDataImpl))
        csiRequestDataImpl = new CsiRequestData(this.GetOwnerDocument(), (ICsiXmlElement) this);
      return (ICsiRequestData) csiRequestDataImpl;
    }

    public ICsiResponseData ResponseData() => this.FindChildByName("__responseData") as ICsiResponseData;

    public ICsiExceptionData ExceptionData() => this.GetChildExceptionData();

    public void SetExecute()
    {
      if (this.FindChildByName("__execute") != null)
        return;
      CsiXmlElement csiXmlElementImpl = new CsiXmlElement(this.GetOwnerDocument(), "__execute", (ICsiXmlElement) this);
    }

    public virtual bool UseTxnGuid
    {
      set
      {
        if (value)
          CsiXmlHelper.FindCreateSetValue((ICsiXmlElement) this, "__txnGUID", CsiXmlHelper.GenerateGuid());
        else
          this.RemoveChildByName((ICsiXmlElement) this, "__txnGUID");
      }
    }

    public virtual string GetUtcOffset() => this.FindChildByName("__utcOffset") is CsiXmlElement childByName ? (childByName.GetDomElement().FirstChild as XmlText).Data : (string) null;

    public virtual void SetUtcOffset(string offset)
    {
      try
      {
        bool flag = offset.StartsWith("-");
        if (offset.StartsWith("-") || offset.StartsWith("+"))
          offset = offset.Remove(0, 1);
        DateTime dateTime = DateTime.Parse(offset);
        string str = dateTime.Hour <= 12 ? dateTime.ToString("HH:mm") : throw new FormatException();
        CsiXmlHelper.FindCreateSetValue((ICsiXmlElement) this, "__utcOffset", (flag ? "-" : "+") + str);
      }
      catch (Exception ex)
      {
        throw new CsiClientException(-2147467259L, ex, this.GetType().FullName + ".setUTCOffset()");
      }
    }
  }
}

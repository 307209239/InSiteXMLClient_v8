using Camstar.XMLClient.Interface;
using System;
using System.Xml;

namespace Camstar.XMLClient.API
{
    internal class CsiRequestSelectionValuesEx :
      CsiXmlElement,
      ICsiRequestSelectionValuesEx,
      ICsiXmlElement
    {
        public CsiRequestSelectionValuesEx(ICsiDocument doc, ICsiXmlElement parent)
          : base(doc, "__requestSelectionValuesEx", parent)
        {
        }

        public CsiRequestSelectionValuesEx(ICsiDocument doc, XmlElement element)
          : base(doc, element)
        {
        }

        public virtual long GetResultsetSize() => this.FindChildByName("__rowSetSize") is ICsiDataField childByName ? long.Parse(childByName.GetValue()) : -1L;

        public virtual void SetResultsetSize(long size)
        {
            try
            {
                ICsiXmlElement childByName = this.FindChildByName("__rowSetSize");
                if (childByName != null)
                    this.RemoveChild(childByName);
                CsiDataField csiDataFieldImpl = new CsiDataField(this.GetOwnerDocument(), "__rowSetSize", (ICsiXmlElement)this, Convert.ToString(size));
            }
            catch 
            {
            }
        }

        public virtual long GetStartRow() => this.FindChildByName("__startRow") is ICsiDataField childByName ? long.Parse(childByName.GetValue()) : -1L;

        public virtual void SetStartRow(long val)
        {
            try
            {
                ICsiXmlElement childByName = this.FindChildByName("__startRow");
                if (childByName != null)
                    this.RemoveChild(childByName);
                CsiDataField csiDataFieldImpl = new CsiDataField(this.GetOwnerDocument(), "__startRow", (ICsiXmlElement)this, Convert.ToString(val));
            }
            catch 
            {
            }
        }

        public virtual ICsiQueryParameters CreateQueryParameters()
        {
            if (!(this.FindChildByName("__queryParameters") is ICsiQueryParameters csiQueryParameters))
                csiQueryParameters = (ICsiQueryParameters)new CsiQueryParameters(this.GetOwnerDocument(), (ICsiXmlElement)this);
            return csiQueryParameters;
        }

        public virtual bool GetRequestRecordCount()
        {
            ICsiDataField childByName = this.FindChildByName("__requestRecordCount") as ICsiDataField;
            try
            {
                return bool.Parse(childByName.GetValue());
            }
            catch 
            {
                return false;
            }
        }

        public virtual void SetRequestRecordCount(bool val)
        {
            try
            {
                this.RemoveChildByName((ICsiXmlElement)this, "__requestRecordCount");
                if (!val)
                    return;
                CsiDataField csiDataFieldImpl = new CsiDataField(this.GetOwnerDocument(), "__requestRecordCount", (ICsiXmlElement)this, "true");
            }
            catch 
            {
            }
        }

        public void ClearParameters() => this.FindChildByName("__queryParameters")?.RemoveAllChildren();

        public virtual void SetParameter(string param, string val) => this.CreateQueryParameters().SetParameter(param, val);
    }
}
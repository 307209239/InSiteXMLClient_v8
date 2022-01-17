using Camstar.XMLClient.Interface;
using System;
using System.Xml;

namespace Camstar.XMLClient.API
{
    internal class CsiQuery : CsiXmlElement, ICsiQuery, ICsiXmlElement
    {
        public CsiQuery(ICsiDocument doc, ICsiXmlElement parent)
          : base(doc, "__query", parent)
        {
        }

        public CsiQuery(ICsiDocument doc, XmlElement element)
          : base(doc, element)
        {
        }

        public virtual ICsiRecordset GetRecordset()
        {
            try
            {
                return this.FindChildByName("__responseData").FindChildByName("__recordSet") as ICsiRecordset;
            }
            catch 
            {
                return null;
            }
        }

        public virtual long GetRowsetSize()
        {
            try
            {
                return long.Parse(((CsiDataField)this.FindChildByName("__rowSetSize")).GetValue());
            }
            catch 
            {
                return -1;
            }
        }

        public virtual void SetRowsetSize(long size)
        {
            try
            {
                this.RemoveChildByName((ICsiXmlElement)this, "__rowSetSize");
                CsiDataField csiDataFieldImpl = new CsiDataField(this.GetOwnerDocument(), "__rowSetSize", (ICsiXmlElement)this, Convert.ToString(size));
            }
            catch 
            {
            }
        }

        public virtual string GetSqlText()
        {
            try
            {
                return (this.FindChildByName("__queryText") as CsiDataField).GetValue();
            }
            catch 
            {
                return null;
            }
        }

        public virtual void SetSqlText(string sql)
        {
            try
            {
                this.RemoveChildByName((ICsiXmlElement)this, "__queryText");
                CsiDataField csiDataFieldImpl = new CsiDataField(this.GetOwnerDocument(), "__queryText", (ICsiXmlElement)this, sql);
            }
            catch 
            {
            }
        }

        public virtual long GetStartRow()
        {
            try
            {
                return long.Parse((this.FindChildByName("__startRow") as CsiDataField).GetValue());
            }
            catch 
            {
                return -1;
            }
        }

        public virtual void SetStartRow(long row)
        {
            try
            {
                this.RemoveChildByName((ICsiXmlElement)this, "__startRow");
                CsiDataField csiDataFieldImpl = new CsiDataField(this.GetOwnerDocument(), "__startRow", (ICsiXmlElement)this, Convert.ToString(row));
            }
            catch 
            {
            }
        }

        public virtual string GetQueryName()
        {
            try
            {
                return (this.FindChildByName("__queryName") as CsiDataField).GetValue();
            }
            catch 
            {
                return null;
            }
        }

        public virtual void SetQueryName(string query)
        {
            try
            {
                this.RemoveChildByName((ICsiXmlElement)this, "__queryName");
                CsiDataField csiDataFieldImpl = new CsiDataField(this.GetOwnerDocument(), "__queryName", (ICsiXmlElement)this, query);
            }
            catch 
            {
            }
        }

        public virtual ICsiQueryParameters GetQueryParameters()
        {
            if (!(this.FindChildByName("__queryParameters") is ICsiQueryParameters csiQueryParameters))
                csiQueryParameters = (ICsiQueryParameters)new CsiQueryParameters(this.GetOwnerDocument(), (ICsiXmlElement)this);
            return csiQueryParameters;
        }

        public virtual ICsiRecordsetHeader GetRecordsetHeader()
        {
            try
            {
                return this.FindChildByName("__responseData").FindChildByName("__recordSetHeader") as ICsiRecordsetHeader;
            }
            catch 
            {
                return null;
            }
        }

        public virtual bool GetRequestRecordCount()
        {
            try
            {
                return bool.Parse(((CsiDataField)this.FindChildByName("__requestRecordCount")).GetValue());
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

        public virtual long GetRecordCount()
        {
            try
            {
                return long.Parse(((CsiDataField)this.FindChildByName("__responseData").FindChildByName("__recordCount")).GetValue());
            }
            catch 
            {
                return 0;
            }
        }

        public ICsiExceptionData ExceptionData() => this.GetChildExceptionData();

        public virtual void SetCdoTypeId(int Id)
        {
            try
            {
                this.RemoveChildByName((ICsiXmlElement)this, "__CDOTypeId");
                CsiDataField csiDataFieldImpl = new CsiDataField(this.GetOwnerDocument(), "__CDOTypeId", (ICsiXmlElement)this, Convert.ToString(Id));
            }
            catch 
            {
            }
        }

        public virtual long GetUserQueryChangeCount()
        {
            try
            {
                return long.Parse(((CsiDataField)this.FindChildByName("__responseData").FindChildByName("__changeCount")).GetValue());
            }
            catch 
            {
                return 0;
            }
        }

        public void ClearParameters() => this.FindChildByName("__queryParameters")?.RemoveAllChildren();

        public void SetParameter(string param, string val)
        {
            try
            {
                (this.GetQueryParameters() ?? (ICsiQueryParameters)new CsiQueryParameters(this.GetOwnerDocument(), (ICsiXmlElement)this)).SetParameter(param, val);
            }
            catch 
            {
            }
        }

        public string GetParameter(string param)
        {
            ICsiQueryParameters queryParameters = this.GetQueryParameters();
            if (queryParameters == null)
            {
                string src = this.GetType().FullName + ".getParameter()";
                throw new CsiClientException(-1L, CsiXmlHelper.GetNotExists("__parameters"), src);
            }
            ICsiParameter parameterByName = queryParameters.GetParameterByName(param);
            if (parameterByName != null)
                return parameterByName.GetValue();
            string src1 = this.GetType().FullName + ".getParameter()";
            throw new CsiClientException(-1L, CsiXmlHelper.GetNotExists(param), src1);
        }

        public void SetUserQueryName(string queryName, long changeCount)
        {
            try
            {
                this.RemoveChildByName((ICsiXmlElement)this, "__queryName");
                ICsiXmlElement csiXmlElement = (ICsiXmlElement)new CsiDataField(this.GetOwnerDocument(), "__queryName", (ICsiXmlElement)this, queryName);
                csiXmlElement.SetAttribute("__type", "user");
                csiXmlElement.SetAttribute("__changeCount", Convert.ToString(changeCount));
            }
            catch 
            {
            }
        }
    }
}
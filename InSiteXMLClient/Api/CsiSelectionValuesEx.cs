using Camstar.XMLClient.Interface;
using System;
using System.Xml;

namespace Camstar.XMLClient.API
{
    internal class CsiSelectionValuesEx : CsiXmlElement, ICsiSelectionValuesEx, ICsiXmlElement
    {
        public CsiSelectionValuesEx(ICsiDocument doc, ICsiXmlElement parent)
          : base(doc, "__selectionValuesEx", parent)
        {
        }

        public CsiSelectionValuesEx(ICsiDocument doc, XmlElement element)
          : base(doc, element)
        {
        }

        public virtual ICsiRecordset GetRecordset()
        {
            try
            {
                return this.FindChildByName("__recordSet") as ICsiRecordset;
            }
            catch 
            {
                return null;
            }
        }

        public virtual ICsiRecordsetHeader GetRecordsetHeader()
        {
            try
            {
                return this.FindChildByName("__recordSetHeader") as ICsiRecordsetHeader;
            }
            catch 
            {
                return null;
            }
        }

        public virtual long GetRecordCount()
        {
            long num = 0;
            if (this.FindChildByName("__responseData") is CsiDataField childByName)
            {
                ICsiDataField count = childByName.FindChildByName("__recordCount") as ICsiDataField;
                try
                {
                    num = long.Parse(count?.GetValue());
                }
                catch 
                {
                }
            }
            return num;
        }
    }
}
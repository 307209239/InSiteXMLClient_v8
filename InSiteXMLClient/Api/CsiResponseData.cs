using Camstar.XMLClient.Interface;
using System;
using System.Xml;

namespace Camstar.XMLClient.API
{
    internal class CsiResponseData : CsiXmlElement, ICsiResponseData, ICsiXmlElement
    {
        public CsiResponseData(ICsiDocument doc, XmlElement domElement)
          : base(doc, domElement)
        {
        }

        public CsiResponseData(ICsiDocument doc, ICsiXmlElement oParent)
          : base(doc, "__responseData", oParent)
        {
        }

        public virtual Array GetResponseFields() => this.GetAllChildren(true);

        public virtual ICsiSubentity GetSessionValues() => this.FindChildByName("__sessionValues") as ICsiSubentity;

        public ICsiField GetResponseFieldByName(string fieldName) => this.FindChildByName(fieldName) as ICsiField;
    }
}
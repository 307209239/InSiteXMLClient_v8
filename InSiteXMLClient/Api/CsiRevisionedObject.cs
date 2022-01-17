using Camstar.XMLClient.Interface;
using System.Xml;

namespace Camstar.XMLClient.API
{
    internal class CsiRevisionedObject :
      CsiObject,
      ICsiRevisionedObject,
      ICsiObject,
      ICsiField,
      ICsiXmlElement
    {
        public CsiRevisionedObject(ICsiDocument doc, string name, ICsiXmlElement parent)
          : base(doc, name, parent)
        {
        }

        public CsiRevisionedObject(ICsiDocument doc, XmlElement domElement)
          : base(doc, domElement)
        {
        }

        public override bool IsRevisionedObject() => true;

        public virtual string GetName() => this.FindChildByName("__name") is CsiXmlElement childByName ? CsiXmlHelper.GetFirstTextNodeValue(childByName) : string.Empty;

        public virtual string GetRevision() => this.FindChildByName("__rev") is CsiXmlElement childByName ? CsiXmlHelper.GetFirstTextNodeValue(childByName) : string.Empty;

        public virtual bool GetUseRor() => this.FindChildByName("__useROR") is CsiXmlElement childByName && CsiXmlHelper.GetFirstTextNodeValue(childByName).Equals("true");

        public void SetRef(string name, string revision, bool useROR)
        {
            CsiXmlHelper.FindCreateSetValue((ICsiXmlElement)this, "__name", name, true);
            if (!useROR && revision != null)
                CsiXmlHelper.FindCreateSetValue((ICsiXmlElement)this, "__rev", revision, true);
            CsiXmlHelper.FindCreateSetValue((ICsiXmlElement)this, "__useROR", useROR ? "true" : "false");
        }

        public void GetRef(out string name, out string revision, out bool useROR)
        {
            name = this.GetName();
            revision = this.GetRevision();
            useROR = this.GetUseRor();
        }
    }
}
using System.Xml;
using Camstar.XMLClient.Interface;

namespace Camstar.XMLClient.API
{
  internal class CsiNamedObject : 
    CsiObject,
    ICsiNamedObject,
    ICsiObject,
    ICsiField,
    ICsiXmlElement
  {
    public CsiNamedObject(ICsiDocument doc, string name, ICsiXmlElement parent)
      : base(doc, name, parent)
    {
    }

    public CsiNamedObject(ICsiDocument doc, XmlElement domElement)
      : base(doc, domElement)
    {
    }

    public override bool IsNamedObject() => true;

    public virtual string GetRef() => CsiXmlHelper.GetFirstTextNodeValue(this.FindChildByName("__name") as CsiXmlElement);

    public virtual void SetRef(string val) => CsiXmlHelper.FindCreateSetValue((ICsiXmlElement) this, "__name", val, true);
  }
}

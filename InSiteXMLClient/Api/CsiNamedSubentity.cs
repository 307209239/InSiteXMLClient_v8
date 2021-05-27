using System.Xml;
using Camstar.XMLClient.Interface;

namespace Camstar.XMLClient.API
{
  internal class CsiNamedSubentity : 
    CsiObject,
    ICsiNamedSubentity,
    ICsiObject,
    ICsiField,
    ICsiXmlElement
  {
    public CsiNamedSubentity(ICsiDocument doc, string name, ICsiXmlElement parent)
      : base(doc, name, parent)
    {
    }

    public CsiNamedSubentity(ICsiDocument doc, XmlElement domElement)
      : base(doc, domElement)
    {
    }

    public override bool IsNamedSubentity() => true;

    public virtual string GetName() => CsiXmlHelper.GetFirstTextNodeValue(this.FindChildByName("__name") as CsiXmlElement);

    public virtual void SetName(string name) => CsiXmlHelper.FindCreateSetValue((ICsiXmlElement) this, "__name", name, true);

    public virtual ICsiParentInfo GetParentInfo() => this.FindChildByName("__parent") as ICsiParentInfo;

    public virtual void SetParentId(string parentId) => CsiXmlHelper.FindCreateSetValue2((ICsiXmlElement) this, "__parent", "__Id", parentId);

    public ICsiParentInfo ParentInfo() => this.GetParentInfo() ?? (ICsiParentInfo) new CsiParentInfo(this.GetOwnerDocument(), "__parent", (ICsiXmlElement) this);
  }
}

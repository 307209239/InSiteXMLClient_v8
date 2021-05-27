using Camstar.Util;
using System.Xml;
using Camstar.XMLClient.Interface;

namespace Camstar.XMLClient.API
{
  internal class CsiContainer : CsiObject, ICsiContainer, ICsiObject, ICsiField, ICsiXmlElement
  {
    public CsiContainer(ICsiDocument doc, string name, ICsiXmlElement parent)
      : base(doc, name, parent)
    {
    }

    public CsiContainer(ICsiDocument doc, XmlElement element)
      : base(doc, element)
    {
    }

    public override bool IsContainer() => true;

    public virtual string GetLevel()
    {
      CsiXmlElement childByName1 = this.FindChildByName("__level") as CsiXmlElement;
      string str = string.Empty;
      if (childByName1 != null)
      {
        CsiXmlElement childByName2 = (CsiXmlElement) childByName1.FindChildByName("__name");
        if (childByName2 != null)
          str = CsiXmlHelper.GetFirstTextNodeValue(childByName2);
      }
      return str;
    }

    public virtual string GetName()
    {
      CsiXmlElement childByName = (CsiXmlElement) this.FindChildByName("__name");
      string str = string.Empty;
      if (childByName != null)
        str = CsiXmlHelper.GetFirstTextNodeValue(childByName);
      return str;
    }

    public void SetRef(string name, string level)
    {
      CsiXmlHelper.FindCreateSetValue((ICsiXmlElement) this, "__name", name, true);
      if (StringUtil.IsEmptyString(level))
        return;
      CsiXmlHelper.FindCreateSetValue2((ICsiXmlElement) this, "__level", "__name", level, true);
    }

    public virtual void GetRef(out string name, out string level)
    {
      name = this.GetName();
      level = this.GetLevel();
    }

    protected internal bool Equals(string name, string level)
    {
      bool flag = true;
      if (!name.Equals(this.GetName()))
        flag = false;
      if (level != null && !level.Equals(this.GetLevel()))
        flag = false;
      return flag;
    }
  }
}

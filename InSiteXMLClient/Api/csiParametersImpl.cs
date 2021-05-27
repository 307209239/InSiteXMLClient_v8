using System.Xml;
using Camstar.XMLClient.Interface;

namespace Camstar.XMLClient.API
{
  internal class CsiParameters : CsiXmlElement, ICsiParameters, ICsiXmlElement
  {
    public CsiParameters(ICsiDocument doc, string tagName, ICsiXmlElement parent)
      : base(doc, tagName, parent)
    {
      if (!tagName.Equals("__parameters") && !tagName.Equals("__queryParameters"))
      {
        string src = this.GetType().FullName + ".Constructor()";
        throw new CsiClientException(-1L, CsiXmlHelper.GetInvalidElement(tagName) + "(valid elements are: __parameters and " + "__queryParameters)", src);
      }
    }

    public CsiParameters(ICsiDocument doc, XmlElement element)
      : base(doc, element)
    {
    }

    public virtual long GetCount()
    {
      long num = 0;
      foreach (object allChild in this.GetAllChildren())
      {
        if ((allChild as CsiXmlElement).GetDomElement().NodeType == XmlNodeType.Element)
          ++num;
      }
      return num;
    }

    public virtual ICsiParameter GetParameterByName(string paramName) => this.FindChildByName("__parameter", "__name", paramName) as ICsiParameter;

    public virtual void ClearAll() => this.RemoveAllChildren();

    public virtual void SetParameter(string name, string val) => (this.GetParameterByName(name) ?? (ICsiParameter) new CsiParameter(this.GetOwnerDocument(), name, (ICsiXmlElement) this)).SetValue(val);

    public virtual void RemoveParameterByName(string parameterName)
    {
      if (!(this.GetParameterByName(parameterName) is CsiParameter parameterByName))
        throw new CsiClientException(-1L, string.Format("Parameter '{0}' does not exists.", (object) parameterName));
      this.GetDomElement().RemoveChild((XmlNode) parameterByName.GetDomElement());
    }
  }
}

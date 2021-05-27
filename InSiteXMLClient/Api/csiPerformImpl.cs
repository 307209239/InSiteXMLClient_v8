using Camstar.XMLClient.Interface;

namespace Camstar.XMLClient.API
{
  internal class CsiPerform : CsiXmlElement, ICsiPerform, ICsiXmlElement
  {
    public CsiPerform(ICsiDocument document, ICsiXmlElement parent)
      : base(document, "__perform", parent)
    {
    }

    public ICsiParameters AddParameters()
    {
      if (!(this.FindChildByName("__parameters") is ICsiParameters csiParameters))
        csiParameters = (ICsiParameters) new CsiParameters(this.GetOwnerDocument(), "__parameters", (ICsiXmlElement) this);
      return csiParameters;
    }
  }
}

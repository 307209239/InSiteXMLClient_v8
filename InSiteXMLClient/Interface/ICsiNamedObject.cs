

namespace Camstar.XMLClient.Interface
{
  public interface ICsiNamedObject : ICsiObject, ICsiField, ICsiXmlElement
  {
    string GetRef();

    void SetRef(string name);
  }
}

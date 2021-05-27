

namespace Camstar.XMLClient.Interface
{
  public interface ICsiNamedSubentity : ICsiObject, ICsiField, ICsiXmlElement
  {
    string GetName();

    void SetName(string name);

    ICsiParentInfo GetParentInfo();

    void SetParentId(string Id);

    ICsiParentInfo ParentInfo();
  }
}



namespace Camstar.XMLClient.Interface
{
  public interface ICsiContainer : ICsiObject, ICsiField, ICsiXmlElement
  {
    void GetRef(out string name, out string level);

    void SetRef(string name, string level);

    string GetName();

    string GetLevel();
  }
}

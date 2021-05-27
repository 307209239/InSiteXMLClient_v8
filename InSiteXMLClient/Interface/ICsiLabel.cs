

namespace Camstar.XMLClient.Interface
{
  public interface ICsiLabel : ICsiXmlElement
  {
    string GetDefaultValue();

    string GetValue();

    int GetLabelId();

    string GetLabelName();
  }
}

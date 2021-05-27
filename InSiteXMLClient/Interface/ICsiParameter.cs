namespace Camstar.XMLClient.Interface
{
  public interface ICsiParameter : ICsiXmlElement
  {
    string GetValue();

    void SetValue(string val);
  }
}

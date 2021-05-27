namespace Camstar.XMLClient.Interface
{
  public interface ICsiParameters : ICsiXmlElement
  {
    long GetCount();

    ICsiParameter GetParameterByName(string name);

    void ClearAll();

    void SetParameter(string name, string val);

    void RemoveParameterByName(string name);
  }
}

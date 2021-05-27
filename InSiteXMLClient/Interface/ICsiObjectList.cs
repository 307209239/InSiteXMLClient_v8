namespace Camstar.XMLClient.Interface
{
  public interface ICsiObjectList : ICsiList, ICsiField, ICsiXmlElement
  {
    void AppendItemById(string instanceId);
  }
}

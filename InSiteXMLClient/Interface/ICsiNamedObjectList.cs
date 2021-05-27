namespace Camstar.XMLClient.Interface
{
  public interface ICsiNamedObjectList : ICsiObjectList, ICsiList, ICsiField, ICsiXmlElement
  {
    ICsiNamedObject AppendItem(string name);

    void DeleteItemByName(string name);

    ICsiNamedObject ChangeItemByName(string name);

    ICsiNamedObject ChangeItemByIndex(int index);

    ICsiNamedObject GetItemByIndex(int index);

    ICsiNamedObject GetItemByName(string name);
  }
}

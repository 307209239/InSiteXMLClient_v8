
namespace Camstar.XMLClient.Interface
{
  public interface ICsiRevisionedObjectList : ICsiObjectList, ICsiList, ICsiField, ICsiXmlElement
  {
    ICsiRevisionedObject AppendItem(string name, string revision, bool useROR);

    void DeleteItemByRef(string name, string revision, bool useROR);

    ICsiRevisionedObject ChangeItemByRef(
      string name,
      string revision,
      bool useROR);

    ICsiRevisionedObject ChangeItemByIndex(int index);

    ICsiRevisionedObject GetItemByIndex(int index);

    ICsiRevisionedObject GetItemByRef(string name, string revision, bool useROR);
  }
}

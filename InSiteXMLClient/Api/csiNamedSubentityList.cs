

namespace Camstar.XMLClient.Interface
{
  public interface ICsiNamedSubentityList : ICsiObjectList, ICsiList, ICsiField, ICsiXmlElement
  {
    ICsiNamedSubentity AppendItem(string name);

    ICsiNamedSubentity DeleteItemByName(string name);

    ICsiNamedSubentity ChangeItemByName(string name);

    ICsiNamedSubentity ChangeItemByIndex(int index);

    ICsiNamedSubentity GetItemByIndex(int index);

    ICsiNamedSubentity GetItemByName(string name);
  }
}

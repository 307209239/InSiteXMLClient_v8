namespace Camstar.XMLClient.Interface
{
    public interface ICsiSubentityList : ICsiObjectList, ICsiList, ICsiField, ICsiXmlElement
    {
        ICsiSubentity AppendItem();

        ICsiSubentity ChangeItemByIndex(int index);

        ICsiSubentity GetItemByIndex(int index);
    }
}
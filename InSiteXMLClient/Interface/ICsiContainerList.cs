namespace Camstar.XMLClient.Interface
{
    public interface ICsiContainerList : ICsiObjectList, ICsiList, ICsiField, ICsiXmlElement
    {
        ICsiContainer AppendItem(string name, string level);

        void DeleteItemByRef(string name, string level);

        ICsiContainer ChangeItemByRef(string name, string level);

        ICsiContainer ChangeItemByIndex(int index);

        ICsiContainer GetItemByIndex(int index);

        ICsiContainer GetItemByRef(string name, string level);
    }
}
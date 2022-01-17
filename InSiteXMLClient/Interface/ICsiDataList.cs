namespace Camstar.XMLClient.Interface
{
    public interface ICsiDataList : ICsiList, ICsiField, ICsiXmlElement
    {
        ICsiDataField AppendItem(string valueRenamed);

        ICsiDataField DeleteItemByValue(string valueRenamed);

        ICsiDataField ChangeItemByIndex(int index, string valueRenamed);

        ICsiDataField ChangeItemByValue(string oldValue, string newValue);

        ICsiDataField GetItemByIndex(int index);
    }
}
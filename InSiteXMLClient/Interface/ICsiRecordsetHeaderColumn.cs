namespace Camstar.XMLClient.Interface
{
    public interface ICsiRecordsetHeaderColumn : ICsiXmlElement
    {
        string GetName();

        ICsiLabel GetLabel();

        ICsiFieldType GetFieldType();
    }
}
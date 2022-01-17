namespace Camstar.XMLClient.Interface
{
    public interface ICsiRecordsetField : ICsiXmlElement
    {
        string GetName();

        string GetValue();
    }
}
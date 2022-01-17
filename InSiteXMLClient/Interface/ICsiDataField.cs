using Camstar.XMLClient.Enum;

namespace Camstar.XMLClient.Interface
{
    public interface ICsiDataField : ICsiField, ICsiXmlElement
    {
        string GetValue();

        void SetValue(string val);

        void SetEncryptedValue(string val);

        bool IsEmptyValue();

        void SetFormattedValue(string val, DataFormats format);

        string GetFormattedValue(DataFormats format);

        void SetEmptyValue();
    }
}
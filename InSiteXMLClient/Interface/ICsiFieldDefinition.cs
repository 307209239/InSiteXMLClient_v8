namespace Camstar.XMLClient.Interface
{
    public interface ICsiFieldDefinition : ICsiXmlElement
    {
        string GetFieldName();

        int GetFieldId();

        ICsiLabel GetFieldLabel();

        string GetDataType();

        ICsiCdoType GetFieldCdoType();

        bool IsHidden();

        bool IsReadOnly();

        bool IsListField();

        bool IsObjectField();

        bool IsRequired();

        bool IsValueRequired();

        bool IsUserDefinedField();

        bool IsTypeUnique();

        bool OwnsObject();

        bool HasSelectionValues();
    }
}
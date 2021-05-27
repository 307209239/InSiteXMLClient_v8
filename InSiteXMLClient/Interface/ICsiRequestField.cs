
using Camstar.XMLClient.Enum;

namespace Camstar.XMLClient.Interface
{
  public interface ICsiRequestField : ICsiField, ICsiXmlElement
  {
    ICsiRequestField RequestField(string fieldName);

    void RequestSelectionValues();

    void RequestAllFields();

    void RequestAllFieldsRecursive();

    ICsiRequestField RequestListItemByIndex(
      int index,
      string fieldName,
      string CDOTypeName);

    void RequestUserDefinedFields();

    void RequestFieldDefinition();

    void RequestCdoDefinition();

    void RequestUserDefinedFieldDefinitions();

    void RequestCaption();

    ICsiRequestField RequestListItemByName(
      string name,
      string fieldName,
      string CDOTypeName);

    void RequestDefaultValue();

    ICsiRequestSelectionValuesEx RequestSelectionValuesEx();

    void SetSerializationMode(SerializationModes mode);
  }
}

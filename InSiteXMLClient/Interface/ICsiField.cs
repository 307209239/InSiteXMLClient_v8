using System;

namespace Camstar.XMLClient.Interface
{
  public interface ICsiField : ICsiXmlElement
  {
    ICsiSelectionValues GetSelectionValues();

    ICsiLabel GetCaption();

    ICsiFieldDefinition GetFieldDefinition();

    ICsiCdoDefinition GetCdoDefinition();

    ICsiField GetDefaultValue();

    ICsiSelectionValuesEx GetSelectionValuesEx();

    string GetSpecificType();

   Enum.CsiGenericTypes GetGenericType();

   Enum.CsiReferenceTypes GetReferenceType();

    Array GetFields();
  }
}

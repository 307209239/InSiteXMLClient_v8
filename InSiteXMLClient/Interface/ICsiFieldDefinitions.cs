using System;

namespace Camstar.XMLClient.Interface
{
  public interface ICsiFieldDefinitions : ICsiXmlElement
  {
    Array GetAllFieldDefinitions();

    ICsiFieldDefinition GetFieldDefinitionByName(string name);

    ICsiFieldDefinition GetFieldDefinitionById(int id);
  }
}

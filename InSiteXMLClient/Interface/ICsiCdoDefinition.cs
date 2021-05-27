

namespace Camstar.XMLClient.Interface
{
  public interface ICsiCdoDefinition
  {
    string GetCdoTypeName();

    int GetCdoTypeId();

    ICsiFieldDefinitions GetFieldDefinitions();
  }
}

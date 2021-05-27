

using Camstar.XMLClient.Enum;

namespace Camstar.XMLClient.Interface
{
  public interface ICsiFieldType : ICsiXmlElement
  {
    CsiGenericTypes GetGenericType();

    string GetSpecificType();

    CsiReferenceTypes GetReferenceType();

    string GetNodeType();
  }
}

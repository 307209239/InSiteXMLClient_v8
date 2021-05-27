using System.Xml;

namespace Camstar.XMLClient.Interface
{
  public interface ICsiExceptionData : ICsiXmlElement
  {
    int GetErrorCode();

    string GetDescription();

    string GetSource();

    string GetSystemMessage();

    int GetSeverity();

    string GetFailureContext();

    XmlNodeList GetExceptionParameters();

    string GetExceptionParameter(string tagName);
  }
}

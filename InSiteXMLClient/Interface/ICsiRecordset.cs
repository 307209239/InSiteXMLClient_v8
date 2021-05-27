using System;

namespace Camstar.XMLClient.Interface
{
  public interface ICsiRecordset : ICsiXmlElement
  {
    long GetRecordCount();

    Array GetFields();

    void MoveFirst();

    void MoveLast();

    void MoveNext();

    void MovePrevious();
  }
}

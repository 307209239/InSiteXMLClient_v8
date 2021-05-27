using System;
using Camstar.XMLClient.Enum;

namespace Camstar.XMLClient.Interface
{
  public interface ICsiList : ICsiField, ICsiXmlElement
  {
    void SetListAction(ListActions action);

    Array GetListItems();

    void SetProxyField(string name);

    void DeleteItemByIndex(int index);
  }
}

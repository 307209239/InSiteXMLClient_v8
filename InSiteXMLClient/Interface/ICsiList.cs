using Camstar.XMLClient.Enum;
using System;

namespace Camstar.XMLClient.Interface
{
    public interface ICsiList : ICsiField, ICsiXmlElement
    {
        void SetListAction(ListActions action);

        ICsiXmlElement[] GetListItems();

        void SetProxyField(string name);

        void DeleteItemByIndex(int index);
    }
}
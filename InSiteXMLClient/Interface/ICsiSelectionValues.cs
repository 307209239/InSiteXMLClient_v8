using System;

namespace Camstar.XMLClient.Interface
{
    public interface ICsiSelectionValues : ICsiXmlElement
    {
        Array GetAllSelectionValues();

        ICsiSelectionValue GetSelectionValueByName(string name);
    }
}
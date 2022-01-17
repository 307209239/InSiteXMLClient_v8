using System;

namespace Camstar.XMLClient.Interface
{
    public interface ICsiRecordsetHeader : ICsiXmlElement
    {
        long GetCount();

        Array GetColumns();
    }
}
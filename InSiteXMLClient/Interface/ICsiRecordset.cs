using System;
using System.Collections.Generic;

namespace Camstar.XMLClient.Interface
{
    public interface ICsiRecordset : ICsiXmlElement
    {
        long GetRecordCount();

        IEnumerable<ICsiRecordsetField> GetFields();

        void MoveFirst();

        void MoveLast();

        bool MoveNext();

        void MovePrevious();

        System.Data.DataTable GetAsDataTable();
    }
}
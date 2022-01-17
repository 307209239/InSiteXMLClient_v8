using System;

namespace Camstar.XMLClient.Interface
{
    public interface ICsiResponseData : ICsiXmlElement
    {
        Array GetResponseFields();

        ICsiSubentity GetSessionValues();

        ICsiField GetResponseFieldByName(string fieldName);
    }
}
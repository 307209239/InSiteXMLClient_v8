using Camstar.XMLClient.Enum;
using System;

namespace Camstar.XMLClient.Interface
{
    public interface ICsiRequestData : ICsiXmlElement
    {
        void SetSerializationMode(SerializationModes mode);

        ICsiRequestField RequestField(string fieldName);

        void RequestSessionValues();

        void RequestAllFields();

        void RequestAllFieldsRecursive();

        void RequestFields(Array fields);
    }
}
namespace Camstar.XMLClient.Interface
{
    public interface ICsiService : ICsiObject, ICsiField, ICsiXmlElement
    {
        string GetUtcOffset();

        void SetUtcOffset(string offset);

        bool UseTxnGuid { set; }

        string ServiceTypeName();

        ICsiObject InputData();

        ICsiRequestData RequestData();

        ICsiResponseData ResponseData();

        ICsiExceptionData ExceptionData();

        void SetExecute();
    }
}
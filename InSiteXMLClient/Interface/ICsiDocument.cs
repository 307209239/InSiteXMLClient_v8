namespace Camstar.XMLClient.Interface
{
    public interface ICsiDocument
    {
        bool GetAlwaysReturnSelectionValues();

        void SetAlwaysReturnSelectionValues(bool alwaysReturn);

        ICsiService[] GetServices();

        ICsiQuery[] GetQueries();

        ICsiExceptionData[] GetExceptions();

        ICsiService GetService();

        ICsiQuery GetQuery();

        void BuildFromString(string xml);

        ICsiService CreateService(string serviceType);

        ICsiQuery CreateQuery();

        ICsiDocument Submit();

        bool CheckErrors();

        ICsiExceptionData ExceptionData();

        ICsiRequestData RequestData();

        ICsiResponseData ResponseData();

        string AsXml();

        string SaveRequestData(string filename, bool append);

        string SaveResponseData(string filename, bool append);

        string GetTxnGuid();
    }
}
namespace Camstar.XMLClient.Interface
{
    public interface ICsiRequestSelectionValuesEx : ICsiXmlElement
    {
        long GetResultsetSize();

        void SetResultsetSize(long size);

        long GetStartRow();

        void SetStartRow(long startRow);

        ICsiQueryParameters CreateQueryParameters();

        bool GetRequestRecordCount();

        void SetRequestRecordCount(bool val);

        void ClearParameters();

        void SetParameter(string name, string val);
    }
}
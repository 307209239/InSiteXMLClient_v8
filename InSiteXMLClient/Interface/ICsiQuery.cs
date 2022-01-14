namespace Camstar.XMLClient.Interface
{
  public interface ICsiQuery : ICsiXmlElement
  {
    ICsiRecordset GetRecordset();

    long GetRowsetSize();

    string GetSqlText();

    long GetStartRow();

    string GetQueryName();

    ICsiExceptionData ExceptionData();

    void SetUserQueryName(string queryName, long changeCount=0);

    ICsiQueryParameters GetQueryParameters();

    void SetRowsetSize(long size);

    void SetSqlText(string sql);

    void SetStartRow(long startRow);

    void SetQueryName(string queryName);

    void ClearParameters();

    void SetParameter(string name, string value);

    string GetParameter(string name);

    ICsiRecordsetHeader GetRecordsetHeader();

    long GetUserQueryChangeCount();

    bool GetRequestRecordCount();

    void SetRequestRecordCount(bool isRequested);

    long GetRecordCount();

    void SetCdoTypeId(int typeId);
  }
}

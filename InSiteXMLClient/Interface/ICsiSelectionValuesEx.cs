namespace Camstar.XMLClient.Interface
{
  public interface ICsiSelectionValuesEx : ICsiXmlElement
  {
    ICsiRecordset GetRecordset();

    ICsiRecordsetHeader GetRecordsetHeader();

    long GetRecordCount();
  }
}

using System.Collections;

namespace Camstar.XMLClient.API
{
  internal sealed class CsiConstants
  {
    public static readonly Hashtable NodeToClassMapping = new Hashtable();
    public const string mkAssemblyPrefix = "Camstar.XMLClient.API.";
    public const string mkDefaultClass = "Camstar.XMLClient.API.CsiXmlElement";

    static CsiConstants()
    {
      CsiConstants.NodeToClassMapping[ "__query"] =typeof(CsiQuery).FullName;
      CsiConstants.NodeToClassMapping[ "__recordSet"] = typeof(Camstar.XMLClient.API.CsiRecordset).FullName;
      CsiConstants.NodeToClassMapping[ "__responseData"] = typeof(Camstar.XMLClient.API.CsiResponseData).FullName;
      CsiConstants.NodeToClassMapping[ "__parameters"] = typeof(Camstar.XMLClient.API.CsiParameters).FullName;
      CsiConstants.NodeToClassMapping[ "__queryParameters"] = typeof(Camstar.XMLClient.API.CsiQueryParameters).FullName;
      CsiConstants.NodeToClassMapping[ "__parameter"] = typeof(Camstar.XMLClient.API.CsiParameter).FullName;
      CsiConstants.NodeToClassMapping[ "__service"] = typeof(Camstar.XMLClient.API.CsiService).FullName;
      CsiConstants.NodeToClassMapping[ "__exceptionData"] = typeof( Camstar.XMLClient.API.CsiExceptionData).FullName;
      CsiConstants.NodeToClassMapping[ "__selectionValue"] = typeof( Camstar.XMLClient.API.CsiSelectionValue).FullName;
      CsiConstants.NodeToClassMapping[ "__selectionValues"] = typeof( Camstar.XMLClient.API.CsiSelectionValues).FullName;
      CsiConstants.NodeToClassMapping[ "__requestData"] = typeof( Camstar.XMLClient.API.CsiRequestData).FullName;
      CsiConstants.NodeToClassMapping[ "__metadata"] = typeof( Camstar.XMLClient.API.CsiMetaData).FullName;
      CsiConstants.NodeToClassMapping[ "__CDOType"] = typeof(Camstar.XMLClient.API.CsiMetaData).FullName;
      CsiConstants.NodeToClassMapping[ "__CDOSubType"] = typeof(Camstar.XMLClient.API.CsiMetaData).FullName;
      CsiConstants.NodeToClassMapping[ "__CDOSubTypes"] = typeof(Camstar.XMLClient.API.CsiMetaData).FullName;
      CsiConstants.NodeToClassMapping[ "__CDODefinition"] = typeof(Camstar.XMLClient.API.CsiMetaData).FullName;
      CsiConstants.NodeToClassMapping[ "__label"] = typeof(Camstar.XMLClient.API.CsiMetaData).FullName;
      CsiConstants.NodeToClassMapping[ "__field"] = typeof(Camstar.XMLClient.API.CsiMetaData).FullName;
      CsiConstants.NodeToClassMapping[ "__listItem"] = typeof(Camstar.XMLClient.API.CsiDataField).FullName;
      CsiConstants.NodeToClassMapping[ "__parent"] = typeof(Camstar.XMLClient.API.CsiParentInfo).FullName;
      CsiConstants.NodeToClassMapping[ "__fieldDefs"] = typeof(Camstar.XMLClient.API.CsiMetaData).FullName;
      CsiConstants.NodeToClassMapping[ "__fieldDef"] = typeof(Camstar.XMLClient.API.CsiMetaData).FullName;
      CsiConstants.NodeToClassMapping[ "__defaultValue"] = typeof(Camstar.XMLClient.API.CsiDataField).FullName;
      CsiConstants.NodeToClassMapping[ "__requestSelectionValuesEx"] = typeof(Camstar.XMLClient.API.CsiRequestSelectionValuesEx).FullName;
      CsiConstants.NodeToClassMapping[ "__selectionValuesEx"] = typeof(Camstar.XMLClient.API.CsiSelectionValuesEx).FullName;
      CsiConstants.NodeToClassMapping[ "__recordSetHeader"] = typeof(Camstar.XMLClient.API.CsiRecordsetHeader).FullName;
      CsiConstants.NodeToClassMapping[ "__column"] = typeof(Camstar.XMLClient.API.CsiRecordsetHeaderColumn).FullName;
      CsiConstants.NodeToClassMapping[ "__fieldType"] = typeof(Camstar.XMLClient.API.CsiFieldType).FullName;
      CsiConstants.NodeToClassMapping[ "__row"] = typeof(Camstar.XMLClient.API.CsiRecordsetField).FullName;
      CsiConstants.NodeToClassMapping[ "Object"] = typeof(Camstar.XMLClient.API.CsiObject).FullName;
      CsiConstants.NodeToClassMapping[ "ObjRef"] = typeof(Camstar.XMLClient.API.CsiObject).FullName;
      CsiConstants.NodeToClassMapping[ "ObjRefList"] = typeof(Camstar.XMLClient.API.CsiObjectList).FullName;
      CsiConstants.NodeToClassMapping[ "ObjectList"] = typeof(Camstar.XMLClient.API.CsiObjectList).FullName;
      CsiConstants.NodeToClassMapping[ "NamedObject"] = typeof(Camstar.XMLClient.API.CsiNamedObject).FullName;
      CsiConstants.NodeToClassMapping[ "NamedObjRef"] = typeof(Camstar.XMLClient.API.CsiNamedObject).FullName;
      CsiConstants.NodeToClassMapping[ "NamedObjRefList"] = typeof(Camstar.XMLClient.API.CsiNamedObjectList).FullName;
      CsiConstants.NodeToClassMapping[ "NamedList"] = typeof(Camstar.XMLClient.API.CsiNamedObjectList).FullName;
      CsiConstants.NodeToClassMapping[ "RevisionObject"] = typeof(Camstar.XMLClient.API.CsiRevisionedObject).FullName;
      CsiConstants.NodeToClassMapping[ "RevObjRef"] = typeof(Camstar.XMLClient.API.CsiRevisionedObject).FullName;
      CsiConstants.NodeToClassMapping[ "RevObjRefList"] = typeof(Camstar.XMLClient.API.CsiRevisionedObjectList).FullName;
      CsiConstants.NodeToClassMapping[ "RevisionList"] = typeof(Camstar.XMLClient.API.CsiRevisionedObjectList).FullName;
      CsiConstants.NodeToClassMapping[ "ContainerObject"] = typeof(Camstar.XMLClient.API.CsiContainer).FullName;
      CsiConstants.NodeToClassMapping[ "ContainerObject"] = typeof(Camstar.XMLClient.API.CsiContainer).FullName;
      CsiConstants.NodeToClassMapping[ "ContainerObjRef"] = typeof(Camstar.XMLClient.API.CsiContainer).FullName;
      CsiConstants.NodeToClassMapping[ "ContainerObjRefList"] = typeof(Camstar.XMLClient.API.CsiContainerList).FullName;
      CsiConstants.NodeToClassMapping[ "ContainerList"] = typeof(Camstar.XMLClient.API.CsiContainerList).FullName;
      CsiConstants.NodeToClassMapping[ "Subentity"] = typeof(Camstar.XMLClient.API.CsiSubentity).FullName;
      CsiConstants.NodeToClassMapping[ "SubentityObjRef"] = typeof(Camstar.XMLClient.API.CsiSubentity).FullName;
      CsiConstants.NodeToClassMapping[ "SubentityObjRefList"] = typeof(Camstar.XMLClient.API.CsiSubentityList).FullName;
      CsiConstants.NodeToClassMapping[ "SubentityList"] = typeof(Camstar.XMLClient.API.CsiSubentityList).FullName;
      CsiConstants.NodeToClassMapping[ "NamedSubentity"] = typeof(Camstar.XMLClient.API.CsiNamedSubentity).FullName;
      CsiConstants.NodeToClassMapping[ "NamedSubentityObjRef"] = typeof(Camstar.XMLClient.API.CsiNamedSubentity).FullName;
      CsiConstants.NodeToClassMapping[ "NamedSubentityObjRefList"] = typeof(Camstar.XMLClient.API.CsiNamedSubentityList).FullName;
      CsiConstants.NodeToClassMapping[ "NamedSubentityList"] = typeof(Camstar.XMLClient.API.CsiNamedSubentityList).FullName;
      CsiConstants.NodeToClassMapping[ "DataField"] = typeof(Camstar.XMLClient.API.CsiDataField).FullName;
      CsiConstants.NodeToClassMapping[ "Integer"] = typeof(Camstar.XMLClient.API.CsiDataField).FullName;
      CsiConstants.NodeToClassMapping[ "String"] = typeof(Camstar.XMLClient.API.CsiDataField).FullName;
      CsiConstants.NodeToClassMapping[ "Float"] = typeof(Camstar.XMLClient.API.CsiDataField).FullName;
      CsiConstants.NodeToClassMapping[ "TimeStamp"] = typeof(Camstar.XMLClient.API.CsiDataField).FullName;
      CsiConstants.NodeToClassMapping[ "Currency"] = typeof(Camstar.XMLClient.API.CsiDataField).FullName;
      CsiConstants.NodeToClassMapping[ "Boolean"] = typeof(Camstar.XMLClient.API.CsiDataField).FullName;
      CsiConstants.NodeToClassMapping[ "Data"] = typeof(Camstar.XMLClient.API.CsiDataField).FullName;
      CsiConstants.NodeToClassMapping[ "DataList"] = typeof(Camstar.XMLClient.API.CsiDataList).FullName;
      CsiConstants.NodeToClassMapping[ "IntegerList"] = typeof(Camstar.XMLClient.API.CsiDataList).FullName;
      CsiConstants.NodeToClassMapping[ "StringList"] = typeof(Camstar.XMLClient.API.CsiDataList).FullName;
      CsiConstants.NodeToClassMapping[ "FloatList"] = typeof(Camstar.XMLClient.API.CsiDataList).FullName;
      CsiConstants.NodeToClassMapping[ "TimeStampList"] = typeof(Camstar.XMLClient.API.CsiDataList).FullName;
      CsiConstants.NodeToClassMapping[ "CurrencyList"] = typeof(Camstar.XMLClient.API.CsiDataList).FullName;
      CsiConstants.NodeToClassMapping[ "BooleanList"] = typeof(Camstar.XMLClient.API.CsiDataList).FullName;
      CsiConstants.NodeToClassMapping[ "__rowSetSize"] = typeof(Camstar.XMLClient.API.CsiDataField).FullName;
      CsiConstants.NodeToClassMapping[ "__startRow"] = typeof(Camstar.XMLClient.API.CsiDataField).FullName;
      CsiConstants.NodeToClassMapping[ "__requestRecordCount"] = typeof(Camstar.XMLClient.API.CsiDataField).FullName;
      CsiConstants.NodeToClassMapping[ "__recordCount"] = typeof(Camstar.XMLClient.API.CsiDataField).FullName;
      CsiConstants.NodeToClassMapping[ "__changeCount"] = typeof(Camstar.XMLClient.API.CsiDataField).FullName;
      CsiConstants.NodeToClassMapping[ "__sessionValues"] = typeof(Camstar.XMLClient.API.CsiSubentity).FullName;
      CsiConstants.NodeToClassMapping[ "__queryName"] = typeof(Camstar.XMLClient.API.CsiDataField).FullName;
      CsiConstants.NodeToClassMapping[ "__queryText"] = typeof(Camstar.XMLClient.API.CsiDataField).FullName;
      CsiConstants.NodeToClassMapping[ "__name"] = typeof(Camstar.XMLClient.API.CsiDataField).FullName;
      CsiConstants.NodeToClassMapping[ "__rev"] = typeof(Camstar.XMLClient.API.CsiDataField).FullName;
      CsiConstants.NodeToClassMapping[ "__Id"] = typeof(Camstar.XMLClient.API.CsiDataField).FullName;
      CsiConstants.NodeToClassMapping[ "__level"] = typeof(Camstar.XMLClient.API.CsiNamedObject).FullName;
      CsiConstants.NodeToClassMapping[ "__CDOTypeName"] = typeof(Camstar.XMLClient.API.CsiDataField).FullName;
      CsiConstants.NodeToClassMapping[ "__CDOTypeId"] = typeof(Camstar.XMLClient.API.CsiDataField).FullName;
      CsiConstants.NodeToClassMapping[ "__value"] = typeof(Camstar.XMLClient.API.CsiDataField).FullName;
      CsiConstants.NodeToClassMapping[ "__default"] = typeof(Camstar.XMLClient.API.CsiDataField).FullName;
      CsiConstants.NodeToClassMapping[ "__dataType"] = typeof(Camstar.XMLClient.API.CsiDataField).FullName;
      CsiConstants.NodeToClassMapping[ "__dataSourceName"] = typeof(Camstar.XMLClient.API.CsiDataField).FullName;
    }
  }
}

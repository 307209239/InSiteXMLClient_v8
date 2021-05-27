using System;
using System.Collections;

namespace Camstar.XMLClient.API
{
  public sealed class CsiClientException : Exception
  {
    private string mLongMessage = string.Empty;
    private long mErrorCode = 0;
    private static readonly Hashtable mErrorMessages = new Hashtable();
    public const long mkGeneralError = 1024;
    public const long mkInvalidCDODefID = 1025;
    public const long mkInvalidCDOFieldID = 1034;
    public const long mkObjectNotFound = 13500435;
    public const long mkAccessDenied = 13500701;
    public const long mkBadPointer = 14548995;
    public const long mkCreateObjFailed = 14549023;
    public const long mkCsiXMLClientCannotCreateANewNode = 3014676;
    public const long mkCsiXMLClientCannotCreateAnObject = 3014660;
    public const long mkCsiXMLClientCannotCreateRequestTag = 3014664;
    public const long mkCsiXMLClientCannotFindAChild = 3014661;
    public const long mkCsiXMLClientCannotMakeDeepCopy = 3014663;
    public const long mkCsiXMLClientDOMErrGetNodeNameValue = 3014656;
    public const long mkCsiXMLClientDOMErrInvalidName = 3014657;
    public const long mkCsiXMLClientDOMNoModificationAllowed = 3014659;
    public const long mkCsiXMLClientDOMUnknownErr = 3014658;
    public const long mkCsiXMLClientFailToCreateAllFieldTag = 3014666;
    public const long mkCsiXMLClientFailToCreateAnAppendItem = 3014669;
    public const long mkCsiXMLClientFailToGetAllResponseFields = 3014665;
    public const long mkCsiXMLClientFailToGetAllSelectionValuesField = 3014668;
    public const long mkCsiXMLClientFailToGetDataSourceName = 3014671;
    public const long mkCsiXMLClientFailToGetQueryName = 3014672;
    public const long mkCsiXMLClientFailToGetRowSetSize = 3014673;
    public const long mkCsiXMLClientFailToGetSqlText = 3014674;
    public const long mkCsiXMLClientFailToGetStartRow = 3014675;
    public const long mkCsiXMLClientFailToRemoveParameterNodes = 3014670;
    public const long mkCsiXMLClientInsiteTagNotFound = 3014662;
    public const long mkCsiXMLClientNamedParameterNotFound = 3014667;
    public const long mkCsiXMLClientDocumentWithSameNameExists = 3014677;
    public const long mkCsiXMLClientEmptyName = 3014678;
    public const long mkCsiXMLClientUnknownFormat = -2147467259;
    public const long mkCsiXMLClientSessionWithSameNameExists = 3014680;
    public const long mkCsiXMLClientConnectionWithSameHostPortExists = 3014681;
    public const long mkCsiXMLClientWrongParameters = 3014682;
    public const long mkCsiXMLClientWrongUserNameOrPassword = 3014683;

    public override string Message => this.mLongMessage;

    public long ErrorCode => this.mErrorCode;

    internal CsiClientException(long err, Exception exp, string src)
      : this(err, exp.Message, src)
    {
    }

    internal CsiClientException(long err, string src)
    {
      this.mErrorCode = err;
      string str = "";
      if (err != -1L)
        str = (string) CsiClientException.mErrorMessages[(object) err];
      this.mLongMessage = "(Error Code = " + err.ToString() + ", Source = " + src + "): " + str;
    }

    internal CsiClientException(long err, string desc, string src)
    {
      this.mErrorCode = err;
      this.mLongMessage = "(Error Code = " + err.ToString() + ", Source = " + src + "): " + desc;
    }

    static CsiClientException()
    {
        CsiClientException.mErrorMessages.Add((object) 1024L, (object) "Admininstrative System Error");
        CsiClientException.mErrorMessages.Add((object) 1025L, (object) "无效 CDO Definition \"#ErrorMsg.CDOID\"");
        CsiClientException.mErrorMessages.Add((object) 1034L, (object) "无效的CDO ID");
        CsiClientException.mErrorMessages.Add((object) 13500435L, (object) "对象未找到");
        CsiClientException.mErrorMessages.Add((object) 13500701L, (object) "没有权限");
        CsiClientException.mErrorMessages.Add((object) 14548995L, (object) "Bad Pointer");
        CsiClientException.mErrorMessages.Add((object) 14549023L, (object)"创建对象失败");
        CsiClientException.mErrorMessages.Add((object) 3014676L, (object)"不能创建新的DOM元素");
        CsiClientException.mErrorMessages.Add((object) 3014660L, (object)"不能创建XMLClient对象");
        CsiClientException.mErrorMessages.Add((object) 3014664L, (object) "不能创建 <__Request> 标签");
        CsiClientException.mErrorMessages.Add((object) 3014661L, (object) "Cannot find a specified child node");
        CsiClientException.mErrorMessages.Add((object) 3014663L, (object) "Cannot make a deep copy");
        CsiClientException.mErrorMessages.Add((object) 3014656L,
            (object) "Error occurs when DOM tries to get name and value of a node");
        CsiClientException.mErrorMessages.Add((object) 3014657L, (object) "无效的Name");
        CsiClientException.mErrorMessages.Add((object) 3014659L, (object) "No modification Allowed on the DOM node");
        CsiClientException.mErrorMessages.Add((object) 3014658L, (object) " xml DOM中存在未知的错误");
        CsiClientException.mErrorMessages.Add((object) 3014666L, (object) "Failed to create <__allFields/> tag");
        CsiClientException.mErrorMessages.Add((object) 3014669L, (object) "Failed to create <__listItem/> element");
        CsiClientException.mErrorMessages.Add((object) 3014665L, (object) "Failed to get all the response fields");
        CsiClientException.mErrorMessages.Add((object) 3014668L, (object) "Failed to get selection values");
        CsiClientException.mErrorMessages.Add((object) 3014671L, (object) "没有 <__dataSourceName> 标签");
        CsiClientException.mErrorMessages.Add((object) 3014672L, (object) "没有 <__queryName> 标签");
        CsiClientException.mErrorMessages.Add((object) 3014673L, (object)"没有 <__rowSetSize> 标签");
        CsiClientException.mErrorMessages.Add((object) 3014674L, (object)"没有 <__queryText> 标签");
        CsiClientException.mErrorMessages.Add((object) 3014675L, (object)"没有 <__startRow> 标签");
        CsiClientException.mErrorMessages.Add((object) 3014670L, (object) "删除 <__parameter/> 节点失败");
        CsiClientException.mErrorMessages.Add((object) 3014662L, (object) "没有 <__InSite> 标签");
        CsiClientException.mErrorMessages.Add((object) 3014667L, (object) "the specfied parameter not found!");
        CsiClientException.mErrorMessages.Add((object) 3014684L, (object) "the specfied parameter not found!");
        CsiClientException.mErrorMessages.Add((object) 3014683L,"用户名或密码错误");
        CsiClientException.mErrorMessages.Add((object) 3014680L, (object) "会话中存在一样的名臣");
        CsiClientException.mErrorMessages.Add((object) 3014677L, (object) "文档中存在一样的名称");
        CsiClientException.mErrorMessages.Add((object) 3014678L, (object) "Name为空");
        CsiClientException.mErrorMessages.Add((object) -2147467259L, (object) "未知的格式");
        CsiClientException.mErrorMessages.Add((object) 3014682L, (object) "错误的参数");
    }
  }
}

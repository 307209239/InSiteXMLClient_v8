namespace Camstar.Exceptions
{
  public class InSiteException : CamstarException
  {
    private string mErrorCode = string.Empty;
    private string mErrorDescription = string.Empty;
    private string mErrorSource = string.Empty;
    private string mErrorSystemMessage = string.Empty;
    private string mFieldName = string.Empty;
    private string mSeverity = string.Empty;

    public InSiteException(
      string errorCode,
      string errorDescription,
      string errorSource,
      string errorSystemMessage,
      string fieldName,
      string severity)
      : base(nameof (InSiteException))
    {
      this.mErrorCode = errorCode;
      this.mErrorDescription = errorDescription;
      this.mErrorSource = errorSource;
      this.mErrorSystemMessage = errorSystemMessage;
      this.mFieldName = fieldName;
      this.mSeverity = severity;
    }

    public override string Id => this.mErrorCode != null && this.mErrorCode != string.Empty ? this.mErrorCode : base.Id;

    public override string Message => this.mErrorDescription != null && this.mErrorDescription != string.Empty ? this.mErrorDescription : base.Message;

    public virtual string InSiteErrorCode => this.mErrorCode;

    public virtual string InSiteErrorDescription => this.mErrorDescription;

    public virtual string InSiteErrorSource => this.mErrorSource;

    public virtual string InSiteErrorSystemMessage => this.mErrorSystemMessage;

    public virtual string InSiteFieldName => this.mFieldName;

    public virtual string InSiteSeverity => this.mSeverity;
  }
}

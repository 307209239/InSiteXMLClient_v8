using System;
using System.Reflection;
using System.Resources;

namespace Camstar.Exceptions
{
  public class CamstarException : ApplicationException
  {
    private ResourceManager mRM;
    private Assembly mExecAssembly;
    private string mNamespace = string.Empty;
    private string mKey = string.Empty;
    private string[] mParameters = (string[]) null;

    public CamstarException() => this.mKey = this.GetType().Name;

    public CamstarException(string[] parameters)
    {
      this.mKey = this.GetType().Name;
      this.mParameters = parameters;
    }

    public CamstarException(string key) => this.mKey = key;

    public CamstarException(string key, string parameter)
    {
      this.mKey = key;
      this.mParameters = new string[1]{ parameter };
    }

    public CamstarException(string key, string[] parameters)
    {
      this.mKey = key;
      this.mParameters = parameters;
    }

    public virtual string Id => ExceptionUtil.GetIdValue(this.ResourceManager, this.mKey);

    public virtual string Key => this.mKey;

    public override string Message => ExceptionUtil.GetMessageValue(this.ResourceManager, this.mKey, this.mParameters);

    protected virtual ResourceManager ResourceManager
    {
      get
      {
        if (this.mRM == null)
          this.mRM = new ResourceManager(ExceptionUtil.GetStringResourcesBaseName(this.Namespace), this.Assembly);
        return this.mRM;
      }
    }

    protected virtual string Namespace => this.mNamespace.Length > 0 ? this.mNamespace : this.GetType().Namespace;

    protected virtual Assembly Assembly => this.mExecAssembly != (Assembly) null ? this.mExecAssembly : Assembly.GetExecutingAssembly();

    protected void Initialize(string callerNamespace, Assembly executingAssembly)
    {
      this.mNamespace = callerNamespace;
      this.mExecAssembly = executingAssembly;
    }
  }
}

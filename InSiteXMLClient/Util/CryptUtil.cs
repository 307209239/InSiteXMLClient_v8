// Decompiled with JetBrains decompiler
// Type: Camstar.Util.CryptUtil
// Assembly: Camstar.Util, Version=8.3.7535.15629, Culture=neutral, PublicKeyToken=null
// MVID: BDD78B04-B363-4ACB-8ADC-34FD2F207CCE
// Assembly location: C:\Program Files (x86)\Camstar\InSite XML Client\Camstar.Util.dll

namespace Camstar.Util
{
  public class CryptUtil
  {
    public const string UID = "{8700F239-6C00-43e9-BA57-F2393B34D1DA}";

    public static string Encrypt(string fieldData)
    {
      string str = (string) null;
      if (fieldData != null)
        str = new RC2StringProvider().Encrypt("{8700F239-6C00-43e9-BA57-F2393B34D1DA}", fieldData);
      return str;
    }

    public static string Decrypt(string fieldData)
    {
      string str = (string) null;
      if (fieldData != null)
        str = new RC2StringProvider().Decrypt("{8700F239-6C00-43e9-BA57-F2393B34D1DA}", fieldData);
      return str;
    }

    public static void Encrypt(object fieldObj)
    {
      if (fieldObj == null)
        return;
      ServiceObject serviceObject = new ServiceObject(fieldObj);
      string data = serviceObject.GetValue("__encrypted") as string;
      if (!StringUtil.IsEmptyString(data) && data == "false")
      {
        serviceObject.SetValue("__encrypted", (object) "true");
        string strMessage = serviceObject.GetValue("Value") as string;
        RC2StringProvider rc2StringProvider = new RC2StringProvider();
        serviceObject.SetValue("Value", (object) rc2StringProvider.Encrypt("{8700F239-6C00-43e9-BA57-F2393B34D1DA}", strMessage));
      }
    }
  }
}

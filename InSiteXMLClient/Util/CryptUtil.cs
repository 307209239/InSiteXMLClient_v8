namespace Camstar.Util
{
    public class CryptUtil
    {
        public const string UID = "{8700F239-6C00-43e9-BA57-F2393B34D1DA}";

        public static string Encrypt(string fieldData)
        {
            string str = null;
            if (fieldData != null)
                str = new RC2StringProvider().Encrypt("{8700F239-6C00-43e9-BA57-F2393B34D1DA}", fieldData);
            return str;
        }

        public static string Decrypt(string fieldData)
        {
            string str = null;
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
                serviceObject.SetValue("__encrypted", "true");
                string strMessage = serviceObject.GetValue("Value") as string;
                RC2StringProvider rc2StringProvider = new RC2StringProvider();
                serviceObject.SetValue("Value", rc2StringProvider.Encrypt("{8700F239-6C00-43e9-BA57-F2393B34D1DA}", strMessage));
            }
        }
    }
}
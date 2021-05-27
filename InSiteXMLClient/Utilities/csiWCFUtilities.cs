using ServiceReference1;
using System;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace Camstar.XMLClient.API.Utilities
{
    internal class csiWCFUtilities
    {
        private const string Localhost = "localhost";

        private static string GetValueFromCamstarRegistry(
          string subkey,
          string value,
          string defaultValue)
        {
            string str1 = "SOFTWARE\\Wow6432Node\\Camstar\\";
            string str2 = "SOFTWARE\\Camstar\\";
            string str3 = (string)null;
            
            str3 = defaultValue;
            
            return string.IsNullOrWhiteSpace(str3) ? defaultValue : str3;
        }

        private static AuthenticationServiceClient AuthenticationClient(
          string host)
        {
            bool flag1 = false;
            if (string.IsNullOrEmpty(host))
                host = "localhost";
            else
                flag1 = true;
            UriBuilder uriBuilder = new UriBuilder(csiWCFUtilities.GetValueFromCamstarRegistry("Camstar InSite Common", "AuthenticationServiceUrl", string.Format("https://{0}/camstarsecurityservices/authenticationservice.svc", (object)host)));
            if (flag1)
                uriBuilder.Host = host;
            Uri uri = uriBuilder.Uri;
            bool flag2 = "https".Equals(uri.Scheme, StringComparison.OrdinalIgnoreCase);
            EndpointAddress remoteAddress = new EndpointAddress(uri, Array.Empty<AddressHeader>());
            return new AuthenticationServiceClient(flag2 ? (Binding)new BasicHttpsBinding() : (Binding)new BasicHttpBinding(), remoteAddress);
        }

        public static string LogIn(
          string userName,
          string userPassword,
          out string sessionId,
          string host)
        {
            sessionId = string.Empty;
            string str = string.Empty;
            AuthenticationServiceClient authenticationServiceClient = csiWCFUtilities.AuthenticationClient(host);
            try
            {
                var rq = new LoginFromXMLClientRequest(userName, userPassword,sessionId);
                var rp = authenticationServiceClient.LoginFromXMLClientAsync(rq).GetAwaiter().GetResult();
                var resultStatus = rp.LoginFromXMLClientResult;
                sessionId = rp.sessionGuid;
                if (resultStatus != null && !resultStatus.IsSuccess)
                    str = resultStatus.Message;
                
            }
            catch (Exception ex)
            {
                str = ex.Message;
            }
            finally
            {
                authenticationServiceClient.CloseAsync();
            }
            return str;
        }

        public static string Logout(string sessionId, string host)
        {
            AuthenticationServiceClient authenticationServiceClient = csiWCFUtilities.AuthenticationClient(host);
            string str = string.Empty;
            try
            {
                ResultStatus resultStatus = authenticationServiceClient.LogoutAsync(sessionId).GetAwaiter().GetResult();
                if (resultStatus != null && !resultStatus.IsSuccess)
                    str = resultStatus.Message;
            }
            catch (Exception ex)
            {
                str = ex.Message;
            }
            finally
            {
                authenticationServiceClient.CloseAsync();
            }
            return str;
        }
    }
}

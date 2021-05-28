using System;
using System.IO;
using System.Net;
using System.Text;

namespace Camstar.XMLClient.API.Utilities
{
    public class csiWCFUtilities
    {

        public static string LogIn(
          string userName,
          string userPassword,
          out string sessionId,
          string host, bool isSSL=true)
        {
            try
            {
                sessionId = string.Empty;
                var xml =
               $"<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:tem=\"http://tempuri.org/\"><soapenv:Header/><soapenv:Body><tem:LoginFromXMLClient><tem:userName>{userName}</tem:userName><tem:password>{userPassword}</tem:password><tem:sessionGuid></tem:sessionGuid></tem:LoginFromXMLClient></soapenv:Body></soapenv:Envelope>";
                byte[] bytes = Encoding.UTF8.GetBytes(xml);
                ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, errors) => true;
                HttpWebRequest request =isSSL? (HttpWebRequest)WebRequest.Create($"https://{host}/camstarsecurityservices/authenticationservice.svc"): (HttpWebRequest)WebRequest.Create($"http://{host}/camstarsecurityservices/authenticationservice.svc")
                        ;
                request.Method = "POST";
                request.ContentLength = bytes.Length;
                request.Headers.Add("SOAPAction", "http://tempuri.org/IAuthenticationService/LoginFromXMLClient");
                request.ContentType = "text/xml;charset=UTF-8";
                Stream reqstream = request.GetRequestStream();
                reqstream.Write(bytes, 0, bytes.Length);
                request.Timeout = 60000;
                request.Headers.Set("Pragma", "no-cache");
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Stream streamReceive = response.GetResponseStream();
                    Encoding encoding = Encoding.UTF8;

                    StreamReader streamReader = new StreamReader(streamReceive, encoding);
                    string strResult = streamReader.ReadToEnd();
                    streamReceive.Dispose();
                    streamReader.Dispose();
                    var start = strResult.IndexOf("<sessionGuid>");
                    if (start < 0)
                    {
                        return "error";
                    }
                    else
                    {
                        start += "<sessionGuid>".Length;
                    }
                    var end = strResult.IndexOf("</sessionGuid>");
                    sessionId = strResult.Substring(start, end - start);


                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return "";

        }

        public static string Logout(string sessionId, string host,bool isSSL=true)
        {
            try
            {
               
                var xml =
               $"<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:tem=\"http://tempuri.org/\"><soapenv:Header/><soapenv:Body><tem:Logout><tem:sessionGuid>{sessionId}</tem:sessionGuid></tem:Logout></soapenv:Body></soapenv:Envelope>";
                byte[] bytes = Encoding.UTF8.GetBytes(xml);
                ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, errors) => true;
                HttpWebRequest request = isSSL?(HttpWebRequest)WebRequest.Create($"https://{host}/camstarsecurityservices/authenticationservice.svc"):
                    (HttpWebRequest)WebRequest.Create($"http://{host}/camstarsecurityservices/authenticationservice.svc");
                request.Method = "POST";
                request.ContentLength = bytes.Length;
                request.Headers.Add("SOAPAction", "http://tempuri.org/IAuthenticationService/Logout");
                request.ContentType = "text/xml;charset=UTF-8";
                Stream reqstream = request.GetRequestStream();
                reqstream.Write(bytes, 0, bytes.Length);
                request.Timeout = 60000;
                request.Headers.Set("Pragma", "no-cache");
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Stream streamReceive = response.GetResponseStream();
                    Encoding encoding = Encoding.UTF8;
                    StreamReader streamReader = new StreamReader(streamReceive, encoding);
                    string strResult = streamReader.ReadToEnd();
                    streamReceive.Dispose();
                    streamReader.Dispose();
                    var s = strResult.Contains("<a:IsSuccess>true</a:IsSuccess>");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return "";
           
        }

    
}
}

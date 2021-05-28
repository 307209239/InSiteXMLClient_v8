using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using Camstar.XMLClient.Interface;
using InSiteXMLClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace InsiteXMLClient.Test
{
    [TestClass]
    public class UnitTest1
    {
        private CamstarHelper helper => new CamstarHelper("172.17.176.217", 443, "camstaradmin", "abc123..");

        [TestMethod]
        public void NDO_Add_Test()
        {
            var com = helper;
            com.CreateService("FactoryMaint");
            com.New("F1");
            var re = com.ExecuteResult();
            Console.WriteLine(re.Message);
            Assert.IsTrue(re.Status);
        }
        [TestMethod]
        public void NDO_Update_Test()
        {
            var com = helper;
            com.CreateService("FactoryMaint");
            var objectChanges = com.Changes("F1");
            objectChanges.DataField("Description").SetValue("123");
            var re = com.ExecuteResult();
            Console.WriteLine(re.Message);
            Assert.IsTrue(re.Status);
        }

        [TestMethod]
        public void Login()
        {
            var str =
                "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:tem=\"http://tempuri.org/\"><soapenv:Header/><soapenv:Body><tem:LoginFromXMLClient><tem:userName>camstaradmin</tem:userName><tem:password>17cd184d799d9e565a5917bf647259d08b40488f8b9d8b82</tem:password><tem:sessionGuid>1111111</tem:sessionGuid></tem:LoginFromXMLClient></soapenv:Body></soapenv:Envelope>";
            byte[] bytes = Encoding.UTF8.GetBytes(str);
            ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(CheckValidationResult);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://172.17.176.217/camstarsecurityservices/authenticationservice.svc");
            request.Method = "POST";
            request.ContentLength = bytes.Length;
            request.Headers.Add("SOAPAction", "http://tempuri.org/IAuthenticationService/LoginFromXMLClient");
            request.ContentType = "text/xml;charset=UTF-8";
            Stream reqstream = request.GetRequestStream();
            reqstream.Write(bytes, 0, bytes.Length);
            //设置连接超时时间
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
                var ms = Regex.Matches(strResult, "<sessionGuid>(?<name>\\S*)</sessionGuid>")?.FirstOrDefault();
                if (ms!=null)
                {
                    foreach (Group m in ms.Groups)
                    {
                        if(m.Name=="name")
                            str= m.Value;
                    }
                    Assert.IsTrue(true);
                }

            }

        }
        public bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true;
        }
    }
}

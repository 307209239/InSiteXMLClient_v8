using System;
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
    }
}

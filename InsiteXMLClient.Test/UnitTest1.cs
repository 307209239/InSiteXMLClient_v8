using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using Camstar.XMLClient.API.Utilities;
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
        public void QueryTable_Test()
        {
            var com = helper;
            var dt= com.QueryTable("select * from container where containername=?name", new Dictionary<string, string>() { { "name", "LOT01" } });
            Assert.IsTrue(dt.Rows.Count>0);
        }
        [TestMethod]
        public void QueryModel_Test()
        {
            Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
            var com = helper;
            var dt = com.Query<Container>("select * from historymainline where containername=?name",new Dictionary<string, string>(){{"name","LOT01"}});
            stopwatch.Stop();
            Console.WriteLine("数据量：" + dt.Count());
            Console.WriteLine("耗时："+ stopwatch.ElapsedMilliseconds+"ms");
            Assert.IsTrue(dt.Any());
        }
        [TestMethod]
        public void Login()
        {
           var s=  csiWCFUtilities.Logout("1111", "172.17.176.217");


        }
       
    }

    public class Container
    {
        public string ContainerName { get; set; }

        public bool IsAutoStart { get; set; }

        public int CDOTypeId { get; set; }

        public DateTime TxnDate { get; set; }


        public int Qty { get; set; }
    }
}

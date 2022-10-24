using Microsoft.VisualStudio.TestTools.UnitTesting;
using InSiteXMLClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace InSiteXMLClient.Tests
{
    [TestClass()]
    public class CamstarHelperTests
    {
        [TestMethod()]
        public void CreateQueryTest()
        {
            var helper = new CamstarHelper("192.168.18.244", 2881, "camstaradmin", "abc123..");
            //var query= helper.CreateQuery();
            // query.SetUserQueryName("mlxGetContainers");
            // var dt= query.ExecuteTable();
            var dt = helper.QueryTable("select * from container");
            Assert.IsTrue(dt.Rows.Count>0);
        }
    }
}
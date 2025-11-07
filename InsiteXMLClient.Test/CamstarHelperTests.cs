using Camstar.Util;
using InSiteXMLClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            //c325a4daa21cc13c4248b4332cc5eba89ef797b3d775c24c
            //var s = "c325a4daa21cc13c4248b4332cc5eba89ef797b3d775c24c";
            //var encryptStr = CryptUtil.Encrypt("Cam1star");
            //Assert.AreEqual(s, encryptStr);

            var helper = new CamstarHelper("", 443, "", "");
            //var query= helper.CreateQuery();
            // query.SetUserQueryName("mlxGetContainers");
            // var dt= query.ExecuteTable();
            var dt = helper.QueryTable("select top 1 * from container where containername='dsadsadzxzncjh/ewewqe'");
            Assert.IsTrue(dt.Rows.Count >= 0);
        }
    }
}
using System;
using CookComputing.XmlRpc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Odoo.Integration.Service;

namespace Odoo.Integration.Test.BaseTests
{
    [TestClass]
    public class CreateTest : OdooTestBase
    {
        [TestMethod]
        public void CreateResPartner()
        {
            var odooService = new OdooService(Connection);

            var resPartner = new XmlRpcStruct();
            resPartner.Add("name", "212 Ticaret A.Ş.");
            resPartner.Add("street", "Güneştepe Mh. Çalışkan Sk. No:7/9 Güngören - İstanbul, TR");
            resPartner.Add("active", true);
            resPartner.Add("balance", 25.50);
            resPartner.Add("opt_out", false);
            resPartner.Add("credit", 25.50);
            resPartner.Add("credit_limit", 100.000);

            var result = odooService.Create("res.partner", resPartner);
            Assert.IsNotNull(result);
        }
    }
}

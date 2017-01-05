using System;
using System.Collections;
using System.Net;
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
            resPartner.Add("name", "TwoByFour Inc.");
            resPartner.Add("street", "Güneştepe Mh. Çalışkan Sk. No:7/9 Güngören - İstanbul, TR");
            resPartner.Add("active", true);
            resPartner.Add("balance", 25.50);
            resPartner.Add("opt_out", false);
            resPartner.Add("credit", 25.50);
            resPartner.Add("credit_limit", 100.000);

            var result = odooService.Create("res.partner", resPartner);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void CreateAccountVoucher()
        {
            var odooService = new OdooService(Connection);

            var partnerFilter = new object[] { new object[] { "vat", "=", "TR1234567890" } };
            var partnerIds = odooService.Search("res.partner", partnerFilter, 0, 1);
            if (partnerIds.Length == 1)
            {
                var voucherDescription = string.Format("Card No:{0} - Card User Name:{1} - Installment:{2}",
                    "402278******4543",
                    "ABDURRAHMAN ISIK",
                    3);

                var accountJournalFilter = new object[] { new object[] { "code", "=", "GRTRY" } };
                var accountJournalFields = new string[] { "id", "code", "default_debit_account_id", "default_credit_account_id", "type" };
                var accountJournals = odooService.SearchAndRead("account.journal", accountJournalFilter, accountJournalFields);

                int accountId = 0, journalId = 0;
                foreach (var accountJournal in accountJournals)
                {
                    foreach (DictionaryEntry de in accountJournal)
                    {
                        if ((string)de.Key == "id")
                            journalId = (int)de.Value;
                        if ((string)de.Key == "default_debit_account_id")
                            accountId = int.Parse((de.Value as object[])[0].ToString());
                    }
                }
                
                var accountVoucher = new XmlRpcStruct();
                accountVoucher.Add("partner_id", partnerIds[0]);
                accountVoucher.Add("journal_id", journalId);
                accountVoucher.Add("name", voucherDescription);
                accountVoucher.Add("account_id", accountId);
                accountVoucher.Add("type", "receipt");
                accountVoucher.Add("reference", "10201713161549");
                // Amount has to be double type, decimal is not supported by odoo
                // You'll getting an exception like below
                // Exception: A parameter is of, or contains an instance of, type CookComputing.XmlRpc.XmlRpcStruct which cannot be mapped to an XML-RPC type
                accountVoucher.Add("amount", 250.85);
                accountVoucher.Add("date", DateTime.Now.ToString("yyyy-MM-dd"));

                var result = odooService.Create("account.voucher", accountVoucher);
                Assert.IsTrue(result > 0);

                var trigger = odooService.ButtonProformaVoucher("account.voucher", new int[] {result});
                Assert.IsTrue(trigger.Count > 0);
            }
        }
    }
}

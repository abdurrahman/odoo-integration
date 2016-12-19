using Microsoft.VisualStudio.TestTools.UnitTesting;
using Odoo.Integration.Service;
using Odoo.Integration.Test.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Odoo.Integration.Test.BaseTests
{
    [TestClass]
    public class OdooReadTest : OdooTestBase
    {
        [TestMethod]
        public void PartnerListTest()
        {
            var odooService = new OdooService(Connection);

            // res.partner search filter for user erp code
            // Sample erp code: TR1234567890
            var partnerFilter = new object[]
            {
                new object[] {"vat", "=", "TR1234567890"},
            };

            // res.partner for partner id
            var partnerIds = odooService.SearchAndRead("res.partner", partnerFilter, new string[] { "id", "name", "user_id", "ref" }, 0, 1);

            // account.account.type search filter
            // fields: receivable, payable
            var accountTypeFilter = new object[]
            {
                new object[] {"code", "in", new object[] { "receivable", "payable"}},
            };
            var accountTypes = odooService.SearchAndRead("account.account.type", accountTypeFilter, new string[] { "id" });

            var accountTypeIdList = new List<int>(10);
            foreach (var accountType in accountTypes)
            {
                foreach (DictionaryEntry de in accountType)
                {
                    int accountTypeId = (int)de.Value;
                    accountTypeIdList.Add(accountTypeId);
                }
            }

            // account.account search filter
            // fields: user_type
            var accountFilter = new object[]
            {
                new object[] {"user_type", "in", accountTypeIdList.ToArray() }
            };
            var accounts = odooService.SearchAndRead("account.account", accountFilter, new string[] { "id" });

            var accountIdList = new List<int>(50);
            foreach (var account in accounts)
            {
                foreach (DictionaryEntry de in account)
                {
                    int accountId = (int)de.Value;
                    accountIdList.Add(accountId);
                }
            }

            int partnerId = 0;
            foreach (var partner in partnerIds)
            {
                foreach (DictionaryEntry de in partner)
                {
                    if ((string)de.Key == "id")
                        partnerId = (int)de.Value;
                }
            }
            // account.move.line search filter
            // fields: partner_id and account_id
            var accountMoveFilter = new object[]
            {
                new object[] {"partner_id", "=", partnerId},
                new object[] {"account_id", "in", accountIdList.ToArray()}
            };
            // account.move.line select fields
            var accountMoveFields = new string[] { "name", "date", "date_maturity", "currency_id", "account_id", "amount_currency", "debit", "credit" };
            var accountMoveList = odooService.SearchAndRead("account.move.line", accountMoveFilter, accountMoveFields);

            var accountMoveModelList = new List<AccountMoveLine>(50);
            foreach (var accountMove in accountMoveList)
            {
                var collection = new Dictionary<string, object>();
                var accountMoveModel = new AccountMoveLine();
                foreach (DictionaryEntry de in accountMove)
                {
                    if (!collection.ContainsKey((string)de.Key))
                        collection.Add((string)de.Key, de.Value);
                }
                foreach (var parameter in collection)
                {
                    // We're continue if parameter value is coming null from Odoo.
                    if (parameter.Value is bool && !(bool)parameter.Value)
                        continue;

                    if (parameter.Key == "name")
                    {
                        accountMoveModel.Name = Convert.ToString(parameter.Value);
                    }
                    if (parameter.Key == "date")
                    {
                        accountMoveModel.Date = DateTime.ParseExact(parameter.Value.ToString(), "yyyy-MM-dd", System.Threading.Thread.CurrentThread.CurrentCulture);
                    }
                    if (parameter.Key == "date_maturity")
                    {
                        accountMoveModel.DateMaturity = DateTime.ParseExact(parameter.Value.ToString(), "yyyy-MM-dd", System.Threading.Thread.CurrentThread.CurrentCulture);
                    }
                    if (parameter.Key == "currency_id")
                    {
                        if (parameter.Value is object[])
                            accountMoveModel.CurrencyId = int.Parse((parameter.Value as object[])[0].ToString());
                    }
                    if (parameter.Key == "account_id")
                    {
                        if (parameter.Value is object[])
                            //var val = (parameter.Value as object[]).Length > 0 ? (parameter.Value as object[])[0] : null;
                            accountMoveModel.AccountId = int.Parse((parameter.Value as object[])[0].ToString());
                    }
                    if (parameter.Key == "amount_currency")
                    {
                        accountMoveModel.AmountCurrency = decimal.Parse(parameter.Value.ToString());
                    }
                    if (parameter.Key == "debit")
                    {
                        if (parameter.Value != null)
                            accountMoveModel.Debit = decimal.Parse(parameter.Value.ToString());
                    }
                    if (parameter.Key == "credit")
                    {
                        if (parameter.Value != null)
                            accountMoveModel.Credit = decimal.Parse(parameter.Value.ToString());
                    }
                }
                accountMoveModelList.Add(accountMoveModel);
            }

            var currencies = odooService.SearchAndRead("res.currency", null, new string[] { "id", "name" });
            var accountJournals = odooService.SearchAndRead("account.journal", null, new string[] { "id", "name" });

            var currencyList = new List<Currency>();
            foreach (var currencyType in currencies)
            {
                var collection = new Dictionary<string, object>();
                var currencyModel = new Currency();
                foreach (DictionaryEntry de in currencyType)
                {
                    if (!collection.ContainsKey((string)de.Key))
                        collection.Add((string)de.Key, de.Value);
                }
                foreach (var parameter in collection)
                {
                    if (parameter.Key == "id")
                        currencyModel.Id = (int)parameter.Value;
                    if (parameter.Key == "name")
                        currencyModel.Name = (string)parameter.Value;
                }
                currencyList.Add(currencyModel);
            }

            var accountJournalList = new List<AccountJournal>();
            foreach (var accountJournal in accountJournals)
            {
                var collection = new Dictionary<string, object>();
                var accountJournalModel = new AccountJournal();
                foreach (DictionaryEntry de in accountJournal)
                {
                    if (!collection.ContainsKey((string)de.Key))
                        collection.Add((string)de.Key, de.Value);
                }

                foreach (var parameter in collection)
                {
                    if (parameter.Key == "id")
                        accountJournalModel.Id = (int)parameter.Value;
                    if (parameter.Key == "name")
                        accountJournalModel.Name = (string)parameter.Value;
                }
                accountJournalList.Add(accountJournalModel);
            }

            var query = from c in accountMoveModelList
                join k in accountJournalList
                    on c.AccountId equals k.Id
                join t in currencyList
                    on c.CurrencyId equals t.Id
                    into cs
                from ces in cs.DefaultIfEmpty()
                select new AccountMove
                {
                    DocumentDate = c.Date,
                    ExpiryDate = c.DateMaturity,
                    DocumentType = k.Name,
                    DocumentNumber = c.Name,
                    Description = c.Name,
                    Currency = ces == null ? "TL" : ces.Name,
                    CurrencyAmount = ces == null ? (c.Debit + (c.Credit*-1)) : c.AmountCurrency
                };

            var customerMove = query.ToList();
            Assert.IsNotNull(customerMove);
        }
    }
}
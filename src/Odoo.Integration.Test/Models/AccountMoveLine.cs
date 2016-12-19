using System;

namespace Odoo.Integration.Test.Models
{
    public class AccountMoveLine
    {
        // "name", "date", "date_maturity", "currency_id", "account_id", "amount_currency", "debit", "credit"
        public string Name { get; set; }

        public DateTime Date { get; set; }

        public DateTime? DateMaturity { get; set; }

        public int CurrencyId { get; set; }

        public int AccountId { get; set; }

        public decimal AmountCurrency { get; set; }

        public decimal Debit { get; set; }

        public decimal Credit { get; set; }
    }
}
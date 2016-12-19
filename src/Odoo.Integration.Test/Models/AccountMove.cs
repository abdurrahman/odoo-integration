using System;

namespace Odoo.Integration.Test.Models
{
    public class AccountMove
    {
        public string DocumentType { get; set; }

        public DateTime DocumentDate { get; set; }

        public DateTime? ExpiryDate { get; set; }

        public string DocumentNumber { get; set; }

        public string Description { get; set; }

        public decimal CurrencyAmount { get; set; }

        public string Currency { get; set; }
    }
}
using Kros.Generators.Flattening.IntegrationTests.Domains.ValueObjects;
using System;
using System.Collections.Generic;

namespace Kros.Generators.Flattening.IntegrationTests.Domains
{

    public class Document
    {
        public long Id { get; set; }

        public Contact Supplier { get; set; }

        public Purchaser Purchaser { get; set; }

        public string DocumentNumber { get; set; }

        public DateTime IssueDate { get; set; }

        public VatPayerType VatPayerType { get; set; }

        public bool IsTotalPriceInclVat { get; set; }

        public decimal VatTotalPrice { get; set; }

        public DateTimeOffset Created { get; set; }

        public IList<DocumentItem> Items { get; set; }
    }
}

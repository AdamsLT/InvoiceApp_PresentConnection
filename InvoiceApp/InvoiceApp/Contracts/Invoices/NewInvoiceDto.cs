using InvoiceApp.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvoiceApp.Contracts.Invoices
{
    public class NewInvoiceDto
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public DateTime DateCreated { get; set; }

        public ClientType Client { get; set; }
        public ClientPayerType ClientPayer { get; set; }
        public string ClientCountry { get; set; }

        public ProviderType Provider { get; set; }
        public ProviderPayerType ProviderPayer { get; set; }
        public string ProviderCountry { get; set; }
    }
}

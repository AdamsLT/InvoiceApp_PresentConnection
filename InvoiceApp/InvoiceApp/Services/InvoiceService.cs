using InvoiceApp.Constants;
using InvoiceApp.Contracts.Invoices;
using InvoiceApp.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvoiceApp.Services
{
    public class InvoiceService : IInvoiceService
    {
        /// <summary>
        /// List of existing invoices
        /// </summary>
        private readonly IList<InvoiceDto> _invoices;

        /// <summary>
        /// Class constructor
        /// </summary>
        public InvoiceService()
        {
            _invoices = new List<InvoiceDto>();
        }

        /// <summary>
        /// Function to add a new invoice to the list
        /// </summary>
        /// <param name="newInvoice">New invoice data object</param>
        /// <returns>Newly created invoice</returns>
        public InvoiceDto Add(NewInvoiceDto newInvoice)
        {
            InvoiceDto _newInvoice = CreateInvoicePoco(newInvoice);

            _invoices.Add(_newInvoice);
            return _newInvoice;
        }

        /// <summary>
        /// Function to return a list of all invoices
        /// </summary>
        /// <returns>All existing invoices</returns>
        public IEnumerable<InvoiceDto> GetAllInvoices()
        {
            return _invoices;
        }

        /// <summary>
        /// Function to return a specific invoice by ID
        /// </summary>
        /// <param name="id">ID of the invoice</param>
        /// <returns>Located invoice</returns>
        public InvoiceDto GetById(Guid id)
        {
            return _invoices.Where(invoice => invoice.Id == id).FirstOrDefault();
        }

        /// <summary>
        /// Method to remove an invoice by ID
        /// </summary>
        /// <param name="id">ID of the invoice</param>
        public void Remove(Guid id)
        {
            var existing = _invoices.First(invoice => invoice.Id == id);
            _invoices.Remove(existing);
        }

        /// <summary>
        /// Function to determine the VAT for a specific situation
        /// </summary>
        /// <param name="clientCountryCode">ISO 3166-1 alpha-3 country code of the client</param>
        /// <param name="clientPayer">Is the client a VAT payer</param>
        /// <param name="providerCountryCode">ISO 3166-1 alpha-3 country code of the provider</param>
        /// <param name="providerPayer">Is the provider a VAT payer</param>
        /// <returns>The VAT amount for the given situation</returns>
        private decimal DetermineVAT(string clientCountryCode, ClientPayerType clientPayer, string providerCountryCode, ProviderPayerType providerPayer)
        {
            VATService determineVAT = new VATService(clientCountryCode, clientPayer, providerCountryCode, providerPayer);
            return determineVAT.FindVAT();
        }

        /// <summary>
        /// Creates a new invoice object
        /// </summary>
        /// <param name="newInvoice">New invoice object</param>
        /// <returns>A full invoice object</returns>
        private InvoiceDto CreateInvoicePoco(NewInvoiceDto newInvoice)
        {
            var _invoice = new InvoiceDto
            {
                Id = Guid.NewGuid(),
                Client = newInvoice.Client,
                DateCreated = newInvoice.DateCreated,
                ClientCountry = newInvoice.ClientCountry,
                ClientPayer = newInvoice.ClientPayer,
                Description = newInvoice.Description,
                Name = newInvoice.Name,
                Price = newInvoice.Price,
                Provider = newInvoice.Provider,
                ProviderPayer = newInvoice.ProviderPayer,
                ProviderCountry = newInvoice.ProviderCountry,
                VATAmount = DetermineVAT(newInvoice.ClientCountry, newInvoice.ClientPayer, newInvoice.ProviderCountry, newInvoice.ProviderPayer),
            };
            return _invoice;
        }
    }
}

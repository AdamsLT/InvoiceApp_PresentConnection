using InvoiceApp.Contracts.Invoices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvoiceApp.Services.Interfaces
{
    public interface IInvoiceService
    {
        IEnumerable<InvoiceDto> GetAllInvoices();
        InvoiceDto Add(NewInvoiceDto newItem);
        InvoiceDto GetById(Guid id);
        void Remove(Guid id);
    }
}

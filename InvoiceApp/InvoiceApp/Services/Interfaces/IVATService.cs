using InvoiceApp.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvoiceApp.Services.Interfaces
{
    public interface IVATService
    {
        decimal FindVAT();
    }
}

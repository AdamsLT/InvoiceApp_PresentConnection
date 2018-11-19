using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvoiceApp.Constants
{
    /// <summary>
    /// Is the client a VAT Payer
    /// </summary>
    public enum ClientPayerType
    {
        VATPayer = 0,
        NotPayer = 1
    }
}

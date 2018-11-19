using InvoiceApp.Constants;
using InvoiceApp.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvoiceApp.Services
{
    public class VATService : IVATService
    {
        /// <summary>
        /// The country code and VAT payer value for the client
        /// </summary>
        public string ClientCountryCode { get; set; }
        public ClientPayerType ClientPayer { get; set; }

        /// <summary>
        /// The country code and VAT payer value for the provider
        /// </summary>
        public string ProviderCountryCode { get; set; }
        public ProviderPayerType ProviderPayer { get; set; }

        /// <summary>
        /// Constructor for the service class
        /// </summary>
        /// <param name="clientCountryCode">ISO 3166-1 alpha-3 standard country code</param>
        /// <param name="clientPayer">Whether the client is a VAT payer</param>
        /// <param name="providerCountryCode">ISO 3166-1 alpha-3 standard country code</param>
        /// <param name="providerPayer">Whether the provider is a VAT payer</param>
        public VATService(string clientCountryCode, ClientPayerType clientPayer, string providerCountryCode, ProviderPayerType providerPayer)
        {
            ClientCountryCode = clientCountryCode;
            ClientPayer = clientPayer;
            ProviderCountryCode = providerCountryCode;
            ProviderPayer = providerPayer;
        }

        /// <summary>
        /// Determine the correct VAT for the given data about the client and provider
        /// </summary>
        /// <returns>The correct VAT value for the invoice</returns>
        public decimal FindVAT()
        {
            if (ProviderPayer == ProviderPayerType.NotPayer) //If the provider IS NOT a VAT payer
            {
                return 0;
            }
            else if (ProviderPayer == ProviderPayerType.VATPayer) //If the provider IS a VAT payer
            {
                if (!isEuropeanCountry(ClientCountryCode))
                { //If the country IS from the EU
                    return 0;
                }
                else if (ClientPayer == ClientPayerType.NotPayer && !ClientCountryCode.Equals(ProviderCountryCode))
                { //If the client IS NOT a VAT payer and both the client and provider are from DIFFERENT countries
                    return getVATbyCountry(ClientCountryCode);
                }
                else if (ClientPayer == ClientPayerType.VATPayer && !ClientCountryCode.Equals(ProviderCountryCode))
                { //If the client IS a VAT payer and both the client and provider are from DIFFERENT countries
                    return 0;
                }
                else if (ClientCountryCode.Equals(ProviderCountryCode))
                { //If the client and provider are from the SAME country
                    return getVATbyCountry(ProviderCountryCode);
                }

            }
            throw new ArgumentOutOfRangeException(); //Unknown data passed to the service
        }

        /// <summary>
        /// Method to determine if a country is from the EU
        /// </summary>
        /// <param name="countryCode">ISO 3166-1 alpha-3</param>
        /// <returns>True if the country is from the EU, false - otherwise</returns>
        private bool isEuropeanCountry(string countryCode)
        {
            switch (countryCode)
            { //European Union Countries
                case "LTU": return true;
                case "LAT": return true;
                case "EST": return true;
                case "SWE": return true;
                case "POL": return true;
                case "GER": return true;
                case "FRN": return true;
                case "SPN": return true;
                case "ITA": return true;
                case "IRE": return true;
                default: return false;
            }
        }

        /// <summary>
        /// Method to determine the VAT by country
        /// </summary>
        /// <param name="countryCode">ISO 3166-1 alpha-3</param>
        /// <returns>VAT value</returns>
        private decimal getVATbyCountry(string countryCode)
        {
            switch (countryCode)
            {
                case "LTU": return 21;
                case "LAT": return 22;
                case "EST": return 23;
                case "SWE": return 24;
                case "POL": return 25;
                case "GER": return 26;
                case "FRN": return 27;
                case "SPN": return 28;
                case "ITA": return 29;
                case "IRE": return 30;
                default: return 0;
            }
        }
    }
}

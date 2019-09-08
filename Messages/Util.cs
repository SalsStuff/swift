using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messages
{
    public class Util
    {
        public Util()
        {

        }

        /// <summary>
        /// IsAllUpper
        ///     Will return true if the string is al upper case.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public bool IsAllUpper(string input)
        {
            for (int i = 0; i < input.Length; i++)
            {
                if (Char.IsLetter(input[i]) && !Char.IsUpper(input[i]))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// IsDate
        ///     Will return true if date is in the following format YYYYMMDD
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool IsDate(Object obj)
        {
            string strDate = obj.ToString();
            try
            {
                string[] format = { "yyyyMMdd" };
                DateTime date;

                if (DateTime.TryParseExact(strDate, format, System.Globalization.CultureInfo.InvariantCulture,
                                           System.Globalization.DateTimeStyles.None, out date))
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// IsValidCcy
        ///     Will return true if the currency is an ISO 4217 currency; false otherwise
        /// </summary>
        /// <param name="ccy"></param>
        /// <returns></returns>
        public bool IsValidCcy(string ccy)
        {
            bool valid = false;

            switch(ccy)
            {
                case "AED":
                case "AFN":
                case "ALL":
                case "AMD":
                case "ANG":
                case "AOA":
                case "ARS":
                case "AUD":
                case "AWG":
                case "AZN":
                case "BAM":
                case "BBD":
                case "BDT":
                case "BGN":
                case "BHD":
                case "BIF":
                case "BMD":
                case "BND":
                case "BOB":
                case "BOV":
                case "BRL":
                case "BSD":
                case "BTN":
                case "BWP":
                case "BYR":
                case "BZD":
                case "CAD":
                case "CDF":
                case "CHE":
                case "CHF":
                case "CHW":
                case "CLF":
                case "CLP":
                case "CNY":
                case "COP":
                case "COU":
                case "CRC":
                case "CUC":
                case "CUP":
                case "CVE":
                case "CZK":
                case "DJF":
                case "DKK":
                case "DOP":
                case "DZD":
                case "EGP":
                case "ERN":
                case "ETB":
                case "EUR":
                case "FJD":
                case "FKP":
                case "GBP":
                case "GEL":
                case "GHS":
                case "GIP":
                case "GMD":
                case "GNF":
                case "GTQ":
                case "GYD":
                case "HKD":
                case "HNL":
                case "HRK":
                case "HTG":
                case "HUF":
                case "IDR":
                case "ILS":
                case "INR":
                case "IQD":
                case "IRR":
                case "ISK":
                case "JMD":
                case "JOD":
                case "KES":
                case "KGS":
                case "KHR":
                case "KMF":
                case "KPW":
                case "KRW":
                case "KWD":
                case "KYD":
                case "KZT":
                case "LAK":
                case "LBP":
                case "LKR":
                case "LRD":
                case "LSL":
                case "LYD":
                case "MAD":
                case "MDL":
                case "MGA":
                case "MKD":
                case "MMK":
                case "MNT":
                case "MOP":
                case "MRO":
                case "MUR":
                case "MVR":
                case "MWK":
                case "MXN":
                case "MXV":
                case "MYR":
                case "MZN":
                case "NAD":
                case "NGN":
                case "NIO":
                case "NOK":
                case "NPR":
                case "NZD":
                case "OMR":
                case "PAB":
                case "PEN":
                case "PGK":
                case "PHP":
                case "PKR":
                case "PLN":
                case "PY":
                case "PYG":
                case "QAR":
                case "RON":
                case "RSD":
                case "RUB":
                case "RWF":
                case "SAR":
                case "SBD":
                case "SCR":
                case "SDG":
                case "SEK":
                case "SGD":
                case "SHP":
                case "SLL":
                case "SOS":
                case "SRD":
                case "SSP":
                case "STD":
                case "SVC":
                case "SYP":
                case "SZL":
                case "THB":
                case "TJS":
                case "TMT":
                case "TND":
                case "TOP":
                case "TRY":
                case "TTD":
                case "TWD":
                case "TZS":
                case "UAH":
                case "UGX":
                case "USD":
                case "USN":
                case "UYI":
                case "UYU":
                case "UZS":
                case "VEF":
                case "VND":
                case "VUV":
                case "WST":
                case "XAF":
                case "XAG":
                case "XAU":
                case "XBA":
                case "XBB":
                case "XBC":
                case "XBD":
                case "XCD":
                case "XDR":
                case "XOF":
                case "XPD":
                case "XPF":
                case "XPT":
                case "XSU":
                case "XTS":
                case "XUA":
                case "XXX":
                case "YER":
                case "ZAR":
                case "ZMW":
                case "ZWL":
                    valid = true;
                    break;
                default:
                    valid = false;
                    break;
            }

            return valid;
        }
    }
}

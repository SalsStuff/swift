using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Database;

namespace Messages
{
    public class Util : DBUtils
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

        #region CURRENCY TABLE
        /// <summary>
        /// IsValidCcy
        ///     Will return true if the currency is an ISO 4217 currency; false otherwise
        /// </summary>
        /// <param name="ccy"></param>
        /// <returns></returns>
        public bool IsValidCcy(string ccy)
       
        {
            sqlCmdStr = "SELECT * FROM currency where currency_code = '" + ccy + "'";

            return DBExecute_Bool();
        }

        /// <summary>
        /// Returns the currency name from the currency table, given at least one of the following:
        /// currency_code and/or entity. If not supplying a value set it to "null".
        /// </summary>
        /// <param name="currency_code"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public DataTable getCurrencyNameFromCurrency(string currency_code, string entity)
        {
            bool useAnd = false;

            if (currency_code == null && entity == null)
                return null;

            sqlCmdStr = "SELECT currency_name FROM currency WHERE ";
            if(currency_code != null)
            {
                sqlCmdStr += "currency_code = '" + currency_code + "' ";
                useAnd = true;
            }

            if(entity != null)
            {
                if (useAnd == true)
                    sqlCmdStr += "and entity = '" + entity + "' ";
                else
                    sqlCmdStr += "entity = '" + entity + "' ";
            }

            return DBExecute_DT();
        }

        /// <summary>
        /// Returns the currency code from the currency table, given at least one of the following:
        /// currency_name and/or entity. If not supplying a value set it to "null".
        /// </summary>
        /// <param name="currency_name"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public DataTable getCurrencyCodeFromCurrency(string currency_name, string entity)
        {
            bool useAnd = false;

            if (currency_name == null && entity == null)
                return null;

            sqlCmdStr = "SELECT currency_code FROM currency WHERE ";
            if (currency_name != null)
            {
                sqlCmdStr += "currency_name = '" + currency_name + "' ";
                useAnd = true;
            }

            if (entity != null)
            {
                if (useAnd == true)
                    sqlCmdStr += "and entity = '" + entity + "' ";
                else
                    sqlCmdStr += "entity = '" + entity + "' ";
            }

            return DBExecute_DT();
        }

        /// <summary>
        /// Returns the entity from the currency table, given at least one of the following:
        /// currency_code and/or currency_name. If not supplying a value set it to "null".
        /// </summary>
        /// <param name="currency_code"></param>
        /// <param name="currency_name"></param>
        /// <returns></returns>
        public DataTable getEntityFromCurrency(string currency_code, string currency_name)
        {
            bool useAnd = false;

            if (currency_code == null && currency_name == null)
                return null;

            sqlCmdStr = "SELECT entity FROM currency WHERE ";
            if (currency_code != null)
            {
                sqlCmdStr += "currency_code = '" + currency_code + "' ";
                useAnd = true;
            }

            if (currency_name != null)
            {
                if (useAnd == true)
                    sqlCmdStr += "and currency_name = '" + currency_name + "' ";
                else
                    sqlCmdStr += "currency_name = '" + currency_name + "' ";
            }

            return DBExecute_DT();
        }
        #endregion

        #region COUNTRY_CODES TABLE
        /// <summary>
        /// Check to see is code is a valid alpha_2, alpha_3 or numeric code
        /// </summary>
        /// <param name="country"></param>
        /// <returns></returns>
        public bool isValidISOCountryCode(string country)
        {
            int result = 0;

            if ( int.TryParse(country, out result) == true )
                sqlCmdStr = "SELECT * FROM country_codes where numeric_code = '" + country + "'";
            else if ( country.Length == 2 )
                sqlCmdStr = "SELECT * FROM country_codes where alpha_2_code = '" + country + "'";
            else if (country.Length == 3)
                sqlCmdStr = "SELECT * FROM country_codes where alpha_3_code = '" + country + "'";

            return DBExecute_Bool();
        }

        /// <summary>
        /// Returns the country name from the country_codes table given at least one of the following:
        /// alpha_2_code, alpha_3_code and/or numeric_code. If not supplying a code set it to "null".
        /// </summary>
        /// <param name="alpha2"></param>
        /// <param name="alpha3"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        public string getCountryNameFromCountryCodes(string alpha2, string alpha3, string num)
        {
            bool useAnd = false;

            if (alpha2 == null && alpha3 == null && num == null)
                return null;

            sqlCmdStr = "SELECT country_name FROM country_codes WHERE ";
            if (alpha2 != null)
            {
                sqlCmdStr += "alpha_2_code = '" + alpha2 + "' ";
                useAnd = true;
            }

            if (alpha3 != null)
            {
                if (useAnd == true)
                    sqlCmdStr += "and alpha_3_code = '" + alpha3 + "' ";
                else
                    sqlCmdStr += "alpha_3_code = '" + alpha3 + "' ";
                useAnd = true;
            }

            if (num != null)
            {
                if (useAnd == true)
                    sqlCmdStr += "and numeric_code = '" + num + "' ";
                else
                    sqlCmdStr += "numeric_code = '" + num + "' ";
            }

            return DBExecute_String();
        }

        /// <summary>
        /// Returns the alpha_2_code from the country_codes table given at least one of the following:
        /// country_name, alpha_3_code and/or numeric_code. If not supplying a code set it to "null".
        /// </summary>
        /// <param name="countryName"></param>
        /// <param name="alpha3"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        private string getAlpha2FromCountryCodes(string countryName, string alpha3, string num)
        {
            bool useAnd = false;

            if (countryName == null && alpha3 == null && num == null)
                return null;

            sqlCmdStr = "SELECT alpha_2 FROM country_codes WHERE ";
            if (countryName != null)
            {
                sqlCmdStr += "country_name = '" + countryName + "' ";
                useAnd = true;
            }

            if (alpha3 != null)
            {
                if (useAnd == true)
                    sqlCmdStr += "and alpha_3_code = '" + alpha3 + "' ";
                else
                    sqlCmdStr += "alpha_3_code = '" + alpha3 + "' ";
                useAnd = true;
            }

            if (num != null)
            {
                if (useAnd == true)
                    sqlCmdStr += "and numeric_code = '" + num + "' ";
                else
                    sqlCmdStr += "numeric_code = '" + num + "' ";
            }

            return DBExecute_String();
        }

        /// <summary>
        /// Returns the alpha_3_code from the country_codes table given at least one of the following:
        /// country_name, alpha_2_code and/or numeric_code. If not supplying a code set it to "null".
        /// </summary>
        /// <param name="countryName"></param>
        /// <param name="alpha2"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        private string getAlpha3FromCountryCodes(string countryName, string alpha2, string num)
        {
            bool useAnd = false;

            if (countryName == null && alpha2 == null && num == null)
                return null;

            sqlCmdStr = "SELECT alpha_3 FROM country_codes WHERE ";
            if (countryName != null)
            {
                sqlCmdStr += "country_name = '" + countryName + "' ";
                useAnd = true;
            }

            if (alpha2 != null)
            {
                if (useAnd == true)
                    sqlCmdStr += "and alpha_2_code = '" + alpha2 + "' ";
                else
                    sqlCmdStr += "alpha_2_code = '" + alpha2 + "' ";
                useAnd = true;
            }

            if (num != null)
            {
                if (useAnd == true)
                    sqlCmdStr += "and numeric_code = '" + num + "' ";
                else
                    sqlCmdStr += "numeric_code = '" + num + "' ";
            }

            return DBExecute_String();
        }

        /// <summary>
        /// Returns the numeric_code from the country_codes table given at least one of the following:
        /// country_name, alpha_2_code and/or alpha_3_code. If not supplying a code set it to "null".
        /// </summary>
        /// <param name="countryName"></param>
        /// <param name="alpha2"></param>
        /// <param name="alpha3"></param>
        /// <returns></returns>
        public string getNumericFromCountryCodes(string countryName, string alpha2, string alpha3)
        {
            bool useAnd = false;

            if (countryName == null && alpha2 == null && alpha3 == null)
                return null;

            sqlCmdStr = "SELECT alpha_3 FROM country_codes WHERE ";
            if (countryName != null)
            {
                sqlCmdStr += "country_name = '" + countryName + "' ";
                useAnd = true;
            }

            if (alpha2 != null)
            {
                if (useAnd == true)
                    sqlCmdStr += "and alpha_2_code = '" + alpha2 + "' ";
                else
                    sqlCmdStr += "alpha_2_code = '" + alpha2 + "' ";
                useAnd = true;
            }

            if (alpha3 != null)
            {
                if (useAnd == true)
                    sqlCmdStr += "and alpha_3_code = '" + alpha3 + "' ";
                else
                    sqlCmdStr += "alpha_3_code = '" + alpha3 + "' ";
            }

            return DBExecute_String();
        }

        #endregion
    }
}

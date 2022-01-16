using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.Helpers
{
    public class GlobalFunctions
    {
        public static string RemoveSpecialChars(string str)
        {
            try
            {
                // Create  a string array and add the special characters you want to remove
                string[] chars = new string[] { "/", "!", "#", "$", "%", "^", "&", "*", "'", "\"", ";", "_", "(", ")", ":", "|", "[", "]", "\n", "\r" };
                //Iterate the number of times based on the String array length.
                for (int i = 0; i < chars.Length; i++)
                {
                    if (str.Contains(chars[i]))
                    {
                        str = str.Replace(chars[i], " ");
                    }
                }
                return str;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static string RemoveIlegalChars(string filaname)
        {
            //ilegal chars in filename
            try
            {
                // Create  a string array and add the special characters you want to remove
                string[] chars = new string[] { "+", "%", "/", "!", "#", "$", "^", "&", "*", "'", "\"", ";", ":", "|", "[", "]", "\n", "\r" };
                //Iterate the number of times based on the String array length.
                for (int i = 0; i < chars.Length; i++)
                {
                    if (filaname.Contains(chars[i]))
                    {
                        filaname = filaname.Replace(chars[i], "-");
                    }
                }

                return filaname;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public static string RemoveSpecialCharsFromInput(string str)
        {
            try
            {
                // Create  a string array and add the special characters you want to remove
                string[] chars = new string[] { "/", "#", "$", "%", "^", "&", "*", "{", "}", "\"", ";", "_", "(", ")", ":", "|", "[", "]", "<", ">", "\n", "\r" };
                //Iterate the number of times based on the String array length.
                for (int i = 0; i < chars.Length; i++)
                {
                    if (str.Contains(chars[i]))
                    {
                        str = str.Replace(chars[i], " ");
                    }
                }
                return str;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static string FormatCookie(string cookie)
        {
            // c=sq-AL|uic=sq-AL
            var list = cookie.Split('=', StringSplitOptions.RemoveEmptyEntries).ToList();

            return list[list.Count - 1];
        }

        public static string DateFormat = "";

        public static string GetCulture(HttpContext httpContext)
        {
            var cookie = httpContext.Request.Cookies[CookieRequestCultureProvider.DefaultCookieName];

            if (cookie != null)
            {
                var formatedCookie = FormatCookie(cookie);

                return formatedCookie;
            }
            else
            {
                // Fallback
                return "sq-AL";
            }
        }

        public static string FormatDateTimeString(DateTime dateTime, HttpContext httpContext)
        {
            var cultureString = GlobalFunctions.GetCulture(httpContext);
            var dateFormat = "";
            if (cultureString == "en-US")
            {
                dateFormat = "MM/dd/yyyy";
            }
            else if (cultureString == "en-GB")
            {
                dateFormat = "dd/MM/yyyy";
            }
            else
            {
                dateFormat = "dd.MM.yyyy";
            }

            var timeFormat = "HH:mm";
            var DateTimeFormat = dateFormat + " " + timeFormat;

            var formated = DateTime.Parse(dateTime.ToString()).ToString(DateTimeFormat);
            return formated;
        }

        public static string GetDateTimeFormat(HttpContext httpContext)
        {
            var cultureString = GlobalFunctions.GetCulture(httpContext);
            var dateFormat = "";
            if (cultureString == "en-US")
            {
                dateFormat = "MM/dd/yyyy";
            }
            else if (cultureString == "en-GB")
            {
                dateFormat = "dd/MM/yyyy";
            }
            else
            {
                dateFormat = "dd.MM.yyyy";
            }

            var timeFormat = "HH:mm";
            var DateTimeFormat = dateFormat + " " + timeFormat;

            return DateTimeFormat;
        }

        public static string GetDateTimeWithSecondsFormat(HttpContext httpContext)
        {
            var cultureString = GlobalFunctions.GetCulture(httpContext);
            var dateFormat = "";
            if (cultureString == "en-US")
            {
                dateFormat = "MM/dd/yyyy";
            }
            else if (cultureString == "en-GB")
            {
                dateFormat = "dd/MM/yyyy";
            }
            else
            {
                dateFormat = "dd.MM.yyyy";
            }

            var timeFormat = "HH:mm:ss";
            var DateTimeFormat = dateFormat + " " + timeFormat;

            return DateTimeFormat;
        }

        public static string GetDateFormat(HttpContext httpContext)
        {
            var cultureString = GlobalFunctions.GetCulture(httpContext);
            var dateFormat = "";
            if (cultureString == "en-US")
            {
                dateFormat = "MM/dd/yyyy";
            }
            else if (cultureString == "en-GB")
            {
                dateFormat = "dd/MM/yyyy";
            }
            else
            {
                dateFormat = "dd.MM.yyyy";
            }

            return dateFormat;
        }

        public static string GetSplitKey(HttpContext httpContext)
        {
            var cultureString = GlobalFunctions.GetCulture(httpContext);
            var dateFormat = "";
            if (cultureString == "en-US")
            {
                dateFormat = "";
            }
            else if (cultureString == "en-GB")
            {
                dateFormat = "/";
            }
            else
            {
                dateFormat = ".";
            }

            return dateFormat;
        }
        public static DateTime FormatDateTime(string dateTime, HttpContext httpContext)
        {
            var cultureString = GlobalFunctions.GetCulture(httpContext);
            CultureInfo culture = new CultureInfo(cultureString);
            culture.DateTimeFormat.ShortTimePattern = "HH:mm";
            var result = DateTime.Parse(dateTime, culture);
            return result;

        }

        public static string FormatDate(DateTime dateTime)
        {
            return dateTime.Day + " " + dateTime.ToString("MMM") + ", " + dateTime.Year;
        }

        public static string Encrypt(string Message, string Passphrase)
        {
            byte[] results;
            var utf8 = new UTF8Encoding();

            // Step 1. We hash the passphrase using MD5
            // We use the MD5 hash generator as the result is a 128 bit byte array
            // which is a valid length for the TripleDES encoder we use below

            var hashProvider = new MD5CryptoServiceProvider();
            byte[] TDESKey = hashProvider.ComputeHash(utf8.GetBytes(Passphrase));

            // Step 2. Create a new TripleDESCryptoServiceProvider object
            var tdesAlgorithm = new TripleDESCryptoServiceProvider();

            // Step 3. Setup the encoder
            tdesAlgorithm.Key = TDESKey;
            tdesAlgorithm.Mode = CipherMode.ECB;
            tdesAlgorithm.Padding = PaddingMode.PKCS7;

            // Step 4. Convert the input string to a byte[]
            var dataToEncrypt = utf8.GetBytes(Message);

            // Step 5. Attempt to encrypt the string
            try
            {
                var encryptor = tdesAlgorithm.CreateEncryptor();
                results = encryptor.TransformFinalBlock(dataToEncrypt, 0, dataToEncrypt.Length);
            }
            finally
            {
                // Clear the TripleDes and Hashprovider services of any sensitive information
                tdesAlgorithm.Clear();
                hashProvider.Clear();
            }

            // Step 6. Return the encrypted string as a base64 encoded string
            return Convert.ToBase64String(results);
        }

        public static string Decrypt(string CipheredMessage, string Passphrase)
        {
            byte[] results;
            var utf8 = new UTF8Encoding();

            // Step 1. We hash the passphrase using MD5
            // We use the MD5 hash generator as the result is a 128 bit byte array
            // which is a valid length for the TripleDES encoder we use below

            var hashProvider = new MD5CryptoServiceProvider();
            var tdesKey = hashProvider.ComputeHash(utf8.GetBytes(Passphrase));

            // Step 2. Create a new TripleDESCryptoServiceProvider object
            var tdesAlgorithm = new TripleDESCryptoServiceProvider();

            // Step 3. Setup the decoder
            tdesAlgorithm.Key = tdesKey;
            tdesAlgorithm.Mode = CipherMode.ECB;
            tdesAlgorithm.Padding = PaddingMode.PKCS7;

            // Step 4. Convert the input string to a byte[]
            var dataToDecrypt = Convert.FromBase64String(CipheredMessage);

            // Step 5. Attempt to decrypt the string
            try
            {
                var decryptor = tdesAlgorithm.CreateDecryptor();
                results = decryptor.TransformFinalBlock(dataToDecrypt, 0, dataToDecrypt.Length);
            }
            finally
            {
                // Clear the TripleDes and Hashssssssssssssssssssssprovider services of any sensitive information
                tdesAlgorithm.Clear();
                hashProvider.Clear();
            }

            // Step 6. Return the decrypted string in UTF8 format
            return utf8.GetString(results);
        }

        public static string GetNameOfDayOfTheWeek(DateTime date, string culture)
        {
            try
            {
                var day = (int)date.DayOfWeek;
                var str = "";
                if (culture == "sq-AL")
                {
                    if (day == 0)
                    {
                        str = "E dielë";
                    }
                    else if (day == 1)
                    {
                        str = "E hënë";
                    }
                    else if (day == 2)
                    {
                        str = "E martë";
                    }
                    else if (day == 3)
                    {
                        str = "E mërkur";
                    }
                    else if (day == 4)
                    {
                        str = "E enjte";
                    }
                    else if (day == 5)
                    {
                        str = "E premte";
                    }
                    else if (day == 6)
                    {
                        str = "E shtunë";
                    }
                }
                else
                {
                    str = date.DayOfWeek.ToString();
                }

                return str;
            }
            catch (Exception)
            {

                throw;
            }

        }
        public static string GetAlphabeticalLetter(int number)
        {
            return ((char)(64 + number)).ToString();
        }
    }
}

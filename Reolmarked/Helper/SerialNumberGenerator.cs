using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;


namespace Reolmarked.Helper
{
    public static class SerialNumberGenerator
    {
        /*
         Klasse til at genere random 10 cifret barcode.
         Vi bruger do-while loop for at undgå at vi får en string der allerede eksistere.
         */
        public static string GenerateRandomString()
        {
            using var context = DbContextFactory.CreateContext();
            Random random = new Random();
            const string chars = "0123456789";
            string serialNumber;
            bool isUnique;

            do
            {
                char[] stringChars = new char[10];

                for (int i = 0; i < 10; i++)
                {
                    stringChars[i] = chars[random.Next(chars.Length)];
                }

                serialNumber = new string(stringChars);
                isUnique = !context.Product.Any(p => p.ProductSerialNumber == serialNumber);
            } while (!isUnique);
            
                return serialNumber;

        }
    }
}

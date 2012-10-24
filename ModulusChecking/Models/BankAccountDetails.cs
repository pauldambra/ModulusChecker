using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModulusChecking.Models
{
    class BankAccountDetails
    {
        public SortCode SortCode { get; set; }
        public AccountNumber AccountNumber { get; private set; }

        /// <summary>
        /// Santander (formerly Alliance & Leicester)
        /// Replace the last digit of the sorting code with 
        /// the first digit of the account number, then use 
        /// the last eight digits of the account number only.
        /// </summary>
        /// <param name="sortCode"></param>
        /// <param name="accountNumber"></param>
        public BankAccountDetails(string sortCode, string accountNumber)
        {
            //should ignore hyphens
            accountNumber = accountNumber.Replace("-", "");
            sortCode = sortCode.Replace("-", "");
            //should ignore spaces
            accountNumber = accountNumber.Replace(" ", "");
            sortCode = sortCode.Replace(" ", "");

            switch (accountNumber.Length)
            {
                case 9:
                    var chars = sortCode.ToCharArray();
                    chars[5] = accountNumber[0];
                    sortCode = new string(chars);
                    accountNumber = accountNumber.Substring(1);
                    break;
                case 10:
                    if (IsCooperativeBankSortCode(sortCode))
                    {
                        accountNumber = accountNumber.Substring(0, 8);
                    }
                    else if (IsNatWestSortCode(sortCode))
                    {
                        accountNumber = accountNumber.Substring(2);
                    } else
                    {
                        throw new ArgumentException(string.Format("Ten Digit Account Numbers can only come from Natwest or Coop sortcodes. {0} does not appear to be either",sortCode));
                    }
                    break;
            }
        
            SortCode = new SortCode(sortCode);
            AccountNumber = new AccountNumber(accountNumber);

        }

        private static bool IsCooperativeBankSortCode(string sortCode)
        {
            return sortCode.StartsWith("08")||sortCode.StartsWith("839");
        }

        private static bool IsNatWestSortCode(string sortCode)
        {
            return sortCode.StartsWith("600") 
                || sortCode.StartsWith("606")
                || sortCode.StartsWith("601")
                || sortCode.StartsWith("609")
                || sortCode.StartsWith("830")
                || sortCode.StartsWith("602");
        }

        public String ToCombinedString()
        {
            return SortCode.ToString() + AccountNumber;
        }

        public override string ToString()
        {
            return string.Format("sc: {0} | an: {1}", SortCode, AccountNumber);
        }
    }
}

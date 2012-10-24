using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModulusChecking.Models
{
    public class BankAccountDetails
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
            if (accountNumber.Length == 9)
            {
                var chars = sortCode.ToCharArray();
                chars[5] = accountNumber[0];
                sortCode = new string(chars);
                accountNumber = accountNumber.Substring(1);
            }
            SortCode = new SortCode(sortCode);
            AccountNumber = new AccountNumber(accountNumber);

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

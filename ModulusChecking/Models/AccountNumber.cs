using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ModulusChecking.Models
{
    /// <summary>
    /// -Ten Digit Numbers-
    /// Natwest use last 8 digits of 10 or 11; ignore hyphens
    /// Coop use the first 8 digits only
    /// -Nine Digit Numbers-
    /// Santander (formerly AandL Replace the last digit of the sorting 
    /// code with the first digit of the account number, then use 
    /// the last eight digits of the account number only.
    /// -Seven Digit Numbers-
    /// prefix with a zero
    /// -Six Digit Number -
    /// prefix with two zeroes
    /// </summary>
    public class AccountNumber
    {
        private readonly string _accountNumber;

        public AccountNumber(string accountNumber)
        {
            switch (accountNumber.Length)
            {
                case 6:
                    _accountNumber = "00" + accountNumber;
                    break;
                case 7:
                    _accountNumber = "0" + accountNumber;
                    break;
                case 8:
                    _accountNumber = accountNumber;
                    break;
                case 9:
                    //argh!
                    throw new NotImplementedException();
                case 10:
                    //argh!
                    throw new NotImplementedException();
            }
            if (!Regex.IsMatch(_accountNumber, @"^[0-9]{8}$"))
            {
                throw new ArgumentException(string.Format("Couldn't format {0} into a valid account number. tried and get {1}",accountNumber,_accountNumber));
            }
        }

        public int GetExceptionFourCheckValue
        {
            get { return Int32.Parse(_accountNumber.Substring(6)); }
        }

        /// <summary>
        /// Exception Six Indicates that these sorting codes may contain foreign currency accounts which cannot be checked. 
        /// Perform the first and second checks, except:
        /// • If a = 4, 5, 6, 7 or 8, and g and h are the same, the accounts are for a foreign currency and the checks 
        /// cannot be used.
        /// </summary>
        public bool ValidateExceptionSix
        {
            get { return IntegerAt(0) >= 4 && IntegerAt(0) <= 8 && IntegerAt(6) == IntegerAt(7); }
        }

        public override string ToString()
        {
            return _accountNumber;
        }

        public int IntegerAt(int i)
        {
            return Int16.Parse(_accountNumber[i].ToString(CultureInfo.InvariantCulture));
        }
    }
}

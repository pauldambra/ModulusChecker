using System;
using System.Collections.Generic;
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
    public class AccountNumber : BankAccountPart
    {
        public AccountNumber(string accountNumber)
        {
            if (!Regex.IsMatch(accountNumber, @"^[0-9]{6,8}$"))
            {
                throw new ArgumentException(string.Format("The provided account number {0} must be a string of between six and eight digits",accountNumber));
            }
            switch (accountNumber.Length)
            {
                case 6:
                    Value = "00" + accountNumber;
                    break;
                case 7:
                    Value = "0" + accountNumber;
                    break;
                case 8:
                    Value = accountNumber;
                    break;
                default:
                    throw new ArgumentException(string.Format("the provided account number ({0}) should be 6, 7, 8 or 10 digits long not {1}",accountNumber, accountNumber.Length));
            }

        }

        public int GetExceptionFourCheckValue
        {
            get { return Int32.Parse(Value.Substring(6)); }
        }

        /// <summary>
        /// Exception Six Indicates that these sorting codes may contain foreign currency accounts which cannot be checked. 
        /// Perform the first and second checks, except:
        /// • If a = 4, 5, 6, 7 or 8, and g and h are the same, the accounts are for a foreign currency and the checks 
        /// cannot be used.
        /// This method returns true if this is a foreign currency account and therefore this account cannot be checked.
        /// </summary>
        public bool IsForeignCurrencyAccount
        {
            get { return IntegerAt(0) >= 4 && IntegerAt(0) <= 8 && IntegerAt(6) == IntegerAt(7); }
        }

        public bool ValidateExceptionTen
        {
            get
            {
                var aIsZeroOrNine = (IntegerAt(0) == 0 || IntegerAt(0) == 9);
                var bIsNine = IntegerAt(1) == 9;
                var gIsNine = IntegerAt(6) == 9;
                return (aIsZeroOrNine && bIsNine && gIsNine);
            }
        }


        public bool IsValidCouttsNumber
        {
            get { return IntegerAt(7) == 0 || IntegerAt(7) == 1 || IntegerAt(7) == 9; }
        }
    }
}

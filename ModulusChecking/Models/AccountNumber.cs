using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace ModulusChecking.Models
{
    class AccountNumber : BankAccountPart
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

        public bool ExceptionTenShouldZeroiseWeights
        {
            get
            {
                return
                (MatchFirstTwoCharacters(0, 9) || MatchFirstTwoCharacters(9, 9))
                &&
                IntegerAt(6) == 9;
            }
        }

        public bool MatchFirstTwoCharacters(int first, int second)
        {
            return IntegerAt(0) == first && IntegerAt(1) == second;
        }

        public bool IsValidCouttsNumber
        {
            get { return IntegerAt(7) == 0 || IntegerAt(7) == 1 || IntegerAt(7) == 9; }
        }

        public AccountNumber SlideFinalDigitOff()
        {
            var removeFinalDigit = Value.Substring(0,7);
            return new AccountNumber("0" + removeFinalDigit);
        }
    }
}

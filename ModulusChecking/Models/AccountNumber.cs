using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace ModulusChecking.Models
{
    internal class AccountNumber : IEquatable<AccountNumber>
    {
        private static readonly Regex _accountNumberRegex = new Regex("^[0-9]{6,8}$", RegexOptions.Compiled);

        private readonly int[] _accountNumber;

        public static AccountNumber Parse(string accountNumber)
        {
            if (!_accountNumberRegex.IsMatch(accountNumber))
            {
                throw new ArgumentException(string.Format("The provided account number {0} must be a string of between six and eight digits", accountNumber));
            }

            accountNumber = accountNumber.PadLeft(8, '0');

            var parsedAccountNumber = new int[8];
            for (var index = 0; index < accountNumber.Count(); index++)
            {
                var character = accountNumber[index];
                parsedAccountNumber[index] = int.Parse(character.ToString());
            }

            return new AccountNumber(parsedAccountNumber);
        }

        private AccountNumber(int[] accountNumber)
        {
            _accountNumber = accountNumber;
        }

        public int GetExceptionFourCheckValue
        {
            get
            {
                var checkString = string.Format("{0}{1}", _accountNumber[6], _accountNumber[7]);
                return int.Parse(checkString);
            }
        }

        /// <summary>
        /// Exception Six Indicates that these sorting codes may contain foreign currency accounts which cannot be checked. 
        /// Perform the first and second checks, except:
        /// • If a = 4, 5, 6, 7 or 8, and g and h are the same, the accounts are for a foreign currency and the checks 
        /// cannot be used.
        /// This method returns true if this is a foreign currency account and therefore this account cannot be checked.
        /// </summary>
        public bool IsForeignCurrencyAccount => 
            new[]{4,5,6,7,8}.Contains(_accountNumber[0]) 
            && _accountNumber[6] == _accountNumber[7];

        public bool ExceptionTenShouldZeroiseWeights => 
            (MatchFirstTwoCharacters(0, 9) || MatchFirstTwoCharacters(9, 9))
            && _accountNumber[6] == 9;

        private bool MatchFirstTwoCharacters(int first, int second)
        {
            return _accountNumber[0] == first && _accountNumber[1] == second;
        }

        public bool IsValidCouttsNumber => _accountNumber[7] == 0 || _accountNumber[7] == 1 || _accountNumber[7] == 9;

        /// <summary>
        /// For second exception 14 check a new account number is created by removing the final digit and prepending a 0
        /// </summary>
        public AccountNumber SlideFinalDigitOff()
        {
            var newNumber = new int[8];
            newNumber[0] = 0;
            Array.Copy(_accountNumber, 0, newNumber, 1, 7);
            return new AccountNumber(newNumber);
        }

        public int IntegerAt(int i)
        {
            return _accountNumber[i];
        }

        public override string ToString()
        {
            return string.Join("", _accountNumber.Select(el => el.ToString()).ToArray());
        }

        public bool Equals(AccountNumber other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(_accountNumber, other._accountNumber);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((AccountNumber) obj);
        }

        public override int GetHashCode()
        {
            return (_accountNumber != null ? _accountNumber.GetHashCode() : 0);
        }

        public static bool operator ==(AccountNumber left, AccountNumber right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(AccountNumber left, AccountNumber right)
        {
            return !Equals(left, right);
        }
    }
}

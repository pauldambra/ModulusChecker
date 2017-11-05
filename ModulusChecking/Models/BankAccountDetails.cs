using System;
using System.Collections.Generic;
using System.Linq;

namespace ModulusChecking.Models
{
    internal class BankAccountDetails
    {
        public static readonly int[] AisNotZeroAndGisNotNineWeights = { 0, 0, 1, 2, 5, 3, 6, 4, 8, 7, 10, 9, 3, 1 };
        public static readonly int[] AisNotZeroAndGisNineWeights = { 0, 0, 0, 0, 0, 0, 0, 0, 8, 7, 10, 9, 3, 1 };

        private IEnumerable<ModulusWeightMapping> _weightMappings;
        public SortCode SortCode { get; set; }
        public AccountNumber AccountNumber { get; }
        public bool FirstResult { get; set; }
        public bool SecondResult { get; set; }

        public IEnumerable<ModulusWeightMapping> WeightMappings
        {
            get => _weightMappings;
            set
            {
                if (value.Count() > 2)
                {
                    throw new InvalidOperationException(string.Format("a given bank details pair should have zero, one or two mappings. not {0}",value.Count()));
                }

                if (!value.Any())
                {
                    _weightMappings = value;
                    return;
                }

                var modulusWeightMappings = value as IList<ModulusWeightMapping> ?? value.ToList();

                modulusWeightMappings = ExceptionSevenPreprocessing(modulusWeightMappings);

                ExceptionEightPreProcessing(modulusWeightMappings);
                modulusWeightMappings = ExceptionTenPreProcessing(modulusWeightMappings);

                _weightMappings = modulusWeightMappings;
            }
        }

        private static string PrepareString(string value)
        {
            return value.Replace(" ", "").Replace("-", "");
        }

        public BankAccountDetails(string sortCode, string accountNumber)
        {
            accountNumber = PrepareString(accountNumber);
            sortCode = PrepareString(sortCode);

            switch (accountNumber.Length)
            {
                case 9:
                    var chars = sortCode.ToCharArray();
                    chars[5] = accountNumber[0];
                    sortCode = new string(chars);
                    accountNumber = accountNumber.Substring(1);
                    break;
                case 10:
                    if (SortCode.IsCooperativeBankSortCode(sortCode))
                    {
                        accountNumber = accountNumber.Substring(0, 8);
                    }
                    else if (SortCode.IsNatWestSortCode(sortCode))
                    {
                        accountNumber = accountNumber.Substring(2);
                    } else
                    {
                        throw new ArgumentException(string.Format("Ten Digit Account Numbers can only come from Natwest or Coop sortcodes. {0} does not appear to be either",sortCode));
                    }
                    break;
            }
        
            SortCode = new SortCode(sortCode);
            AccountNumber = AccountNumber.Parse(accountNumber);
        }

        public bool IsValidForModulusCheck()
        {
            if (WeightMappings.Any())
            {
                return true;
            }
            FirstResult = true; // details that can't be checked pass the first test
            return false;
        }

        public bool IsUncheckableForeignAccount()
        {
            if (!WeightMappings.Any()) return false;

            if (WeightMappings.First().Exception == 6 && AccountNumber.IsForeignCurrencyAccount)
            {
                FirstResult = true;
                return true;
            }

            return false;
        }

        public string ToCombinedString() => SortCode.ToString() + AccountNumber;

        public override string ToString() => $"sc: {SortCode} | an: {AccountNumber}";

        public bool IsSecondCheckRequired()
        {
            var exceptionsThatRequireSecondCheck = new List<int> {2, 9, 10, 11, 12, 13, 14};
            if (FirstResult)
            {
                return WeightMappings.Count() != 1 ||
                       !exceptionsThatRequireSecondCheck.Contains(WeightMappings.First().Exception);
            }
            return exceptionsThatRequireSecondCheck.Contains(WeightMappings.First().Exception);
        }


        private IList<ModulusWeightMapping> ExceptionSevenPreprocessing(IList<ModulusWeightMapping> mappings)
        {
            if (mappings.First().Exception != 7) return mappings;
            if (AccountNumber.IntegerAt(6) != 9) return mappings;

            return mappings.Select((m, index) => index == 0 ? ZeroiseUtoB(m) : m).ToList();
        }

        private void ExceptionEightPreProcessing(IEnumerable<ModulusWeightMapping> mappings)
        {
            if (mappings.First().Exception == 8)
            {
                SortCode = new SortCode("090126");
            }
        }

        private IList<ModulusWeightMapping> ExceptionTenPreProcessing(IList<ModulusWeightMapping> mappings)
        {
            if (mappings.First().Exception == 10 && AccountNumber.ExceptionTenShouldZeroiseWeights)
            {
                return mappings.Select((m, index) => index == 0 ? ZeroiseUtoB(m) : m).ToList();
            }
            return mappings;
        }

        private static ModulusWeightMapping ZeroiseUtoB(ModulusWeightMapping weightMapping)
        {
            return new ModulusWeightMapping(weightMapping)
            {
                WeightValues = weightMapping.WeightValues.Select((wv, index) => index < 8 ? 0 : wv).ToArray()
            };
        }

        public bool RequiresCouttsAccountCheck() => WeightMappings?.First().Exception == 14;

        public bool IsExceptionThreeAndCanSkipSecondCheck()
        {
            return WeightMappings.Second()
                                 .Exception == 3
                   && (AccountNumber.IntegerAt(2) == 6
                       || AccountNumber.IntegerAt(2) == 9);
        }

        public int[] GetExceptionTwoAlternativeWeights(int[] originalWeights)
        {
            if (AccountNumber.IntegerAt(0) != 0)
            {
                return AccountNumber.IntegerAt(6) == 9
                                              ? AisNotZeroAndGisNineWeights
                                              : AisNotZeroAndGisNotNineWeights;
            }
            return originalWeights;
        }

        public bool IsExceptionTwoAndFirstCheckPassed()
        {
            return FirstResult && WeightMappings.First().Exception == 2;
        }

        public static BankAccountDetails From(SortCode sortCode, AccountNumber accountNumber, IEnumerable<ModulusWeightMapping> weightMappings)
        {
            return new BankAccountDetails(sortCode.ToString(),
                                          accountNumber.ToString())
            {
                WeightMappings = weightMappings
            };
        }
    }
}

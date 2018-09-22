using System.Linq;
using ModulusChecking.Loaders;
using ModulusChecking.Models;
using ModulusChecking.ModulusChecks;
using ModulusChecking.Properties;

namespace ModulusChecking.Steps.Calculators
{
    internal class FirstStandardModulusElevenCalculatorExceptionFive : FirstStandardModulusTenCalculator
    {
        private readonly SortCodeSubstitution _sortCodeSubstitution;

        public FirstStandardModulusElevenCalculatorExceptionFive(SortCodeSubstitution sortCodeSubstitution)
        {
            _sortCodeSubstitution = sortCodeSubstitution;
            Modulus = 11;
        }

        public override bool Process(BankAccountDetails bankAccountDetails)
        {
            bankAccountDetails.SortCode = new SortCode(_sortCodeSubstitution.GetSubstituteSortCode(bankAccountDetails.SortCode.ToString()));
            return ProcessWeightingRule(bankAccountDetails, bankAccountDetails.WeightMappings.First());
        }

        /// For the standard check with exception 5 the checkdigit is g from the original account number.
        /// � After dividing the result by 11;
        /// - if the remainder=0 and g=0 the account number is valid
        /// - if the remainder=1 the account number is invalid
        /// - for all other remainders, take the remainder away from 11. If the number you get is the same as g 
        /// then the account number is valid.
        private new bool ProcessWeightingRule(BankAccountDetails bankAccountDetails, ModulusWeightMapping modulusWeightMapping)
        {
            var weightingSum = new StandardModulusCheck().GetModulusSum(bankAccountDetails, modulusWeightMapping);
            var remainder = weightingSum % Modulus;
            switch (remainder)
            {
                case 0 :
                    return bankAccountDetails.AccountNumber.IntegerAt(6) == 0;
                case 1 :
                    return false;
                default :
                    return Modulus - remainder == bankAccountDetails.AccountNumber.IntegerAt(6);
            }
        }
    }
}
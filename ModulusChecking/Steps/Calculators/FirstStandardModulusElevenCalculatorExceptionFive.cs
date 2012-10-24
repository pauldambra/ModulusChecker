using System;
using System.Linq;
using ModulusChecking.Models;
using ModulusChecking.ModulusChecks;
using ModulusChecking.Parsers;

namespace ModulusChecking.Steps.Calculators
{
    class FirstStandardModulusElevenCalculatorExceptionFive : FirstStandardModulusTenCalculator
    {
        public FirstStandardModulusElevenCalculatorExceptionFive()
        {
            Modulus = 11;
        }

        private readonly SortCodeSubstitution _sortCodeSubstitution = new SortCodeSubstitution();

        public override bool Process(BankAccountDetails bankAccountDetails, IModulusWeightTable modulusWeightTable)
        {
            bankAccountDetails.SortCode = new SortCode(_sortCodeSubstitution.GetSubstituteSortCode(bankAccountDetails.SortCode.ToString()));
            var weightRules = modulusWeightTable.GetRuleMappings(bankAccountDetails.SortCode).ToList();
            if (weightRules.Count != 2)
            {
                throw new Exception("Exception 5 Modulus Check should always have two weight rules.");
            }
            return ProcessWeightingRule(bankAccountDetails, weightRules.First());
        }

        /// For the standard check with exception 5 the checkdigit is g from the original account number.
        /// • After dividing the result by 11;
        /// - if the remainder=0 and g=0 the account number is valid
        /// - if the remainder=1 the account number is invalid
        /// - for all other remainders, take the remainder away from 11. If the number you get is the same as g 
        /// then the account number is valid.
        private bool ProcessWeightingRule(BankAccountDetails bankAccountDetails, IModulusWeightMapping modulusWeightMapping)
        {
            var weightingSum = new StandardModulusCheck().GetModulusSum(bankAccountDetails, modulusWeightMapping.WeightValues);
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
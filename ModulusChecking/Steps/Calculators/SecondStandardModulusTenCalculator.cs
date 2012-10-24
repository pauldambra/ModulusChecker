using System.Linq;
using ModulusChecking.Models;
using ModulusChecking.ModulusChecks;
using ModulusChecking.Parsers;

namespace ModulusChecking.Steps.Calculators
{
    public class SecondStandardModulusTenCalculator : FirstStandardModulusTenCalculator
    {
        protected readonly int[] NoMatchWeights = new[] { 0, 0, 1, 2, 5, 3, 6, 4, 8, 7, 10, 9, 3, 1 };
        protected readonly int[] OneMatchWeights = new[] { 0, 0, 0, 0, 0, 0, 0, 0, 8, 7, 10, 9, 3, 1 };

        public override bool Process(BankAccountDetails bankAccountDetails, ModulusWeights modulusWeights)
        {
            var firstRule = modulusWeights.GetRuleMappings(bankAccountDetails.SortCode).First();   
            var secondRule = modulusWeights.GetRuleMappings(bankAccountDetails.SortCode).ElementAt(1);
            bool secondResult;
 
            if (firstRule.Exception == 2 && secondRule.Exception == 9)
            {
                SetupForExceptionTwoAndNine(bankAccountDetails, secondRule);
                secondResult = ProcessWeightingRule(bankAccountDetails, secondRule);
                //may be Lloyd's TSB euro account quoted with a sterling sort code
                if (!secondResult)
                {
                    bankAccountDetails.SortCode = new SortCode("309634");
                    //load new step after change of sort code
                    secondResult = ProcessWeightingRule(bankAccountDetails,
                                                        modulusWeights
                                                            .GetRuleMappings(bankAccountDetails.SortCode)
                                                            .First());
                }
            } 
            else
            {
                secondResult = ProcessWeightingRule(bankAccountDetails,
                                                    secondRule);
            }
            return secondResult;
        }

        private void SetupForExceptionTwoAndNine(BankAccountDetails bankAccountDetails, ModulusWeightMapping secondRule)
        {
            if (bankAccountDetails.AccountNumber.IntegerAt(0) != 0)
            {
                secondRule.WeightValues = bankAccountDetails.AccountNumber.IntegerAt(6) == 9
                                              ? OneMatchWeights
                                              : NoMatchWeights;
            }
        }
    }
}
using System.Linq;
using ModulusChecking.Models;

namespace ModulusChecking.Steps.Calculators
{
    class SecondStandardModulusElevenCalculator : BaseModulusCalculator
    {
        public SecondStandardModulusElevenCalculator()
        {
            Modulus = 11;
        }

        public override bool Process(BankAccountDetails bankAccountDetails)
        {
            var firstRule = bankAccountDetails.WeightMappings.First();
            var secondRule = bankAccountDetails.WeightMappings.ElementAt((int) ModulusWeightMapping.Step.Second);

            if (firstRule.Exception == 2 && secondRule.Exception == 9)
            {
                bankAccountDetails.SecondResult = InitialSecondCheck(bankAccountDetails, secondRule);
                //may be Lloyd's TSB euro account quoted with a sterling sort code
                if (!bankAccountDetails.SecondResult)
                {
                    bankAccountDetails.SortCode = new SortCode("309634");
                    //load new step after change of sort code
                    bankAccountDetails.SecondResult = ProcessWeightingRule(bankAccountDetails, secondRule);
                }
            }
            else
            {
                bankAccountDetails.SecondResult = ProcessWeightingRule(bankAccountDetails,
                                                    secondRule);
            }
            return bankAccountDetails.SecondResult;
        }

        private bool InitialSecondCheck(BankAccountDetails bankAccountDetails, IModulusWeightMapping mapping)
        {
            var alternativeWeightMapping = new ModulusWeightMapping(mapping)
                                               {
                                                   WeightValues =
                                                       bankAccountDetails
                                                       .GetExceptionTwoAlternativeWeights(
                                                           mapping.WeightValues)
                                               };
            return ProcessWeightingRule(bankAccountDetails, alternativeWeightMapping);
        }
    }
}
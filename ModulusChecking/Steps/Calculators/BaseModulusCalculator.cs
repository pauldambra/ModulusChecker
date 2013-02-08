using System;
using System.Linq;
using ModulusChecking.Loaders;
using ModulusChecking.Models;
using ModulusChecking.ModulusChecks;

namespace ModulusChecking.Steps.Calculators
{
    abstract class BaseModulusCalculator : IStep
    {
        protected int Modulus = 10;
        public abstract bool Process(BankAccountDetails bankAccountDetails);

        protected bool ProcessWeightingRule(BankAccountDetails bankAccountDetails, IModulusWeightMapping modulusWeightMapping)
        {
            var weightingSum = new StandardModulusCheck().GetModulusSum(bankAccountDetails,modulusWeightMapping.WeightValues);
            var remainder = weightingSum%Modulus;
            return modulusWeightMapping.Exception == 4 
                       ? bankAccountDetails.AccountNumber.GetExceptionFourCheckValue == remainder
                       : remainder == 0;
        }

        protected void ValidateEnoughMappingRulesForStepCount(BankAccountDetails bankAccountDetails, ModulusWeightMapping.Step
                                                                                                  step)
        {
            if ((int) step + 1 > bankAccountDetails.WeightMappings.Count())
            {
                throw new ArgumentOutOfRangeException(
                    string.Format("This calculator is for step {0} but the provided details only have {1} mappings",
                                  step, bankAccountDetails.WeightMappings.Count()));
            }
        }
    }
}

using ModulusChecking.Models;
using ModulusChecking.ModulusChecks;
using ModulusChecking.Parsers;

namespace ModulusChecking.Steps.Calculators
{
    abstract class BaseModulusCalculator : IStep
    {
        public enum Step
        {
            First,
            Second
        }

        protected int Modulus = 10;
        public abstract bool Process(BankAccountDetails bankAccountDetails, IModulusWeightTable modulusWeightTable);

        protected bool ProcessWeightingRule(BankAccountDetails bankAccountDetails, IModulusWeightMapping modulusWeightMapping)
        {
            var weightingSum = new StandardModulusCheck().GetModulusSum(bankAccountDetails,modulusWeightMapping.WeightValues);
            var remainder = weightingSum%Modulus;
            return modulusWeightMapping.Exception == 4 
                       ? bankAccountDetails.AccountNumber.GetExceptionFourCheckValue == remainder
                       : remainder == 0;
        }
    }
}

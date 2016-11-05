using ModulusChecking.Models;
using ModulusChecking.ModulusChecks;

namespace ModulusChecking.Steps.Calculators
{
    abstract class BaseModulusCalculator
    {
        protected int Modulus = 10;
        public abstract bool Process(BankAccountDetails bankAccountDetails);

        protected bool ProcessWeightingRule(BankAccountDetails bankAccountDetails, ModulusWeightMapping modulusWeightMapping)
        {
            var weightingSum = new StandardModulusCheck().GetModulusSum(bankAccountDetails,modulusWeightMapping);
            var remainder = weightingSum%Modulus;
            return modulusWeightMapping.Exception == 4 
                       ? bankAccountDetails.AccountNumber.GetExceptionFourCheckValue == remainder
                       : remainder == 0;
        }
    }
}

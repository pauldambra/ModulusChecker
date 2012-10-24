using ModulusChecking.Models;
using ModulusChecking.Parsers;

namespace ModulusChecking.Steps.Calculators
{
    public class StandardModulusExceptionFourteenCalculator : FirstStandardModulusElevenCalculator
    {
        public override bool Process(BankAccountDetails bankAccountDetails, ModulusWeights modulusWeights)
        {
            if (!bankAccountDetails.AccountNumber.IsValidCouttsNumber)
            {
                return false;
            }
            bankAccountDetails.AccountNumber.SetElementAt(7, '0');
            return base.Process(bankAccountDetails, modulusWeights);
        }
    }
}
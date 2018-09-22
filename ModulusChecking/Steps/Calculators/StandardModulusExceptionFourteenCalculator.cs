using ModulusChecking.Models;

namespace ModulusChecking.Steps.Calculators
{
    internal class StandardModulusExceptionFourteenCalculator : FirstStandardModulusElevenCalculator
    {
        public StandardModulusExceptionFourteenCalculator(FirstStandardModulusElevenCalculatorExceptionFive firstStandardModulusElevenCalculatorExceptionFive) 
            : base(firstStandardModulusElevenCalculatorExceptionFive)
        {
        }

        public override bool Process(BankAccountDetails bankAccountDetails)
        {
            if (!bankAccountDetails.AccountNumber.IsValidCouttsNumber)
            {
                return false;
            }

            var secondCheckDetails = BankAccountDetails.From(bankAccountDetails.SortCode, 
                                                             bankAccountDetails.AccountNumber.SlideFinalDigitOff(),
                                                             bankAccountDetails.WeightMappings);

            return base.Process(secondCheckDetails);
        }
    }
}
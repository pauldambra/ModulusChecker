using System.Linq;
using ModulusChecking.Models;

namespace ModulusChecking.Steps.Calculators
{
    internal class FirstStandardModulusElevenCalculator : FirstStandardModulusTenCalculator
    {
        private readonly FirstStandardModulusElevenCalculatorExceptionFive _firstStandardModulusElevenCalculatorExceptionFive;

        public FirstStandardModulusElevenCalculator(FirstStandardModulusElevenCalculatorExceptionFive firstStandardModulusElevenCalculatorExceptionFive)
        {
            _firstStandardModulusElevenCalculatorExceptionFive = firstStandardModulusElevenCalculatorExceptionFive;
            Modulus = 11;
        }

        public override bool Process(BankAccountDetails bankAccountDetails)
        {
            return bankAccountDetails.WeightMappings.First().Exception == 5
                       ? _firstStandardModulusElevenCalculatorExceptionFive.Process(bankAccountDetails)
                       : ProcessWeightingRule(bankAccountDetails, bankAccountDetails.WeightMappings.First());
        }
    }
}
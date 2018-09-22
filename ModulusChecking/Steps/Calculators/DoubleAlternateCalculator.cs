using System.Collections.Generic;
using System.Linq;
using ModulusChecking.Models;
using ModulusChecking.ModulusChecks;

namespace ModulusChecking.Steps.Calculators
{
    internal class FirstDoubleAlternateCalculator : DoubleAlternateCalculator
    {
        public FirstDoubleAlternateCalculator(FirstDoubleAlternateCalculatorExceptionFive firstDoubleAlternateCalculatorExceptionFive)
        {
            DoubleAlternateCalculatorExceptionFive = firstDoubleAlternateCalculatorExceptionFive;
        }

        protected override int GetMappingException(IEnumerable<ModulusWeightMapping> weightMappings)
            => weightMappings.First().Exception;

        protected override int GetWeightSumForStep(BankAccountDetails bankAccountDetails)
            => new DoubleAlternateModulusCheck().GetModulusSum(
                bankAccountDetails,
                bankAccountDetails.WeightMappings.First());
    }

    internal class SecondDoubleAlternateCalculator : DoubleAlternateCalculator
    {
        public SecondDoubleAlternateCalculator(SecondDoubleAlternateCalculatorExceptionFive secondDoubleAlternateCalculatorExceptionFive)
        {
            DoubleAlternateCalculatorExceptionFive = secondDoubleAlternateCalculatorExceptionFive;
        }

        protected override int GetMappingException(IEnumerable<ModulusWeightMapping> weightMappings)
            => weightMappings.Second().Exception;

        protected override int GetWeightSumForStep(BankAccountDetails bankAccountDetails)
            => new DoubleAlternateModulusCheck().GetModulusSum(
                bankAccountDetails,
                bankAccountDetails.WeightMappings.Second());
    }

    abstract class DoubleAlternateCalculator : BaseModulusCalculator
    {
        protected DoubleAlternateCalculatorExceptionFive DoubleAlternateCalculatorExceptionFive;

        protected abstract int GetMappingException(IEnumerable<ModulusWeightMapping> weightMappings);
        protected abstract int GetWeightSumForStep(BankAccountDetails bankAccountDetails);

        public override bool Process(BankAccountDetails bankAccountDetails)
            => GetMappingException(bankAccountDetails.WeightMappings) == 5
                ? DoubleAlternateCalculatorExceptionFive.Process(bankAccountDetails)
                : (GetWeightSumForStep(bankAccountDetails) % Modulus) == 0;
    }
}
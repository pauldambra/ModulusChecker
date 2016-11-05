using System.Collections.Generic;
using System.Linq;
using ModulusChecking.Models;
using ModulusChecking.ModulusChecks;

namespace ModulusChecking.Steps.Calculators
{
    internal class FirstDoubleAlternateCalculator : DoubleAlternateCalculator
    {
        public FirstDoubleAlternateCalculator()
        {
            DoubleAlternateCalculatorExceptionFive = new FirstDoubleAlternateCalculatorExceptionFive();
        }

        public FirstDoubleAlternateCalculator(FirstDoubleAlternateCalculatorExceptionFive exceptionFive)
        {
            DoubleAlternateCalculatorExceptionFive = exceptionFive;
        }

        protected override int GetMappingException(IEnumerable<ModulusWeightMapping> weightMappings)
        {
            return weightMappings.First().Exception;
        }

        protected override int GetWeightSumForStep(BankAccountDetails bankAccountDetails)
        {
            return new DoubleAlternateModulusCheck().GetModulusSum(bankAccountDetails,
                                                            bankAccountDetails.WeightMappings
                                                                              .First());
        }
    }

    internal class SecondDoubleAlternateCalculator : DoubleAlternateCalculator
    {
        public SecondDoubleAlternateCalculator()
        {
            DoubleAlternateCalculatorExceptionFive = new SecondDoubleAlternateCalculatorExceptionFive();
        }

        public SecondDoubleAlternateCalculator(SecondDoubleAlternateCalculatorExceptionFive exceptionFive)
        {
            DoubleAlternateCalculatorExceptionFive = exceptionFive;
        }

        protected override int GetMappingException(IEnumerable<ModulusWeightMapping> weightMappings)
        {
            return weightMappings.Second().Exception;
        }

        protected override int GetWeightSumForStep(BankAccountDetails bankAccountDetails)
        {
            return new DoubleAlternateModulusCheck().GetModulusSum(bankAccountDetails,
                                                            bankAccountDetails.WeightMappings
                                                                              .Second());
        }
    }

    abstract class DoubleAlternateCalculator : BaseModulusCalculator
    {
        protected DoubleAlternateCalculatorExceptionFive DoubleAlternateCalculatorExceptionFive;

        protected abstract int GetMappingException(IEnumerable<ModulusWeightMapping> weightMappings);
        protected abstract int GetWeightSumForStep(BankAccountDetails bankAccountDetails);

        public override bool Process(BankAccountDetails bankAccountDetails)
        {
            return GetMappingException(bankAccountDetails.WeightMappings) == 5
                       ? DoubleAlternateCalculatorExceptionFive.Process(bankAccountDetails)
                       : (GetWeightSumForStep(bankAccountDetails)%Modulus) == 0;
        }
    }
}
using System;
using System.Linq;
using ModulusChecking.Loaders;
using ModulusChecking.Models;
using ModulusChecking.Steps.Calculators;

namespace ModulusChecking.Steps
{
    class SecondModulusCalculatorStep : IStep
    {
        private readonly SecondStandardModulusTenCalculator _secondStandardModulusCalculatorTenCalculator;
        private readonly SecondStandardModulusElevenCalculator _secondStandardModulusElevenCalculator;
        private readonly DoubleAlternateCalculator _doubleAlternateCalculator;
        private readonly DoubleAlternateCalculatorExceptionFive _doubleAlternateCalculatorExceptionFive;

        public SecondModulusCalculatorStep()
        {
            _secondStandardModulusCalculatorTenCalculator = new SecondStandardModulusTenCalculator();
            _secondStandardModulusElevenCalculator = new SecondStandardModulusElevenCalculator();
            _doubleAlternateCalculator = new DoubleAlternateCalculator(ModulusWeightMapping.Step.Second);
            _doubleAlternateCalculatorExceptionFive = new DoubleAlternateCalculatorExceptionFive(ModulusWeightMapping.Step.Second);
        }

        public SecondModulusCalculatorStep(SecondStandardModulusTenCalculator secondStandardModulusCalculatorTenCalculator, SecondStandardModulusElevenCalculator secondStandardModulusElevenCalculator, DoubleAlternateCalculator doubleAlternateCalculator, DoubleAlternateCalculatorExceptionFive daf)
        {
            _secondStandardModulusCalculatorTenCalculator = secondStandardModulusCalculatorTenCalculator;
            _secondStandardModulusElevenCalculator = secondStandardModulusElevenCalculator;
            _doubleAlternateCalculator = doubleAlternateCalculator;
            _doubleAlternateCalculatorExceptionFive = daf;
        }

        public virtual bool Process(BankAccountDetails bankAccountDetails)
        {
            return GetSecondModulusCheckResult(bankAccountDetails);
        }

        private bool GetSecondModulusCheckResult(BankAccountDetails bankAccountDetails)
        {
            var mapping = bankAccountDetails.WeightMappings.ElementAt(
                (int) ModulusWeightMapping.Step.Second);
            switch (mapping.Algorithm)
            {
                case ModulusAlgorithm.Mod10:
                    bankAccountDetails.SecondResult = _secondStandardModulusCalculatorTenCalculator.Process(bankAccountDetails);
                    break;
                case ModulusAlgorithm.Mod11:
                    bankAccountDetails.SecondResult = _secondStandardModulusElevenCalculator.Process(bankAccountDetails);
                    break;
                case ModulusAlgorithm.DblAl:
                    bankAccountDetails.SecondResult = mapping.Exception == 5
                                 ? _doubleAlternateCalculatorExceptionFive.Process(bankAccountDetails)
                                 : _doubleAlternateCalculator.Process(bankAccountDetails);
                    break;
                default:
                    throw new Exception("ModulusMapping had an unknown algorithm type: " + mapping.Algorithm);
            }
            return bankAccountDetails.SecondResult;
        }
    }
}
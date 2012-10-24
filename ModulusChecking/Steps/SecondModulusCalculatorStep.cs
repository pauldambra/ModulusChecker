using System;
using System.Linq;
using ModulusChecking.Models;
using ModulusChecking.Parsers;
using ModulusChecking.Steps.Calculators;

namespace ModulusChecking.Steps
{
    public class SecondModulusCalculatorStep : BaseStep
    {
        private readonly SecondStandardModulusTenCalculator _secondStandardModulusCalculatorTenCalculator;
        private readonly SecondStandardModulusCalculatorElevenCalculator _secondStandardModulusCalculatorElevenCalculator;
        private readonly DoubleAlternateCalculator _doubleAlternateCalculator;
        private readonly DoubleAlternateCalculatorExceptionFive _doubleAlternateCalculatorExceptionFive;

        public SecondModulusCalculatorStep()
        {
            _secondStandardModulusCalculatorTenCalculator = new SecondStandardModulusTenCalculator();
            _secondStandardModulusCalculatorElevenCalculator = new SecondStandardModulusCalculatorElevenCalculator();
            _doubleAlternateCalculator = new DoubleAlternateCalculator();
            _doubleAlternateCalculatorExceptionFive = new DoubleAlternateCalculatorExceptionFive(BaseModulusCalculator.Step.Second);
        }

        public SecondModulusCalculatorStep(SecondStandardModulusTenCalculator secondStandardModulusCalculatorTenCalculator, SecondStandardModulusCalculatorElevenCalculator secondStandardModulusCalculatorElevenCalculator, DoubleAlternateCalculator doubleAlternateCalculator, DoubleAlternateCalculatorExceptionFive daf)
        {
            _secondStandardModulusCalculatorTenCalculator = secondStandardModulusCalculatorTenCalculator;
            _secondStandardModulusCalculatorElevenCalculator = secondStandardModulusCalculatorElevenCalculator;
            _doubleAlternateCalculator = doubleAlternateCalculator;
            _doubleAlternateCalculatorExceptionFive = daf;
        }

        public override bool Process(BankAccountDetails bankAccountDetails, ModulusWeights modulusWeights)
        {
            var modulusWeightMappings = modulusWeights.GetRuleMappings(bankAccountDetails.SortCode).ToList();
            var weightMapping = modulusWeightMappings.ElementAt(1);

            if (weightMapping.Exception == 6)
            { return bankAccountDetails.AccountNumber.ValidateExceptionSix; }
            HandleExceptionSeven(bankAccountDetails, weightMapping);

            var result = false;
            switch (weightMapping.Algorithm)
            {
                case ModulusWeightMapping.ModulusAlgorithm.Mod10:
                    result = _secondStandardModulusCalculatorTenCalculator.Process(bankAccountDetails, modulusWeights);
                    break;
                case ModulusWeightMapping.ModulusAlgorithm.Mod11:
                    result = _secondStandardModulusCalculatorElevenCalculator.Process(bankAccountDetails, modulusWeights);
                    break;
                case ModulusWeightMapping.ModulusAlgorithm.DblAl:
                    result = weightMapping.Exception == 5
                                 ? _doubleAlternateCalculatorExceptionFive.Process(bankAccountDetails, modulusWeights)
                                 : _doubleAlternateCalculator.Process(bankAccountDetails, modulusWeights);
                    break;
                default:
                    throw new Exception("ModulusMapping had an unknown algorithm type: " + weightMapping.Algorithm);
            }

            return result;
        }
    }
}
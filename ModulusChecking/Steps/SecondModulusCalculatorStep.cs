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
            _doubleAlternateCalculator = new DoubleAlternateCalculator(BaseModulusCalculator.Step.Second);
            _doubleAlternateCalculatorExceptionFive = new DoubleAlternateCalculatorExceptionFive(BaseModulusCalculator.Step.Second);
        }

        public SecondModulusCalculatorStep(SecondStandardModulusTenCalculator secondStandardModulusCalculatorTenCalculator, SecondStandardModulusElevenCalculator secondStandardModulusElevenCalculator, DoubleAlternateCalculator doubleAlternateCalculator, DoubleAlternateCalculatorExceptionFive daf)
        {
            _secondStandardModulusCalculatorTenCalculator = secondStandardModulusCalculatorTenCalculator;
            _secondStandardModulusElevenCalculator = secondStandardModulusElevenCalculator;
            _doubleAlternateCalculator = doubleAlternateCalculator;
            _doubleAlternateCalculatorExceptionFive = daf;
        }

        public virtual bool Process(BankAccountDetails bankAccountDetails, IModulusWeightTable modulusWeightTable)
        {
            var modulusWeightMappings = modulusWeightTable.GetRuleMappings(bankAccountDetails.SortCode).ToList();
            var weightMapping = modulusWeightMappings.ElementAt((int) BaseModulusCalculator.Step.Second);

            ModulusRuleExceptionHandlers.HandleExceptionSeven(bankAccountDetails, weightMapping);

            return GetSecondModulusCheckResult(bankAccountDetails, modulusWeightTable, weightMapping);
        }

        private bool GetSecondModulusCheckResult(BankAccountDetails bankAccountDetails, IModulusWeightTable modulusWeightTable,
                                                 IModulusWeightMapping weightMapping)
        {
            bool result;
            switch (weightMapping.Algorithm)
            {
                case ModulusAlgorithm.Mod10:
                    result = _secondStandardModulusCalculatorTenCalculator.Process(bankAccountDetails, modulusWeightTable);
                    break;
                case ModulusAlgorithm.Mod11:
                    result = _secondStandardModulusElevenCalculator.Process(bankAccountDetails, modulusWeightTable);
                    break;
                case ModulusAlgorithm.DblAl:
                    result = weightMapping.Exception == 5
                                 ? _doubleAlternateCalculatorExceptionFive.Process(bankAccountDetails, modulusWeightTable)
                                 : _doubleAlternateCalculator.Process(bankAccountDetails, modulusWeightTable);
                    break;
                default:
                    throw new Exception("ModulusMapping had an unknown algorithm type: " + weightMapping.Algorithm);
            }
            return result;
        }
    }
}
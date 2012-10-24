using System;
using System.Linq;
using ModulusChecking.Models;
using ModulusChecking.Parsers;
using ModulusChecking.Steps.Calculators;

namespace ModulusChecking.Steps
{
    public class FirstModulusCalculatorStep : BaseStep
    {
        private readonly FirstStandardModulusTenCalculator _firstStandardModulusTenCalculator;
        private readonly FirstStandardModulusElevenCalculatorExceptionFive _firstStandardModulusElevenCalculatorExceptionFive;
        private readonly FirstStandardModulusElevenCalculator _firstStandardModulusElevenCalculator;
        private readonly DoubleAlternateCalculator _doubleAlternateCalculator;
        private readonly SecondModulusCalculatorStep _secondModulusCalculatorStep;
        private readonly DoubleAlternateCalculatorExceptionFive _doubleAlternateCalculatorExceptionFive;

        public FirstModulusCalculatorStep()
        {
            _firstStandardModulusTenCalculator = new FirstStandardModulusTenCalculator();
            _firstStandardModulusElevenCalculator = new FirstStandardModulusElevenCalculator();
            _doubleAlternateCalculator = new DoubleAlternateCalculator();  
            _firstStandardModulusElevenCalculatorExceptionFive = new FirstStandardModulusElevenCalculatorExceptionFive();
            _doubleAlternateCalculatorExceptionFive = new DoubleAlternateCalculatorExceptionFive(BaseModulusCalculator.Step.First);
            _secondModulusCalculatorStep = new SecondModulusCalculatorStep();
        }

        public FirstModulusCalculatorStep(FirstStandardModulusTenCalculator st, FirstStandardModulusElevenCalculator se, 
            DoubleAlternateCalculator da, FirstStandardModulusElevenCalculatorExceptionFive smte5, SecondModulusCalculatorStep smc, 
            DoubleAlternateCalculatorExceptionFive daf)
        {
            _firstStandardModulusTenCalculator = st;
            _firstStandardModulusElevenCalculator = se;
            _doubleAlternateCalculator = da;
            _doubleAlternateCalculatorExceptionFive = daf;
            _firstStandardModulusElevenCalculatorExceptionFive = smte5;
            _secondModulusCalculatorStep = smc;
        }

        public override bool Process(BankAccountDetails bankAccountDetails, ModulusWeights modulusWeights)
        {
            var modulusWeightMappings = modulusWeights.GetRuleMappings(bankAccountDetails.SortCode).ToList();
            var weightMapping = modulusWeightMappings.First();

            if (weightMapping.Exception == 6)
            {
                return bankAccountDetails.AccountNumber.ValidateExceptionSix;
            }
            HandleExceptionSeven(bankAccountDetails, weightMapping);
            if (weightMapping.Exception == 8)
            {
                bankAccountDetails.SortCode = new SortCode("090126");
            }

            var result = false;
            switch (weightMapping.Algorithm)
            {
                case ModulusWeightMapping.ModulusAlgorithm.Mod10:
                    result = _firstStandardModulusTenCalculator.Process(bankAccountDetails, modulusWeights);
                    break;
                case ModulusWeightMapping.ModulusAlgorithm.Mod11:
                    result = weightMapping.Exception == 5
                                 ? _firstStandardModulusElevenCalculatorExceptionFive.Process(bankAccountDetails,
                                                                                              modulusWeights)
                                 : _firstStandardModulusElevenCalculator.Process(bankAccountDetails, modulusWeights);
                    break;
                case ModulusWeightMapping.ModulusAlgorithm.DblAl:
                    result = weightMapping.Exception == 5
                                 ? _doubleAlternateCalculatorExceptionFive.Process(bankAccountDetails, modulusWeights)
                                 : _doubleAlternateCalculator.Process(bankAccountDetails, modulusWeights);
                    break;
                default:
                    throw new Exception("ModulusMapping had an unknown algorithm type: " + weightMapping.Algorithm);
            }

            if (modulusWeightMappings.Count() == 1)
            {
                return result;
            }

            var secondWeightMapping = modulusWeightMappings.ElementAt(1);
            //are there any exceptions that alter the second step or it's conditions?
            if (secondWeightMapping.Exception == 3)
            {
                if (bankAccountDetails.AccountNumber.IntegerAt(2) == 6
                    || bankAccountDetails.AccountNumber.IntegerAt(2) == 9)
                {
                    //the second check isn't required
                    return result;
                }
            }

            var secondResult = _secondModulusCalculatorStep.Process(bankAccountDetails, modulusWeights);
            if (weightMapping.Exception == 5)
            {
                return result && secondResult;
            }
            if (weightMapping.Exception == 10 && secondWeightMapping.Exception == 11)
            {
                  return secondResult || result;
            }
            return secondResult;
        }
    }
}
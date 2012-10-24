using System;
using System.Collections.Generic;
using System.Linq;
using ModulusChecking.Models;
using ModulusChecking.Parsers;
using ModulusChecking.Steps.Calculators;

namespace ModulusChecking.Steps
{
    /// <summary>
    /// Once the sortcode is confirmed to be present in the ModulusWeightTable.Determine and complete
    /// carry out the first check.
    /// </summary>
    class FirstModulusCalculatorStep : BaseStep
    {
        private readonly FirstStandardModulusTenCalculator _firstStandardModulusTenCalculator;
        private readonly FirstStandardModulusElevenCalculatorExceptionFive _firstStandardModulusElevenCalculatorExceptionFive;
        private readonly FirstStandardModulusElevenCalculator _firstStandardModulusElevenCalculator;
        private readonly DoubleAlternateCalculator _doubleAlternateCalculator;
        private readonly SecondModulusCalculatorStep _secondModulusCalculatorStep;
        private readonly DoubleAlternateCalculatorExceptionFive _doubleAlternateCalculatorExceptionFive;
        private readonly StandardModulusExceptionFourteenCalculator _exceptionFourteenCalculator;

        public FirstModulusCalculatorStep()
        {
            _firstStandardModulusTenCalculator = new FirstStandardModulusTenCalculator();
            _firstStandardModulusElevenCalculator = new FirstStandardModulusElevenCalculator();
            _doubleAlternateCalculator = new DoubleAlternateCalculator(BaseModulusCalculator.Step.Second);  
            _firstStandardModulusElevenCalculatorExceptionFive = new FirstStandardModulusElevenCalculatorExceptionFive();
            _doubleAlternateCalculatorExceptionFive = new DoubleAlternateCalculatorExceptionFive(BaseModulusCalculator.Step.Second);
            _secondModulusCalculatorStep = new SecondModulusCalculatorStep();
            _exceptionFourteenCalculator = new StandardModulusExceptionFourteenCalculator();
        }

        public FirstModulusCalculatorStep(FirstStandardModulusTenCalculator st, FirstStandardModulusElevenCalculator se, 
            DoubleAlternateCalculator da, FirstStandardModulusElevenCalculatorExceptionFive smte5, SecondModulusCalculatorStep smc, 
            DoubleAlternateCalculatorExceptionFive daf, StandardModulusExceptionFourteenCalculator efc)
        {
            _firstStandardModulusTenCalculator = st;
            _firstStandardModulusElevenCalculator = se;
            _doubleAlternateCalculator = da;
            _doubleAlternateCalculatorExceptionFive = daf;
            _firstStandardModulusElevenCalculatorExceptionFive = smte5;
            _secondModulusCalculatorStep = smc;
            _exceptionFourteenCalculator = efc;
        }

        public override bool Process(BankAccountDetails bankAccountDetails, IModulusWeightTable modulusWeightTable)
        {
            var modulusWeightMappings = modulusWeightTable.GetRuleMappings(bankAccountDetails.SortCode).ToList();
            var weightMapping = modulusWeightMappings.First();

            if (weightMapping.Exception == 6 && bankAccountDetails.AccountNumber.IsForeignCurrencyAccount)
            {
                return true;
            }
            
            HandleExceptionSeven(bankAccountDetails, weightMapping);
            HandleExceptionEight(bankAccountDetails, weightMapping);
            HandleExceptionTen(bankAccountDetails,weightMapping);

            bool firstModulusCheckResult;
            switch (weightMapping.Algorithm)
            {
                case ModulusAlgorithm.Mod10:
                    firstModulusCheckResult = _firstStandardModulusTenCalculator.Process(bankAccountDetails, modulusWeightTable);
                    break;
                case ModulusAlgorithm.Mod11:
                    firstModulusCheckResult = weightMapping.Exception == 5
                                 ? _firstStandardModulusElevenCalculatorExceptionFive.Process(bankAccountDetails,
                                                                                              modulusWeightTable)
                                 : _firstStandardModulusElevenCalculator.Process(bankAccountDetails, modulusWeightTable);
                    break;
                case ModulusAlgorithm.DblAl:
                    firstModulusCheckResult = weightMapping.Exception == 5
                                 ? _doubleAlternateCalculatorExceptionFive.Process(bankAccountDetails, modulusWeightTable)
                                 : _doubleAlternateCalculator.Process(bankAccountDetails, modulusWeightTable);
                    break;
                default:
                    throw new Exception("ModulusMapping had an unknown algorithm type: " + weightMapping.Algorithm);
            }

            if (modulusWeightMappings.Count() == 1)
            {
                return firstModulusCheckResult;
            }

            if (!firstModulusCheckResult)
            {
                if (!(new List<int> { 2, 9, 10, 11, 12, 13, 14 }).Contains(weightMapping.Exception))
                {
                    return false;
                }
            }

            if (weightMapping.Exception == 14)
            {
                return firstModulusCheckResult || _exceptionFourteenCalculator.Process(bankAccountDetails, modulusWeightTable);
            }

            var secondWeightMapping = modulusWeightMappings.ElementAt(1);
            //are there any exceptions that alter the second step or it's conditions?
            if (secondWeightMapping.Exception == 3)
            {
                if (bankAccountDetails.AccountNumber.IntegerAt(2) == 6
                    || bankAccountDetails.AccountNumber.IntegerAt(2) == 9)
                {
                    //the second check isn't required
                    return firstModulusCheckResult;
                }
            }

            var secondModulusCheckResult = _secondModulusCalculatorStep.Process(bankAccountDetails, modulusWeightTable);
            
            if (weightMapping.Exception == 5)
            {
                return firstModulusCheckResult && secondModulusCheckResult;
            }

            if ((weightMapping.Exception == 10 && secondWeightMapping.Exception == 11)
                || (weightMapping.Exception == 12 && secondWeightMapping.Exception == 13))
            {
                  return secondModulusCheckResult || firstModulusCheckResult;
            }

            return secondModulusCheckResult;
        }

        private static void HandleExceptionEight(BankAccountDetails bankAccountDetails, IModulusWeightMapping weightMapping)
        {
            if (weightMapping.Exception == 8)
            {
                bankAccountDetails.SortCode = new SortCode("090126");
            }
        }
    }
}
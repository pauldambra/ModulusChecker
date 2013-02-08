using System;
using System.Collections.Generic;
using System.Linq;
using ModulusChecking.Loaders;
using ModulusChecking.Models;
using ModulusChecking.Steps.Calculators;

namespace ModulusChecking.Steps
{
    /// <summary>
    /// Once the sortcode is confirmed to be present in the ModulusWeightTable.Determine and complete
    /// carry out the first check.
    /// </summary>
    class FirstModulusCalculatorStep : IStep
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
            _doubleAlternateCalculator = new DoubleAlternateCalculator(ModulusWeightMapping.Step.First);  
            _firstStandardModulusElevenCalculatorExceptionFive = new FirstStandardModulusElevenCalculatorExceptionFive();
            _doubleAlternateCalculatorExceptionFive = new DoubleAlternateCalculatorExceptionFive(ModulusWeightMapping.Step.First);
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

        public virtual bool Process(BankAccountDetails bankAccountDetails)
        {
            if (bankAccountDetails.IsUncheckableForeignAccount()) return true;

            bankAccountDetails.FirstResult = GetFirstModulusCheckResult(bankAccountDetails);

            if (bankAccountDetails.WeightMappings.Count() == 1 || !bankAccountDetails.IsSecondCheckRequired()) 
            { return bankAccountDetails.FirstResult; }
            
            if (ExceptionTwoAndFirstCheckPasses(bankAccountDetails)) return true;

            return bankAccountDetails.RequiresCouttsAccountCheck()
                ? ExceptionFourteenForCouttsAccounts(bankAccountDetails) 
                : HandleSecondModulusCheck(bankAccountDetails);
        }

        private bool HandleSecondModulusCheck(BankAccountDetails bankAccountDetails)
        {
           //are there any exceptions that alter the second step or it's conditions?
            if (bankAccountDetails.IsExceptionThreeAndCanSkipSecondCheck())
            {
                return bankAccountDetails.FirstResult;
            }

            bankAccountDetails.SecondResult = _secondModulusCalculatorStep.Process(bankAccountDetails);

            return PostProcessSecondModulusCheckResult(bankAccountDetails);
        }

        private static bool PostProcessSecondModulusCheckResult(BankAccountDetails bankAccountDetails)
        {
            var firstMapping = bankAccountDetails.WeightMappings.First();
            var secondMapping = bankAccountDetails.WeightMappings.ElementAt((int) ModulusWeightMapping.Step.Second);
            if (firstMapping.Exception == 5)
            {
                return bankAccountDetails.FirstResult && bankAccountDetails.SecondResult;
            }

            if ((firstMapping.Exception == 10 && secondMapping.Exception == 11)
                || (firstMapping.Exception == 12 && secondMapping.Exception == 13))
            {
                return bankAccountDetails.SecondResult || bankAccountDetails.FirstResult;
            }

            return bankAccountDetails.SecondResult;
        }

        private bool ExceptionFourteenForCouttsAccounts(BankAccountDetails bankAccountDetails)
        {
            return bankAccountDetails.FirstResult ||
                   _exceptionFourteenCalculator.Process(bankAccountDetails);
        }

        private static bool ExceptionTwoAndFirstCheckPasses(BankAccountDetails bankAccountDetails)
        {
            return bankAccountDetails.FirstResult && bankAccountDetails.WeightMappings.First().Exception == 2;
        }

        private bool GetFirstModulusCheckResult(BankAccountDetails bankAccountDetails)
        {
            var mapping = bankAccountDetails.WeightMappings.First();
            switch (mapping.Algorithm)
            {
                case ModulusAlgorithm.Mod10:
                    bankAccountDetails.FirstResult = _firstStandardModulusTenCalculator.Process(bankAccountDetails);
                    break;
                case ModulusAlgorithm.Mod11:
                    bankAccountDetails.FirstResult = mapping.Exception == 5
                                                  ? _firstStandardModulusElevenCalculatorExceptionFive.Process(bankAccountDetails)
                                                  : _firstStandardModulusElevenCalculator.Process(bankAccountDetails);
                    break;
                case ModulusAlgorithm.DblAl:
                    bankAccountDetails.FirstResult = mapping.Exception == 5
                                                  ? _doubleAlternateCalculatorExceptionFive.Process(bankAccountDetails)
                                                  : _doubleAlternateCalculator.Process(bankAccountDetails);
                    break;
                default:
                    throw new Exception("ModulusMapping had an unknown algorithm type: " + mapping.Algorithm);
            }
            return bankAccountDetails.FirstResult;
        }
    }
}
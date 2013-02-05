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

        public virtual bool Process(BankAccountDetails bankAccountDetails, IModulusWeightTable modulusWeightTable)
        {
            //get the first matching mapping from the weight table
            var modulusWeightMappings = modulusWeightTable.GetRuleMappings(bankAccountDetails.SortCode).ToList();
            var weightMapping = modulusWeightMappings.First();

            if (IsUncheckableForeignAccount(bankAccountDetails, weightMapping)) return true;
            
            PreProcessFirstModulusCheck(bankAccountDetails, weightMapping);

            var firstModulusCheckResult = GetFirstModulusCheckResult(bankAccountDetails, modulusWeightTable, weightMapping);

            if (modulusWeightMappings.Count() == 1) { return firstModulusCheckResult; }
            if (firstModulusCheckResult ==false && !SecondCheckRequired(weightMapping))
            {
                    return false;
            }

            if (ExceptionTwoAndFirstCheckPasses(firstModulusCheckResult, weightMapping)) return true;

            return RequiresCouttsAccountCheck(weightMapping) 
                ? ExceptionFourteenForCouttsAccounts(bankAccountDetails, modulusWeightTable, firstModulusCheckResult) 
                : HandleSecondModulusCheck(bankAccountDetails, modulusWeightTable, modulusWeightMappings, firstModulusCheckResult, weightMapping);
        }

        private bool HandleSecondModulusCheck(BankAccountDetails bankAccountDetails, IModulusWeightTable modulusWeightTable,
                                              IEnumerable<IModulusWeightMapping> modulusWeightMappings, bool firstModulusCheckResult,
                                              IModulusWeightMapping weightMapping)
        {
            var secondWeightMapping = modulusWeightMappings.ElementAt(1);
            //are there any exceptions that alter the second step or it's conditions?
            if (ExceptionThreeAndCanSkipSecondCheck(bankAccountDetails, secondWeightMapping))
            {
                return firstModulusCheckResult;
            }

            var secondModulusCheckResult = _secondModulusCalculatorStep.Process(bankAccountDetails, modulusWeightTable);

            return PostProcessSecondModulusCheckResult(weightMapping, firstModulusCheckResult, secondModulusCheckResult,
                                                       secondWeightMapping);
        }

        private static void PreProcessFirstModulusCheck(BankAccountDetails bankAccountDetails,
                                                        IModulusWeightMapping weightMapping)
        {
            ModulusRuleExceptionHandlers.HandleExceptionSeven(bankAccountDetails, weightMapping);
            ModulusRuleExceptionHandlers.HandleExceptionEight(bankAccountDetails, weightMapping);
            ModulusRuleExceptionHandlers.HandleExceptionTen(bankAccountDetails, weightMapping);
        }

        private static bool ExceptionThreeAndCanSkipSecondCheck(BankAccountDetails bankAccountDetails,
                                                             IModulusWeightMapping secondWeightMapping)
        {
            return secondWeightMapping.Exception == 3 
                   && (bankAccountDetails.AccountNumber.IntegerAt(2) == 6
                       || bankAccountDetails.AccountNumber.IntegerAt(2) == 9);
        }

        private static bool PostProcessSecondModulusCheckResult(IModulusWeightMapping weightMapping,
                                                                bool firstModulusCheckResult, bool secondModulusCheckResult,
                                                                IModulusWeightMapping secondWeightMapping)
        {
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

        private bool ExceptionFourteenForCouttsAccounts(BankAccountDetails bankAccountDetails,
                                                        IModulusWeightTable modulusWeightTable, bool firstModulusCheckResult)
        {
            return firstModulusCheckResult ||
                   _exceptionFourteenCalculator.Process(bankAccountDetails, modulusWeightTable);
        }

        private static bool RequiresCouttsAccountCheck(IModulusWeightMapping weightMapping)
        {
            return weightMapping.Exception == 14;
        }

        private static bool ExceptionTwoAndFirstCheckPasses(bool firstModulusCheckResult, IModulusWeightMapping weightMapping)
        {
            return firstModulusCheckResult && weightMapping.Exception == 2;
        }

        private bool GetFirstModulusCheckResult(BankAccountDetails bankAccountDetails, IModulusWeightTable modulusWeightTable,
                                                IModulusWeightMapping weightMapping)
        {
            bool firstModulusCheckResult;
            switch (weightMapping.Algorithm)
            {
                case ModulusAlgorithm.Mod10:
                    firstModulusCheckResult = _firstStandardModulusTenCalculator.Process(bankAccountDetails, modulusWeightTable);
                    break;
                case ModulusAlgorithm.Mod11:
                    firstModulusCheckResult = weightMapping.Exception == 5
                                                  ? _firstStandardModulusElevenCalculatorExceptionFive.Process(
                                                      bankAccountDetails,
                                                      modulusWeightTable)
                                                  : _firstStandardModulusElevenCalculator.Process(bankAccountDetails,
                                                                                                  modulusWeightTable);
                    break;
                case ModulusAlgorithm.DblAl:
                    firstModulusCheckResult = weightMapping.Exception == 5
                                                  ? _doubleAlternateCalculatorExceptionFive.Process(bankAccountDetails,
                                                                                                    modulusWeightTable)
                                                  : _doubleAlternateCalculator.Process(bankAccountDetails, modulusWeightTable);
                    break;
                default:
                    throw new Exception("ModulusMapping had an unknown algorithm type: " + weightMapping.Algorithm);
            }
            return firstModulusCheckResult;
        }

        private static bool IsUncheckableForeignAccount(BankAccountDetails bankAccountDetails,
                                                        IModulusWeightMapping weightMapping)
        {
            return weightMapping.Exception == 6 && bankAccountDetails.AccountNumber.IsForeignCurrencyAccount;
        }

        private static bool SecondCheckRequired(IModulusWeightMapping mapping)
        {
            return new List<int> {2, 9, 10, 11, 12, 13, 14}.Contains(mapping.Exception);
        }
    }
}
using System.Linq;
using ModulusChecking.Models;
using ModulusChecking.Steps.Calculators;

namespace ModulusChecking.Steps
{
    /// <summary>
    /// Once the sortcode is confirmed to be present in the ModulusWeightTable.Determine and complete
    /// carry out the first check.
    /// </summary>
    internal class FirstModulusCalculatorStep : IProcessAStep
    {
        private readonly SecondModulusCalculatorStep _secondModulusCalculatorStep;
        private readonly StandardModulusExceptionFourteenCalculator _exceptionFourteenCalculator;

        private readonly FirstStepRouter _firstStepRouter;

        public FirstModulusCalculatorStep()
        {
            _firstStepRouter = new FirstStepRouter();
            _secondModulusCalculatorStep = new SecondModulusCalculatorStep();
            _exceptionFourteenCalculator = new StandardModulusExceptionFourteenCalculator();
        }

        public FirstModulusCalculatorStep(FirstStepRouter firstStepRouter, SecondModulusCalculatorStep smc,
                                          StandardModulusExceptionFourteenCalculator efc)
        {
            _firstStepRouter = firstStepRouter;
            _secondModulusCalculatorStep = smc;
            _exceptionFourteenCalculator = efc;
        }

        public ModulusCheckOutcome Process(BankAccountDetails bankAccountDetails)
        {
            bankAccountDetails.FirstResult = _firstStepRouter.GetModulusCalculation(bankAccountDetails);

            bool result;
            
            if (bankAccountDetails.RequiresCouttsAccountCheck())
            {
                result = ExceptionFourteenForCouttsAccounts(bankAccountDetails);
            }
            else if (bankAccountDetails.WeightMappings.Count() == 1 || !bankAccountDetails.IsSecondCheckRequired())
            {
                result = bankAccountDetails.FirstResult;
            }
            else if (bankAccountDetails.IsExceptionTwoAndFirstCheckPassed())
            {
                result = true;
            }
            else
            {
                result = bankAccountDetails.IsExceptionThreeAndCanSkipSecondCheck()
                    ? bankAccountDetails.FirstResult
                    : _secondModulusCalculatorStep.Process(bankAccountDetails);
            }

            return new ModulusCheckOutcome("FirstModulusCalculatorStep", result);
        }

        private bool ExceptionFourteenForCouttsAccounts(BankAccountDetails bankAccountDetails)
        {
            return bankAccountDetails.FirstResult ||
                   _exceptionFourteenCalculator.Process(bankAccountDetails);
        }
    }
}
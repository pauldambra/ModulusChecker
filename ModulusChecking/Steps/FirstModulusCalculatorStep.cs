using System.Linq;
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

        public virtual bool Process(BankAccountDetails bankAccountDetails)
        {
            bankAccountDetails.FirstResult = _firstStepRouter.GetModulusCalculation(bankAccountDetails);

            if (bankAccountDetails.RequiresCouttsAccountCheck())
            {
                return ExceptionFourteenForCouttsAccounts(bankAccountDetails);
            }

            if (bankAccountDetails.WeightMappings.Count() == 1 || !bankAccountDetails.IsSecondCheckRequired()) 
            { return bankAccountDetails.FirstResult; }
            
            if (bankAccountDetails.IsExceptionTwoAndFirstCheckPassed()) return true;

            return bankAccountDetails.IsExceptionThreeAndCanSkipSecondCheck()
                       ? bankAccountDetails.FirstResult
                       : _secondModulusCalculatorStep.Process(bankAccountDetails);
        }

        private bool ExceptionFourteenForCouttsAccounts(BankAccountDetails bankAccountDetails)
        {
            return bankAccountDetails.FirstResult ||
                   _exceptionFourteenCalculator.Process(bankAccountDetails);
        }
    }
}
using ModulusChecking.Models;

namespace ModulusChecking.Steps
{
    internal class IsExceptionThreeAndCanSkipSecondCheck : IProcessAStep
    {
        private readonly SecondModulusCalculatorStep _nextStep;

        public IsExceptionThreeAndCanSkipSecondCheck()
        {
            _nextStep = new SecondModulusCalculatorStep();
        }

        public IsExceptionThreeAndCanSkipSecondCheck(
            SecondModulusCalculatorStep nextStep)
        {
            _nextStep = nextStep;
        }

        public ModulusCheckOutcome Process(BankAccountDetails bankAccountDetails) => 
            bankAccountDetails.IsExceptionThreeAndCanSkipSecondCheck()
                ? new ModulusCheckOutcome("IsExceptionThreeAndCanSkipSecondCheck")
                : new ModulusCheckOutcome("SECOND CHECK", _nextStep.Process(bankAccountDetails));
    }
}
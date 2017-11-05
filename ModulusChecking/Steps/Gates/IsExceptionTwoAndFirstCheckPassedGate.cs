using ModulusChecking.Models;

namespace ModulusChecking.Steps
{
    internal class IsExceptionTwoAndFirstCheckPassedGate : IProcessAStep
    {
        private readonly IProcessAStep _nextStep;

        public IsExceptionTwoAndFirstCheckPassedGate()
        {
            _nextStep = new IsExceptionThreeAndCanSkipSecondCheck();
        }

        public IsExceptionTwoAndFirstCheckPassedGate(
            IProcessAStep nextStep)
        {
            _nextStep = nextStep;
        }

        public ModulusCheckOutcome Process(BankAccountDetails bankAccountDetails) => 
            bankAccountDetails.IsExceptionTwoAndFirstCheckPassed()
                ? new ModulusCheckOutcome("IsExceptionTwoAndFirstCheckPassed")
                : _nextStep.Process(bankAccountDetails);
    }
}
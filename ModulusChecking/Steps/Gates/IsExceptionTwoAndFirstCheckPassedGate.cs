using ModulusChecking.Models;

namespace ModulusChecking.Steps.Gates
{
    internal class IsExceptionTwoAndFirstCheckPassedGate : IProcessAStep
    {
        private readonly IProcessAStep _nextStep;

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
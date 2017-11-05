using ModulusChecking.Models;

namespace ModulusChecking.Steps.Gates
{
    internal class IsSecondCheckRequiredGate : IProcessAStep
    {
        private readonly IProcessAStep _nextStep;

        public IsSecondCheckRequiredGate()
        {
            _nextStep = new IsExceptionTwoAndFirstCheckPassedGate();
        }

        public IsSecondCheckRequiredGate(IProcessAStep nextStep)
        {
            _nextStep = nextStep;
        }

        public ModulusCheckOutcome Process(BankAccountDetails bankAccountDetails) => 
            bankAccountDetails.IsSecondCheckRequired()
                ? _nextStep.Process(bankAccountDetails)
                : new ModulusCheckOutcome("second check not required", bankAccountDetails.FirstResult);
    }
}
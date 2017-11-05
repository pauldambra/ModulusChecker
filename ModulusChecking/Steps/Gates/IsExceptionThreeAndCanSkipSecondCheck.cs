using ModulusChecking.Models;

namespace ModulusChecking.Steps.Gates
{
    internal class IsExceptionThreeAndCanSkipSecondCheck : IProcessAStep
    {
        private readonly IProcessAStep _nextStep;

        public IsExceptionThreeAndCanSkipSecondCheck(
            IProcessAStep nextStep)
        {
            _nextStep = nextStep;
        }

        public ModulusCheckOutcome Process(BankAccountDetails bankAccountDetails) => 
            bankAccountDetails.IsExceptionThreeAndCanSkipSecondCheck()
                ? new ModulusCheckOutcome("IsExceptionThreeAndCanSkipSecondCheck")
                : _nextStep.Process(bankAccountDetails);
    }
}
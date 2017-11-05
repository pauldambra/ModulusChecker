using System.Collections.Generic;
using ModulusChecking.Models;

namespace ModulusChecking.Steps.Gates
{
    internal class IsSecondCheckRequiredGate : IProcessAStep
    {
        private readonly IProcessAStep _nextStep;

        public IsSecondCheckRequiredGate(IProcessAStep nextStep)
        {
            _nextStep = nextStep;
        }

        public ModulusCheckOutcome Process(BankAccountDetails bankAccountDetails) => 
            bankAccountDetails.IsSecondCheckRequired()
                ? _nextStep.Process(bankAccountDetails)
                : new ModulusCheckOutcome("first weight mapping exception does not require second check", bankAccountDetails.FirstResult);
    }
}
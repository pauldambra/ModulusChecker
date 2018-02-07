using System.Linq;
using ModulusChecking.Models;

namespace ModulusChecking.Steps.Gates
{
    internal class OnlyOneWeightMappingGate : IProcessAStep
    {
        private readonly IProcessAStep _nextStep;

        public OnlyOneWeightMappingGate(IProcessAStep nextStep)
        {
            _nextStep = nextStep;
        }

        public ModulusCheckOutcome Process(BankAccountDetails bankAccountDetails) => 
            bankAccountDetails.WeightMappings.Count() == 1
                ? new ModulusCheckOutcome("not proceeding to the second check as there is only one weight mapping", bankAccountDetails.FirstResult)
                : _nextStep.Process(bankAccountDetails);
    }
}
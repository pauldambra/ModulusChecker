using System.Linq;
using ModulusChecking.Models;

namespace ModulusChecking.Steps
{
    internal class OnlyOneWeightMappingGate : IProcessAStep
    {
        private readonly IProcessAStep _nextStep;

        public OnlyOneWeightMappingGate()
        {
            _nextStep = new IsSecondCheckRequiredGate();
        }

        public OnlyOneWeightMappingGate(IProcessAStep nextStep)
        {
            _nextStep = nextStep;
        }

        public ModulusCheckOutcome Process(BankAccountDetails bankAccountDetails) => 
            bankAccountDetails.WeightMappings.Count() == 1
                ? new ModulusCheckOutcome("only one weight mapping", bankAccountDetails.FirstResult)
                : _nextStep.Process(bankAccountDetails);
    }
}
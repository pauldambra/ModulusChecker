using ModulusChecking.Loaders;
using ModulusChecking.Models;
using ModulusChecking.Properties;

namespace ModulusChecking.Steps.ConfirmDetailsAreValid
{
   
    internal class IsUncheckableForeignAccount : IProcessAStep
    {
        private readonly IProcessAStep _firstModulusCalculatorStep;

        public IsUncheckableForeignAccount(SortCodeSubstitution sortCodeSubstitution) { _firstModulusCalculatorStep = new FirstModulusCalculatorStep(sortCodeSubstitution); }

        public IsUncheckableForeignAccount(IProcessAStep nextStep)
        { _firstModulusCalculatorStep = nextStep; }

        public ModulusCheckOutcome Process(BankAccountDetails bankAccountDetails) => 
            bankAccountDetails.IsUncheckableForeignAccount() 
                ? new ModulusCheckOutcome("This is an uncheckable foreign account") 
                : _firstModulusCalculatorStep.Process(bankAccountDetails);
    }
}
using ModulusChecking.Models;

namespace ModulusChecking.Steps.ConfirmDetailsAreValid
{
   
    internal class IsUncheckableForeignAccount : IProcessAStep
    {
        private readonly IProcessAStep _firstModulusCalculatorStep;

        public IsUncheckableForeignAccount() { _firstModulusCalculatorStep = new FirstModulusCalculatorStep(); }

        public IsUncheckableForeignAccount(IProcessAStep nextStep)
        { _firstModulusCalculatorStep = nextStep; }

        public ModulusCheckOutcome Process(BankAccountDetails bankAccountDetails) => 
            bankAccountDetails.IsUncheckableForeignAccount() 
                ? new ModulusCheckOutcome("This is an uncheckable foreign account") 
                : _firstModulusCalculatorStep.Process(bankAccountDetails);
    }
}
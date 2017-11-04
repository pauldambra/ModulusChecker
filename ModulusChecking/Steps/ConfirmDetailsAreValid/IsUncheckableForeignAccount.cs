using ModulusChecking.Models;

namespace ModulusChecking.Steps.ConfirmDetailsAreValid
{
   
    internal class IsUncheckableForeignAccount
    {
        private readonly FirstModulusCalculatorStep _firstModulusCalculatorStep;

        public IsUncheckableForeignAccount() { _firstModulusCalculatorStep = new FirstModulusCalculatorStep(); }

        public IsUncheckableForeignAccount(FirstModulusCalculatorStep nextStep)
        { _firstModulusCalculatorStep = nextStep; }

        public virtual ModulusCheckOutcome Process(BankAccountDetails bankAccountDetails)
        {
            var isUncheckableForeignAccount = bankAccountDetails.IsUncheckableForeignAccount();

            if (isUncheckableForeignAccount)
                return new ModulusCheckOutcome("This is an uncheckable foreign account");

            var result = _firstModulusCalculatorStep.Process(bankAccountDetails);
            return new ModulusCheckOutcome("explanation not implemented yet", result);
        }
    }
}
using ModulusChecking.Models;

namespace ModulusChecking.Steps
{
    /// <summary>
    /// The first step is to test if the given sort code can be found in the Modulus Weight Mappings.
    /// If not it is presumed to be valid
    /// </summary>
    internal class ConfirmDetailsAreValidForModulusCheck
    {
        private readonly FirstModulusCalculatorStep _firstModulusCalculatorStep;

        public ConfirmDetailsAreValidForModulusCheck() { _firstModulusCalculatorStep = new FirstModulusCalculatorStep(); }

        public ConfirmDetailsAreValidForModulusCheck(FirstModulusCalculatorStep nextStep)
        { _firstModulusCalculatorStep = nextStep; }

        /// <summary>
        /// If there are no SortCode Modulus Weight Mappings available then the BankAccountDetails validate as true.
        /// Otherwise move onto the first modulus calculation step
        /// </summary>
        public ModulusCheckOutcome Process(BankAccountDetails bankAccountDetails)
        {
            var isValidForModulusCheck = bankAccountDetails.IsValidForModulusCheck();
            var isUncheckableForeignAccount = bankAccountDetails.IsUncheckableForeignAccount();

            if (!isValidForModulusCheck)
                return new ModulusCheckOutcome("There are no weight mappings for this sort code");
            
            if (isUncheckableForeignAccount)
                return new ModulusCheckOutcome("This is an uncheckable foreign account");

            var result = _firstModulusCalculatorStep.Process(bankAccountDetails);
            return new ModulusCheckOutcome("explanation not implemented yet", result);
        }
    }
}
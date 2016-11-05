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
        public bool Process(BankAccountDetails bankAccountDetails)
        {
            var isValidForModulusCheck = bankAccountDetails.IsValidForModulusCheck();
            var isUncheckableForeignAccount = bankAccountDetails.IsUncheckableForeignAccount();

            if (!isValidForModulusCheck || isUncheckableForeignAccount)
                return true;

            var result = _firstModulusCalculatorStep.Process(bankAccountDetails);
            return result;
        }
    }
}
using ModulusChecking.Models;

namespace ModulusChecking.Steps.ConfirmDetailsAreValid
{
    
    /// <summary>
    /// The first step is to test if the given sort code can be found in the Modulus Weight Mappings.
    /// If not it is presumed to be valid
    /// </summary>
    internal class HasWeightMappings
    {
        private readonly IsUncheckableForeignAccount _next;

        public HasWeightMappings() { _next = new IsUncheckableForeignAccount(); }

        public HasWeightMappings(IsUncheckableForeignAccount nextStep)
        { _next = nextStep; }

        /// <summary>
        /// If there are no SortCode Modulus Weight Mappings available then the BankAccountDetails validate as true.
        /// Otherwise move onto the first modulus calculation step
        /// </summary>
        public ModulusCheckOutcome Process(BankAccountDetails bankAccountDetails)
        {
            var isValidForModulusCheck = bankAccountDetails.IsValidForModulusCheck();

            if (!isValidForModulusCheck)
                return new ModulusCheckOutcome("There are no weight mappings for this sort code");

            var result = _next.Process(bankAccountDetails);
            return new ModulusCheckOutcome("explanation not implemented yet", result);
        }
    }
}
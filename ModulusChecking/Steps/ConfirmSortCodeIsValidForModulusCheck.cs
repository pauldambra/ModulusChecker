using System.Linq;
using ModulusChecking.Loaders;
using ModulusChecking.Models;

namespace ModulusChecking.Steps
{
    /// <summary>
    /// The first step is to test if the given sort code can be found in the Modulus Weight Mappings.
    /// If not it is presumed to be valid
    /// </summary>
    class ConfirmSortCodeIsValidForModulusCheck : IStep
    {
        private readonly FirstModulusCalculatorStep _firstModulusCalculatorStep;

        public ConfirmSortCodeIsValidForModulusCheck() { _firstModulusCalculatorStep = new FirstModulusCalculatorStep(); }

        public ConfirmSortCodeIsValidForModulusCheck(FirstModulusCalculatorStep nextStep)
        { _firstModulusCalculatorStep = nextStep; }

        /// <summary>
        /// If there are no SortCode Modulus Weight Mappings available then the BankAccountDetails validate as true.
        /// Otherwise move onto the first modulus calculation step
        /// </summary>
        public bool Process(BankAccountDetails bankAccountDetails, IModulusWeightTable modulusWeightTable)
        {
            var sortCodeHasModulusWeightMapping = modulusWeightTable.GetRuleMappings(bankAccountDetails.SortCode).Any();
            return !sortCodeHasModulusWeightMapping || _firstModulusCalculatorStep.Process(bankAccountDetails, modulusWeightTable);
        }
    }
}
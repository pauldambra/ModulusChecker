using System.Linq;
using ModulusChecking.Models;
using ModulusChecking.Parsers;

namespace ModulusChecking.Steps
{
    /// <summary>
    /// The first step is to test if the given sort code can be found in the Modulus Wight Mappings.
    /// If not it is presumed to be valid
    /// </summary>
    class ConfirmSortCodeIsValidForModulusCheck : BaseStep
    {
        private readonly FirstModulusCalculatorStep _firstModulusCalculatorStep;

        public ConfirmSortCodeIsValidForModulusCheck()
        {
            _firstModulusCalculatorStep = new FirstModulusCalculatorStep();
        }

        public ConfirmSortCodeIsValidForModulusCheck(FirstModulusCalculatorStep nextStep)
        {
            _firstModulusCalculatorStep = nextStep;
        }

        public override bool Process(BankAccountDetails bankAccountDetails, IModulusWeightTable modulusWeightTable)
        {
            var sortCodeHasModulusWeightMapping = modulusWeightTable.GetRuleMappings(bankAccountDetails.SortCode).Any();
            return !sortCodeHasModulusWeightMapping || _firstModulusCalculatorStep.Process(bankAccountDetails, modulusWeightTable);
        }
    }
}
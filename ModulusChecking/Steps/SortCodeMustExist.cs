using System.Linq;
using ModulusChecking.Models;
using ModulusChecking.Parsers;

namespace ModulusChecking.Steps
{
    /// <summary>
    /// The first step to process is to test if the given sort code can be found in the Modulus Wight Mappings.
    /// If not it presumed to be valid
    /// </summary>
    public class SortCodeMustExist : BaseStep
    {
        private readonly FirstModulusCalculatorStep _firstModulusCalculatorStep;

        public SortCodeMustExist()
        {
            _firstModulusCalculatorStep = new FirstModulusCalculatorStep();
        }

        public SortCodeMustExist(FirstModulusCalculatorStep nextStep)
        {
            _firstModulusCalculatorStep = nextStep;
        }

        public override bool Process(BankAccountDetails bankAccountDetails, ModulusWeights modulusWeights)
        {
            var sortCodeHasModulusWeightMapping = modulusWeights.GetRuleMappings(bankAccountDetails.SortCode).Any();
            return !sortCodeHasModulusWeightMapping || _firstModulusCalculatorStep.Process(bankAccountDetails, modulusWeights);
        }
    }
}
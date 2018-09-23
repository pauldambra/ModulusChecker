using ModulusChecking.Loaders;
using ModulusChecking.Models;
using ModulusChecking.Properties;
using ModulusChecking.Steps;
using ModulusChecking.Steps.Calculators;

namespace ModulusChecking
{
    public class ModulusChecker
    {
        private readonly IModulusWeightTable _weightTable;
        private readonly SortCodeSubstitution _sortCodeSubstitutions;

        public ModulusChecker()
        {
            _weightTable = new ModulusWeightTable(Resources.valacdos);
            _sortCodeSubstitutions = new SortCodeSubstitution(Resources.scsubtab);
        }

        public ModulusChecker(string weightTableContents, string sortCodeSubstitutionsContent)
        {
            _weightTable = new ModulusWeightTable(weightTableContents);
            _sortCodeSubstitutions = new SortCodeSubstitution(sortCodeSubstitutionsContent);
        }

        public bool CheckBankAccount(string sortCode, string accountNumber)
        {
            var bankAccountDetails = new BankAccountDetails(sortCode, accountNumber);
            bankAccountDetails.WeightMappings = _weightTable.GetRuleMappings(bankAccountDetails.SortCode);
            var confirmDetailsAreValidForModulusCheck = CreateProcessingPipeline();
            return confirmDetailsAreValidForModulusCheck.Process(bankAccountDetails);
        }

        private ConfirmDetailsAreValidForModulusCheck CreateProcessingPipeline()
        {
            var firstStepRouter = new FirstStepRouter(
                new FirstStandardModulusTenCalculator(),
                new FirstStandardModulusElevenCalculator(
                    new FirstStandardModulusElevenCalculatorExceptionFive(_sortCodeSubstitutions)),
                new FirstDoubleAlternateCalculator());
            var secondModulusCalculatorStep = new SecondModulusCalculatorStep();
            var standardModulusExceptionFourteenCalculator =
                new StandardModulusExceptionFourteenCalculator(
                    new FirstStandardModulusElevenCalculatorExceptionFive(_sortCodeSubstitutions));
            var firstModulusCalculatorStep = new FirstModulusCalculatorStep(firstStepRouter, secondModulusCalculatorStep,
                standardModulusExceptionFourteenCalculator);
            var confirmDetailsAreValidForModulusCheck = new ConfirmDetailsAreValidForModulusCheck(firstModulusCalculatorStep);
            return confirmDetailsAreValidForModulusCheck;
        }
    }
}

using ModulusChecking.Loaders;
using ModulusChecking.Models;
using ModulusChecking.Properties;
using ModulusChecking.Steps;
using ModulusChecking.Steps.ConfirmDetailsAreValid;

namespace ModulusChecking
{
    public class ModulusChecker
    {
        private readonly IModulusWeightTable _weightTable;
        private readonly SortCodeSubstitution _sortCodeSubstitution;

        public ModulusChecker()
        {
            _weightTable = ModulusWeightTable.GetInstance;
            _sortCodeSubstitution = SortCodeSubstitution.GetInstance(Resources.scsubtab);
        }

        public bool CheckBankAccount(string sortCode, string accountNumber) 
            => CheckBankAccountWithExplanation(sortCode, accountNumber);

        public ModulusCheckOutcome CheckBankAccountWithExplanation(string sortCode, string accountNumber)
        {
            var bankAccountDetails = new BankAccountDetails(sortCode, accountNumber);
            bankAccountDetails.WeightMappings = _weightTable.GetRuleMappings(bankAccountDetails.SortCode);
            return new HasWeightMappings(_sortCodeSubstitution).Process(bankAccountDetails);
        }
    }
}

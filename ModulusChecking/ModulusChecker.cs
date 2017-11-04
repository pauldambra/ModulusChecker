using ModulusChecking.Loaders;
using ModulusChecking.Models;
using ModulusChecking.Steps;
using ModulusChecking.Steps.ConfirmDetailsAreValid;

namespace ModulusChecking
{
    public class ModulusChecker
    {
        private readonly IModulusWeightTable _weightTable;

        public ModulusChecker()
        {
            _weightTable = ModulusWeightTable.GetInstance;
        }

        public bool CheckBankAccount(string sortCode, string accountNumber)
        {
            var bankAccountDetails = new BankAccountDetails(sortCode, accountNumber);
            bankAccountDetails.WeightMappings = _weightTable.GetRuleMappings(bankAccountDetails.SortCode);
            return new HasWeightMappings().Process(bankAccountDetails);
        }
    }
}

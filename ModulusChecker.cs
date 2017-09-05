using ModulusChecking.Loaders;
using ModulusChecking.Models;
using ModulusChecking.Steps;

namespace ModulusChecking
{
    public class ModulusChecker
    {
        private readonly IModulusWeightTable _weightTable;

        public ModulusChecker()
        {
            _weightTable = ModulusWeightTable.GetInstance;
        }

        /// <summary>
        /// Method to check the sort code against bank account, optionally specifiy what happens when no sort code weightings are found
        /// </summary>
        /// <param name="sortCode"></param>
        /// <param name="accountNumber"></param>
        /// <param name="isNotFoundValid"></param>
        /// <returns></returns>
        public bool CheckBankAccount(string sortCode, string accountNumber, bool isNotFoundValid = true)
        {
            var bankAccountDetails = new BankAccountDetails(sortCode, accountNumber);
            bankAccountDetails.WeightMappings = _weightTable.GetRuleMappings(bankAccountDetails.SortCode);
            return new ConfirmDetailsAreValidForModulusCheck().Process(bankAccountDetails, isNotFoundValid);
        }
    }
}

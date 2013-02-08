using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public ModulusChecker(IModulusWeightTable modulusWeightTable)
        {
            _weightTable = modulusWeightTable;
        }

        public bool CheckBankAccount(string sortCode, string accountNumber)
        {
            var bankAccountDetails = new BankAccountDetails(sortCode, accountNumber);
            bankAccountDetails.WeightMappings = _weightTable.GetRuleMappings(bankAccountDetails.SortCode);
            return new ConfirmSortCodeIsValidForModulusCheck().Process(bankAccountDetails);
        }
    }
}

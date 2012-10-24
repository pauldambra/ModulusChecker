using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModulusChecking.Models;
using ModulusChecking.Parsers;
using ModulusChecking.Steps;

namespace ModulusChecking
{
    public class ModulusChecker
    {
        private readonly IModulusWeightTable _weightTable;

        public ModulusChecker()
        {
            _weightTable = new ModulusWeightTable(new ValacdosSource());
        }

        public ModulusChecker(IModulusWeightTable modulusWeightTable)
        {
            _weightTable = modulusWeightTable;
        }

        public bool CheckBankAccount(string sortCode, string accountNumber)
        {
            var bankAccountDetails = new BankAccountDetails(sortCode, accountNumber);
            return new ConfirmSortCodeIsValidForModulusCheck().Process(bankAccountDetails, _weightTable);
        }
    }
}

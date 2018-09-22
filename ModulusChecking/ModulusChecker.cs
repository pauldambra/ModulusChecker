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

        /// <summary>
        /// uses whichever version of the valacdos files were included at build time
        /// </summary>
        public ModulusChecker()
        {
            _weightTable = new ModulusWeightTable(Resources.valacdos);
            _sortCodeSubstitution = new SortCodeSubstitution(Resources.scsubtab);
        }

        /// <summary>
        /// allows provision of the contents of an arbitrary pair of valacdos files 
        /// there is no validation of these files
        /// </summary>
        public ModulusChecker(string weightMappingFileContents, string scsubtabFileContents)
        {
            _weightTable = new ModulusWeightTable(weightMappingFileContents);
            _sortCodeSubstitution = new SortCodeSubstitution(scsubtabFileContents);
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

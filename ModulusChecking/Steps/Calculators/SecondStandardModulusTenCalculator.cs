using System.Diagnostics;
using System.Linq;
using ModulusChecking.Loaders;
using ModulusChecking.Models;
using ModulusChecking.ModulusChecks;

namespace ModulusChecking.Steps.Calculators
{
    class SecondStandardModulusTenCalculator : FirstStandardModulusTenCalculator
    {

        public override bool Process(BankAccountDetails bankAccountDetails, IModulusWeightTable modulusWeightTable)
        {
            Debug.Assert(modulusWeightTable.GetRuleMappings(bankAccountDetails.SortCode).Count() == 2,
                         "The weight table passed to the Second Standard Modulus Ten Calculatot should have two entries for the given sort code");
            return ProcessWeightingRule(bankAccountDetails,modulusWeightTable.GetRuleMappings(bankAccountDetails.SortCode).ElementAt(1));
        }
    }
}
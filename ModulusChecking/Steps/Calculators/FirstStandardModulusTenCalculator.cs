using System.Diagnostics;
using System.Linq;
using ModulusChecking.Loaders;
using ModulusChecking.Models;

namespace ModulusChecking.Steps.Calculators
{
    class FirstStandardModulusTenCalculator : BaseModulusCalculator
    {
         public override bool Process(BankAccountDetails bankAccountDetails, IModulusWeightTable modulusWeightTable)
        {
            var weightRules = modulusWeightTable.GetRuleMappings(bankAccountDetails.SortCode).ToList();
            Debug.Assert(weightRules.Count == 1 || weightRules.Count == 2, string.Format("There are {0} weight rules and there should only be 1 or 2", weightRules.Count));
            return ProcessWeightingRule(bankAccountDetails, weightRules.First());
        }
    }
}
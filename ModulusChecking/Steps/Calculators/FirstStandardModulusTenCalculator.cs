using System.Linq;
using ModulusChecking.Models;
using ModulusChecking.Parsers;

namespace ModulusChecking.Steps.Calculators
{
    class FirstStandardModulusTenCalculator : BaseModulusCalculator
    {
         public override bool Process(BankAccountDetails bankAccountDetails, IModulusWeightTable modulusWeightTable)
        {
            var weightRules = modulusWeightTable.GetRuleMappings(bankAccountDetails.SortCode).ToList();

            if (weightRules.Count() == 1)
            {
                return ProcessWeightingRule(bankAccountDetails, weightRules.First());
            }
            
            var firstRule = modulusWeightTable.GetRuleMappings(bankAccountDetails.SortCode).First();        
            var firstResult = ProcessWeightingRule(bankAccountDetails, firstRule);

            return firstResult;
        }
    }
}
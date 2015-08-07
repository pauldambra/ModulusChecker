using System.Linq;
using ModulusChecking.Models;

namespace ModulusChecking.Steps.Calculators
{
    internal class FirstStandardModulusTenCalculator : BaseModulusCalculator
    {
         public override bool Process(BankAccountDetails bankAccountDetails)
        {
            return ProcessWeightingRule(bankAccountDetails, bankAccountDetails.WeightMappings.First());
        }
    }
}
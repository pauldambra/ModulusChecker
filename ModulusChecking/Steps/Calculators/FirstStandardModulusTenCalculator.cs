using System.Linq;
using ModulusChecking.Models;
using ModulusChecking.ModulusChecks;
using ModulusChecking.Parsers;

namespace ModulusChecking.Steps.Calculators
{
    public class FirstStandardModulusTenCalculator : BaseModulusCalculator
    {
         public override bool Process(BankAccountDetails bankAccountDetails, ModulusWeights modulusWeights)
        {
            var weightRules = modulusWeights.GetRuleMappings(bankAccountDetails.SortCode).ToList();

            if (weightRules.Count() == 1)
            {
                return ProcessWeightingRule(bankAccountDetails, weightRules.First());
            }
            
            var firstRule = modulusWeights.GetRuleMappings(bankAccountDetails.SortCode).First();        
            var firstResult = ProcessWeightingRule(bankAccountDetails, firstRule);

            return firstResult;
        }

        private bool ProcessWeightingRule(BankAccountDetails bankAccountDetails, ModulusWeightMapping modulusWeightMapping)
        {
            var weightingSum = new StandardModulusCheck().GetModulusSum(bankAccountDetails,modulusWeightMapping.WeightValues);
            var remainder = weightingSum%Modulus;
            return modulusWeightMapping.Exception == 4 
                ? bankAccountDetails.AccountNumber.GetExceptionFourCheckValue == remainder
                : remainder == 0;
        }
    }
}
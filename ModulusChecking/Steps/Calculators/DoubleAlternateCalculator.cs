using System.Linq;
using ModulusChecking.Models;
using ModulusChecking.ModulusChecks;
using ModulusChecking.Parsers;

namespace ModulusChecking.Steps.Calculators
{
    public class DoubleAlternateCalculator : BaseModulusCalculator
    {
        public override bool Process(BankAccountDetails bankAccountDetails, ModulusWeights modulusWeights)
        {
            var modulusWeightMapping = modulusWeights.GetRuleMappings(bankAccountDetails.SortCode).First();
            var weightingSum = new DoubleAlternateModulusCheck().GetModulusSum(bankAccountDetails,
                                                                               modulusWeightMapping.WeightValues,
                                                                               modulusWeightMapping.Exception);
            var remainder = weightingSum % Modulus;
            return remainder == 0;
        }
    }
}
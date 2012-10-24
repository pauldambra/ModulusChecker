using System.Collections.Generic;
using System.Linq;
using ModulusChecking.Models;
using ModulusChecking.ModulusChecks;
using ModulusChecking.Parsers;

namespace ModulusChecking.Steps.Calculators
{
    public class DoubleAlternateCalculator : BaseModulusCalculator
    {
        private readonly Step _step;

        public DoubleAlternateCalculator(Step step)
        {
            _step = step;
        }

        public override bool Process(BankAccountDetails bankAccountDetails, ModulusWeights modulusWeights)
        {
            var modulusWeightMappings = modulusWeights.GetRuleMappings(bankAccountDetails.SortCode);
            var weightMappings = modulusWeightMappings as List<ModulusWeightMapping> ?? modulusWeightMappings.ToList();
            ModulusWeightMapping modulusWeightMapping = weightMappings.Count()==2 ? weightMappings.ElementAt((int) _step) : weightMappings.First();
            var weightingSum = new DoubleAlternateModulusCheck().GetModulusSum(bankAccountDetails,
                                                                               modulusWeightMapping.WeightValues,
                                                                               modulusWeightMapping.Exception);
            var remainder = weightingSum % Modulus;
            return remainder == 0;
        }
    }
}
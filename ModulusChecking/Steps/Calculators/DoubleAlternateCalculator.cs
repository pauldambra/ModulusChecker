using System.Collections.Generic;
using System.Linq;
using ModulusChecking.Loaders;
using ModulusChecking.Models;
using ModulusChecking.ModulusChecks;

namespace ModulusChecking.Steps.Calculators
{
    class DoubleAlternateCalculator : BaseModulusCalculator
    {
        private readonly Step _step;

        public DoubleAlternateCalculator(Step step)
        {
            _step = step;
        }

        public override bool Process(BankAccountDetails bankAccountDetails, IModulusWeightTable modulusWeightTable)
        {
            var modulusWeightMappings = modulusWeightTable.GetRuleMappings(bankAccountDetails.SortCode);
            var weightMappings = modulusWeightMappings as List<IModulusWeightMapping> ?? modulusWeightMappings.ToList();
            var modulusWeightMapping = weightMappings.Count()==2 ? weightMappings.ElementAt((int) _step) : weightMappings.First();
            var weightingSum = new DoubleAlternateModulusCheck().GetModulusSum(bankAccountDetails,
                                                                               modulusWeightMapping.WeightValues,
                                                                               modulusWeightMapping.Exception);
            var remainder = weightingSum % Modulus;
            return remainder == 0;
        }
    }
}
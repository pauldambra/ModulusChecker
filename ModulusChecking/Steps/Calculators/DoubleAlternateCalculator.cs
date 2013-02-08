using System;
using System.Collections.Generic;
using System.Linq;
using ModulusChecking.Loaders;
using ModulusChecking.Models;
using ModulusChecking.ModulusChecks;

namespace ModulusChecking.Steps.Calculators
{
    class DoubleAlternateCalculator : BaseModulusCalculator
    {
        private readonly ModulusWeightMapping.Step _step;

        public DoubleAlternateCalculator(ModulusWeightMapping.Step step)
        {
            _step = step;
        }

        public override bool Process(BankAccountDetails bankAccountDetails)
        {
            ValidateEnoughMappingRulesForStepCount(bankAccountDetails, _step);

            var modulusWeightMapping = bankAccountDetails.WeightMappings.ElementAt((int) _step);
            var weightingSum = new DoubleAlternateModulusCheck().GetModulusSum(bankAccountDetails,
                                                                               modulusWeightMapping.WeightValues,
                                                                               modulusWeightMapping.Exception);
            var remainder = weightingSum % Modulus;
            return remainder == 0;
        }
    }
}
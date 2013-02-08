using System.Diagnostics;
using System.Linq;
using ModulusChecking.Loaders;
using ModulusChecking.Models;
using ModulusChecking.ModulusChecks;

namespace ModulusChecking.Steps.Calculators
{
    class SecondStandardModulusTenCalculator : FirstStandardModulusTenCalculator
    {

        public override bool Process(BankAccountDetails bankAccountDetails)
        {
            ValidateEnoughMappingRulesForStepCount(bankAccountDetails, ModulusWeightMapping.Step.Second);
            return ProcessWeightingRule(bankAccountDetails,
                                        bankAccountDetails.WeightMappings.ElementAt(
                                            (int) ModulusWeightMapping.Step.Second)
                                            );
        }
    }
}
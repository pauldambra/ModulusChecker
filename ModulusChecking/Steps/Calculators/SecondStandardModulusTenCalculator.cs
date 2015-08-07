using System;
using System.Linq;
using ModulusChecking.Models;

namespace ModulusChecking.Steps.Calculators
{
    internal class SecondStandardModulusTenCalculator : FirstStandardModulusTenCalculator
    {
        public override bool Process(BankAccountDetails bankAccountDetails)
        {
            if (bankAccountDetails.WeightMappings.Count() != 2)
            {
                throw new ArgumentException(
                    "Second Step Check must be passed bank details with two weight mapping rules");
            } 
            return ProcessWeightingRule(bankAccountDetails,
                                        bankAccountDetails.WeightMappings.Second());
        }
    }
}
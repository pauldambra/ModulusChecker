using System;
using System.Collections.Generic;
using System.Linq;
using ModulusChecking.Loaders;
using ModulusChecking.Models;
using ModulusChecking.Properties;
using ModulusChecking.Steps.Calculators;

namespace ModulusChecking.Steps
{
    internal class FirstStepRouter
    {
        private readonly FirstStandardModulusTenCalculator _firstStandardModulusTenCalculator;
        private readonly FirstStandardModulusElevenCalculator _firstStandardModulusElevenCalculator;
        private readonly FirstDoubleAlternateCalculator _doubleAlternateCalculator;

        private Dictionary<ModulusAlgorithm, Func<BankAccountDetails, bool>> _firstStepModulusChoices;

        public FirstStepRouter(SortCodeSubstitution sortCodeSubstitution)
        {
            _firstStandardModulusTenCalculator = new FirstStandardModulusTenCalculator();
            var firstStandardModulusElevenCalculatorExceptionFive = new FirstStandardModulusElevenCalculatorExceptionFive(sortCodeSubstitution);
            _firstStandardModulusElevenCalculator = new FirstStandardModulusElevenCalculator(firstStandardModulusElevenCalculatorExceptionFive);
            _doubleAlternateCalculator = new FirstDoubleAlternateCalculator(new FirstDoubleAlternateCalculatorExceptionFive());
            InitialiseRoutingDictionary();
        }

        public FirstStepRouter(FirstStandardModulusTenCalculator st, FirstStandardModulusElevenCalculator se,
                               FirstDoubleAlternateCalculator da)
        {
            _firstStandardModulusTenCalculator = st;
            _firstStandardModulusElevenCalculator = se;
            _doubleAlternateCalculator = da;
            InitialiseRoutingDictionary();
        }

        private void InitialiseRoutingDictionary()
        {
            _firstStepModulusChoices = new Dictionary<ModulusAlgorithm, Func<BankAccountDetails, bool>>
                                           {
                                               {
                                                   ModulusAlgorithm
                                                   .Mod10,
                                                   _firstStandardModulusTenCalculator
                                                   .Process
                                               },
                                               {ModulusAlgorithm.Mod11, _firstStandardModulusElevenCalculator.Process},
                                               {ModulusAlgorithm.DblAl, _doubleAlternateCalculator.Process}
                                           };
        }

        public bool GetModulusCalculation(BankAccountDetails bankAccountDetails)
        {
            Func<BankAccountDetails, bool> modulusChoice;
            if (_firstStepModulusChoices.TryGetValue(bankAccountDetails.WeightMappings.First().Algorithm,
                                                     out modulusChoice))
            {
                return modulusChoice(bankAccountDetails);
            }
            throw new InvalidOperationException("The routing dictionary had no match for the modulus algorithm provided");
        }
    }
}

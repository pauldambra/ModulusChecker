using System;
using System.Collections.Generic;
using ModulusChecking.Models;
using ModulusChecking.Steps.Calculators;

namespace ModulusChecking.Steps
{
    internal class SecondStepRouter
    {
        private readonly SecondStandardModulusTenCalculator _secondStandardModulusTenCalculator;
        private readonly SecondStandardModulusElevenCalculator _secondStandardModulusElevenCalculator;
        private readonly SecondDoubleAlternateCalculator _secondDoubleAlternateCalculator;

        private Dictionary<ModulusAlgorithm, Func<BankAccountDetails, bool>> _secondStepModulusChoices;

        public SecondStepRouter()
        {
            _secondStandardModulusTenCalculator = new SecondStandardModulusTenCalculator();
            _secondStandardModulusElevenCalculator = new SecondStandardModulusElevenCalculator();
            _secondDoubleAlternateCalculator = new SecondDoubleAlternateCalculator(new SecondDoubleAlternateCalculatorExceptionFive());
            InitialiseRoutingDictionary();
        }

        public SecondStepRouter(SecondStandardModulusTenCalculator smtc, SecondStandardModulusElevenCalculator smte,
                                SecondDoubleAlternateCalculator sdac)
        {
            _secondStandardModulusTenCalculator = smtc;
            _secondStandardModulusElevenCalculator = smte;
            _secondDoubleAlternateCalculator = sdac;
            InitialiseRoutingDictionary();
        }

        private void InitialiseRoutingDictionary()
        {
            _secondStepModulusChoices = new Dictionary<ModulusAlgorithm, Func<BankAccountDetails, bool>>
                                            {
                                                {ModulusAlgorithm.Mod10, _secondStandardModulusTenCalculator.Process},
                                                {ModulusAlgorithm.Mod11, _secondStandardModulusElevenCalculator.Process},
                                                {ModulusAlgorithm.DblAl, _secondDoubleAlternateCalculator.Process}
                                            };
        }

        public bool GetModulusCalculation(BankAccountDetails bankAccountDetails)
        {
            Func<BankAccountDetails, bool> modulusChoice;
            if (_secondStepModulusChoices.TryGetValue(bankAccountDetails.WeightMappings.Second().Algorithm,
                                                     out modulusChoice))
            {
                return modulusChoice(bankAccountDetails);
            }
            throw new InvalidOperationException("The routing dictionary had no match for the modulus algorithm provided");
        }
    }
}

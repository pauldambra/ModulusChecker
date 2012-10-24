using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModulusChecking.Models;
using ModulusChecking.Parsers;
using ModulusChecking.Steps.Calculators;
using NUnit.Framework;

namespace ModulusCheckingTests.Rules.Calculators
{
    public class FirstStandardModulusElevenCalculatorTests
    {
        private FirstStandardModulusElevenCalculator _calculator;

        [SetUp]
        public void Setup()
        {
            _calculator = new FirstStandardModulusElevenCalculator();
        }

        [Test]
        public void ExceptionThreeWhereCisNeitherSixNorNine()
        {
            var accountDetails = new BankAccountDetails("827101", "28748352");
            var result = _calculator.Process(accountDetails, new ModulusWeights(new ValacdosSource()));
            Assert.IsTrue(result);
        }

        [Test]
        public void CanPassBasicModulus11Test()
        {
            var accountDetails = new BankAccountDetails("202959", "63748472");
            var result = _calculator.Process(accountDetails, new ModulusWeights(new ValacdosSource()));
            Assert.IsTrue(result);
        }
    }
}

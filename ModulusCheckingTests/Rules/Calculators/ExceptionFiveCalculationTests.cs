using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModulusChecking.Loaders;
using ModulusChecking.Models;
using ModulusChecking.Steps;
using ModulusChecking.Steps.Calculators;
using NUnit.Framework;

namespace ModulusCheckingTests.Rules.Calculators
{
    public class ExceptionFiveCalculationTests
    {
        private DoubleAlternateCalculatorExceptionFive _doubleAlternateExceptionFiveCalculator;
        private FirstStandardModulusElevenCalculatorExceptionFive _standardExceptionFiveCalculator;

        [SetUp]
        public void Setup()
        {
            _doubleAlternateExceptionFiveCalculator =
                new DoubleAlternateCalculatorExceptionFive(ModulusWeightMapping.Step.Second);
            _standardExceptionFiveCalculator = new FirstStandardModulusElevenCalculatorExceptionFive();
        }

        [Test]
        public void CanCalculateForExceptionFiveWhereCheckPasses()
        {
            var accountDetails = new BankAccountDetails("938611", "07806039");
            accountDetails.WeightMappings = ModulusWeightTable.GetInstance.GetRuleMappings(accountDetails.SortCode);
            var standardResult = _standardExceptionFiveCalculator.Process(accountDetails);
            var doubleResult = _doubleAlternateExceptionFiveCalculator.Process(accountDetails);
            Assert.IsTrue(standardResult);
            Assert.IsTrue(doubleResult);
        }

        [Test]
        public void CanCalculateForExceptionFiveWhereCheckPassesWithSubstitution()
        {
            var accountDetails = new BankAccountDetails("938600", "42368003");
            accountDetails.WeightMappings = ModulusWeightTable.GetInstance.GetRuleMappings(accountDetails.SortCode);
            var standardResult = _standardExceptionFiveCalculator.Process(accountDetails);
            var doubleResult = _doubleAlternateExceptionFiveCalculator.Process(accountDetails);
            Assert.IsTrue(standardResult);
            Assert.IsTrue(doubleResult);
        }

        [Test]
        public void CanCalculateForExceptionFiveWhereBothChecksPass()
        {
            var accountDetails = new BankAccountDetails("938063", "55065200");
            accountDetails.WeightMappings = ModulusWeightTable.GetInstance.GetRuleMappings(accountDetails.SortCode);
            var standardResult = _standardExceptionFiveCalculator.Process(accountDetails);
            var doubleResult = _doubleAlternateExceptionFiveCalculator.Process(accountDetails);
            Assert.IsTrue(standardResult);
            Assert.IsTrue(doubleResult);
        }


        [Test]
        public void CanCalculateForExceptionFiveWhereFirstCheckDigitIsCorrectAndSecondIncorrect()
        {
            var accountDetails = new BankAccountDetails("938063", "15764273");
            accountDetails.WeightMappings = ModulusWeightTable.GetInstance.GetRuleMappings(accountDetails.SortCode);
            var standardResult = _standardExceptionFiveCalculator.Process(accountDetails);
            var doubleResult = _doubleAlternateExceptionFiveCalculator.Process(accountDetails);
            Assert.IsTrue(standardResult);
            Assert.IsFalse(doubleResult);
        }

        [Test]
        public void CanCalculateForExceptionFiveWhereFirstCheckDigitIsIncorrectAndSecondIsCorrect()
        {
            var accountDetails = new BankAccountDetails("938063", "15764264");
            accountDetails.WeightMappings = ModulusWeightTable.GetInstance.GetRuleMappings(accountDetails.SortCode);
            var standardResult = _standardExceptionFiveCalculator.Process(accountDetails);
            var doubleResult = _doubleAlternateExceptionFiveCalculator.Process(accountDetails);
            Assert.IsFalse(standardResult);
            Assert.IsTrue(doubleResult);
        }

        [Test]
        public void CanCalculateForExceptionFiveWhereFirstCheckDigitIsIncorrectWithARemainderOfOne()
        {
            var accountDetails = new BankAccountDetails("938063", "15763217");
            accountDetails.WeightMappings = ModulusWeightTable.GetInstance.GetRuleMappings(accountDetails.SortCode);
            var result = _standardExceptionFiveCalculator.Process(accountDetails);
            var doubleResult = _doubleAlternateExceptionFiveCalculator.Process(accountDetails);
            Assert.IsFalse(result);
            Assert.IsTrue(doubleResult);
        }
    }
}

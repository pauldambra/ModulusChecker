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
            _doubleAlternateExceptionFiveCalculator = new DoubleAlternateCalculatorExceptionFive(BaseModulusCalculator.Step.Second);
            _standardExceptionFiveCalculator = new FirstStandardModulusElevenCalculatorExceptionFive();
        }

        [Test]
        public void CanCalculateForExceptionFiveWhereCheckPasses()
        {
            var accountDetails = new BankAccountDetails("938611", "07806039");
            var standardResult = _standardExceptionFiveCalculator.Process(accountDetails, ModulusWeightTable.GetInstance);
            var doubleResult = _doubleAlternateExceptionFiveCalculator.Process(accountDetails,
                                                                               ModulusWeightTable.GetInstance);
            Assert.IsTrue(standardResult);
            Assert.IsTrue(doubleResult);
        }

        [Test]
        public void CanCalculateForExceptionFiveWhereCheckPassesWithSubstitution()
        {
            var accountDetails = new BankAccountDetails("938600", "42368003");
            var standardResult = _standardExceptionFiveCalculator.Process(accountDetails, ModulusWeightTable.GetInstance);
            var doubleResult = _doubleAlternateExceptionFiveCalculator.Process(accountDetails,
                                                                               ModulusWeightTable.GetInstance);
            Assert.IsTrue(standardResult);
            Assert.IsTrue(doubleResult);
        }

        [Test]
        public void CanCalculateForExceptionFiveWhereBothChecksPass()
        {
            var accountDetails = new BankAccountDetails("938063", "55065200");
            var standardResult = _standardExceptionFiveCalculator.Process(accountDetails, ModulusWeightTable.GetInstance);
            var doubleResult = _doubleAlternateExceptionFiveCalculator.Process(accountDetails,
                                                                               ModulusWeightTable.GetInstance);
            Assert.IsTrue(standardResult);
            Assert.IsTrue(doubleResult);
        }


        [Test]
        public void CanCalculateForExceptionFiveWhereFirstCheckDigitIsCorrectAndSecondIncorrect()
        {
            var accountDetails = new BankAccountDetails("938063", "15764273");
            var standardResult = _standardExceptionFiveCalculator.Process(accountDetails, ModulusWeightTable.GetInstance);
            var doubleResult = _doubleAlternateExceptionFiveCalculator.Process(accountDetails,
                                                                               ModulusWeightTable.GetInstance);
            Assert.IsTrue(standardResult);
            Assert.IsFalse(doubleResult);
        }

        [Test]
        public void CanCalculateForExceptionFiveWhereFirstCheckDigitIsIncorrectAndSecondIsCorrect()
        {
            var accountDetails = new BankAccountDetails("938063", "15764264");
            var standardResult = _standardExceptionFiveCalculator.Process(accountDetails, ModulusWeightTable.GetInstance);
            var doubleResult = _doubleAlternateExceptionFiveCalculator.Process(accountDetails,
                                                                               ModulusWeightTable.GetInstance);
            Assert.IsFalse(standardResult);
            Assert.IsTrue(doubleResult);
        }

        [Test]
        public void CanCalculateForExceptionFiveWhereFirstCheckDigitIsIncorrectWithARemainderOfOne()
        {
            var accountDetails = new BankAccountDetails("938063", "15763217");
            var result = _standardExceptionFiveCalculator.Process(accountDetails, ModulusWeightTable.GetInstance);
            var doubleResult = _doubleAlternateExceptionFiveCalculator.Process(accountDetails,
                                                                   ModulusWeightTable.GetInstance);
            Assert.IsFalse(result);
            Assert.IsTrue(doubleResult);
        }
}
}

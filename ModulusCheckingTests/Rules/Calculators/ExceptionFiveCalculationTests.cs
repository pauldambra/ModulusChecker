using ModulusChecking.Loaders;
using ModulusChecking.Models;
using ModulusChecking.Properties;
using ModulusChecking.Steps.Calculators;
using NUnit.Framework;

namespace ModulusCheckingTests.Rules.Calculators
{
    public class ExceptionFiveCalculationTests
    {
        private DoubleAlternateCalculatorExceptionFive _secondDoubleAlternateExceptionFiveCalculator;
        private FirstStandardModulusElevenCalculatorExceptionFive _standardExceptionFiveCalculator;

        [SetUp]
        public void Setup()
        {
            _secondDoubleAlternateExceptionFiveCalculator =
                new SecondDoubleAlternateCalculatorExceptionFive();
            _standardExceptionFiveCalculator = new FirstStandardModulusElevenCalculatorExceptionFive(new SortCodeSubstitution(Resources.scsubtab));
        }

        [Test]
        public void CanCalculateForExceptionFiveWhereCheckPasses()
        {
            var accountDetails = new BankAccountDetails("938611", "07806039");
            accountDetails.WeightMappings = new ModulusWeightTable(Resources.valacdos).GetRuleMappings(accountDetails.SortCode);
            var standardResult = _standardExceptionFiveCalculator.Process(accountDetails);
            var doubleResult = _secondDoubleAlternateExceptionFiveCalculator.Process(accountDetails);
            Assert.IsTrue(standardResult);
            Assert.IsTrue(doubleResult);
        }

        [Test]
        public void CanCalculateForExceptionFiveWhereCheckPassesWithSubstitution()
        {
            var accountDetails = new BankAccountDetails("938600", "42368003");
            accountDetails.WeightMappings = new ModulusWeightTable(Resources.valacdos).GetRuleMappings(accountDetails.SortCode);
            var standardResult = _standardExceptionFiveCalculator.Process(accountDetails);
            var doubleResult = _secondDoubleAlternateExceptionFiveCalculator.Process(accountDetails);
            Assert.IsTrue(standardResult);
            Assert.IsTrue(doubleResult);
        }

        [Test]
        public void CanCalculateForExceptionFiveWhereBothChecksPass()
        {
            var accountDetails = new BankAccountDetails("938063", "55065200");
            accountDetails.WeightMappings = new ModulusWeightTable(Resources.valacdos).GetRuleMappings(accountDetails.SortCode);
            var standardResult = _standardExceptionFiveCalculator.Process(accountDetails);
            var doubleResult = _secondDoubleAlternateExceptionFiveCalculator.Process(accountDetails);
            Assert.IsTrue(standardResult);
            Assert.IsTrue(doubleResult);
        }


        [Test]
        public void CanCalculateForExceptionFiveWhereFirstCheckDigitIsCorrectAndSecondIncorrect()
        {
            var accountDetails = new BankAccountDetails("938063", "15764273");
            accountDetails.WeightMappings = new ModulusWeightTable(Resources.valacdos).GetRuleMappings(accountDetails.SortCode);
            var standardResult = _standardExceptionFiveCalculator.Process(accountDetails);
            var doubleResult = _secondDoubleAlternateExceptionFiveCalculator.Process(accountDetails);
            Assert.IsTrue(standardResult);
            Assert.IsFalse(doubleResult);
        }

        [Test]
        public void CanCalculateForExceptionFiveWhereFirstCheckDigitIsIncorrectAndSecondIsCorrect()
        {
            var accountDetails = new BankAccountDetails("938063", "15764264");
            accountDetails.WeightMappings = new ModulusWeightTable(Resources.valacdos).GetRuleMappings(accountDetails.SortCode);
            var standardResult = _standardExceptionFiveCalculator.Process(accountDetails);
            var doubleResult = _secondDoubleAlternateExceptionFiveCalculator.Process(accountDetails);
            Assert.IsFalse(standardResult);
            Assert.IsTrue(doubleResult);
        }

        [Test]
        public void CanCalculateForExceptionFiveWhereFirstCheckDigitIsIncorrectWithARemainderOfOne()
        {
            var accountDetails = new BankAccountDetails("938063", "15763217");
            accountDetails.WeightMappings = new ModulusWeightTable(Resources.valacdos).GetRuleMappings(accountDetails.SortCode);
            var result = _standardExceptionFiveCalculator.Process(accountDetails);
            var doubleResult = _secondDoubleAlternateExceptionFiveCalculator.Process(accountDetails);
            Assert.IsFalse(result);
            Assert.IsTrue(doubleResult);
        }
    }
}

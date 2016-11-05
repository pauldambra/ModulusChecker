using ModulusChecking.Models;
using ModulusChecking.Steps;
using ModulusChecking.Steps.Calculators;
using NUnit.Framework;
using Moq;

namespace ModulusCheckingTests.Rules
{
    internal class SecondStepRouterTests
    {
        private SecondStepRouter _targetRouter;
        private Mock<SecondDoubleAlternateCalculator> _mockSecondDoubleAlternator;
        private Mock<SecondStandardModulusElevenCalculator> _mockSecondStandardElevenCalculator;
        private Mock<SecondStandardModulusTenCalculator> _mockSecondStandardTenCalculator;
        private Mock<SecondDoubleAlternateCalculatorExceptionFive> _mockSecondDoubleAlternateExceptionFiveCalculator;
        private SecondStepRouter _targetRouterForExceptionFive;

        [SetUp]
        public void Setup()
        {
            _mockSecondStandardTenCalculator = new Mock<SecondStandardModulusTenCalculator>();
            _mockSecondStandardElevenCalculator =
                new Mock<SecondStandardModulusElevenCalculator>();
            _mockSecondDoubleAlternateExceptionFiveCalculator =
                new Mock<SecondDoubleAlternateCalculatorExceptionFive>();
            _mockSecondDoubleAlternator =
                new Mock<SecondDoubleAlternateCalculator>(_mockSecondDoubleAlternateExceptionFiveCalculator
                                                              .Object);
            _targetRouter = new SecondStepRouter(_mockSecondStandardTenCalculator.Object,
                                                 _mockSecondStandardElevenCalculator.Object,
                                                 _mockSecondDoubleAlternator.Object);
            _targetRouterForExceptionFive = new SecondStepRouter(_mockSecondStandardTenCalculator.Object,
                                                                 new SecondStandardModulusElevenCalculator(),
                                                                 new SecondDoubleAlternateCalculator(
                                                                     _mockSecondDoubleAlternateExceptionFiveCalculator
                                                                         .Object));
        }

        [Test]
        public void CanProcessModulusTen()
        {
            var bankDetails = new BankAccountDetails("123456", "12345678")
            {
                WeightMappings = new[]
                {
                    ModulusWeightMapping.From(
                        "090150 090156 MOD10    0    0    0    0    0    9    8    7    6    5    4    3    2    1"),
                    ModulusWeightMapping.From(
                        "090150 090156 MOD10    0    0    0    0    0    9    8    7    6    5    4    3    2    1")
                }
            };
            _targetRouter.GetModulusCalculation(bankDetails);
            _mockSecondStandardTenCalculator.Verify(calc=>calc.Process(bankDetails));
        }

        [Test]
        public void CanProcessModulusEleven()
        {
            var bankDetails = new BankAccountDetails("123456", "12345678")
            {
                WeightMappings = new[]
                {
                    ModulusWeightMapping.From(
                        "090150 090156 MOD11    0    0    0    0    0    9    8    7    6    5    4    3    2    1"),
                    ModulusWeightMapping.From(
                        "090150 090156 MOD11    0    0    0    0    0    9    8    7    6    5    4    3    2    1")
                }
            };
            _targetRouter.GetModulusCalculation(bankDetails);
            _mockSecondStandardElevenCalculator.Verify(calc => calc.Process(bankDetails));
        }

        [Test]
        public void CanProcessDoubleAlternate()
        {
            var bankDetails = new BankAccountDetails("123456", "12345678")
            {
                WeightMappings = new[]
                {
                    ModulusWeightMapping.From(
                        "090150 090156 DBLAL    0    0    0    0    0    9    8    7    6    5    4    3    2    1"),
                    ModulusWeightMapping.From(
                        "090150 090156 DBLAL    0    0    0    0    0    9    8    7    6    5    4    3    2    1")
                }
            };
            _targetRouter.GetModulusCalculation(bankDetails);
            _mockSecondDoubleAlternator.Verify(calc => calc.Process(bankDetails));
        }

        [Test]
        public void CanProcessDoubleAlternateWithExceptionFive()
        {
            var bankDetails = new BankAccountDetails("123456", "12345678")
            {
                WeightMappings = new[]
                {
                    ModulusWeightMapping.From(
                        "090150 090156 DBLAL    0    0    0    0    0    9    8    7    6    5    4    3    2    1    5"),
                    ModulusWeightMapping.From(
                        "090150 090156 DBLAL    0    0    0    0    0    9    8    7    6    5    4    3    2    1    5")
                }
            };
            _targetRouterForExceptionFive.GetModulusCalculation(bankDetails);
            _mockSecondDoubleAlternateExceptionFiveCalculator.Verify(calc => calc.Process(bankDetails));
        }
    }
}

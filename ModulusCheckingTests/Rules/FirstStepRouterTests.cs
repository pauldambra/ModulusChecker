using ModulusChecking.Models;
using ModulusChecking.Steps;
using ModulusChecking.Steps.Calculators;
using NUnit.Framework;
using Moq;

namespace ModulusCheckingTests.Rules
{
    internal class FirstStepRouterTests
    {
        private FirstStepRouter _targetRouter;
        private Mock<FirstDoubleAlternateCalculator> _mockFirstDoubleAlternator;
        private Mock<FirstStandardModulusElevenCalculator> _mockFirstStandardElevenCalculator;
        private Mock<FirstStandardModulusTenCalculator> _mockFirstStandardTenCalculator;
        private Mock<FirstStandardModulusElevenCalculatorExceptionFive> _mockFirstStandardElevenExceptionFiveCalculator;
        private Mock<FirstDoubleAlternateCalculatorExceptionFive> _mockFirstDoubleAlternateExceptionFiveCalculator;
        private FirstStepRouter _targetRouterForExceptionFive;

        [SetUp]
        public void Setup()
        {
            _mockFirstStandardTenCalculator = new Mock<FirstStandardModulusTenCalculator>();
            _mockFirstStandardElevenExceptionFiveCalculator =
                new Mock<FirstStandardModulusElevenCalculatorExceptionFive>(null);
            _mockFirstStandardElevenCalculator =
                new Mock<FirstStandardModulusElevenCalculator>(_mockFirstStandardElevenExceptionFiveCalculator.Object);
            _mockFirstDoubleAlternateExceptionFiveCalculator =
                new Mock<FirstDoubleAlternateCalculatorExceptionFive>();
            _mockFirstDoubleAlternator = new Mock<FirstDoubleAlternateCalculator>(_mockFirstDoubleAlternateExceptionFiveCalculator
                                                                                 .Object);
            _targetRouter = new FirstStepRouter(_mockFirstStandardTenCalculator.Object,
                                                _mockFirstStandardElevenCalculator.Object,
                                                _mockFirstDoubleAlternator.Object);
            _targetRouterForExceptionFive = new FirstStepRouter(_mockFirstStandardTenCalculator.Object,
                                                                new FirstStandardModulusElevenCalculator(
                                                                    _mockFirstStandardElevenExceptionFiveCalculator
                                                                        .Object),
                                                                new FirstDoubleAlternateCalculator(
                                                                    _mockFirstDoubleAlternateExceptionFiveCalculator
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
                        "090150 090156 MOD10    0    0    0    0    0    9    8    7    6    5    4    3    2    1")
                }
            };
            _targetRouter.GetModulusCalculation(bankDetails);
            _mockFirstStandardTenCalculator.Verify(calc=>calc.Process(bankDetails));
        }

        [Test]
        public void CanProcessModulusEleven()
        {
            var bankDetails = new BankAccountDetails("123456", "12345678")
            {
                WeightMappings = new[]
                {
                    ModulusWeightMapping.From(
                        "090150 090156 MOD11    0    0    0    0    0    9    8    7    6    5    4    3    2    1")
                }
            };
            _targetRouter.GetModulusCalculation(bankDetails);
            _mockFirstStandardElevenCalculator.Verify(calc => calc.Process(bankDetails));
        }

        [Test]
        public void CanProcessModulusElevenExceptionFive()
        {
            var bankDetails = new BankAccountDetails("123456", "12345678")
            {
                WeightMappings = new[]
                {
                    ModulusWeightMapping.From(
                        "090150 090156 MOD11    0    0    0    0    0    9    8    7    6    5    4    3    2    1    5")
                }
            };
            _targetRouterForExceptionFive.GetModulusCalculation(bankDetails);
            _mockFirstStandardElevenExceptionFiveCalculator.Verify(calc => calc.Process(bankDetails));
        }

        [Test]
        public void CanProcessDoubleAlternate()
        {
            var bankDetails = new BankAccountDetails("123456", "12345678")
            {
                WeightMappings = new[]
                {
                    ModulusWeightMapping.From(
                        "090150 090156 DBLAL    0    0    0    0    0    9    8    7    6    5    4    3    2    1")
                }
            };
            _targetRouter.GetModulusCalculation(bankDetails);
            _mockFirstDoubleAlternator.Verify(calc => calc.Process(bankDetails));
        }

        [Test]
        public void CanProcessDoubleAlternateWithExceptionFive()
        {
            var bankDetails = new BankAccountDetails("123456", "12345678")
            {
                WeightMappings = new[]
                {
                    ModulusWeightMapping.From(
                        "090150 090156 DBLAL    0    0    0    0    0    9    8    7    6    5    4    3    2    1    5")
                }
            };
            _targetRouterForExceptionFive.GetModulusCalculation(bankDetails);
            _mockFirstDoubleAlternateExceptionFiveCalculator.Verify(calc => calc.Process(bankDetails));
        }
    }
}

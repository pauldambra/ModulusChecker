using ModulusChecking.Loaders;
using ModulusChecking.Models;
using ModulusChecking.Properties;
using ModulusChecking.Steps;
using ModulusChecking.Steps.Calculators;
using Moq;
using NUnit.Framework;

namespace ModulusCheckingTests.Rules
{
    public class FirstModulusCalculatorStepTests
    {
        private Mock<FirstDoubleAlternateCalculator> _firstDoubleAlternate;
        private Mock<FirstDoubleAlternateCalculatorExceptionFive> _firstDoubleAlternateExceptionFive;
        private Mock<StandardModulusExceptionFourteenCalculator> _exceptionFourteenCalculator;
        private Mock<IModulusWeightTable> _mockModulusWeightTable;
        private Mock<SecondModulusCalculatorStep> _secondModulusCalculation;
        private Mock<FirstStandardModulusElevenCalculator> _standardEleven;
        private Mock<FirstStandardModulusElevenCalculatorExceptionFive> _standardExceptionFive;
        private Mock<FirstStandardModulusTenCalculator> _standardTen;
        private Mock<FirstStepRouter> _firstStepRouter;
        private BankAccountDetails _exceptionFourteenPassesFirstTime;
        private BankAccountDetails _exceptionFourteenPassesSecondTime;

        [SetUp]
        public void Before()
        {
            _standardTen = new Mock<FirstStandardModulusTenCalculator>();
            _standardEleven = new Mock<FirstStandardModulusElevenCalculator>(new FirstStandardModulusElevenCalculatorExceptionFive(new SortCodeSubstitution(Resources.scsubtab)));
            _firstDoubleAlternate = new Mock<FirstDoubleAlternateCalculator>();
            _standardExceptionFive = new Mock<FirstStandardModulusElevenCalculatorExceptionFive>();
            _secondModulusCalculation = new Mock<SecondModulusCalculatorStep>();
            _firstDoubleAlternateExceptionFive =
                new Mock<FirstDoubleAlternateCalculatorExceptionFive>();
            _exceptionFourteenCalculator = new Mock<StandardModulusExceptionFourteenCalculator>(new FirstStandardModulusElevenCalculatorExceptionFive(new SortCodeSubstitution(Resources.scsubtab)));

            var mappingSource = new Mock<IRuleMappingSource>();
            mappingSource.Setup(ms => ms.GetModulusWeightMappings)
                .Returns(new[]
                {
                    ModulusWeightMapping.From(
                        "010004 010006 MOD10 2 1 2 1 2  1 2 1 2 1 2 1 2 1"),
                    ModulusWeightMapping.From(
                        "010004 010006 DBLAL 2 1 2 1 2  1 2 1 2 1 2 1 2 1"),
                    ModulusWeightMapping.From(
                        "010007 010010 DBLAL  2 1 2 1 2  1 2 1 2 1 2 1 2 1"),
                    ModulusWeightMapping.From(
                        "010011 010013 MOD11    2 1 2 1 2  1 2 1 2 1 2 1 2 1"),
                    ModulusWeightMapping.From(
                        "010016 010016 dblal    2 1 2 1 2 1 2 1 2 1 2 1 2 1 5"),
                    ModulusWeightMapping.From(
                        "010014 010014 MOD11    2 1 2 1 2  1 2 1 2 1 2 1 2 1 5")
                });
            _mockModulusWeightTable = new Mock<IModulusWeightTable>();
            _mockModulusWeightTable.Setup(mwt => mwt.GetRuleMappings(new SortCode("010004"))).Returns(
                new []
                {
                    ModulusWeightMapping.From(
                        "010004 010006 MOD10 2 1 2 1 2  1 2 1 2 1 2 1 2 1"),
                    ModulusWeightMapping.From(
                        "010004 010006 DBLAL 2 1 2 1 2  1 2 1 2 1 2 1 2 1")
                });
            _mockModulusWeightTable.Setup(mwt => mwt.GetRuleMappings(new SortCode("010008"))).Returns(
                new[]
                {
                    ModulusWeightMapping.From(
                        "010007 010010 DBLAL  2 1 2 1 2  1 2 1 2 1 2 1 2 1")
                });
            _mockModulusWeightTable.Setup(mwt => mwt.GetRuleMappings(new SortCode("010013"))).Returns(
                new[]
                {
                    ModulusWeightMapping.From(
                        "010011 010013 MOD11    2 1 2 1 2  1 2 1 2 1 2 1 2 1")
                });
            _mockModulusWeightTable.Setup(mwt => mwt.GetRuleMappings(new SortCode("010014"))).Returns(
                new[]
                {
                    ModulusWeightMapping.From(
                        "010014 010014 MOD11    2 1 2 1 2  1 2 1 2 1 2 1 2 1 5")
                });
            _mockModulusWeightTable.Setup(mwt => mwt.GetRuleMappings(new SortCode("010016"))).Returns(
                new[]
                {
                    ModulusWeightMapping.From(
                        "010016 010016 dblal    2 1 2 1 2 1 2 1 2 1 2 1 2 1 5")
                });
            _mockModulusWeightTable.Setup(mwt => mwt.GetRuleMappings(new SortCode("180002"))).Returns(
                new[]
                {
                    ModulusWeightMapping.From("180002 180002 Mod11 0 0 0 0 0 0 8 7 6 5 4 3 2 1 14")
                });
            _mockModulusWeightTable.Setup(mwt => mwt.GetRuleMappings(new SortCode("200915"))).Returns(
                new[]
                {
                    ModulusWeightMapping.From("200901 201159 Mod11 0 0 0 0 0 0 0 7 6 5 4 3 2 1 6"),
                    ModulusWeightMapping.From("200901 201159 DblAl 2 1 2 1 2 1 2 1 2 1 2 1 2 1 6")
                });

            _standardTen.Setup(nr => nr.Process(It.IsAny<BankAccountDetails>())).Returns(true);
            _exceptionFourteenPassesFirstTime = new BankAccountDetails("180002", "98093517");
            _exceptionFourteenPassesSecondTime = new BankAccountDetails("180002", "00000190");
            _standardEleven.Setup(nr => nr.Process(_exceptionFourteenPassesFirstTime)).Returns(true);
            _standardEleven.Setup(nr => nr.Process(_exceptionFourteenPassesSecondTime)).Returns(false);
            _firstDoubleAlternate.Setup(nr => nr.Process(It.IsAny<BankAccountDetails>())).Returns(true);
            _standardExceptionFive.Setup(nr => nr.Process(It.IsAny<BankAccountDetails>())).Returns(true);
            _secondModulusCalculation.Setup(nr => nr.Process(It.IsAny<BankAccountDetails>())).Returns(
                true);
            _firstDoubleAlternateExceptionFive.Setup(nr => nr.Process(It.IsAny<BankAccountDetails>())).
                Returns(true);
            _exceptionFourteenCalculator.Setup(nr => nr.Process(It.IsAny<BankAccountDetails>())).Returns
                (true);

            _firstStepRouter = new Mock<FirstStepRouter>(_standardTen.Object, _standardEleven.Object, _firstDoubleAlternate.Object);
        }

        [Test]
        public void CanCallSecondCalculationCorrectly()
        {
            var accountDetails = new BankAccountDetails("010004", "12345678");
            accountDetails.WeightMappings = _mockModulusWeightTable.Object.GetRuleMappings(accountDetails.SortCode);
            new FirstModulusCalculatorStep(_firstStepRouter.Object, 
                                           _secondModulusCalculation.Object,
                                           _exceptionFourteenCalculator.Object)
                .Process(accountDetails);
            _secondModulusCalculation.Verify(calc => calc.Process(accountDetails));
        }

        [Test]
        public void CanProcessExceptionFourteenCorrectlyWhenFirstCheckPasses()
        {
            _exceptionFourteenPassesFirstTime.WeightMappings = _mockModulusWeightTable.Object.GetRuleMappings(_exceptionFourteenPassesFirstTime.SortCode);
            new FirstModulusCalculatorStep(_firstStepRouter.Object,
                                           _secondModulusCalculation.Object,
                                           _exceptionFourteenCalculator.Object)
                .Process(_exceptionFourteenPassesFirstTime);
            _exceptionFourteenCalculator.Verify(calc => calc.Process(_exceptionFourteenPassesFirstTime), Times.Never());
        }

        [Test]
        public void CanProcessExceptionFourteenCorrectlyWhenFirstCheckFails()
        {
            _exceptionFourteenPassesSecondTime.WeightMappings = _mockModulusWeightTable.Object.GetRuleMappings(_exceptionFourteenPassesSecondTime.SortCode);
            new FirstModulusCalculatorStep(_firstStepRouter.Object,
                                           _secondModulusCalculation.Object,
                                           _exceptionFourteenCalculator.Object)
                .Process(_exceptionFourteenPassesSecondTime);
            _exceptionFourteenCalculator.Verify(calc => calc.Process(_exceptionFourteenPassesSecondTime));
        }

    }
}
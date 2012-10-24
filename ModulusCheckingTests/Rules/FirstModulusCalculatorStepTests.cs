using System.Collections.Generic;
using System.Linq;
using ModulusChecking.Models;
using ModulusChecking.Parsers;
using ModulusChecking.Steps;
using ModulusChecking.Steps.Calculators;
using Moq;
using NUnit.Framework;

namespace ModulusCheckingTests.Rules
{
    public class FirstModulusCalculatorStepTests
    {
        private Mock<DoubleAlternateCalculator> _doubleAlternate;
        private Mock<DoubleAlternateCalculatorExceptionFive> _doubleAlternateExceptionFive;
        private Mock<StandardModulusExceptionFourteenCalculator> _exceptionFourteenCalculator;
        private ModulusWeightTable _modulusWeightTable;
        private Mock<SecondModulusCalculatorStep> _secondModulusCalculation;
        private Mock<FirstStandardModulusElevenCalculator> _standardEleven;
        private Mock<FirstStandardModulusElevenCalculatorExceptionFive> _standardExceptionFive;
        private Mock<FirstStandardModulusTenCalculator> _standardTen;

        [SetUp]
        public void Before()
        {
            _standardTen = new Mock<FirstStandardModulusTenCalculator>();
            _standardEleven = new Mock<FirstStandardModulusElevenCalculator>();
            _doubleAlternate = new Mock<DoubleAlternateCalculator>(BaseModulusCalculator.Step.Second);
            _standardExceptionFive = new Mock<FirstStandardModulusElevenCalculatorExceptionFive>();
            _secondModulusCalculation = new Mock<SecondModulusCalculatorStep>();
            _doubleAlternateExceptionFive =
                new Mock<DoubleAlternateCalculatorExceptionFive>(BaseModulusCalculator.Step.Second);
            _exceptionFourteenCalculator = new Mock<StandardModulusExceptionFourteenCalculator>();

            var mappingSource = new Mock<IRuleMappingSource>();
            mappingSource.Setup(ms => ms.GetModulusWeightMappings())
                .Returns(new List<ModulusWeightMapping>
                             {
                                 new ModulusWeightMapping(
                                     "010004 010006 MOD10 2 1 2 1 2  1 2 1 2 1 2 1 2 1"),
                                 new ModulusWeightMapping(
                                     "010004 010006 DBLAL 2 1 2 1 2  1 2 1 2 1 2 1 2 1"),
                                 new ModulusWeightMapping(
                                     "010007 010010 DBLAL  2 1 2 1 2  1 2 1 2 1 2 1 2 1"),
                                 new ModulusWeightMapping(
                                     "010011 010013 MOD11    2 1 2 1 2  1 2 1 2 1 2 1 2 1"),
                                     new ModulusWeightMapping(
                                     "010016 010016 dblal    2 1 2 1 2 1 2 1 2 1 2 1 2 1 5"),
                                 new ModulusWeightMapping(
                                     "010014 010014 MOD11    2 1 2 1 2  1 2 1 2 1 2 1 2 1 5")
                             });
            _modulusWeightTable = new ModulusWeightTable(mappingSource.Object);

            _standardTen.Setup(nr => nr.Process(It.IsAny<BankAccountDetails>(), _modulusWeightTable)).Returns(true);
            _standardEleven.Setup(nr => nr.Process(It.IsAny<BankAccountDetails>(), _modulusWeightTable)).Returns(true);
            _doubleAlternate.Setup(nr => nr.Process(It.IsAny<BankAccountDetails>(), _modulusWeightTable)).Returns(true);
            _standardExceptionFive.Setup(nr => nr.Process(It.IsAny<BankAccountDetails>(), _modulusWeightTable)).Returns(true);
            _secondModulusCalculation.Setup(nr => nr.Process(It.IsAny<BankAccountDetails>(), _modulusWeightTable)).Returns(
                true);
            _doubleAlternateExceptionFive.Setup(nr => nr.Process(It.IsAny<BankAccountDetails>(), _modulusWeightTable)).
                Returns(true);
            _exceptionFourteenCalculator.Setup(nr => nr.Process(It.IsAny<BankAccountDetails>(), _modulusWeightTable)).Returns
                (true);
        }

        [Test]
        [TestCase("010004", "MOD10")]
        [TestCase("010008", "DBLAL")]
        [TestCase("010013", "MOD11")]
        [TestCase("010016", "DBLAL")]
        [TestCase("010014", "MOD11")]
        public void CanSelectForModulus(string sc, string expectedModulusCheck)
        {
            var accountDetails = new BankAccountDetails(sc, "12345678");

            new FirstModulusCalculatorStep(_standardTen.Object, _standardEleven.Object,
                                           _doubleAlternate.Object, _standardExceptionFive.Object,
                                           _secondModulusCalculation.Object, _doubleAlternateExceptionFive.Object,
                                           _exceptionFourteenCalculator.Object)
                .Process(accountDetails, _modulusWeightTable);

            switch (expectedModulusCheck)
            {
                case "MOD10":
                    _standardTen.Verify(nr => nr.Process(accountDetails, _modulusWeightTable));
                    break;
                case "MOD11":
                    if (_modulusWeightTable.GetRuleMappings(accountDetails.SortCode).First().Exception == 5)
                    {
                        _standardExceptionFive.Verify(nr => nr.Process(accountDetails, _modulusWeightTable));
                    }
                    else
                    {
                        _standardEleven.Verify(nr => nr.Process(accountDetails, _modulusWeightTable));
                    }
                    break;
                case "DBLAL":
                    if (_modulusWeightTable.GetRuleMappings(accountDetails.SortCode).First().Exception == 5)
                    {
                        _doubleAlternateExceptionFive.Verify(nr => nr.Process(accountDetails, _modulusWeightTable));
                    }
                    else
                    {
                        _doubleAlternate.Verify(nr => nr.Process(accountDetails, _modulusWeightTable));
                    }
                    break;
            }
        }

        [Test]
        [TestCase("010004", "MOD10")]
        public void CanCallSecondCalculationCorrectly(string sc)
        {
            var accountDetails = new BankAccountDetails(sc, "12345678");

            new FirstModulusCalculatorStep(_standardTen.Object, _standardEleven.Object,
                                           _doubleAlternate.Object, _standardExceptionFive.Object,
                                           _secondModulusCalculation.Object, _doubleAlternateExceptionFive.Object,
                                           _exceptionFourteenCalculator.Object)
                .Process(accountDetails, _modulusWeightTable);
            _secondModulusCalculation.Verify(smc => smc.Process(accountDetails, _modulusWeightTable));
        }
    }
}
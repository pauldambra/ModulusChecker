using System.Collections.Generic;
using System.Linq;
using ModulusChecking.Loaders;
using ModulusChecking.Models;
using ModulusChecking.Steps;
using ModulusChecking.Steps.Calculators;
using Moq;
using NUnit.Framework;

namespace ModulusCheckingTests.Rules
{
    public class SecondModulusCalculatorStepTests
    {
        private Mock<DoubleAlternateCalculator> _doubleAlternate;
        private Mock<DoubleAlternateCalculatorExceptionFive> _doubleAlternateExceptionFive;
        private Mock<IModulusWeightTable> _mockModulusWeightTable;
        private Mock<SecondStandardModulusElevenCalculator> _standardEleven;
        private Mock<SecondStandardModulusTenCalculator> _standardTen;

        [SetUp]
        public void Before()
        {
            _standardTen = new Mock<SecondStandardModulusTenCalculator>();
            _standardEleven = new Mock<SecondStandardModulusElevenCalculator>();
            _doubleAlternate = new Mock<DoubleAlternateCalculator>(ModulusWeightMapping.Step.Second);
            _doubleAlternateExceptionFive =
                new Mock<DoubleAlternateCalculatorExceptionFive>(ModulusWeightMapping.Step.Second);
            
            var mappingSource = new Mock<IRuleMappingSource>();
            mappingSource.Setup(ms => ms.GetModulusWeightMappings())
                .Returns(new List<ModulusWeightMapping>
                             {
                                 new ModulusWeightMapping(
                                     "010004 010006 MOD10 2 1 2 1 2  1 2 1 2 1 2 1 2 1"),
                                 new ModulusWeightMapping(
                                     "010007 010010 DBLAL  2 1 2 1 2  1 2 1 2 1 2 1 2 1"),
                                 new ModulusWeightMapping(
                                     "010007 010010 DBLAL  2 1 2 1 2  1 2 1 2 1 2 1 2 1"),
                                 new ModulusWeightMapping(
                                     "010011 010013 MOD11    2 1 2 1 2  1 2 1 2 1 2 1 2 1"),
                                 new ModulusWeightMapping(
                                     "010011 010013 MOD11    2 1 2 1 2  1 2 1 2 1 2 1 2 1"),
                                 new ModulusWeightMapping(
                                     "010016 010016 dblal    2 1 2 1 2 1 2 1 2 1 2 1 2 1 5"),
                                 new ModulusWeightMapping(
                                     "010016 010016 dblal    2 1 2 1 2 1 2 1 2 1 2 1 2 1 5"),
                                 new ModulusWeightMapping(
                                     "010014 010014 MOD11    2 1 2 1 2  1 2 1 2 1 2 1 2 1 5"),
                                 new ModulusWeightMapping(
                                     "010014 010014 MOD11    2 1 2 1 2  1 2 1 2 1 2 1 2 1 5"),
                                 new ModulusWeightMapping(
                                     "010004 010006 MOD10 2 1 2 1 2  1 2 1 2 1 2 1 2 1"),
                             });
            _mockModulusWeightTable = new Mock<IModulusWeightTable>();
            _mockModulusWeightTable.Setup(mwt => mwt.GetRuleMappings(new SortCode("010004"))).Returns(
                new List<IModulusWeightMapping>
                    {
                        new ModulusWeightMapping(
                                     "010004 010006 MOD10 2 1 2 1 2  1 2 1 2 1 2 1 2 1"),
                        new ModulusWeightMapping(
                            "010004 010006 MOD10 2 1 2 1 2  1 2 1 2 1 2 1 2 1")
                    });
            _mockModulusWeightTable.Setup(mwt => mwt.GetRuleMappings(new SortCode("010008"))).Returns(
                new List<IModulusWeightMapping>
                    {
                        new ModulusWeightMapping(
                                     "010007 010010 DBLAL  2 1 2 1 2  1 2 1 2 1 2 1 2 1"),
                                 new ModulusWeightMapping(
                                     "010007 010010 DBLAL  2 1 2 1 2  1 2 1 2 1 2 1 2 1"),
                    });
            _mockModulusWeightTable.Setup(mwt => mwt.GetRuleMappings(new SortCode("010013"))).Returns(
                new List<IModulusWeightMapping>
                    {
                        new ModulusWeightMapping(
                                     "010011 010013 MOD11    2 1 2 1 2  1 2 1 2 1 2 1 2 1"),
                                 new ModulusWeightMapping(
                                     "010011 010013 MOD11    2 1 2 1 2  1 2 1 2 1 2 1 2 1"),
                    });
            _mockModulusWeightTable.Setup(mwt => mwt.GetRuleMappings(new SortCode("010014"))).Returns(
                new List<IModulusWeightMapping>
                    {
                      new ModulusWeightMapping(
                                     "010014 010014 MOD11    2 1 2 1 2  1 2 1 2 1 2 1 2 1 5"),
                                 new ModulusWeightMapping(
                                     "010014 010014 MOD11    2 1 2 1 2  1 2 1 2 1 2 1 2 1 5"),
                    });
            _mockModulusWeightTable.Setup(mwt => mwt.GetRuleMappings(new SortCode("010016"))).Returns(
                new List<IModulusWeightMapping>
                    {
                        new ModulusWeightMapping(
                                     "010016 010016 dblal    2 1 2 1 2 1 2 1 2 1 2 1 2 1 5"),
                                 new ModulusWeightMapping(
                                     "010016 010016 dblal    2 1 2 1 2 1 2 1 2 1 2 1 2 1 5")
                    });
            _standardTen.Setup(nr => nr.Process(It.IsAny<BankAccountDetails>())).Returns(true);
            _standardEleven.Setup(nr => nr.Process(It.IsAny<BankAccountDetails>())).Returns(true);
            _doubleAlternate.Setup(nr => nr.Process(It.IsAny<BankAccountDetails>())).Returns(true);
            _doubleAlternateExceptionFive.Setup(nr => nr.Process(It.IsAny<BankAccountDetails>())).
                Returns(true);
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
            accountDetails.WeightMappings = _mockModulusWeightTable.Object.GetRuleMappings(accountDetails.SortCode);
            new SecondModulusCalculatorStep(_standardTen.Object, _standardEleven.Object, _doubleAlternate.Object,
                                            _doubleAlternateExceptionFive.Object)
                .Process(accountDetails);

            switch (expectedModulusCheck)
            {
                case "MOD10":
                    _standardTen.Verify(nr => nr.Process(accountDetails));
                    break;
                case "MOD11":
                        _standardEleven.Verify(nr => nr.Process(accountDetails));
                    break;
                case "DBLAL":
                    if (_mockModulusWeightTable.Object.GetRuleMappings(accountDetails.SortCode).First().Exception == 5)
                    {
                        _doubleAlternateExceptionFive.Verify(nr => nr.Process(accountDetails));
                    }
                    else
                    {
                        _doubleAlternate.Verify(nr => nr.Process(accountDetails));
                    }
                    break;
            }
        }
    }
}
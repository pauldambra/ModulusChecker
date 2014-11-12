using System.Collections.Generic;
using ModulusChecking.Loaders;
using ModulusChecking.Models;
using ModulusChecking.Steps.Calculators;
using Moq;
using NUnit.Framework;

namespace ModulusCheckingTests.Rules
{
    public class SecondModulusCalculatorStepTests
    {
        private Mock<SecondDoubleAlternateCalculator> _doubleAlternate;
        private Mock<SecondDoubleAlternateCalculatorExceptionFive> _doubleAlternateExceptionFive;
        private Mock<IModulusWeightTable> _mockModulusWeightTable;
        private Mock<SecondStandardModulusElevenCalculator> _standardEleven;
        private Mock<SecondStandardModulusTenCalculator> _standardTen;

        [SetUp]
        public void Before()
        {
            _standardTen = new Mock<SecondStandardModulusTenCalculator>();
            _standardEleven = new Mock<SecondStandardModulusElevenCalculator>();
            _doubleAlternate = new Mock<SecondDoubleAlternateCalculator>();
            _doubleAlternateExceptionFive =
                new Mock<SecondDoubleAlternateCalculatorExceptionFive>();
            
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
    }
}
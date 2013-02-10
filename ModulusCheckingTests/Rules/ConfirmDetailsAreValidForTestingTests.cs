using System.Collections.Generic;
using ModulusChecking.Loaders;
using ModulusChecking.Models;
using ModulusChecking.Steps;
using Moq;
using NUnit.Framework;

namespace ModulusCheckingTests.Rules
{
    public class ConfirmDetailsAreValidForTestingTests
    {
        private Mock<IModulusWeightTable> _mockModulusWeightTable;
        private Mock<FirstModulusCalculatorStep> _firstModulusCalculatorStep;
        private ConfirmDetailsAreValidForModulusCheck _ruleStep;

        [SetUp]
        public void Before()
        {
            var mappingSource = new Mock<IRuleMappingSource>();
            mappingSource.Setup(ms => ms.GetModulusWeightMappings())
                .Returns(new List<IModulusWeightMapping>
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
                                     "010014 010014 MOD11    2 1 2 1 2  1 2 1 2 1 2 1 2 1 5")
                             });
            _mockModulusWeightTable = new Mock<IModulusWeightTable>();
            _mockModulusWeightTable.Setup(mwt => mwt.GetRuleMappings(new SortCode("010004"))).Returns(
                new List<IModulusWeightMapping>
                    {
                        new ModulusWeightMapping(
                            "010004 010006 MOD10 2 1 2 1 2  1 2 1 2 1 2 1 2 1"),
                        new ModulusWeightMapping(
                            "010004 010006 DBLAL 2 1 2 1 2  1 2 1 2 1 2 1 2 1"),
                    });
            _mockModulusWeightTable.Setup(mwt => mwt.GetRuleMappings(new SortCode("010009"))).Returns(
                new List<IModulusWeightMapping>
                    {
                        new ModulusWeightMapping(
                            "010007 010010 DBLAL  2 1 2 1 2  1 2 1 2 1 2 1 2 1")
                    });
            _firstModulusCalculatorStep = new Mock<FirstModulusCalculatorStep>();
            _firstModulusCalculatorStep.Setup(fmc => fmc.Process(It.IsAny<BankAccountDetails>())).
                Returns(false);
            _ruleStep = new ConfirmDetailsAreValidForModulusCheck(_firstModulusCalculatorStep.Object);
        }

        [Test]
        public void UnknownSortCodeIsValid()
        {
            const string sortCode = "123456";
            var accountDetails = new BankAccountDetails(sortCode, "12345678");
            accountDetails.WeightMappings = _mockModulusWeightTable.Object.GetRuleMappings(accountDetails.SortCode);
            var result = _ruleStep.Process(accountDetails);
            Assert.IsTrue(result);
            Assert.IsTrue(accountDetails.FirstResult);
            _firstModulusCalculatorStep.Verify(fmc => fmc.Process(It.IsAny<BankAccountDetails>()), Times.Never());
        }

        [Test]
        [TestCase("010004", "12345678",TestName = "CanTestAtStartOfRange")]
        [TestCase("010009", "12345678", TestName = "CanTestAtEndOfRange")]
        public void KnownSortCodeIsTested(string sc, string an)
        {
            var accountDetails = new BankAccountDetails(sc, an);
            accountDetails.WeightMappings = _mockModulusWeightTable.Object.GetRuleMappings(accountDetails.SortCode);
            _ruleStep.Process(accountDetails);
            _firstModulusCalculatorStep.Verify(nr => nr.Process(accountDetails));
        }

        [Test]
        public void CorrectlySkipsUncheckableForeignAccount()
        {
            var accountDetails = new BankAccountDetails("200915", "41011166");
            accountDetails.WeightMappings = _mockModulusWeightTable.Object.GetRuleMappings(accountDetails.SortCode);
            _ruleStep.Process(accountDetails);
            Assert.IsTrue(accountDetails.FirstResult);
            _firstModulusCalculatorStep.Verify(nr => nr.Process(accountDetails), Times.Never());
        }
    }
}
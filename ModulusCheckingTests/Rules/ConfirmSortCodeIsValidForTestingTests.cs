using System.Collections.Generic;
using ModulusChecking.Models;
using ModulusChecking.Parsers;
using ModulusChecking.Steps;
using Moq;
using NUnit.Framework;

namespace ModulusCheckingTests.Rules
{
    public class ConfirmSortCodeIsValidForTestingTests
    {
        private ModulusWeightTable _modulusWeightTable;
        private Mock<FirstModulusCalculatorStep> _firstModulusCalculatorStep;
        private ConfirmSortCodeIsValidForModulusCheck _ruleStep;

        [SetUp]
        public void Before()
        {
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
                                     "010014 010014 MOD11    2 1 2 1 2  1 2 1 2 1 2 1 2 1 5")
                             });
            _modulusWeightTable = new ModulusWeightTable(mappingSource.Object);
            
            _firstModulusCalculatorStep = new Mock<FirstModulusCalculatorStep>();
            _firstModulusCalculatorStep.Setup(fmc => fmc.Process(It.IsAny<BankAccountDetails>(), _modulusWeightTable)).
                Returns(false);
            _ruleStep = new ConfirmSortCodeIsValidForModulusCheck(_firstModulusCalculatorStep.Object);
        }

        [Test]
        public void UnknownSortCodeIsValid()
        {
            const string sortCode = "123456";
            var accountDetails = new BankAccountDetails(sortCode, "12345678");
            var result = _ruleStep.Process(accountDetails, _modulusWeightTable);
            Assert.IsTrue(result);
            _firstModulusCalculatorStep.Verify(fmc => fmc.Process(It.IsAny<BankAccountDetails>(), _modulusWeightTable), Times.Never());
        }

        [Test]
        public void KnownSortCodeIsTested()
        {
            var accountDetails = new BankAccountDetails("010004", "12345678");
            _ruleStep.Process(accountDetails, _modulusWeightTable);
            _firstModulusCalculatorStep.Verify(nr => nr.Process(accountDetails, _modulusWeightTable));
        }

        [Test]
        public void KnownSortCodeIsFoundWithinRange()
        {
            var accountDetails = new BankAccountDetails("010009", "12345678");
            _ruleStep.Process(accountDetails, _modulusWeightTable);
            _firstModulusCalculatorStep.Verify(nr => nr.Process(accountDetails, _modulusWeightTable));
        }
    }
}
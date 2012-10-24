using System.Collections.Generic;
using ModulusChecking.Models;
using ModulusChecking.Parsers;
using ModulusChecking.Steps.Calculators;
using Moq;
using NUnit.Framework;

namespace ModulusCheckingTests.Rules.Calculators
{
    public class DoubleAlternateCalculatorTests
    {
        private ModulusWeightTable _fakedModulusWeightTable;
        private DoubleAlternateCalculator _calculator;

        [SetUp]
        public void Before()
        {
            var mappingSource = new Mock<IRuleMappingSource>();
            mappingSource.Setup(ms => ms.GetModulusWeightMappings()).Returns(new List<ModulusWeightMapping>
                                                                                 {
                                                                                     new ModulusWeightMapping(
                                                                                         "230872 230872 DBLAL    2    1    2    1    2    1    2    1    2    1    2    1    2    1"),
                                                                                         new ModulusWeightMapping(
                                                                                         "499273 499273 DBLAL    2   1    2    1    2    1    2    1    2    1    2    1    2    1   "),
                                                                                     new ModulusWeightMapping(
                                                                                         "499273 499273 DBLAL    2   1    2    1    2    1    2    1    2    1    2    1    2    1   "),
                                                                                         new ModulusWeightMapping(
                                                                                         "200000 200002 DBLAL    2    1    2    1    2    1    2    1    2    1    2    1    2    1   6")
                                                                                 });
            _fakedModulusWeightTable = new ModulusWeightTable(mappingSource.Object);
            _calculator = new DoubleAlternateCalculator(BaseModulusCalculator.Step.Second);
        }

        [Test]
        public void CanProcessDoubleAlternateCheck()
        {
            var accountDetails = new BankAccountDetails("499273", "12345678");
            var result = _calculator.Process(accountDetails, _fakedModulusWeightTable);
            Assert.True(result);
        }

        [Test]
        public void CanProcessVocaLinkDoubleAlternateWithExceptionOne()
        {

            var accountDetails = new BankAccountDetails("118765", "64371389");
            var result = _calculator.Process(accountDetails, new ModulusWeightTable(new ValacdosSource()));
            Assert.True(result);
        }

        [Test]
        public void ExceptionFiveSecondCheckDigitIncorrect()
        {

            var accountDetails = new BankAccountDetails("938063", "15764273");
            var result = _calculator.Process(accountDetails, new ModulusWeightTable(new ValacdosSource()));
            Assert.IsFalse(result);
        }

        [Test]
        public void ExceptionFiveWhereFirstCheckPasses()
        {

            var accountDetails = new BankAccountDetails("938611", "07806039");
            var result = _calculator.Process(accountDetails, new ModulusWeightTable(new ValacdosSource()));
            Assert.IsFalse(result);
        }

        [Test]
        public void ExceptionThreeWhereCisNeitherSixNorNine()
        {
            var accountDetails = new BankAccountDetails("827101", "28748352");
            var result = _calculator.Process(accountDetails, new ModulusWeightTable(new ValacdosSource()));
            Assert.IsTrue(result);
        }

        [Test]
        public void ExceptionSixButNotAForeignAccount()
        {
            var accountDetails = new BankAccountDetails("202959", "63748472");
            var result = _calculator.Process(accountDetails, new ModulusWeightTable(new ValacdosSource()));
            Assert.IsTrue(result);
        }
    }
}

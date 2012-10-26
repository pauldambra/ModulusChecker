using System.Collections.Generic;
using System.Linq;
using ModulusChecking.Loaders;
using ModulusChecking.Models;
using ModulusChecking.Steps.Calculators;
using Moq;
using NUnit.Framework;

namespace ModulusCheckingTests.Rules.Calculators
{
    public class DoubleAlternateCalculatorTests
    {
        private Mock<IModulusWeightTable> _fakedModulusWeightTable;
        private DoubleAlternateCalculator _calculator;

        [SetUp]
        public void Before()
        {
            var mappingSource = new Mock<IRuleMappingSource>();
            mappingSource.Setup(ms => ms.GetModulusWeightMappings()).Returns(new List<IModulusWeightMapping>
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
            _fakedModulusWeightTable = new Mock<IModulusWeightTable>();
            _fakedModulusWeightTable.Setup(fmwt => fmwt.RuleMappings).Returns(mappingSource.Object.GetModulusWeightMappings().ToList());
            _fakedModulusWeightTable.Setup(fmwt => fmwt.GetRuleMappings(new SortCode("499273")))
                .Returns(new List<IModulusWeightMapping>
                             {
                                 new ModulusWeightMapping
                                     ("499273 499273 DBLAL    2   1    2    1    2    1    2    1    2    1    2    1    2    1   "),
                                 new ModulusWeightMapping
                                     ("499273 499273 DBLAL    2   1    2    1    2    1    2    1    2    1    2    1    2    1   ")
                             });
            _calculator = new DoubleAlternateCalculator(BaseModulusCalculator.Step.Second);
        }

        [Test]
        public void CanProcessDoubleAlternateCheck()
        {
            var accountDetails = new BankAccountDetails("499273", "12345678");
            var result = _calculator.Process(accountDetails, _fakedModulusWeightTable.Object);
            Assert.True(result);
        }

        [Test]
        public void CanProcessVocaLinkDoubleAlternateWithExceptionOne()
        {

            var accountDetails = new BankAccountDetails("118765", "64371389");
            var result = _calculator.Process(accountDetails, ModulusWeightTable.GetInstance);
            Assert.True(result);
        }

        [Test]
        public void ExceptionFiveSecondCheckDigitIncorrect()
        {

            var accountDetails = new BankAccountDetails("938063", "15764273");
            var result = _calculator.Process(accountDetails, ModulusWeightTable.GetInstance);
            Assert.IsFalse(result);
        }

        [Test]
        public void ExceptionFiveWhereFirstCheckPasses()
        {

            var accountDetails = new BankAccountDetails("938611", "07806039");
            var result = _calculator.Process(accountDetails, ModulusWeightTable.GetInstance);
            Assert.IsFalse(result);
        }

        [Test]
        public void ExceptionThreeWhereCisNeitherSixNorNine()
        {
            var accountDetails = new BankAccountDetails("827101", "28748352");
            var result = _calculator.Process(accountDetails, ModulusWeightTable.GetInstance);
            Assert.IsTrue(result);
        }

        [Test]
        public void ExceptionSixButNotAForeignAccount()
        {
            var accountDetails = new BankAccountDetails("202959", "63748472");
            var result = _calculator.Process(accountDetails, ModulusWeightTable.GetInstance);
            Assert.IsTrue(result);
        }
    }
}

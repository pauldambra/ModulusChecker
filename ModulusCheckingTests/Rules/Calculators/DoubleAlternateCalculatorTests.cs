using System.Linq;
using ModulusChecking.Loaders;
using ModulusChecking.Models;
using ModulusChecking.Properties;
using ModulusChecking.Steps.Calculators;
using Moq;
using NUnit.Framework;

namespace ModulusCheckingTests.Rules.Calculators
{
    public class DoubleAlternateCalculatorTests
    {
        private Mock<IModulusWeightTable> _fakedModulusWeightTable;
        private FirstDoubleAlternateCalculator _firstStepDblAlCalculator;
        private SecondDoubleAlternateCalculator _secondStepDblAlCalculator;

        [SetUp]
        public void Setup()
        {
            var mappingSource = new Mock<IRuleMappingSource>();
            mappingSource.Setup(ms => ms.GetModulusWeightMappings).Returns(new[]
            {
                ModulusWeightMapping.From(
                    "230872 230872 DBLAL    2    1    2    1    2    1    2    1    2    1    2    1    2    1"),
                ModulusWeightMapping.From(
                    "499273 499273 DBLAL    2   1    2    1    2    1    2    1    2    1    2    1    2    1   "),
                ModulusWeightMapping.From(
                    "499273 499273 DBLAL    2   1    2    1    2    1    2    1    2    1    2    1    2    1   "),
                ModulusWeightMapping.From(
                    "200000 200002 DBLAL    2    1    2    1    2    1    2    1    2    1    2    1    2    1   6")
            });
            _fakedModulusWeightTable = new Mock<IModulusWeightTable>();
            _fakedModulusWeightTable.Setup(fmwt => fmwt.RuleMappings).Returns(mappingSource.Object.GetModulusWeightMappings.ToList());
            _fakedModulusWeightTable.Setup(fmwt => fmwt.GetRuleMappings(new SortCode("499273")))
                .Returns(new []
                {
                    ModulusWeightMapping.From
                        ("499273 499273 DBLAL    2   1    2    1    2    1    2    1    2    1    2    1    2    1   "),
                    ModulusWeightMapping.From
                        ("499273 499273 DBLAL    2   1    2    1    2    1    2    1    2    1    2    1    2    1   ")
                });
            _fakedModulusWeightTable.Setup(fmwt => fmwt.GetRuleMappings(new SortCode("118765")))
                .Returns(new []
                {
                    ModulusWeightMapping.From
                        ("110000 119280 DblAl    0   0    2    1    2    1    2    1    2    1    2    1    2    1   1")
                });
            _firstStepDblAlCalculator = new FirstDoubleAlternateCalculator(new FirstDoubleAlternateCalculatorExceptionFive());
            _secondStepDblAlCalculator = new SecondDoubleAlternateCalculator(new SecondDoubleAlternateCalculatorExceptionFive());
        }

        [Test]
        public void CanProcessDoubleAlternateCheck()
        {
            var accountDetails = new BankAccountDetails("499273", "12345678");
            accountDetails.WeightMappings = _fakedModulusWeightTable.Object.GetRuleMappings(accountDetails.SortCode);
            var result = _firstStepDblAlCalculator.Process(accountDetails);
            Assert.True(result);
        }

        [Test]
        public void CanProcessVocaLinkDoubleAlternateWithExceptionOne()
        {

            var accountDetails = new BankAccountDetails("118765", "64371389");
            accountDetails.WeightMappings = _fakedModulusWeightTable.Object.GetRuleMappings(accountDetails.SortCode);
            var result = _firstStepDblAlCalculator.Process(accountDetails);
            Assert.True(result);
        }

        [Test]
        public void ExceptionFiveSecondCheckDigitIncorrect()
        {

            var accountDetails = new BankAccountDetails("938063", "15764273");
            accountDetails.WeightMappings = new ModulusWeightTable(Resources.valacdos).GetRuleMappings(accountDetails.SortCode);
            var result = _firstStepDblAlCalculator.Process(accountDetails);
            Assert.IsFalse(result);
        }

        [Test]
        public void ExceptionFiveWhereFirstCheckPasses()
        {

            var accountDetails = new BankAccountDetails("938611", "07806039");
            accountDetails.WeightMappings = new ModulusWeightTable(Resources.valacdos).GetRuleMappings(accountDetails.SortCode);
            var result = _firstStepDblAlCalculator.Process(accountDetails);
            Assert.IsFalse(result);
        }

        [Test]
        public void ExceptionThreeWhereCisNeitherSixNorNine()
        {
            var accountDetails = new BankAccountDetails("827101", "28748352");
            accountDetails.WeightMappings = new ModulusWeightTable(Resources.valacdos).GetRuleMappings(accountDetails.SortCode);
            var result = _secondStepDblAlCalculator.Process(accountDetails);
            Assert.IsTrue(result);
        }

        [Test]
        public void ExceptionSixButNotAForeignAccount()
        {
            var accountDetails = new BankAccountDetails("202959", "63748472");
            accountDetails.WeightMappings = new ModulusWeightTable(Resources.valacdos).GetRuleMappings(accountDetails.SortCode);
            var result = _secondStepDblAlCalculator.Process(accountDetails);
            Assert.IsTrue(result);
        }
    }
}

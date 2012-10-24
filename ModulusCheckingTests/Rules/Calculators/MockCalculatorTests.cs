using System.Collections.Generic;
using ModulusChecking.Models;
using ModulusChecking.Parsers;
using ModulusChecking.Steps.Calculators;
using Moq;
using NUnit.Framework;

namespace ModulusCheckingTests.Rules.Calculators
{
    public class MockCalculatorTests
    {
        private ModulusWeightTable _modulusWeightTable;

        [SetUp]
        public void Before()
        {
            var mappingSource = new Mock<IRuleMappingSource>();
            mappingSource.Setup(ms => ms.GetModulusWeightMappings()).Returns(new List<ModulusWeightMapping>
                                                                                 {
                                                                                     new ModulusWeightMapping(
                                                                                         "000000 000100 MOD10 0 0 0 0 0 0 7 5 8 3 4 6 2 1 "),
                                                                                     new ModulusWeightMapping(
                                                                                         "499273 499273 DBLAL    0    0    2    1    2    1    2    1    2    1    2    1    2    1   1"),
                                                                                         new ModulusWeightMapping(
                                                                                         "200000 200002 DBLAL    2    1    2    1    2    1    2    1    2    1    2    1    2    1   6")
                                                                                 });
            _modulusWeightTable = new ModulusWeightTable(mappingSource.Object);
        }

        [Test]
        public void CanProcessStandardElevenCheck()
        {
            var accountDetails = new BankAccountDetails("000000", "58177632");
            var result = new FirstStandardModulusElevenCalculator().Process(accountDetails, _modulusWeightTable);
            Assert.True(result);
        }

        [Test]
        //vocalink test case
        public void CanProcessVocalinkStandardTenCheck()
        {
            var accountDetails = new BankAccountDetails("089999", "66374958");
            var result = new FirstStandardModulusTenCalculator().Process(accountDetails, new ModulusWeightTable(new ValacdosSource()));
            Assert.True(result);
        }

        [Test]
        public void CanProcessVocalinkStandardEleven()
        {
            var accountDetails = new BankAccountDetails("107999", "88837491");
            var result = new FirstStandardModulusElevenCalculator().Process(accountDetails, new ModulusWeightTable(new ValacdosSource()));
            Assert.True(result);
        }

    }
}

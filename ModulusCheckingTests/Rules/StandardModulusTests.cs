using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModulusChecking;
using ModulusChecking.Models;
using ModulusChecking.ModulusChecks;
using ModulusChecking.Parsers;
using ModulusChecking.Rules;
using ModulusChecking.Rules.ModulusChecks;
using Moq;
using NUnit.Framework;

namespace ModulusCheckingTests.ModulusChecks
{
    public class StandardModulusTests
    {
        private ModulusWeights _modulusWeight;

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
            _modulusWeight = new ModulusWeights(mappingSource.Object);
        }

        [Test]
        public void CanProcessStandardElevenCheck()
        {
            var accountDetails = new BankAccountDetails("000000", "58177632");
            var result = new FirstStandardModulusEleven().Process(accountDetails, _modulusWeight);
            Assert.True(result);
        }

        [Test]
        //vocalink test case
        public void CanProcessVocalinkStandardTenCheck()
        {
            var accountDetails = new BankAccountDetails("089999", "66374958");
            var result = new FirstStandardModulusTen().Process(accountDetails, new ModulusWeights(new ValacdosSource()));
            Assert.True(result);
        }

        [Test]
        public void CanProcessVocalinkStandardEleven()
        {
            var accountDetails = new BankAccountDetails("107999", "88837491");
            var result = new FirstStandardModulusEleven().Process(accountDetails, new ModulusWeights(new ValacdosSource()));
            Assert.True(result);
        }

    }
}

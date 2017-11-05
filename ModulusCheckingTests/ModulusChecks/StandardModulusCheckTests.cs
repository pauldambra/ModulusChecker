using System.Linq;
using ModulusChecking.Models;
using ModulusChecking.ModulusChecks;
using NUnit.Framework;

namespace ModulusCheckingTests.ModulusChecks
{
    public class StandardModulusCheckTests
    {
        private StandardModulusCheck _checker;

        [SetUp]
        public void Setup()
        {
            _checker = new StandardModulusCheck();
        }

        [Test]
        [TestCase("000000", "58177632", "012345 012346 mod10 0 0 0 0 0 0 7 5 8 3 4 6 2 1", 176, TestName = "Basic Calculation")]
        [TestCase("938611", "07806039", "012345 012346 mod10 7 6 5 4 3 2 7 6 5 4 3 2 0 0", 250, TestName = "Exception 5 where check passes")]
        [TestCase("827101", "28748352", "012345 012346 mod10 0 0 0 0 0 0 0 0 7 3 4 9 2 1", 132, TestName = "Exception 3 perform both checks")]
        [TestCase("938063", "15764273", "012345 012346 mod10 7 6 5 4 3 2 7 6 5 4 3 2 0 0", 257, TestName = "ExceptionFiveFirstCheckCorrectSecondIncorrect")]
        [TestCase("202959", "63748472", "012345 012346 mod10 0 0 0 0 0 0 0 7 6 5 4 3 2 1", 143, TestName = "Can calculate modulus eleven sum")]
        public void CanCalculateSum(string sc, string an, string mappingString, int expected)
        {
            ValidateStandardModulusWeightSumCalculation(sc,an,mappingString,expected);
        }

        private void ValidateStandardModulusWeightSumCalculation(string sc, string an, string mappingString, int expected)
        {
            var details = new BankAccountDetails(sc, an)
                              {
                                  WeightMappings = new [] { ModulusWeightMapping.From(mappingString) }
                              };
            var actual = _checker.GetModulusSum(details, details.WeightMappings.First());
            Assert.AreEqual(expected, actual);
        }
    }
}

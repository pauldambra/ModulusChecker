using ModulusChecking.Models;
using ModulusChecking.ModulusChecks;
using NUnit.Framework;

namespace ModulusCheckingTests.ModulusChecks
{
    public class DoubleAlternateModulusCheckTests
    {
        private DoubleAlternateModulusCheck _check;

        [SetUp]
        public void Setup()
        {
            _check = new DoubleAlternateModulusCheck();
        }

        [Test]
        public void CalculatesExceptionSixTestCaseCorrectly()
        {
            var details = new BankAccountDetails("202959", "63748472");
            var mapping = new ModulusWeightMapping("012345 012346 dblal 2 1 2 1 2 1 2 1 2 1 2 1 2 1");
            var actual = _check.GetModulusSum(details, mapping);
            Assert.AreEqual(60,actual);
        }

        [Test]
        public void ExceptionOneChangesSum()
        {
            var details = new BankAccountDetails("123456", "12345678");
            var mapping = new ModulusWeightMapping("012345 012346 dblal 1 2 3 4 5 6 7 8 9 10 11 12 13 14");
            var withNoException = _check.GetModulusSum(details, mapping);
            mapping = new ModulusWeightMapping("012345 012346 dblal 1 2 3 4 5 6 7 8 9 10 11 12 13 14 1");
            var withException = _check.GetModulusSum(details, mapping);
            Assert.IsTrue(withNoException + 27 == withException);
        }

        [Test]
        public void CalculatesSumAsExpected()
        {
            var details = new BankAccountDetails("499273", "12345678");
            var mapping = new ModulusWeightMapping("012345 012346 dblal 2 1 2 1 2 1 2 1 2 1 2 1 2 1");
            var actual = _check.GetModulusSum(details, mapping);
            Assert.AreEqual(70,actual);
        }

        [Test]
        public void ExceptionFiveValidationTestSumCheck()
        {
            var details = new BankAccountDetails("938611", "07806039");
            var mapping = new ModulusWeightMapping("012345 012346 dblal 2 1 2 1 2 1 2 1 2 1 2 1 2 0 5");
            var actual = _check.GetModulusSum(details, mapping);
            Assert.AreEqual(51, actual);
        }

        [Test]
        public void ExceptionFiveFirstCheckCorrectSecondIncorrect()
        {
            var details = new BankAccountDetails("938063", "15764273");
            var mapping = new ModulusWeightMapping("012345 012346 dblal 2 1 2 1 2 1 2 1 2 1 2 1 2 0 5");
            var actual = _check.GetModulusSum(details, mapping);
            Assert.AreEqual(58, actual);
        }

        [Test]
        //938000 938696 Mod11 7 6 5 4 3 2 7 6 5 4 3 2 0 0 5
        //938000 938696 DblAl 2 1 2 1 2 1 2 1 2 1 2 1 2 0 5
        public void ExceptionFiveDoubleAlternateWhenBothPass()
        {
            var details = new BankAccountDetails("938063", "55065200");
            var mapping = new ModulusWeightMapping("938000 938696 DblAl 2 1 2 1 2 1 2 1 2 1 2 1 2 0 5");
            var actual = _check.GetModulusSum(details, mapping);
            Assert.AreEqual(40,actual);
            Assert.AreEqual(0,actual%10);
        }
    }
}

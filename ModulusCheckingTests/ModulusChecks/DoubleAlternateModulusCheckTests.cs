using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            var weights = new[] {2, 1, 2, 1, 2, 1, 2, 1, 2, 1, 2, 1, 2, 1};
            var actual = _check.GetModulusSum(details, weights);
            Assert.AreEqual(60,actual);
        }

        [Test]
        public void ExceptionOneChangesSum()
        {
            var details = new BankAccountDetails("123456", "12345678");
            var weights = new[] {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14};
            const int exception = 1;
            var withNoException = _check.GetModulusSum(details, weights);
            var withException = _check.GetModulusSum(details, weights, exception);
            Assert.IsTrue(withNoException + 27 == withException);
        }

        [Test]
        public void CalculatesSumAsExpected()
        {
            var details = new BankAccountDetails("499273", "12345678");
            var weights = new[] { 2,1,2,1,2,1,2,1,2,1,2,1,2,1 };
            var actual = _check.GetModulusSum(details, weights);
            Assert.AreEqual(70,actual);
        }

        [Test]
        public void ExceptionFiveValidationTestSumCheck()
        {
            var details = new BankAccountDetails("938611", "07806039");
            var weights = new[] { 2, 1, 2, 1, 2, 1, 2, 1, 2, 1, 2, 1, 2, 0 };
            var actual = _check.GetModulusSum(details, weights);
            Assert.AreEqual(51, actual);
        }

        [Test]
        public void ExceptionFiveFirstCheckCorrectSecondIncorrect()
        {
            var details = new BankAccountDetails("938063", "15764273");
            var weights = new[] { 2, 1, 2, 1, 2, 1, 2, 1, 2, 1, 2, 1, 2, 0 };
            var actual = _check.GetModulusSum(details, weights);
            Assert.AreEqual(58, actual);
        }
    }
}

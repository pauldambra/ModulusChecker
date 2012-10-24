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
    public class StandardModulusCheckTests
    {
        [Test]
        public void CanCalculateSum()
        {
            var checker = new StandardModulusCheck();
            var details = new BankAccountDetails("000000", "58177632");
            var weights = new[] { 0, 0, 0, 0, 0, 0, 7, 5, 8, 3, 4, 6, 2, 1 };
            var actual = checker.GetModulusSum(details, weights);
            Assert.AreEqual(176,actual);
        }

        [Test]
        public void ExceptionFiveWhereCheckPassesSumTest()
        {
            var checker = new StandardModulusCheck();
            var details = new BankAccountDetails("938611", "07806039");
            var weights = new[] {7, 6, 5, 4, 3, 2, 7, 6, 5, 4, 3, 2, 0, 0};
            var actual = checker.GetModulusSum(details, weights);
            Assert.AreEqual(250, actual);
        }

        [Test]
        public void ExceptionThreePerformBothChecksSumTest()
        {
            var checker = new StandardModulusCheck();
            var details = new BankAccountDetails("827101", "28748352");
            var weights = new[] { 0,0,0,0,0,0,0,0,7,3,4,9,2,1 };
            var actual = checker.GetModulusSum(details, weights);
            Assert.AreEqual(132, actual);
        }

        [Test]
        public void ExceptionFiveFirstCheckCorrectSecondIncorrect()
        {
            var checker = new StandardModulusCheck();
            var details = new BankAccountDetails("938063", "15764273");
            var weights = new[] { 7,6,5,4,3,2,7,6,5,4,3,2,0,0 };
            var actual = checker.GetModulusSum(details, weights);
            Assert.AreEqual(257, actual);
        }
    }
}

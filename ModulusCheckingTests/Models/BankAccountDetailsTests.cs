using System;
using ModulusChecking.Models;
using NUnit.Framework;

namespace ModulusCheckingTests.Models
{
    public class BankAccountDetailsTests
    {
        [Test]
        [TestCase("1234567890", "123456",  "123456", "something")]
        [TestCase("12-34567890", "123456",  "123456", "something")]
        [TestCase("123456789", "123456",  "123451", "23456789")]
        [TestCase("1234567", "123456",  "123456", "01234567")]
        [TestCase("123456", "123456",  "123456", "00123456")]
        public void CanInstantiateOddAccounts(string accountNumber, string sortCode, string expectedSortCode, string expectedAccountNumber)
        {
            var account = new BankAccountDetails(sortCode, accountNumber);
            Assert.AreEqual(expectedAccountNumber, account.AccountNumber.ToString());
            Assert.AreEqual(expectedSortCode,account.SortCode.ToString());
        }

        [Test]
        [TestCase("123456")]
        [TestCase("234",TestName="short string",ExpectedException=typeof(ArgumentException))]
        [TestCase("1234567", TestName = "long string", ExpectedException = typeof(ArgumentException))]
        [TestCase("a", TestName = "not digit string", ExpectedException = typeof(ArgumentException))]
        public void CanInitialiseSortCode(string expected)
        {
            Assert.AreEqual(expected,new BankAccountDetails(expected,"12345678").SortCode.ToString());
        }
    }
}

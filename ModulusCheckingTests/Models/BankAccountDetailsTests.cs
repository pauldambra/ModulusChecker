using System;
using ModulusChecking.Models;
using NUnit.Framework;

namespace ModulusCheckingTests.Models
{
    public class BankAccountDetailsTests
    {
        [Test]
        [TestCase("1234567890", "080000", "080000", "12345678")]
        [TestCase("1234567890", "830000", "830000", "34567890")]
        [TestCase("12-34567890", "839000", "839000", "12345678")]
        [TestCase("12-34567890", "600000", "600000", "34567890")]
        [TestCase("12-34567890", "123456", "123456", "something",ExpectedException = typeof(ArgumentException))]
        [TestCase("123456789", "123456",  "123451", "23456789")]
        [TestCase("1234567", "123456",  "123456", "01234567")]
        [TestCase("123456", "123456",  "123456", "00123456")]
        [TestCase("123456123456123456", "123456", "123456", "00123456",ExpectedException = typeof(ArgumentException))]
        [TestCase("123", "123456", "123456", "00123456", ExpectedException = typeof(ArgumentException))]
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

        [Test]
        [TestCase("123456","09345678",false)]
        [TestCase("123456", "09345698", true)]
        [TestCase("123456", "99345678", false)]
        [TestCase("123456", "99345698", true)]
        [TestCase("123456", "19345698", false)]
        public void CanValidateForExceptionTen(string sc, string an, bool expected)
        {
            Assert.AreEqual(expected,new BankAccountDetails(sc,an).AccountNumber.ValidateExceptionTen);
        }

        [Test]
        [TestCase("123456", "09345678", false)]
        [TestCase("123456", "49345698", false)]
        [TestCase("123456", "59345678", false)]
        [TestCase("123456", "69345698", false)]
        [TestCase("123456", "79345698", false)]
        [TestCase("123456", "89345698", false)]
        [TestCase("123456", "09345677", false)]
        [TestCase("123456", "49345699", true)]
        [TestCase("123456", "59345677", true)]
        [TestCase("123456", "69345699", true)]
        [TestCase("123456", "79345688", true)]
        [TestCase("123456", "89345688", true)]
        public void CanValidateForExceptionSix(string sc, string an, bool expected)
        {
            Assert.AreEqual(expected, new BankAccountDetails(sc, an).AccountNumber.IsForeignCurrencyAccount);
        }

        [Test]
        [TestCase("123456", "09345678", false)]
        [TestCase("123456", "09345690", true)]
        [TestCase("123456", "99345671", true)]
        [TestCase("123456", "99345699", true)]
        public void CanValidateAsCouttsAccountNumber(string sc, string an, bool expected)
        {
            Assert.AreEqual(expected, new BankAccountDetails(sc, an).AccountNumber.IsValidCouttsNumber);
        }

        [Test]
        [TestCase("01234567",0,'9',"91234567")]
        [TestCase("01234567", 2, '9', "01934567")]
        [TestCase("01234567", 7, '9', "01234569")]
        [TestCase("01234567", 2, 'b', "019345",ExpectedException = typeof(ArgumentException))]
        [TestCase("01234567", -1, '9', "019345", ExpectedException = typeof(ArgumentOutOfRangeException))]
        [TestCase("01234567", 14, '9', "019345", ExpectedException = typeof(ArgumentOutOfRangeException))]
        public void CanSetElementByIndex(string original, int index, char newChar, string expected)
        {
            var target = new BankAccountDetails("012345", original);
            target.AccountNumber.SetElementAt(index, newChar);
            Assert.AreEqual(expected, target.AccountNumber.ToString());
        }
    }
}

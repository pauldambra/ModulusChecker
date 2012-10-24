using ModulusChecking.Models;
using ModulusChecking.Parsers;
using ModulusChecking.Steps;
using NUnit.Framework;

namespace ModulusCheckingTests.Rules
{
    /// <summary>
    /// Exception 2 & 9 where the first check passes and second check fails. 309070 02355688 Y
    /// Exception 2 & 9 where the first check fails and second check passes with substitution.  309070 12345668 Y
    /// Exception 2 & 9 where a!=0 and g!=9 and passes. 309070 12345677 Y
    /// Exception 2 & 9 where a!=0 and g=9 and passes. 309070 99345694 Y
    /// </summary>
    public class ModulusExceptionTests
    {
        /// <summary>
        /// /Only occurs for some standard modulus 11 checks, when there is a 2 in the exception column for the 
        ///first check for a sorting code and a 9 in the exception column for the second check for the same 
        ///sorting code. This is used specifically for LTSB euro accounts.
        /// </summary>
        [Test]
        [TestCase("309070","02355688",true, TestName = "2 and 9 where first passes and second fails")]
        [TestCase("309070", "12345668", true, TestName = "2 and 9 where first fails and second passes with substitution")]
        [TestCase("309070", "12345677", true, TestName = "2 and 9 second passes with no match weights")]
        [TestCase("309070", "99345694", true, TestName = "2 and 9 where second passes using one match weights")]
        public void TwoAndNineExceptionTest(string sc, string an, bool expectedResult)
        {
            var accountDetails = new BankAccountDetails(sc, an);
            var result = new FirstModulusCalculatorStep().Process(accountDetails, new ModulusWeights(new ValacdosSource()));
            Assert.AreEqual(expectedResult, result);
        }


        /// <summary>
        /// 3 If c== 6or9 the double alternate check does not need to be carried out.
        /// Exception 3, and the sorting code is the start of a range. As c=6 the second check should be ignored.
        /// 820000 73688637 Y
        /// Exception 3, and the sorting code is the end of a range. As c=9 the second check should be ignored.
        /// 827999 73988638 Y
        /// Exception 3. As c != 6 or 9 perform both checks pass. 827101 28748352 Y
        /// </summary>
        [Test]
        [TestCase("820000", "73688637", true,TestName = "c = 6 so ignore second check")]
        [TestCase("827999", "73988638", true, TestName = "c = 9 so ignore second check")]
        [TestCase("827101", "28748352", true, TestName = "c is neither 6 nor 9. so run second check")]
        public void ThreeException(string sc, string an, bool expectedResult)
        {
            var accountDetails = new BankAccountDetails(sc, an);
            var result = new FirstModulusCalculatorStep().Process(accountDetails, new ModulusWeights(new ValacdosSource()));
            Assert.AreEqual(expectedResult, result); 
        }

        /// <summary>
        /// Exception 4 where the remainder is equal to the checkdigit. 134020 63849203 Y
        /// </summary>
        [Test]
        public void ExceptionFourTest()
        {
            var accountDetails = new BankAccountDetails("134020", "63849203");
            var result = new FirstModulusCalculatorStep().Process(accountDetails, new ModulusWeights(new ValacdosSource()));
            Assert.IsTrue(result); 
        }

        /// <summary>
        /// Exception 5 where the first checkdigit is correct and the second incorrect. 938063 15764273 N
        /// Exception 5 where the first checkdigit is incorrect and the second correct. 938063 15764264 N
        /// Exception 5 where the first checkdigit is incorrect with a remainder of 1. 938063 15763217 N
        /// Exception 5 where the check passes. 938611 07806039 Y
        /// Exception 5 where the check passes with substitution. 938600 42368003 Y
        /// Exception 5 where both checks produce a remainder of 0 and pass. 938063 55065200 Y
        /// </summary>
        [Test]
        [TestCase("938611","07806039", true, TestName = "Exception 5 where the check passes")]
        [TestCase("938600", "42368003", true, TestName = "Exception 5 where the check passes with substitution")]
        [TestCase("938063", "55065200", true, TestName = "Exception 5 where both checks produce a remainder of 0 and pass")]
        [TestCase("938063", "15764273", false, TestName = "Exception 5 where the first checkdigit is correct and the second incorrect.")]
        [TestCase("938063", "15764264", false, TestName = "Exception 5 where the first checkdigit is incorrect and the second correct.")]
        public void ExceptionFiveTest(string sc, string an, bool expectedResult)
        {
            var accountDetails = new BankAccountDetails(sc, an);
            var result = new FirstModulusCalculatorStep().Process(accountDetails, new ModulusWeights(new ValacdosSource()));
            Assert.AreEqual(expectedResult, result); 
        }

        /// <summary>
        /// /• If a = 4, 5, 6, 7 or 8, and g and h are the same, the accounts are for a foreign currency and the checks 
        /// cannot be used.
        /// </summary>
        [Test]
        public void ExceptionSixShouldBeValid()
        {
            var accountDetails = new BankAccountDetails("200915", "41011166");
            var result = new FirstModulusCalculatorStep().Process(accountDetails,
                                                                  new ModulusWeights(new ValacdosSource()));
            Assert.IsTrue(result);
        }

        /// <summary>
        /// Perform the check as specified, except if g=9 zeroise weighting positions u-b.
        /// Exception 7 where passes but would fail the standard check. 772798 99345694 Y
        /// </summary>
        [Test]
        public void ExceptionSeven()
        {
            var accountDetails = new BankAccountDetails("772798", "99345694");
            var result = new FirstModulusCalculatorStep().Process(accountDetails,
                                                                  new ModulusWeights(new ValacdosSource()));
            Assert.IsTrue(result);
        }

        /// <summary>
        /// Perform the check as specified, except substitute the sorting code with 090126, for check purposes only.
        /// Exception 8 where the check passes. 086090 06774744 Y
        /// </summary>
        [Test]
        public void ExceptionEight()
        {
            var accountDetails = new BankAccountDetails("086090", "06774744");
            var result = new FirstModulusCalculatorStep().Process(accountDetails,
                                                                  new ModulusWeights(new ValacdosSource()));
            Assert.IsTrue(result);
        }

        [Test]
        [TestCase("871427","46238510",TestName = "Excptn 10 & 11 where first check passes and second fails")]
        [TestCase("872427", "46238510", TestName = "Excptn 10 & 11 where first check fails and second passes")]
        [TestCase("871427", "09123496", TestName = "Excptn 10 where acc. no. ab = 09 and g=9. first check passes and second fails")]
        [TestCase("871427", "99123496", TestName = "Excptn 10 where acc. no. ab = 99 and g=9. first check passes and second fails")]
        public void ExceptionsTenAndEleven(string sc, string an)
        {
            var accountDetails = new BankAccountDetails(sc, an);
            var result = new FirstModulusCalculatorStep().Process(accountDetails,
                                                                  new ModulusWeights(new ValacdosSource()));
            Assert.IsTrue(result);
        }
    }
}

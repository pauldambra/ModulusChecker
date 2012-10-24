using ModulusChecking.Models;
using ModulusChecking.Parsers;
using ModulusChecking.Steps;
using NUnit.Framework;

namespace ModulusCheckingTests
{
    /// <summary>
    /// These are the test cases included in the Vocalink Validating Account Numbers document.
    /// </summary>
    public class VocalinkTestCases
    {

        private static void ValidateModulusCalculator(string sc, string an, bool expectedResult)
        {
            var accountDetails = new BankAccountDetails(sc, an);
            var result = new FirstModulusCalculatorStep().Process(accountDetails, new ModulusWeightTable(new ValacdosSource()));
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        [TestCase("089999", "66374958", true, TestName = "Pass modulus 10 check.")]
        [TestCase("107999", "88837491", true, TestName = "Pass modulus 11 check.")]
        [TestCase("202959", "63748472", true, TestName = "Pass modulus 11 and double alternate checks.")]
        [TestCase("203099", "66831036", false, TestName = "Pass modulus 11 check and fail double alternate check.")]
        [TestCase("203099", "58716970", false, TestName = "Fail modulus 11 check and pass double alternate check.")]
        [TestCase("089999", "66374959", false, TestName = "Fail modulus 10 check.")]
        [TestCase("107999", "88837493", false, TestName = "Fail modulus 11 check.")]
        public void CanPerformBasicCalculation(string sc, string an, bool expectedResult)
        {
            ValidateModulusCalculator(sc, an, expectedResult);
        }

        [Test]
        [TestCase("871427", "46238510", true, TestName = "Excptn 10 & 11 where first check passes and second fails")]
        [TestCase("872427", "46238510", true, TestName = "Excptn 10 & 11 where first check fails and second passes")]
        [TestCase("871427", "09123496", true, TestName = "Excptn 10 where acc. no. ab = 09 and g=9. first check passes and second fails")]
        [TestCase("871427", "99123496", true, TestName = "Excptn 10 where acc. no. ab = 99 and g=9. first check passes and second fails")]
        public void CanPerformExceptionsTenAndElevenCalculation(string sc, string an, bool expectedResult)
        {
            ValidateModulusCalculator(sc, an, expectedResult);
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
        [TestCase("820000", "73688637", true, TestName = "Excptn 3 and c = 6 so ignore second check")]
        [TestCase("827999", "73988638", true, TestName = "Excptn 3 and c = 9 so ignore second check")]
        [TestCase("827101", "28748352", true, TestName = "Excptn 3 and c is neither 6 nor 9. so run second check")]
        public void CanPerformExceptionThreeCalculation(string sc, string an, bool expectedResult)
        {
            ValidateModulusCalculator(sc, an, expectedResult);
        }

        /// <summary>
        /// Exception 4 where the remainder is equal to the checkdigit.
        /// </summary>
        [Test]
        public void CanPerformExceptionFourCalculation()
        {
            ValidateModulusCalculator("134020", "63849203", true);
        }

        /// <summary>
        /// 
        /// and passes double alternate modulus check.
        /// </summary>
        [Test]
        [TestCase("118765","64371389",true,TestName = "Exception 1 – ensures that 27 has been added to the accumulated total")]
        [TestCase("118765", "64371388", false, TestName = "Exception 1 where it fails double alternate check.")]
        public void CanPerformExceptionOneCalculation(string sc, string an, bool expectedResult)
        {
            ValidateModulusCalculator(sc, an, expectedResult);
        }

        /// <summary>
        /// Exception 6 where the account fails standard check but is a foreign currency account.
        /// </summary>
        [Test]
        public void CanPerformExceptionSixCalculation()
        {
            ValidateModulusCalculator("200915", "41011166",true);
        }
        
        [Test]
        [TestCase("938611","07806039", true, TestName = "Exception 5 where the check passes")]
        [TestCase("938600", "42368003", true, TestName = "Exception 5 where the check passes with substitution")]
        [TestCase("938063", "55065200", true, TestName = "Exception 5 where both checks produce a remainder of 0 and pass")]
        [TestCase("938063", "15764273", false, TestName = "Exception 5 where the first checkdigit is correct and the second incorrect.")]
        [TestCase("938063", "15764264", false, TestName = "Exception 5 where the first checkdigit is incorrect and the second correct.")]
        public void CanPerformExceptionFiveCalculations(string sc, string an, bool expectedResult)
        {
            ValidateModulusCalculator(sc, an, expectedResult);
        }

        /// <summary>
        /// Exception 7 where passes but would fail the standard check.
        /// </summary>
        [Test]
        public void CanPerformExceptionSevenCalculation()
        {
            ValidateModulusCalculator("772798", "99345694",true);
        }

        /// <summary>
        /// Exception 8 where the check passes.
        /// </summary>
        [Test]
        public void CanPerformExceptionEightCalculation()
        {
            ValidateModulusCalculator("086090", "06774744",true);
        }

        [Test]
        [TestCase("309070", "02355688", true, TestName = "2 and 9 where first passes and second fails")]
        [TestCase("309070", "12345668", true, TestName = "2 and 9 where first fails and second passes with substitution")]
        [TestCase("309070", "12345677", true, TestName = "2 and 9 second passes with no match weights")]
        [TestCase("309070", "99345694", true, TestName = "2 and 9 where second passes using one match weights")]
        public void CanPerformExceptionTwoAndNineCalculation(string sc, string an, bool expectedResult)
        {
            ValidateModulusCalculator(sc, an, expectedResult);
        }

        [Test]
        [TestCase("074456", "12345112", TestName = "Exception 12 and 13 where passes modulus 11 (would fail modulus 10)")]
        [TestCase("070116", "34012583", TestName = "Exception 12 and 13 where passes modulus 11 (would pass modulus 10)")]
        [TestCase("074456", "11104102", TestName = "Exception 12 and 13 where fails modulus 11 but passes modulus 10")]
        public void CanPerformExceptionTwelveAndThirteenCalculations(string sc, string an, bool expectedResult)
        {
            ValidateModulusCalculator(sc, an, expectedResult);
        }

        /// <summary>
        /// Exception 14 where the first check fails and the second check passes.
        /// </summary>
        [Test]
        [TestCase("180002", "00000190", true, "Exception 14 where the first check fails and the second check passes.")]
        [TestCase("180002", "98093517", true, "Exception 14 where the first check passes.")]
        public void CanPerformExceptionFourteenCalculations(string sc, string an, bool expectedResult)
        {
            ValidateModulusCalculator(sc, an, expectedResult);
        }
    }
}

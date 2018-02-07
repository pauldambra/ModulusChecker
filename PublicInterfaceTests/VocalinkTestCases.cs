using System;
using ModulusChecking;
using NUnit.Framework;

namespace PublicInterfaceTests
{
    public class VocalinkTestCases
    {
        private readonly ModulusChecker _modulusChecker = new ModulusChecker();

        [Test]
        [TestCase("089999", "66374958", true, TestName = "1. Pass modulus 10 check.")]
        [TestCase("107999", "88837491", true, TestName = "2. Pass modulus 11 check.")]
        [TestCase("202959", "63748472", true, TestName = "3. Pass modulus 11 and double alternate checks.")]
        [TestCase("871427", "46238510", true, TestName = "4. Exception 10 & 11 where first check passes and second fails")]
        [TestCase("872427", "46238510", true, TestName = "5. Exception 10 & 11 where first check fails and second passes")]
        [TestCase("871427", "09123496", true, TestName = "6. Exception 10 where acc. no. ab = 09 and g=9. first check passes and second fails")]
        [TestCase("871427", "99123496", true, TestName = "7. Exception 10 where acc. no. ab = 99 and g=9. first check passes and second fails")]
        [TestCase("820000", "73688637", true, TestName = "8. Exception 3 and c = 6 so ignore second check")]
        [TestCase("827999", "73988638", true, TestName = "9. Exception 3 and c = 9 so ignore second check")]
        [TestCase("827101", "28748352", true, TestName = "10. Exception 3 and c is neither 6 nor 9. so run second check")]
        [TestCase("134020", "63849203", true, TestName = "11. Exception 4 where the remainder is equal to the checkdigit.")]
        [TestCase("118765", "64371389", true, TestName = "12. Exception 1 – ensures that 27 has been added to the accumulated total")]
        [TestCase("200915", "41011166", true, TestName = "13. Exception 6 where the account fails standard check but is a foreign currency account.")]
        [TestCase("938611", "07806039", true, TestName = "14. Exception 5 where the check passes")]
        [TestCase("938600", "42368003", true, TestName = "15. Exception 5 where the check passes with substitution")]
        [TestCase("938063", "55065200", true, TestName = "16. Exception 5 where both checks produce a remainder of 0 and pass")]
        [TestCase("772798", "99345694", true, TestName = "17. Exception 7 where passes but would fail the standard check.")]
        [TestCase("086090", "06774744", true, TestName = "18. Exception 8 where the check passes.")]
        [TestCase("309070", "02355688", true, TestName = "19. 2 and 9 where first passes and second fails")]
        [TestCase("309070", "12345668", true, TestName = "20. 2 and 9 where first fails and second passes with substitution")]
        [TestCase("309070", "12345677", true, TestName = "21. 2 and 9 second passes with no match weights")]
        [TestCase("309070", "99345694", true, TestName = "22. 2 and 9 where second passes using one match weights")]
        [TestCase("938063", "15764273", false, TestName = "23. Exception 5 where the first checkdigit is correct and the second incorrect.")]
        [TestCase("938063", "15764264", false, TestName = "24. Exception 5 where the first checkdigit is incorrect and the second correct.")]
        [TestCase("938063", "15763217", false, TestName = "25. Exception 5 where the first checkdigit is incorrect with a remainder of 1.")]
        [TestCase("118765", "64371388", false, TestName = "26. Exception 1 where it fails double alternate check.")]
        [TestCase("203099", "66831036", false, TestName = "27. Pass modulus 11 check and fail double alternate check.")]
        [TestCase("203099", "58716970", false, TestName = "28. Fail modulus 11 check and pass double alternate check.")]
        [TestCase("089999", "66374959", false, TestName = "29. Fail modulus 10 check.")]
        [TestCase("107999", "88837493", false, TestName = "30. Fail modulus 11 check.")]
        [TestCase("074456", "12345112", true, TestName = "31. Exception 12 and 13 where passes modulus 11 (would fail modulus 10)")]
        [TestCase("070116", "34012583", true, TestName = "32. Exception 12 and 13 where passes modulus 11 (would pass modulus 10)")]
        [TestCase("074456", "11104102", true, TestName = "33. Exception 12 and 13 where fails modulus 11 but passes modulus 10")]
        [TestCase("180002", "00000190", true, TestName = "34. Exception 14 where the first check fails and the second check passes.")]
        public void CanPassCurrentVocalinkTestCases(string sc, string an, bool expectedResult)
        {
            Assert.AreEqual(expectedResult,_modulusChecker.CheckBankAccount(sc, an));

            var outcomeWithExplanation = _modulusChecker.CheckBankAccountWithExplanation(sc, an);
            Assert.AreEqual(expectedResult,outcomeWithExplanation.Result);
            
            Console.WriteLine(outcomeWithExplanation.Explanation);
            Assert.IsNotEmpty(outcomeWithExplanation.Explanation);
        }
    }
}

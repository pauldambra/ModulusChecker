using ModulusChecking.Steps;
using NUnit.Framework;

namespace ModulusCheckingTests.Rules
{
    public class SecondStepPostProcessingStepTests
    {
        [Test]
        [TestCase(false, false, false)]
        [TestCase(false, true, false)]
        [TestCase(true, false, false)]
        [TestCase(true, true, true)]
        public void ExceptionFiveBothChecksMustPass(bool firstCheck, bool secondCheck, bool expected)
        {
            var step = new PostProcessModulusCheckResult();

            var bankAccountDetails = new BankDetailsTestMother()
                .WithFirstWeightMapping(BankDetailsTestMother.WeightMappingWithException(5))
                .WithSecondWeightMapping(BankDetailsTestMother.WeightMappingWithException(5))
                .WithFirstCheckResult(firstCheck)
                .WithSecondCheckResult(secondCheck)
                .Build();

            var modulusCheckOutcome = step.Process(bankAccountDetails);
            
            Assert.AreEqual(expected, modulusCheckOutcome.Result);
            Assert.AreEqual("exception 5 - so first and second check must pass", modulusCheckOutcome.Explanation);
        }

        [Test]
        [TestCase(false, true, true)]
        [TestCase(true, false, true)]
        [TestCase(false, false, false)]
        public void ExceptionTenAndElevenEitherCanPass(bool firstCheck, bool secondCheck, bool expected)
        {
            var step = new PostProcessModulusCheckResult();

            var bankAccountDetails = new BankDetailsTestMother()
                .WithFirstWeightMapping(BankDetailsTestMother.WeightMappingWithException(10))
                .WithSecondWeightMapping(BankDetailsTestMother.WeightMappingWithException(11))
                .WithFirstCheckResult(firstCheck)
                .WithSecondCheckResult(secondCheck)
                .Build();

            var modulusCheckOutcome = step.Process(bankAccountDetails);
            
            Assert.AreEqual(expected, modulusCheckOutcome.Result);
            Assert.AreEqual("exception 10 and 11 - so second or first check must pass", modulusCheckOutcome.Explanation);
        }

        [Test]
        [TestCase(false, true, true)]
        [TestCase(true, false, true)]
        [TestCase(false, false, false)]
        public void ExceptionTwelveAndThirteenEitherCanPass(bool firstCheck, bool secondCheck, bool expected)
        {
            var step = new PostProcessModulusCheckResult();

            var bankAccountDetails = new BankDetailsTestMother()
                .WithFirstWeightMapping(BankDetailsTestMother.WeightMappingWithException(12))
                .WithSecondWeightMapping(BankDetailsTestMother.WeightMappingWithException(13))
                .WithFirstCheckResult(firstCheck)
                .WithSecondCheckResult(secondCheck)
                .Build();

            var modulusCheckOutcome = step.Process(bankAccountDetails);
            
            Assert.AreEqual(expected, modulusCheckOutcome.Result);
            Assert.AreEqual("exception 12 and 13 - so second or first check must pass", modulusCheckOutcome.Explanation);
        }

        [Test]
        [TestCase(false, true, true)]
        [TestCase(false, false, false)]
        [TestCase(true, false, false)]
        [TestCase(true, true, true)]
        public void OtherwiseSecondCheckDeterminesResult(bool firstCheck, bool secondCheck, bool expected)
        {
            var step = new PostProcessModulusCheckResult();

            var bankAccountDetails = new BankDetailsTestMother()
                .WithFirstWeightMapping(BankDetailsTestMother.WeightMappingWithException(-1))
                .WithSecondWeightMapping(BankDetailsTestMother.WeightMappingWithException(-1))
                .WithFirstCheckResult(firstCheck)
                .WithSecondCheckResult(secondCheck)
                .Build();

            var modulusCheckOutcome = step.Process(bankAccountDetails);
            
            Assert.AreEqual(expected, modulusCheckOutcome.Result);
            Assert.AreEqual("no exceptions affect result - using second check result", modulusCheckOutcome.Explanation);
        }
    }
}
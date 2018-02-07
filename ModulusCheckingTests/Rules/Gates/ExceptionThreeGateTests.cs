using ModulusChecking;
using ModulusChecking.Models;
using ModulusChecking.Steps.Gates;
using Moq;
using NUnit.Framework;

namespace ModulusCheckingTests.Rules.Gates
{
    public class ExceptionThreeGateTests
    {
        private Mock<IProcessAStep> _nextStep;
        private IsExceptionThreeAndCanSkipSecondCheck _isExceptionThreeAndCanSkipSecondCheck;

        [SetUp]
        public void Setup()
        {
            _nextStep = new Mock<IProcessAStep>();
            _isExceptionThreeAndCanSkipSecondCheck = new IsExceptionThreeAndCanSkipSecondCheck(_nextStep.Object);
        }
        
        [Test]
        [TestCase("6", TestName = "When account number is 6")]
        [TestCase("9", TestName = "When account number is 9")]
        public void CanSkipSecondCheckWhenExceptionThreeAndSixOrNineAtPositionTwoInAccountNumber(string accountNumber)
        {
            var bankAccountDetails = new BankAccountDetails("012345", $"00{accountNumber}00000")
            {
                WeightMappings = new[]
                {
                    BankDetailsTestMother.WeightMappingWithException(3),
                    BankDetailsTestMother.WeightMappingWithException(3)
                }
            };

            _isExceptionThreeAndCanSkipSecondCheck.Process(bankAccountDetails);

            _nextStep.Verify(ns => ns.Process(bankAccountDetails), Times.Never);
        }

        [Test]
        public void CanExplainSkippingSecondCheck()
        {
            var bankAccountDetails = new BankAccountDetails("012345", "00600000")
            {
                WeightMappings = new[]
                {
                    BankDetailsTestMother.WeightMappingWithException(3),
                    BankDetailsTestMother.WeightMappingWithException(3)
                }
            };

            var modulusCheckOutcome = _isExceptionThreeAndCanSkipSecondCheck.Process(bankAccountDetails);
            
            Assert.AreEqual("IsExceptionThreeAndCanSkipSecondCheck", modulusCheckOutcome.Explanation);
        }

        [Test]
        [TestCase(1, "6", TestName = "When Exception is not three")]
        [TestCase(3, "5", TestName = "When account number character is not six or nine")]
        public void CallsNextStepWhenAccountRequiresSecondCheck(int exception, string accountNumberCharacter)
        {
            var bankAccountDetails = new BankAccountDetails("012345", $"00{accountNumberCharacter}00000")
            {
                WeightMappings = new[]
                {
                    BankDetailsTestMother.WeightMappingWithException(3),
                    BankDetailsTestMother.WeightMappingWithException(exception)
                }
            };

            _isExceptionThreeAndCanSkipSecondCheck.Process(bankAccountDetails);

            _nextStep.Verify(ns => ns.Process(bankAccountDetails), Times.Once);
        }
        
    }
}
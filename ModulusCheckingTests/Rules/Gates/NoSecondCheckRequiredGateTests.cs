using ModulusChecking;
using ModulusChecking.Steps.Gates;
using Moq;
using NUnit.Framework;

namespace ModulusCheckingTests.Rules.Gates
{
    public class NoSecondCheckRequiredGateTests
    {
        private Mock<IProcessAStep> _nextStep;
        private IsSecondCheckRequiredGate _isSecondCheckRequiredGate;

        [SetUp]
        public void Setup()
        {
            _nextStep = new Mock<IProcessAStep>();
            _isSecondCheckRequiredGate = new IsSecondCheckRequiredGate(_nextStep.Object);
        }
        
        [Test]
        [TestCase(2)]
        [TestCase(9)]
        [TestCase(10)]
        [TestCase(11)]
        [TestCase(12)]
        [TestCase(13)]
        [TestCase(14)]
        public void ExceptionRequiresSecondCheckItCallsNext(int exception)
        {
            var bankAccountDetails = BankDetailsTestMother.BankDetailsWithException(exception);

            _isSecondCheckRequiredGate.Process(bankAccountDetails);
            
            _nextStep.Verify(ns => ns.Process(bankAccountDetails), Times.Once);
        }

        [Test]
        [TestCase(1)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        [TestCase(15)]
        public void DoesNotCallSecondCheckIfExceptionDoesNotRequireIt(int exception)
        {
            var bankAccountDetails = BankDetailsTestMother.BankDetailsWithException(exception);

            _isSecondCheckRequiredGate.Process(bankAccountDetails);
            
            _nextStep.Verify(ns => ns.Process(bankAccountDetails), Times.Never);
        }

        [Test]
        public void CanExplainWhyItDoesNotCallSecondCheck()
        {
            var bankAccountDetails = BankDetailsTestMother.BankDetailsWithException(3);

            var modulusCheckOutcome = _isSecondCheckRequiredGate.Process(bankAccountDetails);
            
            Assert.AreEqual("first weight mapping exception does not require second check", modulusCheckOutcome.Explanation);
        }
    }
}
using ModulusChecking;
using ModulusChecking.Steps.Gates;
using Moq;
using NUnit.Framework;

namespace ModulusCheckingTests.Rules.Gates
{
    /// <summary>
    ///          public bool IsExceptionTwoAndFirstCheckPassed()
    ///        {
    ///            return FirstResult &amp;&amp; WeightMappings.First().Exception == 2;
    ///        }
    /// </summary>
    public class ExceptionTwoGateTests
    {
        private Mock<IProcessAStep> _nextStep;
        private IsExceptionTwoAndFirstCheckPassedGate _isExceptionTwoAndFirstCheckPassedGate;

        [SetUp]
        public void Setup()
        {
            _nextStep = new Mock<IProcessAStep>();
            _isExceptionTwoAndFirstCheckPassedGate = new IsExceptionTwoAndFirstCheckPassedGate(_nextStep.Object);
        }
        
        [Test]
        public void CanSkipSecondCheckForExceptionTwoWithPassedFirstCheck()
        {
            var details = BankDetailsTestMother.BankDetailsWithException(2);
            details.FirstResult = true;
            
            _isExceptionTwoAndFirstCheckPassedGate.Process(details);
            
            _nextStep.Verify(ns => ns.Process(details), Times.Never);
        }

        [Test]
        public void CanExplainSkippingSecondCheck()
        {
            var details = BankDetailsTestMother.BankDetailsWithException(2);
            details.FirstResult = true;

            var modulusCheckOutcome = _isExceptionTwoAndFirstCheckPassedGate.Process(details);
            
            Assert.AreEqual("IsExceptionTwoAndFirstCheckPassed", modulusCheckOutcome.Explanation);
        }

        [Test]
        [TestCase(1, true, TestName = "When exception is not 2")]
        [TestCase(2, false, TestName = "When first check failed")]
        public void CanCallNextStepWhenAccountDoesNotQualifyToSkip(int exception, bool firstCheck)
        {
            var details = BankDetailsTestMother.BankDetailsWithException(exception);
            details.FirstResult = firstCheck;
            
            _isExceptionTwoAndFirstCheckPassedGate.Process(details);
            
            _nextStep.Verify(ns => ns.Process(details), Times.Once);
        }
    }
}
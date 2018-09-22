using ModulusChecking;
using ModulusChecking.Steps.Calculators;
using ModulusChecking.Steps.Gates;
using Moq;
using NUnit.Framework;

namespace ModulusCheckingTests.Rules.Gates
{
    public class ExceptionFourteenGateTests
    {
        public class WhenIsCoutts
        {
            private static readonly FirstStandardModulusElevenCalculatorExceptionFive
                StandardModulusElevenCalculatorExceptionFive =
                    new Mock<FirstStandardModulusElevenCalculatorExceptionFive>(null).Object;
            private readonly Mock<StandardModulusExceptionFourteenCalculator> _mockCalc = new Mock<StandardModulusExceptionFourteenCalculator>(StandardModulusElevenCalculatorExceptionFive);
            private readonly Mock<IProcessAStep> _nextStep = new Mock<IProcessAStep>();
            private ExceptionFourteenGate _exceptionFourteenGate;

            [SetUp]
            public void Setup()
            {
                _exceptionFourteenGate = new ExceptionFourteenGate(_mockCalc.Object, _nextStep.Object);
            }
                
            [Test]
            public void ItReturnsFirstResultWhenThatPasses()
            {
                var details = BankDetailsTestMother.BankDetailsWithException(14);
                details.FirstResult = true;
                    
                _exceptionFourteenGate.Process(details);
                    
                _nextStep.Verify(ns => ns.Process(details), Times.Never);
                _mockCalc.Verify(mc => mc.Process(details), Times.Never);
            }
                
            [Test]
            public void ItExplainsThatItReturnsFirstResultWhenThatPasses()
            {
                var details = BankDetailsTestMother.BankDetailsWithException(14);
                details.FirstResult = true;

                var modulusCheckOutcome = _exceptionFourteenGate.Process(details);
                    
                Assert.AreEqual("Coutts Account with passing first check", modulusCheckOutcome.Explanation);
            }

            [Test]
            public void ItCallsTheExceptionCalculatorWhenTheFirstTestFails()
            {
                var details = BankDetailsTestMother.BankDetailsWithException(14);
                details.FirstResult = false;
                    
                _exceptionFourteenGate.Process(details);
                    
                _nextStep.Verify(ns => ns.Process(details), Times.Never);
                _mockCalc.Verify(mc => mc.Process(details), Times.Once);
            }
                
            [Test]
            public void ItExplainsThatItCallsTheExceptionCalculatorWhenTheFirstTestFails()
            {
                var details = BankDetailsTestMother.BankDetailsWithException(14);
                details.FirstResult = false;

                var modulusCheckOutcome = _exceptionFourteenGate.Process(details);
                Assert.AreEqual("StandardModulusExceptionFourteenCalculator", modulusCheckOutcome.Explanation);
            }
        }

        public class WhenIsNotCoutts
        {
            [Test]
            public void ItCallsTheNextStep()
            {
                var 
                standardModulusElevenCalculatorExceptionFive =
                new Mock<FirstStandardModulusElevenCalculatorExceptionFive>(null).Object;

                var mockCalc = new Mock<StandardModulusExceptionFourteenCalculator>(standardModulusElevenCalculatorExceptionFive);
                var nextStep = new Mock<IProcessAStep>();
                var gate = new ExceptionFourteenGate(mockCalc.Object, nextStep.Object);

                var details = BankDetailsTestMother.BankDetailsWithException(0);

                gate.Process(details);
                    
                nextStep.Verify(ns => ns.Process(details), Times.Once);
                mockCalc.Verify(mc => mc.Process(details), Times.Never);
            }
        }

        
    }
}
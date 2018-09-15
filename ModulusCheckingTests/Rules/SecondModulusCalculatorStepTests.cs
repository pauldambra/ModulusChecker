using ModulusChecking;
using ModulusChecking.Models;
using ModulusChecking.Steps;
using ModulusChecking.Steps.Calculators;
using Moq;
using NUnit.Framework;

namespace ModulusCheckingTests.Rules
{
    public class SecondModulusCalculatorStepTests
    {
        private Mock<SecondDoubleAlternateCalculator> _doubleAlternate;
        private Mock<SecondStandardModulusElevenCalculator> _standardEleven;
        private Mock<SecondStandardModulusTenCalculator> _standardTen;
        private SecondStepRouter _secondStepRouter;
        private Mock<IProcessAStep> _nextStep;

        [SetUp]
        public void Before()
        {
            _standardTen = new Mock<SecondStandardModulusTenCalculator>();
            _standardEleven = new Mock<SecondStandardModulusElevenCalculator>();
            _doubleAlternate = new Mock<SecondDoubleAlternateCalculator>(null);

            _standardTen.Setup(nr => nr.Process(It.IsAny<BankAccountDetails>())).Returns(true);
            _standardEleven.Setup(nr => nr.Process(It.IsAny<BankAccountDetails>())).Returns(true);
            _doubleAlternate.Setup(nr => nr.Process(It.IsAny<BankAccountDetails>())).Returns(true);

            _secondStepRouter = new SecondStepRouter(_standardTen.Object, _standardEleven.Object, _doubleAlternate.Object);

            _nextStep = new Mock<IProcessAStep>();
        }

        [Test]
        public void CanChooseMod10()
        {
            var secondModulusCalculatorStep = new SecondModulusCalculatorStep(_secondStepRouter, _nextStep.Object);
            var details = BankDetailsTestMother.BankDetailsWithAlgorithm(ModulusAlgorithm.Mod10);
            secondModulusCalculatorStep.Process(details);
            
            _standardTen.Verify(st => st.Process(details), Times.Once);
            _standardEleven.Verify(st => st.Process(details), Times.Never);
            _doubleAlternate.Verify(st => st.Process(details), Times.Never);
            _nextStep.Verify(st => st.Process(details), Times.Once);
        }
        
        [Test]
        public void CanChooseMod11()
        {
            var secondModulusCalculatorStep = new SecondModulusCalculatorStep(_secondStepRouter, _nextStep.Object);
            var details = BankDetailsTestMother.BankDetailsWithAlgorithm(ModulusAlgorithm.Mod11);
            secondModulusCalculatorStep.Process(details);
            
            _standardTen.Verify(st => st.Process(details), Times.Never);
            _standardEleven.Verify(st => st.Process(details), Times.Once);
            _doubleAlternate.Verify(st => st.Process(details), Times.Never);
            _nextStep.Verify(st => st.Process(details), Times.Once);
        }
        
        [Test]
        public void CanChooseDblAl()
        {
            var secondModulusCalculatorStep = new SecondModulusCalculatorStep(_secondStepRouter, _nextStep.Object);
            var details = BankDetailsTestMother.BankDetailsWithAlgorithm(ModulusAlgorithm.DblAl);
            secondModulusCalculatorStep.Process(details);
            
            _standardTen.Verify(st => st.Process(details), Times.Never);
            _standardEleven.Verify(st => st.Process(details), Times.Never);
            _doubleAlternate.Verify(st => st.Process(details), Times.Once);
            _nextStep.Verify(st => st.Process(details), Times.Once);
        }
    }
}
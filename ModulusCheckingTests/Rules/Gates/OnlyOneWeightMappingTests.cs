using ModulusChecking;
using ModulusChecking.Models;
using ModulusChecking.Steps.Gates;
using Moq;
using NUnit.Framework;

namespace ModulusCheckingTests.Rules.Gates
{
    public class OnlyOneWeightMappingTests
    {
        private Mock<IProcessAStep> _nextStep;
        private OnlyOneWeightMappingGate _onlyOneWeightMappingGate;
        private BankAccountDetails _bankAccountDetails;

        [SetUp]
        public void Setup()
        {
            _nextStep = new Mock<IProcessAStep>();
            _onlyOneWeightMappingGate = new OnlyOneWeightMappingGate(_nextStep.Object);
            _bankAccountDetails = new BankAccountDetails("000000", "00000000");
        }
        
        [Test]
        public void IfThereIsOnlyOneMappingItReturns()
        {
            _bankAccountDetails.WeightMappings = new[]
            {
                BankDetailsTestMother.AnyModulusWeightMapping()
            };

            _onlyOneWeightMappingGate.Process(_bankAccountDetails);
            
            _nextStep.Verify(ns => ns.Process(It.IsAny<BankAccountDetails>()), Times.Never);
        }

        [Test]
        public void ItCanExplainThatThereWasOnlyOneMapping()
        {
            _bankAccountDetails.WeightMappings = new[]
            {
                BankDetailsTestMother.AnyModulusWeightMapping()
            };

            var modulusCheckOutcome = _onlyOneWeightMappingGate.Process(_bankAccountDetails);

            Assert.AreEqual("not proceeding to the second check as there is only one weight mapping", modulusCheckOutcome.Explanation);
        }

        [Test]
        public void IfThereAreTwoMappingsItCallsTheNextStep()
        {
            _bankAccountDetails.WeightMappings = new[]
            {
                BankDetailsTestMother.AnyModulusWeightMapping(),
                BankDetailsTestMother.AnyModulusWeightMapping()
            };

            _onlyOneWeightMappingGate.Process(_bankAccountDetails);
            
            _nextStep.Verify(ns => ns.Process(_bankAccountDetails), Times.Once);
        }
        
        
    }
}
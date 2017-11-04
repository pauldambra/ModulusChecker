using ModulusChecking.Models;
using ModulusChecking.Steps;
using ModulusChecking.Steps.ConfirmDetailsAreValid;
using Moq;
using NUnit.Framework;

namespace ModulusCheckingTests.Rules
{
    public class IsUncheckableForeignAccountTests
    {
        private readonly Mock<FirstModulusCalculatorStep> _firstModulusCalculatorStep = new Mock<FirstModulusCalculatorStep>();
        private IsUncheckableForeignAccount _isUncheckableForeignAccountSteps;

        private static readonly ModulusWeightMapping[] ModulusWeightMappings = {
            new ModulusWeightMapping(
                new SortCode("010007"),
                new SortCode("010010"),
                ModulusAlgorithm.DblAl, 
                new[] {2,1,2,1,2,1,2,1,2,1,2,1,2,1}, 
                6)
        };

        private readonly BankAccountDetails _bankAccountDetails = new BankAccountDetails("200915", "41011166")
        {
            WeightMappings = ModulusWeightMappings
        };

        [SetUp]
        public void Before()
        {
            _isUncheckableForeignAccountSteps = new IsUncheckableForeignAccount(_firstModulusCalculatorStep.Object);
        }

        [Test]
        public void CheckableAccountRunsNextStep()
        {
            var accountDetails = new BankAccountDetails("010008", "400000000")
            {
                WeightMappings = ModulusWeightMappings
            };
            
            _isUncheckableForeignAccountSteps.Process(accountDetails);
            
            _firstModulusCalculatorStep.Verify(nr => nr.Process(accountDetails));
        }

        [Test]
        public void CorrectlySkipsUncheckableForeignAccount()
        {
            var outcome = _isUncheckableForeignAccountSteps.Process(_bankAccountDetails);
            Assert.IsTrue(outcome);
            _firstModulusCalculatorStep.Verify(nr => nr.Process(_bankAccountDetails), Times.Never());
        }
        
        [Test]
        public void CanExplainUncheckableForeignAccount()
        {
            var outcome = _isUncheckableForeignAccountSteps.Process(_bankAccountDetails);
            Assert.IsNotNullOrEmpty(outcome.Explanation);
        }
    }
}
using System.Collections.Generic;
using ModulusChecking;
using ModulusChecking.Models;
using ModulusChecking.Steps.ConfirmDetailsAreValid;
using Moq;
using NUnit.Framework;

namespace ModulusCheckingTests.Rules
{
    public class HasWeightMappingsTests
    {
        private HasWeightMappings _hasWeightMappingsStep;
        private Mock<IProcessAStep> _nextStep;

        private readonly List<ModulusWeightMapping> _literallyAnyMapping = new List<ModulusWeightMapping>
        {
            new ModulusWeightMapping(
                new SortCode("010004"), 
                new SortCode("010004"), 
                ModulusAlgorithm.DblAl,
                new [] {0, 1, 2}, 
                1)
        };

        [SetUp]
        public void Before()
        {
            _nextStep = new Mock<IProcessAStep>(); 
            _hasWeightMappingsStep = new HasWeightMappings(_nextStep.Object);
        }

        [Test]
        public void UnknownSortCodeIsValid()
        {
            const string sortCode = "123456";
            var accountDetails = new BankAccountDetails(sortCode, "12345678")
            {
                //unknown sort code loads no weight mappings
                WeightMappings = new List<ModulusWeightMapping>()
            };
            var result = _hasWeightMappingsStep.Process(accountDetails);
            Assert.IsTrue(result);
            
            Assert.IsTrue(accountDetails.FirstResult);
            
            _nextStep.Verify(fmc => fmc.Process(It.IsAny<BankAccountDetails>()), Times.Never());
        }
        
        [Test]
        public void UnknownSortCodeCanBeExplained()
        {
            var accountDetails = new BankAccountDetails("123456", "12345678")
            {
                //unknown sort code loads no weight mappings
                WeightMappings = new List<ModulusWeightMapping>()
            };
            
            var result = _hasWeightMappingsStep.Process(accountDetails);
            
            Assert.IsNotEmpty(result.Explanation);
        }

        [Test]
        public void KnownSortCodeIsTested()
        {
            var accountDetails = new BankAccountDetails("010004", "12345678")
            {
                WeightMappings = _literallyAnyMapping
            };

            _hasWeightMappingsStep.Process(accountDetails);
            
            _nextStep.Verify(nr => nr.Process(accountDetails));
        }
    }
}
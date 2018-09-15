using System.Collections.Generic;
using ModulusChecking;
using ModulusChecking.Models;
using ModulusChecking.Steps;
using ModulusChecking.Steps.Calculators;
using Moq;
using NUnit.Framework;

namespace ModulusCheckingTests.Rules
{
    public class FirstModulusCalculatorStepTests
    {
        private readonly Mock<FirstDoubleAlternateCalculator> _firstDoubleAlternate = new Mock<FirstDoubleAlternateCalculator>(null);
        private readonly Mock<FirstStandardModulusElevenCalculator> _standardEleven = new Mock<FirstStandardModulusElevenCalculator>(null);
        private readonly Mock<FirstStandardModulusTenCalculator> _standardTen = new Mock<FirstStandardModulusTenCalculator>();
        private readonly Mock<IProcessAStep> _gates = new Mock<IProcessAStep>();
        
        private FirstStepRouter _firstStepRouter;
        private FirstModulusCalculatorStep _firstCalculatorStep;

        [SetUp]
        public void Before()
        {   
            _standardTen.Setup(nr => nr.Process(It.IsAny<BankAccountDetails>())).Returns(true);
            _standardEleven.Setup(nr => nr.Process(It.IsAny<BankAccountDetails>())).Returns(true);
            _firstDoubleAlternate.Setup(nr => nr.Process(It.IsAny<BankAccountDetails>())).Returns(true);
            
            _firstStepRouter = new FirstStepRouter(_standardTen.Object, _standardEleven.Object, _firstDoubleAlternate.Object);
            _firstCalculatorStep = new FirstModulusCalculatorStep(_firstStepRouter, _gates.Object);
        }

        [Test]
        public void CallsStandardTen()
        {
            var accountDetails = BankAccountDetailsForModulusCheck(ModulusAlgorithm.Mod10);
            
            _firstCalculatorStep.Process(accountDetails);

            _standardTen.Verify(st => st.Process(accountDetails), Times.Once);
            _standardEleven.Verify(se => se.Process(accountDetails), Times.Never);
            _firstDoubleAlternate.Verify(fd => fd.Process(accountDetails), Times.Never);
            _gates.Verify(g => g.Process(accountDetails), Times.Once);
        }
        
        [Test]
        public void CallsStandardEleven()
        {
            var accountDetails = BankAccountDetailsForModulusCheck(ModulusAlgorithm.Mod11);
            
            _firstCalculatorStep.Process(accountDetails);
            
            _standardTen.Verify(st => st.Process(accountDetails), Times.Never);
            _standardEleven.Verify(se => se.Process(accountDetails), Times.Once);
            _firstDoubleAlternate.Verify(fd => fd.Process(accountDetails), Times.Never);
            _gates.Verify(g => g.Process(accountDetails), Times.Once);
        }
        
        [Test]
        public void CallsDblAl()
        {
            var accountDetails = BankAccountDetailsForModulusCheck(ModulusAlgorithm.DblAl);
            
            _firstCalculatorStep.Process(accountDetails);
            
            _standardTen.Verify(st => st.Process(accountDetails), Times.Never);
            _standardEleven.Verify(se => se.Process(accountDetails), Times.Never);
            _firstDoubleAlternate.Verify(fd => fd.Process(accountDetails), Times.Once);
            _gates.Verify(g => g.Process(accountDetails), Times.Once);
            
        }

        private static BankAccountDetails BankAccountDetailsForModulusCheck(ModulusAlgorithm modulusAlgorithm)
        {
            var accountDetails = new BankAccountDetails("010004", "12345678")
            {
                WeightMappings = new List<ModulusWeightMapping>
                {
                    new ModulusWeightMapping(
                        new SortCode("010004"),
                        new SortCode("010004"),
                        modulusAlgorithm,
                        new[] {1, 2, 1, 2, 1, 1, 2, 2, 1, 2,},
                        0)
                }
            };
            return accountDetails;
        }
    }
}
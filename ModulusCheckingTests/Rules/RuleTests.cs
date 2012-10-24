using System.Collections.Generic;
using System.Linq;
using ModulusChecking.Models;
using ModulusChecking.ModulusChecks;
using ModulusChecking.Parsers;
using ModulusChecking.Steps;
using ModulusChecking.Steps.Calculators;
using NUnit.Framework;
using Moq;

namespace ModulusCheckingTests.Rules
{
    public class RuleTests
    {
        private ModulusWeights _modulusWeight;

        [SetUp]
        public void Before()
        {
            var mappingSource = new Mock<IRuleMappingSource>();
            mappingSource.Setup(ms => ms.GetModulusWeightMappings()).Returns(new List<ModulusWeightMapping>
                                                                                 {
                                                                                     new ModulusWeightMapping(
                                                                                         "010004 010006 MOD10 2 1 2 1 2  1 2 1 2 1 2 1 2 1"),
                                                                                     new ModulusWeightMapping(
                                                                                         "010004 010006 DBLAL 2 1 2 1 2  1 2 1 2 1 2 1 2 1"),
                                                                                     new ModulusWeightMapping(
                                                                                         "010007 010010 DBLAL  2 1 2 1 2  1 2 1 2 1 2 1 2 1"),
                                                                                     new ModulusWeightMapping(
                                                                                         "010011 010013 MOD11    2 1 2 1 2  1 2 1 2 1 2 1 2 1"),
                                                                                     new ModulusWeightMapping(
                                                                                         "010014 010014 MOD11    2 1 2 1 2  1 2 1 2 1 2 1 2 1 5")
                                                                                 });
            _modulusWeight = new ModulusWeights(mappingSource.Object);
        }

        [Test]
        public void UnknownSortCodeIsValid()
        {
            const string sortCode = "123456";
            var accountDetails = new BankAccountDetails(sortCode, "12345678");
            var result = new SortCodeMustExist().Process(accountDetails, _modulusWeight);
            Assert.IsTrue(result);
        }

        [Test]
        public void KnownSortCodeIsTested()
        {
            var accountDetails = new BankAccountDetails("010004", "12345678");
            var nextStep = new Mock<FirstModulusCalculatorStep>();
            nextStep.Setup(nr => nr.Process(accountDetails,_modulusWeight)).Returns(true);
            new SortCodeMustExist(nextStep.Object).Process(accountDetails, _modulusWeight);
            nextStep.Verify(nr => nr.Process(accountDetails, _modulusWeight));
        }

        [Test]
        public void KnownSortCodeIsFoundWithinRange()
        {
            var accountDetails = new BankAccountDetails("010009", "12345678");
            var nextRule = new Mock<FirstModulusCalculatorStep>();
            nextRule.Setup(nr => nr.Process(accountDetails, _modulusWeight)).Returns(true);
            new SortCodeMustExist(nextRule.Object).Process(accountDetails, _modulusWeight);
            nextRule.Verify(nr => nr.Process(accountDetails, _modulusWeight));
        }

        [Test]
        [TestCase("010004", "MOD10")]
        [TestCase("010008", "DBLAL")]
        [TestCase("010013", "MOD11")]
        [TestCase("010014", "MOD11")]
        public void CanSelectForModulus(string sc, string expectedModulusCheck)
        {
            var accountDetails = new BankAccountDetails(sc,"12345678");
            var standardTen = new Mock<FirstStandardModulusTenCalculator>();
            var standardEleven = new Mock<FirstStandardModulusElevenCalculator>();
            var doubleAlternate = new Mock<DoubleAlternateCalculator>(BaseModulusCalculator.Step.Second);
            var standardExceptionFive = new Mock<FirstStandardModulusElevenCalculatorExceptionFive>();
            var secondModulusCalculation = new Mock<SecondModulusCalculatorStep>();
            var doubleAlternateExceptionFive = new Mock<DoubleAlternateCalculatorExceptionFive>(BaseModulusCalculator.Step.Second);
            var exceptionFourteenCalculator = new Mock<StandardModulusExceptionFourteenCalculator>();

            standardTen.Setup(nr => nr.Process(accountDetails,_modulusWeight)).Returns(true);
            standardEleven.Setup(nr => nr.Process(accountDetails,_modulusWeight)).Returns(true);
            doubleAlternate.Setup(nr => nr.Process(accountDetails,_modulusWeight)).Returns(true);
            standardExceptionFive.Setup(nr => nr.Process(accountDetails,_modulusWeight)).Returns(true);
            secondModulusCalculation.Setup(nr => nr.Process(accountDetails, _modulusWeight)).Returns(true);
            doubleAlternateExceptionFive.Setup(nr => nr.Process(accountDetails, _modulusWeight)).Returns(true);
            exceptionFourteenCalculator.Setup(nr => nr.Process(accountDetails, _modulusWeight)).Returns(true);

            new FirstModulusCalculatorStep(standardTen.Object, standardEleven.Object, 
                                        doubleAlternate.Object, standardExceptionFive.Object, 
                                        secondModulusCalculation.Object, doubleAlternateExceptionFive.Object,
                                        exceptionFourteenCalculator.Object)
                                        .Process(accountDetails, _modulusWeight);
            switch (expectedModulusCheck)
            {
                case "MOD10" :
                    standardTen.Verify(nr => nr.Process(accountDetails, _modulusWeight));
                    break;
                case "MOD11" :
                    if (_modulusWeight.GetRuleMappings(accountDetails.SortCode).First().Exception == 5)
                    {
                        standardExceptionFive.Verify(nr => nr.Process(accountDetails, _modulusWeight));
                    }
                    else
                    {
                        standardEleven.Verify(nr => nr.Process(accountDetails, _modulusWeight));
                    }
                    break;
                case "DBLAL" :
                    doubleAlternate.Verify(nr => nr.Process(accountDetails, _modulusWeight));
                    break;
            }       
        }

        [Test]
        [TestCase("010004", "MOD10")]
        public void CanCallSecondCalculationCorrectly(string sc)
        {
            var accountDetails = new BankAccountDetails(sc, "12345678");
            var standardTen = new Mock<FirstStandardModulusTenCalculator>();
            var standardEleven = new Mock<FirstStandardModulusElevenCalculator>();
            var doubleAlternate = new Mock<DoubleAlternateCalculator>();
            var standardExceptionFive = new Mock<FirstStandardModulusElevenCalculatorExceptionFive>();
            var secondModulusCalculation = new Mock<SecondModulusCalculatorStep>();
            var doubleAlternateExceptionFive = new Mock<DoubleAlternateCalculatorExceptionFive>();
            var exceptionFourteenCalculator = new Mock<StandardModulusExceptionFourteenCalculator>();

            standardTen.Setup(nr => nr.Process(accountDetails, _modulusWeight)).Returns(true);
            standardEleven.Setup(nr => nr.Process(accountDetails, _modulusWeight)).Returns(true);
            doubleAlternate.Setup(nr => nr.Process(accountDetails, _modulusWeight)).Returns(true);
            standardExceptionFive.Setup(nr => nr.Process(accountDetails, _modulusWeight)).Returns(true);
            secondModulusCalculation.Setup(nr => nr.Process(accountDetails, _modulusWeight)).Returns(true);
            doubleAlternateExceptionFive.Setup(nr => nr.Process(accountDetails, _modulusWeight)).Returns(true);
            exceptionFourteenCalculator.Setup(nr => nr.Process(accountDetails, _modulusWeight)).Returns(true);

            new FirstModulusCalculatorStep(standardTen.Object, standardEleven.Object, doubleAlternate.Object,
                                        standardExceptionFive.Object, secondModulusCalculation.Object,
                                        doubleAlternateExceptionFive.Object, exceptionFourteenCalculator.Object)
                                        .Process(accountDetails,_modulusWeight);
            secondModulusCalculation.Verify(smc=>smc.Process(accountDetails, _modulusWeight));
        }
    }
}

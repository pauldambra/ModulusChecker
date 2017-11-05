using System.Linq;
using ModulusChecking.Models;
using ModulusChecking.Steps.Calculators;

namespace ModulusChecking.Steps
{
    internal class Gates : IProcessAStep
    {
        private readonly SecondModulusCalculatorStep _secondModulusCalculatorStep;
        
        private readonly StandardModulusExceptionFourteenCalculator _exceptionFourteenCalculator;
        
        public Gates()
        {
            _secondModulusCalculatorStep = new SecondModulusCalculatorStep();
            _exceptionFourteenCalculator = new StandardModulusExceptionFourteenCalculator();
        }

        public Gates(
            SecondModulusCalculatorStep smc,
            StandardModulusExceptionFourteenCalculator efc)
        {
            _secondModulusCalculatorStep = smc;
            _exceptionFourteenCalculator = efc;
        }
        
        public ModulusCheckOutcome Process(BankAccountDetails bankAccountDetails)
        {
            if (bankAccountDetails.RequiresCouttsAccountCheck())
            {
                return ExceptionFourteenForCouttsAccounts(bankAccountDetails);
            }
            if (bankAccountDetails.WeightMappings.Count() == 1)
            {
                return new ModulusCheckOutcome("only one weight mapping", bankAccountDetails.FirstResult);
            }
            if (!bankAccountDetails.IsSecondCheckRequired())
            {
                return new ModulusCheckOutcome("second check not required", bankAccountDetails.FirstResult);
            }
            if (bankAccountDetails.IsExceptionTwoAndFirstCheckPassed())
            {
                return new ModulusCheckOutcome("IsExceptionTwoAndFirstCheckPassed");
            }
            if (bankAccountDetails.IsExceptionThreeAndCanSkipSecondCheck())
            {
                return new ModulusCheckOutcome("IsExceptionThreeAndCanSkipSecondCheck", bankAccountDetails.FirstResult);
            }
            return new ModulusCheckOutcome("second step", _secondModulusCalculatorStep.Process(bankAccountDetails));
        }
        
        private ModulusCheckOutcome ExceptionFourteenForCouttsAccounts(BankAccountDetails bankAccountDetails)
        {
            return bankAccountDetails.FirstResult 
                ? new ModulusCheckOutcome("Coutts Account with passing first check", bankAccountDetails.FirstResult)
                : new ModulusCheckOutcome("FirstModulusCalculatorStep", _exceptionFourteenCalculator.Process(bankAccountDetails));
        }
    }
}
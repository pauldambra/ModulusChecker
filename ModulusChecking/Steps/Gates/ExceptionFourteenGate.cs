using ModulusChecking.Models;
using ModulusChecking.Steps.Calculators;

namespace ModulusChecking.Steps
{
    internal class ExceptionFourteenGate : IProcessAStep
    {
        private readonly StandardModulusExceptionFourteenCalculator _exceptionFourteenCalculator;
        private readonly IProcessAStep _nextStep;

        public ExceptionFourteenGate()
        {
            _exceptionFourteenCalculator = new StandardModulusExceptionFourteenCalculator();
            
        }

        public ExceptionFourteenGate(
            StandardModulusExceptionFourteenCalculator exceptionFourteenCalculator,
            IProcessAStep nextStep)
        {
            _exceptionFourteenCalculator = exceptionFourteenCalculator;
            _nextStep = nextStep;
        }

        public ModulusCheckOutcome Process(BankAccountDetails bankAccountDetails) => 
            bankAccountDetails.RequiresCouttsAccountCheck() 
                ? ExceptionFourteenForCouttsAccounts(bankAccountDetails) 
                : _nextStep.Process(bankAccountDetails);

        private ModulusCheckOutcome ExceptionFourteenForCouttsAccounts(BankAccountDetails bankAccountDetails)
        {
            return bankAccountDetails.FirstResult 
                ? new ModulusCheckOutcome("Coutts Account with passing first check", bankAccountDetails.FirstResult)
                : new ModulusCheckOutcome("FirstModulusCalculatorStep", _exceptionFourteenCalculator.Process(bankAccountDetails));
        }
    }
}
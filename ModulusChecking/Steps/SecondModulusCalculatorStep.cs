using ModulusChecking.Models;
using ModulusChecking.Steps.Calculators;

namespace ModulusChecking.Steps
{
    internal class SecondModulusCalculatorStep : IProcessAStep
    {
        private readonly SecondStepRouter _secondStepRouter;
        private readonly IProcessAStep _nextStep;

        public SecondModulusCalculatorStep(SecondStepRouter secondStepRouter, IProcessAStep nextStep)
        {
            _secondStepRouter = secondStepRouter;
            _nextStep = nextStep;
        }

        public ModulusCheckOutcome Process(BankAccountDetails bankAccountDetails)
        {
            bankAccountDetails.SecondResult = _secondStepRouter.GetModulusCalculation(bankAccountDetails);
            return _nextStep.Process(bankAccountDetails);
        }
    }
}
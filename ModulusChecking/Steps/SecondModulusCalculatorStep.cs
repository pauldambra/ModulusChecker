using ModulusChecking.Models;
using ModulusChecking.Steps.Calculators;

namespace ModulusChecking.Steps
{
    internal class SecondModulusCalculatorStep : IProcessAStep
    {
        private readonly SecondStepRouter _secondStepRouter;

        public SecondModulusCalculatorStep()
        {
            _secondStepRouter = new SecondStepRouter();
        }

        public ModulusCheckOutcome Process(BankAccountDetails bankAccountDetails)
        {
            bankAccountDetails.SecondResult = _secondStepRouter.GetModulusCalculation(bankAccountDetails);
            return new ModulusCheckOutcome("SecondModulusCalculatorStep", new PostProcessModulusCheckResult().Process(bankAccountDetails));
        }
    }
}
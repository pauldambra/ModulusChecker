using ModulusChecking.Models;
using ModulusChecking.Steps.Calculators;

namespace ModulusChecking.Steps
{
    internal class SecondModulusCalculatorStep
    {
        private readonly SecondStepRouter _secondStepRouter;

        public SecondModulusCalculatorStep()
        {
            _secondStepRouter = new SecondStepRouter();
        }

        public virtual bool Process(BankAccountDetails bankAccountDetails)
        {
            bankAccountDetails.SecondResult = _secondStepRouter.GetModulusCalculation(bankAccountDetails);
            return new PostProcessModulusCheckResult().Process(bankAccountDetails);
        }
    }
}
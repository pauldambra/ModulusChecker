using ModulusChecking.Loaders;
using ModulusChecking.Models;

namespace ModulusChecking.Steps
{
    /// <summary>
    /// Once the sortcode is confirmed to be present in the ModulusWeightTable.Determine and complete
    /// carry out the first check.
    /// </summary>
    internal class FirstModulusCalculatorStep : IProcessAStep
    {
        private readonly FirstStepRouter _firstStepRouter;
        private readonly IProcessAStep _gates;

        public FirstModulusCalculatorStep(SortCodeSubstitution sortCodeSubstitution)
        {
            _firstStepRouter = new FirstStepRouter(sortCodeSubstitution);
            _gates = new Gates.GatePipeline(sortCodeSubstitution);
        }

        public FirstModulusCalculatorStep(FirstStepRouter firstStepRouter, IProcessAStep gates)
        {
            _firstStepRouter = firstStepRouter;
            _gates = gates;
        }

        public ModulusCheckOutcome Process(BankAccountDetails bankAccountDetails)
        {
            bankAccountDetails.FirstResult = _firstStepRouter.GetModulusCalculation(bankAccountDetails);
            return _gates.Process(bankAccountDetails);
        }
    }
}
using ModulusChecking.Models;
using ModulusChecking.Steps.Calculators;

namespace ModulusChecking.Steps.Gates
{
    internal class GatePipeline : IProcessAStep
    {
        private readonly ExceptionFourteenGate _exceptionFourteenGate;

        public GatePipeline()
        {
            var secondModulusCalculatorStep = new SecondModulusCalculatorStep();
            var exceptionFourteenCalculator = new StandardModulusExceptionFourteenCalculator();
            
            var isExceptionThreeAndCanSkipSecondCheck = new IsExceptionThreeAndCanSkipSecondCheck(secondModulusCalculatorStep);
            var isExceptionTwoAndFirstCheckPassedGate = new IsExceptionTwoAndFirstCheckPassedGate(isExceptionThreeAndCanSkipSecondCheck);
            var isSecondCheckRequiredGate = new IsSecondCheckRequiredGate(isExceptionTwoAndFirstCheckPassedGate);
            var onlyOneWeightMappingGate = new OnlyOneWeightMappingGate(isSecondCheckRequiredGate);
            _exceptionFourteenGate = new ExceptionFourteenGate(exceptionFourteenCalculator, onlyOneWeightMappingGate);
        }

        public GatePipeline(
            SecondModulusCalculatorStep smc,
            StandardModulusExceptionFourteenCalculator efc)
        {
            var isExceptionThreeAndCanSkipSecondCheck = new IsExceptionThreeAndCanSkipSecondCheck(smc);
            var isExceptionTwoAndFirstCheckPassedGate = new IsExceptionTwoAndFirstCheckPassedGate(isExceptionThreeAndCanSkipSecondCheck);
            var isSecondCheckRequiredGate = new IsSecondCheckRequiredGate(isExceptionTwoAndFirstCheckPassedGate);
            var onlyOneWeightMappingGate = new OnlyOneWeightMappingGate(isSecondCheckRequiredGate);
            _exceptionFourteenGate = new ExceptionFourteenGate(efc, onlyOneWeightMappingGate);
        }
        
        public ModulusCheckOutcome Process(BankAccountDetails bankAccountDetails) 
            => _exceptionFourteenGate.Process(bankAccountDetails);
    }
}
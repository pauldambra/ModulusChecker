using ModulusChecking.Models;
using ModulusChecking.Parsers;

namespace ModulusChecking.Steps
{
    abstract class BaseStep : IStep
    {
        public abstract bool Process(BankAccountDetails bankAccountDetails, IModulusWeightTable modulusWeightTable);

        protected void HandleExceptionSeven(BankAccountDetails bankAccountDetails, IModulusWeightMapping weightMapping)
        {
            if (weightMapping.Exception != 7) return;
            if (bankAccountDetails.AccountNumber.IntegerAt(6) != 9) return;
            ZeroiseUtoB(weightMapping);
        }

        private static void ZeroiseUtoB(IModulusWeightMapping weightMapping)
        {
            for (var i = 0; i < 8; i++)
            {
                weightMapping.WeightValues[i] = 0;
            }
        }

        protected void HandleExceptionTen(BankAccountDetails bankAccountDetails, IModulusWeightMapping weightMapping)
        {
            if (weightMapping.Exception != 10) return;
            if (!bankAccountDetails.AccountNumber.ValidateExceptionTen) return;
            ZeroiseUtoB(weightMapping);
        }
    }
}
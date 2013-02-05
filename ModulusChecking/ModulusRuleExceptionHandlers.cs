using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModulusChecking.Models;

namespace ModulusChecking
{
    static class ModulusRuleExceptionHandlers
    {

        public static void HandleExceptionSeven(BankAccountDetails bankAccountDetails, IModulusWeightMapping weightMapping)
        {
            if (weightMapping.Exception != 7) return;
            if (bankAccountDetails.AccountNumber.IntegerAt(6) != 9) return;
            ZeroiseUtoB(weightMapping);
        }

        public static void HandleExceptionEight(BankAccountDetails bankAccountDetails, IModulusWeightMapping weightMapping)
        {
            if (weightMapping.Exception == 8)
            {
                bankAccountDetails.SortCode = new SortCode("090126");
            }
        }

        public static void HandleExceptionTen(BankAccountDetails bankAccountDetails, IModulusWeightMapping weightMapping)
        {
            if (weightMapping.Exception != 10) return;
            if (!bankAccountDetails.AccountNumber.ValidateExceptionTen) return;
            ZeroiseUtoB(weightMapping);
        }

        private static void ZeroiseUtoB(IModulusWeightMapping weightMapping)
        {
            for (var i = 0; i < 8; i++)
            {
                weightMapping.WeightValues[i] = 0;
            }
        }
    }
}

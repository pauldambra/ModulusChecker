using System;
using System.Linq;
using ModulusChecking.Models;
using ModulusChecking.ModulusChecks;

namespace ModulusChecking.Steps.Calculators
{
    internal class FirstDoubleAlternateCalculatorExceptionFive : DoubleAlternateCalculatorExceptionFive
    {
        protected override int GetWeightSumForStep(BankAccountDetails bankAccountDetails) 
            => new DoubleAlternateModulusCheck().GetModulusSum(
                bankAccountDetails,
                bankAccountDetails.WeightMappings.First());
    }

    internal class SecondDoubleAlternateCalculatorExceptionFive : DoubleAlternateCalculatorExceptionFive
    {
        protected override int GetWeightSumForStep(BankAccountDetails bankAccountDetails)
        {
            if (bankAccountDetails.WeightMappings.Count() != 2)
            {
                throw new MustHaveTwoWeightMappings(bankAccountDetails.WeightMappings.Count());
            }

            return new DoubleAlternateModulusCheck().GetModulusSum(
                bankAccountDetails,
                bankAccountDetails.WeightMappings.Second());
        }
    }

    internal class MustHaveTwoWeightMappings : Exception
    {
        public MustHaveTwoWeightMappings(int mappingsCount) 
            : base($"This calculator is for step two but the provided details only have {mappingsCount} mappings")
        {
        }
    }

    internal abstract class DoubleAlternateCalculatorExceptionFive : BaseModulusCalculator
    {
        protected abstract int GetWeightSumForStep(BankAccountDetails bankAccountDetails);

        /// <summary>
        /// /Perform the second double alternate check, and for the double alternate check with exception 5 the 
        /// checkdigit is h from the original account number, except:
        /// � After dividing the result by 10;
        /// - if the remainder=0 and h=0 the account number is valid
        /// - for all other remainders, take the remainder away from 10. If the number you get is the same as h 
        /// then the account number is valid.
        /// </summary>
        /// <param name="bankAccountDetails"></param>
        /// <returns></returns>
        public override bool Process(BankAccountDetails bankAccountDetails)
        {
            var remainder = GetWeightSumForStep(bankAccountDetails) % Modulus;
            switch (remainder)
            {
                case 0:
                    return bankAccountDetails.AccountNumber.IntegerAt(7) == 0;
                default:
                    return (Modulus - remainder) == bankAccountDetails.AccountNumber.IntegerAt(7);
            }
        }
    }
}
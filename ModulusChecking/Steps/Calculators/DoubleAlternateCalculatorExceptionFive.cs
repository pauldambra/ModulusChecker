using System.Linq;
using ModulusChecking.Models;
using ModulusChecking.ModulusChecks;
using ModulusChecking.Parsers;

namespace ModulusChecking.Steps.Calculators
{
    class DoubleAlternateCalculatorExceptionFive : BaseModulusCalculator
    {
        private readonly Step _step;

        public DoubleAlternateCalculatorExceptionFive(Step step)
        {
            _step = step;
        }

        /// <summary>
        /// /Perform the second double alternate check, and for the double alternate check with exception 5 the 
        /// checkdigit is h from the original account number, except:
        /// • After dividing the result by 10;
        /// - if the remainder=0 and h=0 the account number is valid
        /// - for all other remainders, take the remainder away from 10. If the number you get is the same as h 
        /// then the account number is valid.
        /// </summary>
        /// <param name="bankAccountDetails"></param>
        /// <param name="modulusWeightTable"> </param>
        /// <param name="ModulusWeightTable"></param>
        /// <returns></returns>
        public override bool Process(BankAccountDetails bankAccountDetails, IModulusWeightTable modulusWeightTable)
        {
            var modulusWeightMapping = modulusWeightTable.GetRuleMappings(bankAccountDetails.SortCode).ElementAt((int)_step);
            var weightingSum = new DoubleAlternateModulusCheck().GetModulusSum(bankAccountDetails,
                                                                               modulusWeightMapping.WeightValues);
            var remainder = weightingSum % Modulus;
            switch (remainder)
            {
                case 0 :
                    return bankAccountDetails.AccountNumber.IntegerAt(7) == 0;
                default :
                    return (Modulus - remainder) == bankAccountDetails.AccountNumber.IntegerAt(7);
            }
        }
    }
}
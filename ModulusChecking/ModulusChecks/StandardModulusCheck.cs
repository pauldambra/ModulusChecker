using System;
using System.Globalization;
using ModulusChecking.Models;

namespace ModulusChecking.ModulusChecks
{
    internal class StandardModulusCheck
    {
        public int GetModulusSum(BankAccountDetails bankAccountDetails, IModulusWeightMapping weightMapping)
        {
            var combinedValue = bankAccountDetails.ToCombinedString();
            if (combinedValue.Length != 14)
            {
                throw new Exception(
                    String.Format("Combined SortCode and Account Number should be 14 characters long not {0}: {1}",
                                  combinedValue.Length, combinedValue));
            }
            var sum = 0;
            for (var i = 0; i < 14; i++)
            {
                sum += (Int16.Parse(combinedValue[i].ToString(CultureInfo.InvariantCulture)) * weightMapping.WeightValues[i]);
            }
            return sum;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Globalization;
using ModulusChecking.Models;

namespace ModulusChecking.ModulusChecks
{
    class StandardModulusCheck : IModulusCheck
    {
        public int GetModulusSum(BankAccountDetails bankAccountDetails, IList<int> weightValues, int exception = -1)
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
                sum += (Int16.Parse(combinedValue[i].ToString(CultureInfo.InvariantCulture)) * weightValues[i]);
            }
            return sum;
        }
    }
}
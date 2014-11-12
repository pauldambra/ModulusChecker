using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using ModulusChecking.Models;

namespace ModulusChecking.ModulusChecks
{
    class DoubleAlternateModulusCheck
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
                var multiplicationResult = (Int16.Parse(combinedValue[i].ToString(CultureInfo.InvariantCulture)) * weightMapping.WeightValues[i]);
                sum += GetIntArray(multiplicationResult).Sum();
            }
            if (weightMapping.Exception == 1)
            {
                sum += 27;
            }
            return sum;
        }

        /// <summary>
        /// The DoubleAlternate rule adds together the individual digits of the numbers that result from 
        /// multiplication of sortcode, account number and modulus weighting value. This method takes an 
        /// integer and returns an array of its individual digits
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        private static IEnumerable<int> GetIntArray(int num)
        {
            var listOfInts = new List<int>();
            while (num > 0)
            {
                listOfInts.Add(num % 10);
                num = num / 10;
            }
            listOfInts.Reverse();
            return listOfInts.ToArray();
        }
    }
}
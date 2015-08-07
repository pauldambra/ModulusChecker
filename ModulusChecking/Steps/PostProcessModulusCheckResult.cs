using System.Linq;
using ModulusChecking.Models;

namespace ModulusChecking.Steps
{
    internal class PostProcessModulusCheckResult
    {
        public bool Process(BankAccountDetails bankAccountDetails)
        {
            var firstMapping = bankAccountDetails.WeightMappings.First();
            var secondMapping = bankAccountDetails.WeightMappings.Second();
            if (firstMapping.Exception == 5)
            {
                return bankAccountDetails.FirstResult && bankAccountDetails.SecondResult;
            }

            if ((firstMapping.Exception == 10 && secondMapping.Exception == 11)
                || (firstMapping.Exception == 12 && secondMapping.Exception == 13))
            {
                return bankAccountDetails.SecondResult || bankAccountDetails.FirstResult;
            }

            return bankAccountDetails.SecondResult;
        }
    }
}
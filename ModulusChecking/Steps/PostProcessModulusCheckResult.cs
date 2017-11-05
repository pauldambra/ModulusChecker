using System.Linq;
using ModulusChecking.Models;

namespace ModulusChecking.Steps
{
    internal class PostProcessModulusCheckResult : IProcessAStep
    {
        public ModulusCheckOutcome Process(BankAccountDetails bankAccountDetails)
        {
            var firstMapping = bankAccountDetails.WeightMappings.First();
            var secondMapping = bankAccountDetails.WeightMappings.Second();
            
            if (firstMapping.Exception == 5)
            {
                var result = bankAccountDetails.FirstResult && bankAccountDetails.SecondResult;
                return new ModulusCheckOutcome("exception 5 - so first and second check must pass", result);
            }

            if (firstMapping.Exception == 10 && secondMapping.Exception == 11)
            {
                var result = bankAccountDetails.SecondResult || bankAccountDetails.FirstResult;
                return new ModulusCheckOutcome("exception 10 and 11 - so second or first check must pass", result);
            }
            
            if (firstMapping.Exception == 12 && secondMapping.Exception == 13)
            {
                var result = bankAccountDetails.SecondResult || bankAccountDetails.FirstResult;
                return new ModulusCheckOutcome("exception 12 and 13 - so second or first check must pass", result);;
            }

            return new ModulusCheckOutcome("no exceptions affect result - using second check result", bankAccountDetails.SecondResult);
        }
    }
}
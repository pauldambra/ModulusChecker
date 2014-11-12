using ModulusChecking.Models;

namespace ModulusChecking.ModulusChecks
{
    interface IModulusCheck
    {
        int GetModulusSum(BankAccountDetails bankAccountDetails, IModulusWeightMapping weightMapping);
    }
}
using System.Collections.Generic;
using ModulusChecking.Models;

namespace ModulusChecking.ModulusChecks
{
    public interface IModulusCheck
    {
        int GetModulusSum(BankAccountDetails bankAccountDetails, IList<int> weightValues, int exception = -1);
    }
}
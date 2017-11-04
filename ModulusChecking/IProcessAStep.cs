using ModulusChecking.Models;

namespace ModulusChecking
{
    internal interface IProcessAStep
    {
        ModulusCheckOutcome Process(BankAccountDetails bankAccountDetails);
    }
}
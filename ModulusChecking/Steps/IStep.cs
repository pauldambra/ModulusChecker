using ModulusChecking.Models;
using ModulusChecking.Parsers;

namespace ModulusChecking.Steps
{
    interface IStep
    {
        bool Process(BankAccountDetails bankAccountDetails, IModulusWeightTable modulusWeightTable);
    }
}
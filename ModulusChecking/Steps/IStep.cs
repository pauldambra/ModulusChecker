using ModulusChecking.Models;
using ModulusChecking.Parsers;

namespace ModulusChecking.Steps
{
    public interface IStep
    {
        bool Process(BankAccountDetails bankAccountDetails, ModulusWeights modulusWeights);
    }
}
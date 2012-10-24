using ModulusChecking.Models;
using ModulusChecking.Parsers;

namespace ModulusChecking.Steps
{
    public abstract class BaseStep : IStep
    {
        public abstract bool Process(BankAccountDetails bankAccountDetails, ModulusWeights modulusWeights);
    }
}
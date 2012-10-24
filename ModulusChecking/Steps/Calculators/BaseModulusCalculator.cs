using ModulusChecking.Models;
using ModulusChecking.Parsers;

namespace ModulusChecking.Steps.Calculators
{
    public abstract class BaseModulusCalculator : IStep
    {
        public enum Step
        {
            First,
            Second
        }

        protected int Modulus = 10;
        public abstract bool Process(BankAccountDetails bankAccountDetails,
                                     ModulusWeights modulusWeights);
    }
}

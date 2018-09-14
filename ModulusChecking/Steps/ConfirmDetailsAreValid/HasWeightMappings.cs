using ModulusChecking.Loaders;
using ModulusChecking.Models;
using ModulusChecking.Properties;

namespace ModulusChecking.Steps.ConfirmDetailsAreValid
{
    
    /// <summary>
    /// The first step is to test if the given sort code can be found in the Modulus Weight Mappings.
    /// If not it is presumed to be valid
    /// </summary>
    internal class HasWeightMappings : IProcessAStep
    {
        private readonly IProcessAStep _nextStep;

        public HasWeightMappings(SortCodeSubstitution sortCodeSubstitution) { _nextStep = new IsUncheckableForeignAccount(sortCodeSubstitution); }

        public HasWeightMappings(IProcessAStep nextStep)
        { _nextStep = nextStep; }

        /// <summary>
        /// If there are no SortCode Modulus Weight Mappings available then the BankAccountDetails validate as true.
        /// Otherwise move onto the first modulus calculation step
        /// </summary>
        public ModulusCheckOutcome Process(BankAccountDetails bankAccountDetails) => 
            bankAccountDetails.IsValidForModulusCheck() 
                ? _nextStep.Process(bankAccountDetails) 
                : new ModulusCheckOutcome("Cannot invalidate these account details as there are no weight mappings for this sort code");
    }
}
namespace ModulusChecking
{
    public struct ModulusCheckOutcome
    {
        public string Explanation { get; }
        public bool Result { get; }
        
        /// <summary>
        /// If the provided details cannot be checked for some reason 
        /// then the result is that the details have not been invalidated
        /// 
        /// Therefore a check outcome defaults to true
        /// </summary>
        public ModulusCheckOutcome(string explanation, bool result = true)
        {
            Explanation = explanation;
            Result = result;
        }

        public static implicit operator bool(ModulusCheckOutcome checkOutcome)
        {
            return checkOutcome.Result;
        }
    }
}
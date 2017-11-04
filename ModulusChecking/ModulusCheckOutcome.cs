namespace ModulusChecking
{
    public struct ModulusCheckOutcome
    {
        /// <summary>
        /// If the provided details cannot be checked for some reason 
        /// then the result is that the details have not been invalidated
        /// 
        /// Therefore a check outcome defaults to true
        /// </summary>
        public ModulusCheckOutcome(bool result = true) 
        {
            Result = result;
        }

        public static implicit operator bool(ModulusCheckOutcome checkOutcome)
        {
            return checkOutcome.Result;
        }


        public bool Result { get; }
    }
}
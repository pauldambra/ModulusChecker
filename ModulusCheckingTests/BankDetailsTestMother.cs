using ModulusChecking.Models;

namespace ModulusCheckingTests
{
    internal static class BankDetailsTestMother
    {
        internal static BankAccountDetails BankDetailsWithException(int exception)
        {
            return new BankAccountDetails("000000", "00000000")
                         {
                             WeightMappings = new[]
                             {
                                 WeightMappingWithException(exception)
                             }
                         };
        }

        internal static ModulusWeightMapping WeightMappingWithException(int exception, ModulusAlgorithm algorithm = ModulusAlgorithm.DblAl)
        {
            return new ModulusWeightMapping(
                new SortCode("000000"),
                new SortCode("000000"),
                algorithm,
                new[] {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,},
                exception
            );
        }

        internal static ModulusWeightMapping AnyModulusWeightMapping()
            => WeightMappingWithException(0);

        public static BankAccountDetails BankDetailsWithAlgorithm(ModulusAlgorithm algorithm)
        {
            var weightMappingWithException = WeightMappingWithException(0, algorithm);
            
            return new BankAccountDetails("000000", "00000000")
            {
                WeightMappings = new[]
                {
                    weightMappingWithException,
                    weightMappingWithException,
                }
            };
        }
    }
}
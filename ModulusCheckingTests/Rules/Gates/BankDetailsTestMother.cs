using ModulusChecking.Models;

namespace ModulusCheckingTests.Rules.Gates
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

        internal static ModulusWeightMapping WeightMappingWithException(int exception)
        {
            return new ModulusWeightMapping(
                new SortCode("000000"),
                new SortCode("000000"),
                ModulusAlgorithm.DblAl,
                new[] {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,},
                exception
            );
        }

        internal static ModulusWeightMapping AnyModulusWeightMapping()
            => WeightMappingWithException(0);
    }
}
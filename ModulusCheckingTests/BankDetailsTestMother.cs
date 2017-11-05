using System.Collections.Generic;
using ModulusChecking.Models;

namespace ModulusCheckingTests
{
    internal class BankDetailsTestMother
    {
        private readonly string _sortcode;
        private readonly string _accountNumber;
        private readonly IList<ModulusWeightMapping> _weightMappings = new List<ModulusWeightMapping>(2);
        private bool _firstCheck;
        private bool _secondCheck;

        public BankDetailsTestMother()
        {
            _sortcode = "000000";
            _accountNumber = "00000000";
        }
        
        public BankDetailsTestMother(string sortcode, string accountNumber)
        {
            _sortcode = sortcode;
            _accountNumber = accountNumber;
        }

        public BankDetailsTestMother WithFirstWeightMapping(ModulusWeightMapping modulusWeightMapping)
        {
            _weightMappings.Insert(0, modulusWeightMapping);
            return this;
        }
        
        public BankDetailsTestMother WithSecondWeightMapping(ModulusWeightMapping modulusWeightMapping)
        {
            _weightMappings.Insert(1, modulusWeightMapping);
            return this;
        }

        public BankDetailsTestMother WithFirstCheckResult(bool checkresult)
        {
            _firstCheck = checkresult;
            return this;
        }
        
        public BankDetailsTestMother WithSecondCheckResult(bool checkresult)
        {
            _secondCheck = checkresult;
            return this;
        }
        
        public BankAccountDetails Build() => 
            new BankAccountDetails(_sortcode, _accountNumber)
            {
                WeightMappings = _weightMappings,
                FirstResult = _firstCheck,
                SecondResult = _secondCheck
            };

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
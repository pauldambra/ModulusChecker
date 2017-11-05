using System;
using System.Collections.Generic;
using System.Linq;
using ModulusChecking.Models;
using NUnit.Framework;

namespace ModulusCheckingTests.Models
{
    public class BankAccountDetailsTests
    {
        private const int ModulusExceptionFlag = -1;

        [Test]
        [TestCase("1234567890", "080000", "080000", "12345678")]
        [TestCase("1234567890", "830000", "830000", "34567890")]
        [TestCase("12-34567890", "839000", "839000", "12345678")]
        [TestCase("12-34567890", "600000", "600000", "34567890")]
        [TestCase("123456789", "123456",  "123451", "23456789")]
        [TestCase("1234567", "123456",  "123456", "01234567")]
        [TestCase("123456", "123456",  "123456", "00123456")]
        public void CanInstantiateOddAccounts(string accountNumber, string sortCode, string expectedSortCode, string expectedAccountNumber)
        {
            var account = new BankAccountDetails(sortCode, accountNumber);
            Assert.AreEqual(expectedAccountNumber, account.AccountNumber.ToString());
            Assert.AreEqual(expectedSortCode,account.SortCode.ToString());
        }

        [Test]
        public void CannotInitialiseNonDigitAccountNumber()
        {
            Assert.Throws<ArgumentException>(() => new BankAccountDetails("000000", "something"));
        }

        [Test]
        public void CannotIntialiseTooLongAccountNumber()
        {
            Assert.Throws<ArgumentException>(() => new BankAccountDetails("000000", "123456123456123456"));
        }

        [Test]
        public void CannotIntialiseTooShortAccountNumber()
        {
            Assert.Throws<ArgumentException>(() => new BankAccountDetails("000000", "1"));
        }
        
        [Test]
        public void CanInitialiseSortCode()
        {
            Assert.AreEqual("123456",new BankAccountDetails("123456","12345678").SortCode.ToString());
        }
        
        [Test]
        public void CannotInitialiseTooShortSortCode()
        {
            Assert.Throws<ArgumentException>(() => new BankAccountDetails("234","12345678"));
        }
        
        [Test]
        public void CannotInitialiseTooLongSortCode()
        {
            Assert.Throws<ArgumentException>(() => new BankAccountDetails("1234567","12345678"));
        }
        
        [Test]
        public void CannotInitialiseLetterAsASortCode()
        {
            Assert.Throws<ArgumentException>(() => new BankAccountDetails("a","12345678"));
        }

        [Test]
        [TestCase("123456","09345678",false)]
        [TestCase("123456", "09345698", true)]
        [TestCase("123456", "99345678", false)]
        [TestCase("123456", "99345698", true)]
        [TestCase("123456", "19345698", false)]
        public void CanValidateForExceptionTen(string sc, string an, bool expected)
        {
            Assert.AreEqual(expected,new BankAccountDetails(sc,an).AccountNumber.ExceptionTenShouldZeroiseWeights);
        }

        [Test]
        [TestCase("123456", "09345678", false)]
        [TestCase("123456", "49345698", false)]
        [TestCase("123456", "59345678", false)]
        [TestCase("123456", "69345698", false)]
        [TestCase("123456", "79345698", false)]
        [TestCase("123456", "89345698", false)]
        [TestCase("123456", "09345677", false)]
        [TestCase("123456", "49345699", true)]
        [TestCase("123456", "59345677", true)]
        [TestCase("123456", "69345699", true)]
        [TestCase("123456", "79345688", true)]
        [TestCase("123456", "89345688", true)]
        public void CanValidateForExceptionSix(string sc, string an, bool expected)
        {
            Assert.AreEqual(expected, new BankAccountDetails(sc, an).AccountNumber.IsForeignCurrencyAccount);
        }

        [Test]
        [TestCase("123456", "09345678", false)]
        [TestCase("123456", "09345690", true)]
        [TestCase("123456", "99345671", true)]
        [TestCase("123456", "99345699", true)]
        public void CanValidateAsCouttsAccountNumber(string sc, string an, bool expected)
        {
            Assert.AreEqual(expected, new BankAccountDetails(sc, an).AccountNumber.IsValidCouttsNumber);
        }

        [Test]
        [TestCase("123455", "01234567", 1,1)]
        [TestCase("123455", "01234567", 0, 0)]
        [TestCase("123455", "01234567", 2, 2)]
        public void CanSetWeightMappings(string sc, string an, int desiredMappings, int expectedCount) {
            var target = new BankAccountDetails(sc, an) {WeightMappings = BuildMappingList(sc, desiredMappings, ModulusExceptionFlag)};
            Assert.AreEqual(expectedCount, target.WeightMappings.Count());
        }

        [Test]
        public void CannotSetIncorrectWeightMappings()
        {
            Assert.Throws<InvalidOperationException>( () => new BankAccountDetails("123455", "01234567")
            {
                WeightMappings = BuildMappingList("123455", 3, ModulusExceptionFlag)
            });
        }

        [Test]
        [TestCase("123455", "01234567", 0, 0, false)]
        [TestCase("123455", "01234567", 1, 0, false)]
        [TestCase("123455", "01234567", 1, 6, false)]
        [TestCase("123455", "01234567", 2, 6, false)]
        [TestCase("123455", "41234566", 1, 6, true)]
        [TestCase("123455", "51234577", 2, 6, true)]
        public void CanCheckForForeignAccountException(string sc, string an, int desiredMappings, int exception, bool expected)
        {
            var target = new BankAccountDetails(sc, an) { WeightMappings = BuildMappingList(sc, desiredMappings, exception) };
            Assert.AreEqual(expected, target.IsUncheckableForeignAccount());
        }

        [Test]
        [TestCase("123455", "01234567", 1, 0, false)]
        [TestCase("123455", "01234567", 1, 6, false)]
        [TestCase("123455", "01234567", 2, 2, true)]
        [TestCase("123455", "41234566", 1, 9, true)]
        [TestCase("123455", "51234577", 2, 10, true)]
        [TestCase("123455", "51234577", 2, 11, true)]
        [TestCase("123455", "51234577", 2, 12, true)]
        [TestCase("123455", "51234577", 2, 13, true)]
        [TestCase("123455", "51234577", 2, 14, true)]
        public void CanIdentifyIfSecondCheckIsRequired(string sc, string an, int desiredMappings, int exception, bool expected)
        {
            var target = new BankAccountDetails(sc, an) { WeightMappings = BuildMappingList(sc, desiredMappings, exception) };
            Assert.AreEqual(expected, target.IsSecondCheckRequired());
        }

        [Test]
        public void CannotIdentifyIfSecondCheckRequiredWithZeroMappings()
        {
            Assert.Throws<InvalidOperationException>(() => new BankAccountDetails("123455", "01234567")
            {
                WeightMappings = BuildMappingList("123455", 0, 0)
            }.IsSecondCheckRequired());
        }


        [Test]
        [TestCase("123455", "01234597", 1, new[] {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13},
                                           new[] {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13})]
        [TestCase("123455", "01234587", 7, new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 },
                                           new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 })]
        [TestCase("123455", "01234597", 7, new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 },
                                           new[] { 0, 0, 0, 0, 0, 0, 0, 0, 9, 10, 11, 12, 13, 14, 15 })]
        [TestCase("123455", "01234597", 10, new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 },
                                           new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 })]
        [TestCase("123455", "91234597", 10, new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 },
                                           new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 })]
        [TestCase("123455", "19234597", 10, new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 },
                                           new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 })]
        [TestCase("123455", "12345698", 10, new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 },
                                           new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 })]
        [TestCase("123455", "09345698", 10, new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 },
                                           new[] { 0, 0, 0, 0, 0, 0, 0, 0, 9, 10, 11, 12, 13, 14, 15 })]
        [TestCase("123455", "09345698", 1, new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 },
                                           new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 })]
        public void CanPreProcessForExceptionSevenTenAndThree(string sortCode, string accountNumber, int exception,
                                                   int[] initialWeightMapping, int[] expectedWeightMapping)
        {
            var target = new BankAccountDetails(sortCode, accountNumber)
                             {
                                 WeightMappings = new []
                                 {
                                     MakeModulusWeightMapping(sortCode, initialWeightMapping, exception)
                                 }
                             };
            Assert.AreEqual(expectedWeightMapping,target.WeightMappings.First().WeightValues);
        }

        [Test]
        [TestCase("123455", "00000000", 1, new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 }, false)]
        [TestCase("123455", "00300000", 3, new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 }, false)]
        [TestCase("123455", "00600000", 3, new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 }, true)]
        [TestCase("123455", "00900000", 3, new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 }, true)]
        public void IsExceptionThreeAndCanSkipSecondCheck(string sortCode, string accountNumber, int exception,
                                                   int[] initialWeightMapping, bool expected)
        {
            var target = new BankAccountDetails(sortCode, accountNumber)
            {
                WeightMappings = new []
                {
                    MakeModulusWeightMapping(sortCode, initialWeightMapping, exception),
                    MakeModulusWeightMapping(sortCode, initialWeightMapping, exception)
                }
            };
            Assert.AreEqual(expected, target.IsExceptionThreeAndCanSkipSecondCheck());
        }

        [Test]
        [TestCase("123456", "01234567", 1, "123456")]
        [TestCase("123456", "01234567", 8, "090126")]
        public void CanPreProcessForExceptionEight(string sc, string an,int exception,string expectedSortCode)
        {
            var target = new BankAccountDetails(sc, an) {WeightMappings = BuildMappingList(sc, 1, exception)};
            Assert.AreEqual(expectedSortCode,target.SortCode.ToString());
        }

        [Test]
        [TestCase("123456", "01234567", 1, false)]
        [TestCase("123456", "01234567", 14, true)]
        public void CanVerifyIfRequiresCouttsCheck(string sc, string an, int exception, bool expected)
        {
            var target = new BankAccountDetails(sc, an) {WeightMappings = BuildMappingList(sc, 1, exception)};
            Assert.AreEqual(expected,target.RequiresCouttsAccountCheck());
        }

        [Test]
        [TestCase("123455", "00000000", new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 }, new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 })]
        public void CanGetUnchangedExceptionTwoWeightValues(string sc, string an, int[] initialWeights, int[] expectedWeights)
        {
            var target = new BankAccountDetails(sc, an) { WeightMappings = BuildMappingList(sc, 1, 1) };
            Assert.AreEqual(expectedWeights,target.GetExceptionTwoAlternativeWeights(initialWeights));
        }

        [Test]
        public void CanGetFirstAlternativeExceptionTwoValue()
        {
            var target = new BankAccountDetails("123456", "10000000") { WeightMappings = BuildMappingList("123456", 1, 1) };
            Assert.AreEqual(BankAccountDetails.AisNotZeroAndGisNotNineWeights, target.GetExceptionTwoAlternativeWeights(new int[1]));
        }

        [Test]
        public void CanGetSecondAlternativeExceptionTwoValue()
        {
            var target = new BankAccountDetails("123456", "10000090")
                             {
                                 WeightMappings = BuildMappingList("123456", 1, 1)
                             };
            Assert.AreEqual(BankAccountDetails.AisNotZeroAndGisNineWeights, target.GetExceptionTwoAlternativeWeights(new int[1]));
        }

        private static IEnumerable<ModulusWeightMapping> BuildMappingList(string sc, int desiredMappings, int exception)
        {
            var items = new List<ModulusWeightMapping>();
            for (var i = 0; i < desiredMappings; i++)
            {
                exception = i == 0
                                ? exception == ModulusExceptionFlag ? i : exception
                                : i;
                items.Add(
                    ModulusWeightMapping.From(
                        string.Format(
                            "{0} 089999 MOD10    0    0    0    0    0    0    7    1    3    7    1    3    7    7    {1}",
                            sc, exception)));
            }
            return items;
        }

        private static ModulusWeightMapping MakeModulusWeightMapping(string sc, int[] initialWeightMappings, int exception)
        {
            return new ModulusWeightMapping(
                new SortCode(sc),
                new SortCode("999999"),
                ModulusAlgorithm.Mod10,
                initialWeightMappings,
                exception
            );
        }
    }
}

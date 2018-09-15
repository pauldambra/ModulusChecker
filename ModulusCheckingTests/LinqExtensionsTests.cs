using System;
using System.Collections.Generic;
using NUnit.Framework;
using ModulusChecking;

namespace ModulusCheckingTests
{
    internal class LinqExtensionsTests
    {
        [Test]
        public void CanCallForSecondItemInEnumerable()
        {
            var target = new List<int>{1, 2, 3, 4, 5};
            Assert.AreEqual(2,target.Second());
        }

        [Test]
        public void ThrowsExceptionAsExpectedWithFewerThanTwoItemList()
        {
            Assert.Throws<LinqExtensions.ListNotLongEnough>(() => new List<int>().Second());
            Assert.Throws<LinqExtensions.ListNotLongEnough>(() => new List<int> {1}.Second());
        }
    }
}

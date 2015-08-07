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
        public void ThrowsExceptionAsExpectedWithSingleItemList()
        {
            var target = new List<int> {1};
            try
            {
                target.Second();
            }
            catch (ArgumentException)
            {
                //this is expected
                return;
            }
            Assert.Fail("should have thrown by now");
        }
    }
}

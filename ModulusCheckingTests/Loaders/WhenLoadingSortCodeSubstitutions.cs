using System;
using ModulusChecking.Loaders;
using NUnit.Framework;

namespace ModulusCheckingTests.Loaders
{
    public class WhenLoadingSortCodeSubstitutions
    {
        [Test]
        public void MustReceiveAString()
        {
            Assert.Throws<ProvidedValacodosContentIsNull>(
                () => new SortCodeSubstitution(null));
        }

        [Test]
        public void MustHaveSomeContents()
        {
            Assert.Throws<ProvidedValacodosContentIsEmpty>(
                () => new SortCodeSubstitution(" "));
        }

        [Test]
        public void MustGenerateAtLeastOneSubstitution()
        {
            Assert.Throws<ProvidedValacodosContentIsProbablyInvalid>(
                () => new SortCodeSubstitution("a\nb"));
        }
    }
}
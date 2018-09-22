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
            Assert.Throws<SortCodeSubstitution.ProvidedSortCodeSubstitutionsAreNull>(
                () => new SortCodeSubstitution(null));
        }

        [Test]
        public void MustHaveSomeContents()
        {
            Assert.Throws<SortCodeSubstitution.ProvidedSortCodeSubstitutionsAreEmpty>(
                () => new SortCodeSubstitution(" "));
        }

        [Test]
        public void MustGenerateAtLeastOneSubstitution()
        {
            Assert.Throws<SortCodeSubstitution.ProvidedSortCodeSubstitutionsAreProbablyInvalid>(
                () => new SortCodeSubstitution("a\nb"));
        }
    }
}
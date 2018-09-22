using ModulusChecking.Loaders;
using NUnit.Framework;

namespace ModulusCheckingTests.Loaders
{
    public class WhenLoadingModulusWeights
    {
        [Test]
        public void MustReceiveAString()
        {
            Assert.Throws<ProvidedValacodosContentIsNull>(
                () => new ModulusWeightTable(null));
        }

        [Test]
        public void MustHaveSomeContents()
        {
            Assert.Throws<ProvidedValacodosContentIsEmpty>(
                () => new ModulusWeightTable(" "));
        }
    }
}
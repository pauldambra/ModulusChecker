using System.Linq;
using ModulusChecking.Loaders;
using ModulusChecking.Models;
using NUnit.Framework;

namespace ModulusCheckingTests.Loaders
{
    public class ModulusWeightTests
    {
        [Test]
        public void CanReadWeightFileResource()
        {
            var weightFile = ModulusChecking.Properties.Resources.valacdos;
            Assert.NotNull(weightFile);
            Assert.IsInstanceOf(typeof(string),weightFile);
        }

        [Test]
        public void CanGetRuleMappings()
        {
            var modulusWeight = ModulusWeightTable.GetInstance;
            Assert.NotNull(modulusWeight.RuleMappings);
            const int numberOfLinesInTheValacdosFile = 1060;
            Assert.AreEqual(numberOfLinesInTheValacdosFile, modulusWeight.RuleMappings.Count());
            Assert.IsInstanceOf<ModulusWeightMapping>(modulusWeight.RuleMappings.ElementAt(0));
        }

        [Test]
        public void ThereAreNoMod10MappingsWithExceptionFive()
        {
            var modulusWeight = ModulusWeightTable.GetInstance;
            Assert.IsFalse(modulusWeight.RuleMappings.Any(rm=>rm.Exception==5 && rm.Algorithm==ModulusAlgorithm.Mod10));
        }

        [Test]
        public void AllExceptionNineRowsAreModEleven()
        {
            var modulusWeight = ModulusWeightTable.GetInstance;
            var exceptionNineRows = modulusWeight.RuleMappings.Where(rm => rm.Exception == 9).ToList();
            Assert.IsTrue(exceptionNineRows.All(r => r.Algorithm == ModulusAlgorithm.Mod11));
        }
    }
}

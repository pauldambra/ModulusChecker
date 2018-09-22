using System.Linq;
using ModulusChecking.Loaders;
using ModulusChecking.Models;
using ModulusChecking.Properties;
using NUnit.Framework;

namespace ModulusCheckingTests.Loaders
{
    public class ModulusWeightTests
    {
        [Test]
        public void CanReadWeightFileResource()
        {
            var weightFile = Resources.valacdos;
            Assert.NotNull(weightFile);
            Assert.IsInstanceOf(typeof(string),weightFile);
        }

        [Test]
        public void CanGetRuleMappings()
        {
            var modulusWeight = new ModulusWeightTable(Resources.valacdos);
            Assert.NotNull(modulusWeight.RuleMappings);
            const int numberOfLinesInTheValacdosFile = 1074;
            Assert.AreEqual(numberOfLinesInTheValacdosFile, modulusWeight.RuleMappings.Count());
            Assert.IsInstanceOf<ModulusWeightMapping>(modulusWeight.RuleMappings.ElementAt(0));
        }

        [Test]
        public void ThereAreNoMod10MappingsWithExceptionFive()
        {
            var modulusWeight = new ModulusWeightTable(Resources.valacdos);
            Assert.IsFalse(modulusWeight.RuleMappings.Any(rm=>rm.Exception==5 && rm.Algorithm==ModulusAlgorithm.Mod10));
        }

        [Test]
        public void AllExceptionNineRowsAreModEleven()
        {
            var modulusWeight = new ModulusWeightTable(Resources.valacdos);
            var exceptionNineRows = modulusWeight.RuleMappings.Where(rm => rm.Exception == 9).ToList();
            Assert.IsTrue(exceptionNineRows.All(r => r.Algorithm == ModulusAlgorithm.Mod11));
        }
    }
}

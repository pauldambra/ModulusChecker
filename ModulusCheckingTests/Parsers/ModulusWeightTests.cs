using System.Linq;
using ModulusChecking.Models;
using ModulusChecking.Parsers;
using NUnit.Framework;

namespace ModulusCheckingTests.Parsers
{
    public class ModulusWeightTests
    {
        [Test]
        public void CanReadWeightFileResource()
        {
            var weightFile = ModulusChecking.Properties.Resources.valacdos; ;
            Assert.NotNull(weightFile);
            Assert.IsInstanceOf(typeof(string),weightFile);
        }

        [Test]
        public void CanLoadWeightFileRows()
        {
            var modulusWeight = new ModulusWeights(new ValacdosSource());
            Assert.AreEqual(984,modulusWeight.RuleMappings.Count());
        }   

        [Test]
        public void CanGetRuleMappings()
        {
            var modulusWeight = new ModulusWeights(new ValacdosSource());
            Assert.NotNull(modulusWeight.RuleMappings);
            Assert.AreEqual(modulusWeight.RuleMappings.Count, 984);
            Assert.IsInstanceOf<ModulusWeightMapping>(modulusWeight.RuleMappings.ElementAt(0));
        }

        [Test]
        public void ThereAreNoMod10MappingsWithExceptionFive()
        {
            var modulusWeight = new ModulusWeights(new ValacdosSource());
            Assert.IsFalse(modulusWeight.RuleMappings.Any(rm=>rm.Exception==5 && rm.Algorithm==ModulusWeightMapping.ModulusAlgorithm.Mod10));
        }
    }
}

using NUnit.Framework;

namespace ModulusCheckingTests.Rules.Gates
{
    /// <summary>
    ///          public bool IsSecondCheckRequired()
    ///        {
    ///            if (FirstResult)
    ///            {
    ///                return !(WeightMappings.Count() == 1 ||
    ///                       new List&lt;int&gt; {2, 9, 10, 11, 12, 13, 14}.Contains(WeightMappings.First().Exception));
    ///            }
    ///            return new List&lt;int&gt; {2, 9, 10, 11, 12, 13, 14}.Contains(WeightMappings.First().Exception);
    ///        }
    /// </summary>
    public class NoSecondCheckRequiredGateTests
    {
        [Test]
        public void METHOD()
        {
            Assert.Inconclusive("Argh!");
        }
    }
}
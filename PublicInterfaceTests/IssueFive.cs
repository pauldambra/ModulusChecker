using ModulusChecking;
using NUnit.Framework;

namespace PublicInterfaceTests
{
    /// <summary>
    /// See https://github.com/pauldambra/ModulusChecker/issues/5
    /// </summary>
    public class IssueFive
    {
        private const string Sortcode = "775024";
        private const string AccNumber = "26862368";

        [Test]
        public void ItCanRevalidateDetailsOnImmediateRepeat()
        {
            var checker = new ModulusChecker();

            Assert.IsTrue(checker.CheckBankAccount(Sortcode, AccNumber));
            Assert.IsTrue(checker.CheckBankAccount(Sortcode, AccNumber));
        }

        [Test]
        [TestCase("776203", "01193899")]
        [TestCase("089999", "66374958")]
        public void SeparatingCheckPassesInIsolation(string sc, string an)
        {
            var checker = new ModulusChecker();
            Assert.IsTrue(checker.CheckBankAccount(Sortcode, AccNumber));
        }

        [Test]
        [TestCase("776203", "01193899")]
        [TestCase("089999", "66374958")]
        public void ItCanRevalidateDetailsOnSeparatedRepeat(string sc, string an)
        {
            var checker = new ModulusChecker();

            Assert.IsTrue(checker.CheckBankAccount(Sortcode, AccNumber), string.Format("first check should have passed for {0} and {1}", Sortcode, AccNumber));
            Assert.IsTrue(checker.CheckBankAccount(sc, an), string.Format("separating check should have passed for {0} and {1}", sc, an));
            Assert.IsTrue(checker.CheckBankAccount(Sortcode, AccNumber), string.Format("second check should have passed for {0} and {1}", Sortcode, AccNumber));
        }
    }
}
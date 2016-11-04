using ModulusChecking;
using NUnit.Framework;

namespace PublicInterfaceTests
{
    /// <summary>
    /// See https://github.com/pauldambra/ModulusChecker/issues/5
    /// </summary>
    public class IssueFive
    {
        [Test]
        public void ItCanRevalidateDetailsOnImmediateRepeat()
        {
            var sortcode = "775024";
            var accNumber = "26862368";

            var checker = new ModulusChecker();

            Assert.IsTrue(checker.CheckBankAccount(sortcode, accNumber));
            //Assert.IsTrue(checker.CheckBankAccount("776203", "01193899"));
            Assert.IsTrue(checker.CheckBankAccount(sortcode, accNumber));
        }

        [Test]
        public void ItCanRevalidateDetailsOnSeparatedRepeat()
        {
            const string sortcode = "775024";
            const string accNumber = "26862368";

            var checker = new ModulusChecker();

            Assert.IsTrue(checker.CheckBankAccount(sortcode, accNumber));
            Assert.IsTrue(checker.CheckBankAccount("776203", "01193899"));
            Assert.IsTrue(checker.CheckBankAccount(sortcode, accNumber));
        }
    }
}
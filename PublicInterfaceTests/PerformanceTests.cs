using System;
using ModulusChecking;
using NUnit.Framework;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace PublicInterfaceTests
{
    public class PerformanceTests
    {
        [TestCase(2)]
        public void ItCanProcessALargeFileInUnder(int seconds)
        {
            var stopwatch = new Stopwatch();
            var modulusChecker = new ModulusChecker();

            var currentDirectory = TestContext.CurrentContext.TestDirectory;
            var fileName = "sa.txt";
            var fullPath = Path.Combine(currentDirectory, fileName);

            using (var sr = new StreamReader(fullPath))
            {
                stopwatch.Start();

                while (sr.Peek() >= 0)
                {
                    var segments = sr
                        .ReadLine()
                        .Split('\t');

                    modulusChecker.CheckBankAccount(segments.First(), segments.Last());
                }

                stopwatch.Stop();
            }
            
            Assert.IsTrue(stopwatch.Elapsed.Seconds <= seconds,
                $"Failed to process a large number of sortcodes and account numbers in under {seconds} seconds. Took {stopwatch.Elapsed.Seconds}");
        }
    }
}

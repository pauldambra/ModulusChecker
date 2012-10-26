using System.Diagnostics;
using System.Linq;
using ModulusChecking.Loaders;
using ModulusChecking.Models;

namespace ModulusChecking.Steps.Calculators
{
    class FirstStandardModulusElevenCalculator : FirstStandardModulusTenCalculator
    {
        public FirstStandardModulusElevenCalculator()
        {
            Modulus = 11;
        }
    }
}
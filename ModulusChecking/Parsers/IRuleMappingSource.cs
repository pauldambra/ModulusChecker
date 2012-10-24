using System.Collections.Generic;
using ModulusChecking.Models;

namespace ModulusChecking.Parsers
{
    public interface IRuleMappingSource
    {
        IEnumerable<ModulusWeightMapping> GetModulusWeightMappings();
    }
}
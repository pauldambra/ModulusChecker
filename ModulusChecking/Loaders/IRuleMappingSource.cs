using System.Collections.Generic;
using ModulusChecking.Models;

namespace ModulusChecking.Loaders
{
    public interface IRuleMappingSource
    {
        IEnumerable<IModulusWeightMapping> GetModulusWeightMappings();
    }
}
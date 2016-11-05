using System;
using System.Collections.Generic;
using System.Linq;
using ModulusChecking.Models;
using ModulusChecking.Properties;

namespace ModulusChecking.Loaders
{
    public class ValacdosSource : IRuleMappingSource
    {
        public IEnumerable<ModulusWeightMapping> GetModulusWeightMappings { get; private set; }

        public ValacdosSource()
        {
            GetModulusWeightMappings = Resources.valacdos
                .Split(new[] {"\r\n", "\n"}, StringSplitOptions.None)
                .Where(row => row.Length > 0)
                .Select(ModulusWeightMapping.From);
        }
    }
}
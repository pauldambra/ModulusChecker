using System;
using System.Collections.Generic;
using System.Linq;
using ModulusChecking.Models;

namespace ModulusChecking.Loaders
{
    public class ValacdosSource : IRuleMappingSource
    {
        public IEnumerable<ModulusWeightMapping> GetModulusWeightMappings { get; }

        public ValacdosSource(string valacdosFileContents)
        {
            GetModulusWeightMappings = valacdosFileContents
                .Split(new[] { "\r\n", "\n" }, StringSplitOptions.None)
                .Where(row => row.Length > 0)
                .Select(ModulusWeightMapping.From)
                .ToArray();
        }
    }
}
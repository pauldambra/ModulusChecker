using System;
using System.Collections.Generic;
using System.Linq;
using ModulusChecking.Models;
using ModulusChecking.Properties;

namespace ModulusChecking.Loaders
{
    public class ValacdosSource : IRuleMappingSource
    {
        private static readonly string[] Rows = Resources.valacdos.Split(new[] {Environment.NewLine}, StringSplitOptions.None);

        public IEnumerable<ModulusWeightMapping> GetModulusWeightMappings()
        {
            return Rows.Where(row => row.Length > 0)
                       .Select(ModulusWeightMapping.From);
        }
    }
}
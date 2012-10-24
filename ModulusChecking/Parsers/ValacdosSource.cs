using System;
using System.Collections.Generic;
using System.Linq;
using ModulusChecking.Models;
using ModulusChecking.Properties;

namespace ModulusChecking.Parsers
{
    public class ValacdosSource : IRuleMappingSource
    {
        public IEnumerable<IModulusWeightMapping> GetModulusWeightMappings()
        {
            var rows = Resources.valacdos.Split(new[] {Environment.NewLine}, StringSplitOptions.None);
            return rows.Where(row => row.Length > 0)
                .Select(row => new ModulusWeightMapping(row));
        } 
    }
}
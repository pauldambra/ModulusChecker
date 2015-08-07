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

        public IEnumerable<IModulusWeightMapping> GetModulusWeightMappings()
        {
            return (from row in Rows 
                    where row.Length > 0 
                    select new ModulusWeightMapping(row))
                    .Cast<IModulusWeightMapping>();
        }
    }
}
using System;
using System.Collections.Generic;
using ModulusChecking.Models;
using ModulusChecking.Properties;

namespace ModulusChecking.Loaders
{
    public class ValacdosSource : IRuleMappingSource
    {
        private static readonly string[] Rows = Resources.valacdos.Split(new[] {Environment.NewLine}, StringSplitOptions.None);

        public IEnumerable<IModulusWeightMapping> GetModulusWeightMappings()
        {
            foreach (var row in Rows)
            {
                if (row.Length > 0) yield return new ModulusWeightMapping(row);
            }
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using ModulusChecking.Models;

namespace ModulusChecking.Loaders
{
    public interface IModulusWeightTable
    {
        IEnumerable<ModulusWeightMapping> RuleMappings { get; }
        IEnumerable<ModulusWeightMapping> GetRuleMappings(SortCode sortCode);
    }

    internal class ModulusWeightTable : IModulusWeightTable
    { 
        private static readonly ModulusWeightTable Instance = new ModulusWeightTable();
        public static ModulusWeightTable GetInstance => Instance;

        public IEnumerable<ModulusWeightMapping> RuleMappings { get; }
        
        private ModulusWeightTable()
        {
            RuleMappings = new ValacdosSource().GetModulusWeightMappings;
        }

        public IEnumerable<ModulusWeightMapping> GetRuleMappings(SortCode sortCode)
        {
            return
                RuleMappings.Where(rm => sortCode.DoubleValue >= rm.SortCodeStart.DoubleValue 
                        && sortCode.DoubleValue <= rm.SortCodeEnd.DoubleValue);
        } 
    }
}
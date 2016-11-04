using System.Collections.Generic;
using System.Linq;
using ModulusChecking.Models;

namespace ModulusChecking.Loaders
{
    public interface IModulusWeightTable
    {
        List<ModulusWeightMapping> RuleMappings { get; }
        IEnumerable<ModulusWeightMapping> GetRuleMappings(SortCode sortCode);
    }

    internal class ModulusWeightTable : IModulusWeightTable
    { 
        private static readonly ModulusWeightTable Instance = new ModulusWeightTable();
        public static ModulusWeightTable GetInstance {get { return Instance; }}

        public List<ModulusWeightMapping> RuleMappings { get; private set; }
        
        private ModulusWeightTable()
        {
            RuleMappings = new ValacdosSource().GetModulusWeightMappings().ToList();
        }

        public IEnumerable<ModulusWeightMapping> GetRuleMappings(SortCode sortCode)
        {
            return
                RuleMappings.Where(rm => sortCode.DoubleValue >= rm.SortCodeStart.DoubleValue 
                        && sortCode.DoubleValue <= rm.SortCodeEnd.DoubleValue);
        } 
    }
}
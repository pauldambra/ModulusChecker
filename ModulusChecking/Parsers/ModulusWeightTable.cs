using System.Collections.Generic;
using System.Linq;
using ModulusChecking.Models;

namespace ModulusChecking.Parsers
{
    public interface IModulusWeightTable
    {
        List<IModulusWeightMapping> RuleMappings { get; }
        IEnumerable<IModulusWeightMapping> GetRuleMappings(SortCode sortCode);
    }

    class ModulusWeightTable : IModulusWeightTable
    { 
        public List<IModulusWeightMapping> RuleMappings { get; private set; } 
        
        public ModulusWeightTable(IRuleMappingSource source)
        {
            RuleMappings = source.GetModulusWeightMappings().ToList();
        }
           
        public IEnumerable<IModulusWeightMapping> GetRuleMappings(SortCode sortCode)
        {
            return
                RuleMappings.Where(rm => sortCode.DoubleValue >= rm.SortCodeStart.DoubleValue 
                        && sortCode.DoubleValue <= rm.SortCodeEnd.DoubleValue);
        } 
    }
}
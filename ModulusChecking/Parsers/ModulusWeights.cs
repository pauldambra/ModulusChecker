using System.Collections.Generic;
using System.Linq;
using ModulusChecking.Models;

namespace ModulusChecking.Parsers
{
    public class ModulusWeights
    { 
        public List<ModulusWeightMapping> RuleMappings { get; private set; } 
        
        public ModulusWeights(IRuleMappingSource source)
        {
            RuleMappings = source.GetModulusWeightMappings().ToList();
        }
           
        public IEnumerable<ModulusWeightMapping> GetRuleMappings(SortCode sc)
        {
            return
                RuleMappings.Where(rm => sc.DoubleValue >= rm.SortCodeStart.DoubleValue 
                        && sc.DoubleValue <= rm.SortCodeEnd.DoubleValue);
        } 
    }
}
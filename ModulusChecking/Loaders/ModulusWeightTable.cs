using System.Collections.Generic;
using System.Linq;
using ModulusChecking.Models;

namespace ModulusChecking.Loaders
{
    public interface IModulusWeightTable
    {
        List<IModulusWeightMapping> RuleMappings { get; }
        IEnumerable<IModulusWeightMapping> GetRuleMappings(SortCode sortCode);
    }

    internal class ModulusWeightTable : IModulusWeightTable
    { 
        private static readonly ModulusWeightTable Instance = new ModulusWeightTable();
        public static ModulusWeightTable GetInstance {get { return Instance; }}

        public List<IModulusWeightMapping> RuleMappings { get; private set; } 
        
        private ModulusWeightTable()
        {
            RuleMappings = new ValacdosSource().GetModulusWeightMappings().ToList();
        }

        public IEnumerable<IModulusWeightMapping> GetRuleMappings(SortCode sortCode)
        {
            return
                RuleMappings.Where(rm => sortCode.DoubleValue >= rm.SortCodeStart.DoubleValue 
                        && sortCode.DoubleValue <= rm.SortCodeEnd.DoubleValue);
        } 
    }
}
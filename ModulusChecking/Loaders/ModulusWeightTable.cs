using System.Collections.Generic;
using System.Linq;
using ModulusChecking.Models;
using ModulusChecking.Properties;

namespace ModulusChecking.Loaders
{
    public interface IModulusWeightTable
    {
        IEnumerable<ModulusWeightMapping> RuleMappings { get; }
        IEnumerable<ModulusWeightMapping> GetRuleMappings(SortCode sortCode);
    }

    internal class ModulusWeightTable : IModulusWeightTable
    { 
        public IEnumerable<ModulusWeightMapping> RuleMappings { get; private set; }

        public ModulusWeightTable(string valacdosFileContents)
        {
            RuleMappings = new ValacdosSource(valacdosFileContents).GetModulusWeightMappings;
        }

        public IEnumerable<ModulusWeightMapping> GetRuleMappings(SortCode sortCode)
        {
            return
                RuleMappings.Where(rm => sortCode.DoubleValue >= rm.SortCodeStart.DoubleValue 
                        && sortCode.DoubleValue <= rm.SortCodeEnd.DoubleValue);
        } 
    }
}
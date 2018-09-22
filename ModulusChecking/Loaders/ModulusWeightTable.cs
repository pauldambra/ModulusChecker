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
        public IEnumerable<ModulusWeightMapping> RuleMappings { get; }

        public ModulusWeightTable(string valacdosFileContents)
        {
            if (valacdosFileContents == null)
            {
                throw new ProvidedValacodosContentIsNull();
            }

            if (string.IsNullOrWhiteSpace(valacdosFileContents))
            {
                throw new ProvidedValacodosContentIsEmpty();
            }
            
            RuleMappings = new ValacdosSource(valacdosFileContents).GetModulusWeightMappings;
        }

        public IEnumerable<ModulusWeightMapping> GetRuleMappings(SortCode sortCode) 
            => RuleMappings.Where(rm => MappingContainsSortCode(sortCode, rm));

        private static bool MappingContainsSortCode(SortCode sortCode, ModulusWeightMapping rm)
        {
            var mappingStart = rm.SortCodeStart.DoubleValue;
            var mappingEnd = rm.SortCodeEnd.DoubleValue;
            var sc = sortCode.DoubleValue;
            return mappingStart <= sc && sc <= mappingEnd;
        }
    }
}
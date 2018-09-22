using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ModulusChecking.Properties;

namespace ModulusChecking.Loaders
{
    internal partial class SortCodeSubstitution
    {
        private static Dictionary<string, string> _sortCodeSubstitutionSource;

        public SortCodeSubstitution(string scsubtabFileContents)
        {
            if (scsubtabFileContents == null)
            {
                throw new ProvidedValacodosContentIsNull();
            }

            if (string.IsNullOrWhiteSpace(scsubtabFileContents))
            {
                throw new ProvidedValacodosContentIsEmpty();
            }
            
            _sortCodeSubstitutionSource = scsubtabFileContents
                .Split(new[] { "\r\n", "\n" }, StringSplitOptions.None)
                .Select(row => row.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries))
                .Where(items => items.Length == 2)
                .ToDictionary(r => r[0], r => r[1]);

            if (_sortCodeSubstitutionSource.Count < 1)
            {
                throw new ProvidedValacodosContentIsProbablyInvalid();
            }
        }

        public string GetSubstituteSortCode(string original)
        {
            string sub;
            return _sortCodeSubstitutionSource.TryGetValue(original, out sub) ? sub : original;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ModulusChecking.Properties;

namespace ModulusChecking.Loaders
{
    internal class SortCodeSubstitution
    {
        private readonly Dictionary<string, string> _sortCodeSubstitutionSource;

        public SortCodeSubstitution(string scSubTabFileContents)
        {
            _sortCodeSubstitutionSource = scSubTabFileContents
                .Split(new[] { "\r\n", "\n" }, StringSplitOptions.None)
                .Select(row => row.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries))
                .Where(items => items.Length == 2)
                .ToDictionary(r => r[0], r => r[1]);
        }

        public string GetSubstituteSortCode(string original)
        {
            string sub;
            return _sortCodeSubstitutionSource.TryGetValue(original, out sub) ? sub : original;
        }
    }
}

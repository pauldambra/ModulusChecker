using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ModulusChecking.Properties;

namespace ModulusChecking.Loaders
{
    internal class SortCodeSubstitution
    {
        private static SortCodeSubstitution _instance;

        public static SortCodeSubstitution GetInstance(string scsubtabFileContents)
        {
            return _instance ?? (_instance = new SortCodeSubstitution(scsubtabFileContents));
        }
        
        private static Dictionary<string, string> _sortCodeSubstitutionSource;

        private SortCodeSubstitution(string scsubtabFileContents)
        {
            _sortCodeSubstitutionSource = scsubtabFileContents
                .Split(new[] { "\r\n", "\n" }, StringSplitOptions.None)
                .Select(row => row.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries))
                .Where(items => items.Length == 2)
                .ToDictionary(r => r[0], r => r[1]);
        }

        public string GetSubstituteSortCode(string original)
        {
            string sub;
            Debug.Assert(_sortCodeSubstitutionSource != null, "_sortCodeSubstitutionSource != null");
            return _sortCodeSubstitutionSource.TryGetValue(original, out sub) ? sub : original;
        }
    }
}

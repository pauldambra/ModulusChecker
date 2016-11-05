using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ModulusChecking.Properties;

namespace ModulusChecking.Loaders
{
    internal class SortCodeSubstitution
    {
        private Dictionary<string, string> _sortCodeSubstitutionSource;

        private void SetupDictionary()
        {
            if (_sortCodeSubstitutionSource != null) return;

            _sortCodeSubstitutionSource = Resources.scsubtab
                .Split(new[] { "\r\n", "\n" }, StringSplitOptions.None)
                .Select(row => row.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries))
                .Where(items => items.Length == 2)
                .ToDictionary(r => r[0], r => r[1]);
        }

        public string GetSubstituteSortCode(string original)
        {
            if (_sortCodeSubstitutionSource == null) {SetupDictionary();}
            string sub;
            Debug.Assert(_sortCodeSubstitutionSource != null, "_sortCodeSubstitutionSource != null");
            return _sortCodeSubstitutionSource.TryGetValue(original, out sub) ? sub : original;
        }
    }
}

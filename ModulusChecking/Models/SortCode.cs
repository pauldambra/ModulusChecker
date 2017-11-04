using System;
using System.Text.RegularExpressions;

namespace ModulusChecking.Models
{
    public class SortCode
    {
        private static readonly Regex _sortCodeRegex = new Regex("^[0-9]{6}$", RegexOptions.Compiled);

        private readonly double _doubleValue;
        public double DoubleValue => _doubleValue;
        private readonly string _value;
        
        public override string ToString()
        {
            return _value;
        }

        public SortCode(string s)
        {
            if (!_sortCodeRegex.IsMatch(s))
            {
                throw new ArgumentException("A Sort Code must be a string consisting of 6 digits. Not " + s);
            }
            _value = s;
            _doubleValue = double.Parse(s);
        }

        private bool Equals(SortCode other)
        {
            return _doubleValue.Equals(other._doubleValue);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((SortCode) obj);
        }

        public override int GetHashCode()
        {
            return _doubleValue.GetHashCode();
        }

        internal static bool IsCooperativeBankSortCode(string sortCode)
        {
            return sortCode.StartsWith("08")||sortCode.StartsWith("839");
        }

        internal static bool IsNatWestSortCode(string sortCode)
        {
            return sortCode.StartsWith("600") 
                   || sortCode.StartsWith("606")
                   || sortCode.StartsWith("601")
                   || sortCode.StartsWith("609")
                   || sortCode.StartsWith("830")
                   || sortCode.StartsWith("602");
        }
    }
}
using System;
using System.Text.RegularExpressions;

namespace ModulusChecking.Models
{
    public class SortCode : BankAccountPart
    {
        private readonly double _doubleValue;
        public double DoubleValue { get {return _doubleValue;} }

        public SortCode(string s)
        {
            if (!Regex.IsMatch(s, "^[0-9]{6}$"))
            {
                throw new ArgumentException("A Sort Code must be a string consisting of 6 digits. Not " + s);
            }
            Value = s;
            _doubleValue = Double.Parse(s);
        }

        protected bool Equals(SortCode other)
        {
            return _doubleValue.Equals(other._doubleValue);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((SortCode) obj);
        }

        public override int GetHashCode()
        {
            return _doubleValue.GetHashCode();
        }
    }
}
using System;
using System.Text.RegularExpressions;

namespace ModulusChecking.Models
{
    public class SortCode : BankAccountPart
    {
        private double _doubleValue = -1;

        public double DoubleValue
        {
            get
            {
                if (_doubleValue < 0)
                {
                    _doubleValue = Double.Parse(Value);
                }
                return _doubleValue;
            }
        }

        public SortCode(string s)
        {
            if (Regex.IsMatch(s,"^[0-9]{6}$"))
            {
                Value = s;
            } 
            else
            {
                throw new ArgumentException("A Sort Code must be a string consisting of 6 digits. Not "+s);    
            }
        }
    }
}
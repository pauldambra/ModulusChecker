using System;
using System.Globalization;

namespace ModulusChecking.Models
{
    public abstract class BankAccountPart
    {
        protected string Value;

        public override string ToString()
        {
            return Value;
        }

        public int IntegerAt(int i)
        {
            return Int16.Parse(Value[i].ToString(CultureInfo.InvariantCulture));
        }

        public void SetElementAt(int i, char newCharacter)
        {
            if (!Char.IsDigit(newCharacter))
            {
                throw new ArgumentException("The new character must be a digit.");
            }
            if (i < 0 || i > Value.Length)
            {
                throw new ArgumentOutOfRangeException("i",
                                                      string.Format(
                                                          "The given index {0} does not fall within the length ({1}) of the value to be altered",
                                                          i, Value.Length));
            }

            var chars = Value.ToCharArray();
            chars[i] = newCharacter;
            Value = new string(chars);
        }

    }
}
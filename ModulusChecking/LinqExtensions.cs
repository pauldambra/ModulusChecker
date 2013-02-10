using System;
using System.Collections.Generic;
using System.Linq;

namespace ModulusChecking
{
    static class LinqExtensions
    {
        public static T Second<T>(this IEnumerable<T> source)
        {
            var enumerable = source as IList<T> ?? source.ToList();
            if (enumerable.Count() < 2)
            {
                throw new ArgumentException("the provided source must contains at least two items");
            }
            return enumerable.ElementAt(1);
        }
    }
}

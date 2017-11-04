using System;
using System.Linq;

namespace ModulusChecking.Models
{
    public class ModulusWeightMapping
    {
        public SortCode SortCodeStart { get; }
        public SortCode SortCodeEnd { get; }
        public ModulusAlgorithm Algorithm { get; }
        public int[] WeightValues { get; set; }
        public int Exception { get; }

        public ModulusWeightMapping(
            SortCode sortCodeStart,
            SortCode sortCodeEnd,
            ModulusAlgorithm modulusAlgorithm,
            int[] weightValues,
            int exception)
        {
            SortCodeStart = sortCodeStart;
            SortCodeEnd = sortCodeEnd;
            Algorithm = modulusAlgorithm;
            WeightValues = weightValues;
            Exception = exception;
        }

        public static ModulusWeightMapping From(string row)
        {   
            var weightValues = new int[14];
            var items = row.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
            var sortCodeStart = new SortCode(items[0]);
            var sortCodeEnd = new SortCode(items[1]);
            var algorithm = (ModulusAlgorithm) Enum.Parse(typeof(ModulusAlgorithm), items[2], true);
            for (var i = 3; i < 17; i++)
            {
                weightValues[i - 3] = int.Parse(items[i]);
            }
            var exception = -1;
            if (items.Length==18)
            {
                exception = int.Parse(items[17]);
            }

            return new ModulusWeightMapping(sortCodeStart, sortCodeEnd, algorithm, weightValues, exception);
        }

        public ModulusWeightMapping(ModulusWeightMapping original)
        {
            WeightValues = new int[14];
            Array.Copy(original.WeightValues, WeightValues, 14);
            Algorithm = original.Algorithm;
            SortCodeStart = original.SortCodeStart;
            SortCodeEnd = original.SortCodeEnd;
            Exception = original.Exception;
        }

    }
}
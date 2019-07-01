using System;
using System.Collections.Generic;

namespace CodeNames.Logic
{
    public class Randomizer
    {
        Random random = new Random();

        public int MaxValue { get; }

        public Randomizer(int maxValue)
        {
            MaxValue = maxValue;
        }

        List<int> used = new List<int>();
        public int Next()
        {
            if (used.Count == MaxValue + 1)
                throw new Exception("No more numbers");

            int suggested;
            do
            {
                suggested = random.Next(MaxValue);
            }
            while (used.Contains(suggested));

            used.Add(suggested);
            return suggested;
        }
    }
}

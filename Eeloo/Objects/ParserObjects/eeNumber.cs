using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Eeloo.Objects.ParserObjects
{
    using Num = Byte; // Aliasing this so it's easier to compare performance and memory usage for different underlying types.
    public class eeNumber
        /* The eeNumber implementation:
         * 
         */
    {
        // 
        private byte[] integers;
        private bool negative;
        
        public eeNumber(long num)
        {
            if (num < 0) // If it's negative
            {
                negative = true;
                num *= -1;
            }

            integers = num.ToString().ToCharArray().Select(x => byte.Parse(x.ToString())).ToArray();
        }

        public eeNumber(string num)
        {
            if (num.StartsWith("-")) // If it's negative
            {
                negative = true;
                num.Replace("-", "");
            }

            integers = num.ToCharArray().Select(x => byte.Parse(x.ToString())).ToArray();
        }

        // Returns the amount of delta between this num and its next overflow
        public Num GetOverflowDelta()
        { return (Num) (Num.MaxValue - integers[integers.Length - 1]); }

        public static eeNumber operator +(eeNumber num1, eeNumber num2)
        {
            // Check if adding the two values would cause an overflow
            bool overflow = false;
            return null;
        }
    }

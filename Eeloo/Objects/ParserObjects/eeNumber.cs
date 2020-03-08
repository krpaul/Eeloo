using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Eeloo.Objects.ParserObjects
{
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

        public eeNumber(byte[] nums)
        { integers = nums;  }

        // Returns the amount of delta between this num and its next overflow
        public Num GetOverflowDelta()
        { return (Num)(Num.MaxValue - integers[integers.Length - 1]); }

        public static eeNumber operator +(eeNumber num1, eeNumber num2)
        {
            // Calculate which number has more digits
            byte[] lhs, rhs;
            if (num1.integers.Length > num2.integers.Length)
            {
                lhs = num1.integers;
                rhs = num2.integers;
            }
            else
            {
                lhs = num2.integers;
                rhs = num1.integers;
            }

            bool carry = false;
            for (int i = rhs.Length - 1; i >= 0; i--)
            {
                // Add the digits
                byte digitSum = (byte) (num1.integers[i] + num2.integers[i]);

                // Account for the carry
                if (carry)
                    digitSum += 1; 

                // Count carry
                if (digitSum > 9)
                {
                    carry = true;
                    digitSum -= 10;
                }

                // Set the digit
                lhs[i] = digitSum;
            }

            // If there is still carry left
            if (carry)
            {
                // Create a new number that is 1 larger
                byte[] newBytes = new byte[lhs.Length + 1];

                // set the first value to 1
                newBytes[0] = 1;

                // copy all the old digits
                lhs.CopyTo(newBytes, 1);

                eeNumber newNum = new eeNumber(new byte { } );
            }
        }
    }
}
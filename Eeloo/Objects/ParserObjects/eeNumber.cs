using System.Linq;
using System.Collections.Generic;
using System;

namespace Eeloo.Objects.ParserObjects
{
    public class eeNumber
    /* The eeNumber implementation:
     * 
     */
    {
        // 
        private byte[] bytes;
        private bool negative;

        // static zero
        static eeNumber ZERO = new eeNumber(0);

        public eeNumber(long num)
        {
            if (num < 0) // If it's negative
            {
                this.negative = true;
                num *= -1;
            }

            bytes = num.ToString().ToCharArray().Select(x => byte.Parse(x.ToString())).ToArray();
        }

        public eeNumber(string num)
        {
            if (num.StartsWith("-")) // If it's negative
            {
                negative = true;
                num = num.Replace("-", "");
            }

            bytes = num.ToCharArray().Select(x => byte.Parse(x.ToString())).ToArray();
        }

        public eeNumber(byte[] nums, bool negative = false)
        {
            bytes = nums;
            this.negative = negative;
        }

        public override string ToString()
        {
            string str = negative ? "-" : "";

            int chunkSize = 0;
            foreach (byte b in this.bytes.Reverse())
            {
                if (chunkSize == 3)
                {
                    str += ",";
                    chunkSize = 0;
                }
                str += b.ToString();
                chunkSize++;
            }

            var chrs = str.ToCharArray();
            Array.Reverse(chrs);
            return new string(chrs);
        }

        /* Removes all preceding zeros from num */
        public void TrimZeros()
        {
            /* Func which removes all preceding zeros from num.
             * We can't use indexing in case the length of the array overflows a long.
             * We also cant use recursion because of maximum recursion depth. 
             * Best solution is a LINQ query             
             */

            // Check if func call is uneeded.
            if (bytes.Length == 1 || bytes[0] != 0)
                return;

            // SkipWhile each first elem is 0
            bytes = bytes.SkipWhile(x => x == 0).ToArray();

            // If it removed all the nums, it's a zero
            if (bytes.Length == 0)
                bytes = new byte[1] { 0 };
        }

        /* Removes all two digits nums from num and carrys over digits */
        public void CarryOver()
        {
            // Iterate over the bytes in reverse order
            byte[] nums = this.bytes.Reverse().ToArray();
            byte carry = 0;
            for (int i = 0; i < nums.Length; i++)
            {
                // Account for previous carry if needed
                if (carry != 0)
                {
                    nums[i] += carry;
                    carry = 0;
                }

                // If this digit overflows
                if (nums[i] > 9)
                {
                    carry = (byte) (nums[i] / 10);
                    nums[i] %= 10;
                }
            }

            // if some carry remains afterwards
            if (carry != 0)
            {
                // stretch the array
                byte[] newBytes = new byte[nums.Length + 1];

                // and make the first byte the leftover carry
                newBytes[0] = carry;
                nums.Reverse().ToArray().CopyTo(newBytes, 1);

                // override the bytes
                this.bytes = newBytes;
            }
            else // otherwise, everything is already in place.
            {
                this.bytes = nums.Reverse().ToArray(); 
            }
        }

        public static eeNumber operator +(eeNumber num1, eeNumber num2)
        {
            bool negate = false;

            // if first num is negative
            if (num1.negative && !num2.negative)
            {
                num1.negative = false;
                return num2 - num1;
            }
            // If 2nd num is negative
            else if (num2.negative && !num1.negative)
            {
                num2.negative = false;
                return num1 - num2;
            }
            // They're both negative
            else if (num1.negative && num2.negative)
            {
                num1.negative = false;
                num2.negative = false;
                negate = true;
            }

            if (num2.bytes.Length > num1.bytes.Length)
            {
                // switch so that num1 is larger
                var buf = num1;
                num1 = num2;
                num2 = buf;
            }

            // Reverse because we're adding from right to left.
            byte[] lhs = num1.bytes.Reverse().ToArray(),
                   rhs = num2.bytes.Reverse().ToArray();

            int i;
            for (i = 0; i < rhs.Length; i++)
            {
                byte digitSum = (byte) (rhs[i] + lhs[i]);
                lhs[i] = digitSum;
            }

            num1.bytes = lhs.Reverse().ToArray();
            num1.CarryOver();

            return num1;
        }

        public static eeNumber operator *(eeNumber num1, eeNumber num2)
        {
            bool negate = false;

            // if either num is negative
            if (num1.negative ^ num2.negative)
            {
                num1.negative = false;
                num2.negative = false;

                negate = true;
            }
            else if (num1.negative && num2.negative)
            {
                num1.negative = false;
                num2.negative = false;
            }

            // Reverse because we're going from right to left.
            byte[] lhs = num1.bytes.Reverse().ToArray(),
                   rhs = num2.bytes.Reverse().ToArray();

            // new digit
            eeNumber finalNum = new eeNumber(0);

            // foreach digit in on the top num
            for (int i = 0; i < rhs.Length; i++)
            {
                byte carry = 0;
                List<byte> product = new List<byte>();

                // foreach digit in the bottom num
                for (int j = 0; j < lhs.Length; j++)
                {
                    // multiply them
                    byte digitProduct = (byte)(lhs[j] * rhs[i]);

                    if (carry != 0)
                    {
                        digitProduct += carry;
                        carry = 0;
                    }

                    if (digitProduct > 9)
                    {
                        carry = (byte) (digitProduct / 10);
                        digitProduct %= 10;
                    }

                    product.Add(digitProduct);
                }

                // If carry is left
                if (carry != 0)
                { product.Add(carry); }

                product.Reverse();

                // Add the needed amount of trailing zeros for place value
                for (int z = 0; z < i; z++)
                    product.Add(0);

                finalNum += new eeNumber(product.ToArray());
            }

            finalNum.negative = negate;
            return finalNum;
        }

        public static eeNumber operator -(eeNumber num1, eeNumber num2)
        {
            // if first num is negative
            if (num1.negative && !num2.negative)
            {
                // add them then negate it
                num2.negative = false;
                var sum = num1 + num2;
                sum.negative = true;
                return sum;
            }
            // if second num is negative
            else if (num2.negative && !num1.negative)
            {
                // minus and negatives cancel
                num2.negative = false;
                return num1 + num2;
            }
            // If they're both negative
            else if (num1.negative && num2.negative)
            {
                // Treat this as adding a negative to a positive
                num2.negative = false;
                return num1 + num2;
            }

            // If we're going to get a negative answer
            bool negate = false;
            if (num2 > num1)
            {
                // reverse the two 
                var buf = num1;
                num1 = num2;
                num2 = buf;

                // and negate
                negate = true;
            }

            // Reverse because we're adding from right to left.
            byte[] l_bytes = num1.bytes.Reverse().ToArray(),
                   r_bytes = num2.bytes.Reverse().ToArray();

            bool carry = false;
            int i;
            for (i = 0; i < r_bytes.Length; i++)
            {
                // Account for the previous carry
                if (carry)
                    r_bytes[i]++;

                // Subtract the digits
                byte digitDiff;
                if (l_bytes[i] < r_bytes[i]) // account for carry
                {
                    digitDiff = (byte)((10 + l_bytes[i]) - r_bytes[i]);
                    carry = true;
                }
                else
                { digitDiff = (byte)(l_bytes[i] - r_bytes[i]); }

                // Set the digit
                l_bytes[i] = digitDiff;
            }


            // If there is still carry left
            while (carry)
            {
                // subtract one from the next value
                if (l_bytes[i] == 0)
                {
                    l_bytes[i] = 9;
                    i++;
                }
                else
                {
                    l_bytes[i] -= 1;
                    carry = false;
                }
            }
            
            // Put it in order again
            l_bytes = l_bytes.Reverse().ToArray();

            num1.bytes = l_bytes;
            num1.negative = negate;
            num1.TrimZeros();

            return num1;
        }

        public static bool operator ==(eeNumber num1, eeNumber num2)
        {
            if (num1.bytes.Length != num2.bytes.Length)
                return false;

            byte[] l_bytes = num1.bytes,
                   r_bytes = num2.bytes;

            for (int i = 0; i < l_bytes.Length; i++)
            {
                if (l_bytes[i] != r_bytes[i])
                    return false; 
            }

            return true;
        }

        public static bool operator !=(eeNumber num1, eeNumber num2)
        { return !(num1 == num2); }

        public static bool operator >(eeNumber num1, eeNumber num2)
        {
            byte[] l_bytes = num1.bytes,
                   r_bytes = num2.bytes;

            // If lhs has more digits
            if (l_bytes.Length > r_bytes.Length)
                return true; 
            // If rhs has more digits
            else if (l_bytes.Length < r_bytes.Length)
                return false;
            else // if they have an equal number of digits
            {
                // iterate through each digit
                for (int i = 0; i < l_bytes.Length; i++)
                {
                    // If the lhs digit is greater, return true
                    if (l_bytes[i] > r_bytes[i])
                        return true;
                    // If they're the same, move on to the next digit
                    else if (l_bytes[i] == r_bytes[i])
                        continue;
                    else // if lhs is smaller, return false
                        return false;
                }
            }

            return false;
        }

        public static bool operator <(eeNumber num1, eeNumber num2)
        { return !(num1 > num2 || num1 == num2); }
    }
}
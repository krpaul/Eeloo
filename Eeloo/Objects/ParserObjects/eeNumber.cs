using System.Linq;

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
            foreach (byte b in this.bytes)
            { str += b.ToString(); }

            return str;
        }

        // Removes all preceding zeros from num
        public void Clean()
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

            // Reverse because we're adding from right to left.
            byte[] lhs = num1.bytes.Reverse().ToArray(),
                   rhs = num2.bytes.Reverse().ToArray();

            bool carry = false;
            for (int i = 0; i < rhs.Length; i++)
            {
                // Add the digits
                byte digitSum = (byte) (lhs[i] + rhs[i]);

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

            // Put it in order again
            lhs = lhs.Reverse().ToArray();

            // If there is still carry left
            if (carry)
            {
                //// If adding a carry to the first number will overflow to the next place: 
                //if (lhs[0] == 9)
                //{
                //    // Create a new number that is 1 larger
                //    byte[] newBytes = new byte[lhs.Length + 1];

                //    // set the first value to 1
                //    newBytes[0] = 1;

                //    // copy all the old digits
                //    lhs.CopyTo(newBytes, 1);

                //    return new eeNumber(newBytes, negate);
                //}
                //else // if not, just add one to the first place
                //    lhs[0]++;

                    lhs[0]++; 
                // if that caused an overflow
                if (lhs[0] == 10)
                {
                    // Create a new array that is 1 larger
                    byte[] newBytes = new byte[lhs.Length + 1];

                    // set the first value to 1
                    newBytes[0] = 1;

                    // set that digit to zero
                    lhs[0] = 0;

                    // copy all the old digits
                    lhs.CopyTo(newBytes, 1);

                    lhs = newBytes;
                }
            }

            num1.bytes = lhs;
            num1.negative = negate;

            return num1;
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

            // if the num is only 1 digit long, it's easier to do regular subtraction as a native c# type
                
            // Reverse because we're adding from right to left.
            byte[] l_bytes = num1.bytes.Reverse().ToArray(),
                   r_bytes = num2.bytes.Reverse().ToArray();

            bool carry = false;
            for (int i = 0; i < r_bytes.Length; i++)
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

            // Put it in order again
            l_bytes = l_bytes.Reverse().ToArray();

            // If there is still carry left
            if (carry)
            {
                // subtract one from the first value
                l_bytes[0]--;
            }

            num1.bytes = l_bytes;
            num1.negative = negate;
            num1.Clean();

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
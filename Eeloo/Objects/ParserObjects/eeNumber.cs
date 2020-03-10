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

            // Calculate which number has more digits
            byte[] lhs, rhs;
            if (num1.bytes.Length > num2.bytes.Length)
            {
                lhs = num1.bytes;
                rhs = num2.bytes;
            }
            else
            {
                lhs = num2.bytes;
                rhs = num1.bytes;
            }

            // Reverse because we're adding from right to left.
            lhs = lhs.Reverse().ToArray();
            rhs = rhs.Reverse().ToArray();

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
                // Create a new number that is 1 larger
                byte[] newBytes = new byte[lhs.Length + 1];

                // set the first value to 1
                newBytes[0] = 1;

                // copy all the old digits
                lhs.CopyTo(newBytes, 1);

                return new eeNumber(newBytes, negate);
            }

            num1.bytes = lhs;
            num1.negative = negate;

            return num1;
        }

        public static eeNumber operator -(eeNumber num1, eeNumber num2)
        {
            // Calculate which number has more digits
            eeNumber lhs, rhs;
            if (num1.bytes.Length > num2.bytes.Length)
            {
                lhs = num1;
                rhs = num2;
            }
            else
            {
                lhs = num2;
                rhs = num1;
            }

            bool negate = false;
            // If we're going to get a negative answer
            if (rhs > lhs)
            {
                // reverse the two 
                var buf = lhs;
                lhs = rhs;
                rhs = buf;

                // and negate
                negate = true;
            }

            // Reverse because we're adding from right to left.
            byte[] l_bytes = lhs.bytes.Reverse().ToArray(),
                   r_bytes = rhs.bytes.Reverse().ToArray();

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
                // Create a new number that is 1 larger
                byte[] newBytes = new byte[l_bytes.Length + 1];

                // set the first value to 1
                newBytes[0] = 1;

                // copy all the old digits
                l_bytes.CopyTo(newBytes, 1);

                var newNum = new eeNumber(newBytes, negate);
                newNum.Clean();
            }

            num1.bytes = l_bytes;
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
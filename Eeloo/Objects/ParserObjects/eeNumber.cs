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
                negative = true;
                num *= -1;
            }

            bytes = num.ToString().ToCharArray().Select(x => byte.Parse(x.ToString())).ToArray();
        }

        public eeNumber(string num)
        {
            if (num.StartsWith("-")) // If it's negative
            {
                negative = true;
                num.Replace("-", "");
            }

            bytes = num.ToCharArray().Select(x => byte.Parse(x.ToString())).ToArray();
        }

        public eeNumber(byte[] nums)
        { bytes = nums;  }

        public override string ToString()
        {
            string str = "";
            foreach (byte b in this.bytes)
            { str += b.ToString(); }

            return str;
        }

        public static eeNumber operator +(eeNumber num1, eeNumber num2)
        {
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

            lhs = lhs.Reverse().ToArray();
            rhs = rhs.Reverse().ToArray();

            // If there is still carry left
            if (carry)
            {
                // Create a new number that is 1 larger
                byte[] newBytes = new byte[lhs.Length + 1];

                // set the first value to 1
                newBytes[0] = 1;

                // copy all the old digits
                lhs.CopyTo(newBytes, 1);

                return new eeNumber(newBytes);
            }

            num1.bytes = lhs;
            return num1;
        }
    }
}
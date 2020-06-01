using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Eeloo.Objects.ParserObjects
{
    public class eeNumber
    /* The eeNumber implementation:
     * 
     */
    {
        private byte[] bytes;
        private bool negative;
        private eeNumber denominator;

        public const int DEFAULTMAXDECIMALPLACE = 8;

        // static often-used nums
        public static eeNumber ZERO = new eeNumber(0),
                               ONE = new eeNumber(1),
                               NEG_ONE = new eeNumber(-1);

        #region Constructors

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

            if (num.Contains('.')) // if it's a decimal/double
            {
                var split = num.Split('.'); // seperate integer and decimal

                // set the denominator
                denominator = new eeNumber("1" + new string('0', split[1].Length));

                var fracNumerator = new eeNumber(split[1]);
                var intPart = new eeNumber(split[0]);

                // continue for the numerator normally
                num = ((intPart * denominator) + fracNumerator).NumeratorToString();
            }

            bytes = num.ToCharArray().Select(x => byte.Parse(x.ToString())).ToArray();
        }

        public eeNumber(byte[] nums, bool negative = false)
        {
            bytes = nums;
            this.negative = negative;
        }

        public eeNumber(IEnumerable<byte> nums, bool negative = false)
        {
            bytes = nums.ToArray();
            this.negative = negative;
        }

        // will glue all given nums together
        public eeNumber(IEnumerable<eeNumber> nums, bool negative = false)
        {
            var bytes = new List<byte>();
            foreach (var num in nums)
            {
                foreach(byte b in num.bytes)
                    bytes.Add(b);
            }
            this.bytes = bytes.ToArray();         
        }

        #endregion

        #region Operators

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

            // Reverse because we're subtracting from right to left.
            byte[] l_bytes = num1.bytes.Reverse().ToArray(),
                   r_bytes = num2.bytes.Reverse().ToArray();

            bool carry = false;
            int i;
            for (i = 0; i < r_bytes.Length; i++)
            {
                // Account for the previous carry
                if (carry)
                {
                    r_bytes[i]++;
                    carry = false;
                }

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

        public static eeNumber operator *(eeNumber num1, eeNumber num2)
        {
            // if either num is 0, return 0
            if (num1 == ZERO || num2 == ZERO)
                return new eeNumber(0);

            bool negate = false;

            /* first account for negatives */
            // if either num is negative
            if (num1.negative ^ num2.negative)
            {
                // set both to non-negative to multiply them
                num1.negative = false;
                num2.negative = false;

                // apply negative afterwards
                negate = true;
            }
            else if (num1.negative && num2.negative)
            {
                // otherwise, negatives just cancel out
                num1.negative = false;
                num2.negative = false;
            }

            eeNumber final;

            // if only one num is a fraction
            if (num1.IsFrac() ^ num2.IsFrac())
            {
                eeNumber frac = num1.denominator != null ? num1 : num2,
                         reg = num1.denominator != null ? num2 : num1;

                var den = frac.PopDenominator();
                var newNum = (frac * reg) / den;

                final = newNum;
            }
            // if both nums are fractions
            else if (num1.IsFrac() && num2.IsFrac())
            {
                eeNumber frac1 = num1.PopDenominator(),
                         frac2 = num2.PopDenominator();

                final = (num1 * num2) / (frac1 * frac2);
            }
            // regular nums
            else
            {
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

                final = finalNum;
            }

            final.negative = negate;
            return final;
        }

        public static eeNumber operator /(eeNumber num1, eeNumber num2)
        {
            /* eeNumbers do not perform traditional division. That is only done
             * when the number needs to be approximated for a text representation.
             * For divison, we always keep the fractional form for arbitrary accuracy.
             */

            bool a = num1.IsFrac(), b = num2.IsFrac();
            // frac divided by num
            if (a && !b)
            {
                num1.denominator *= num2;
                return num1;
            }
            // num divided by num
            else if (!a && !b)
            {
                num1.denominator = num2;
                return num1;
            }
            // num divided by frac
            else if (!a && b)
            {
                eeNumber numerator = num1 * num2.denominator;
                eeNumber denominator = num2;

                return numerator / denominator;
            }
            // frac divided by frac
            else
            {
                var denom1 = num1.PopDenominator();

                eeNumber numerator = num1 * num2.PopDenominator();
                eeNumber denominator = num2 * denom1;

                return numerator / denominator;
            }
        }

        public static eeNumber operator %(eeNumber num1, eeNumber num2)
        {
            if (num1 < num2)
                return num1;

            if (num1.denominator != null || num2.denominator != null)
                throw new Exception("Modulo of non-integers not supported currently");

            num1.IntegerDivision(num2, out eeNumber rem);

            return rem;
        }

        public static bool operator ==(eeNumber num1, eeNumber num2)
        {
            // check if they're null first
            bool nullA = object.Equals(num1, null),
                 nullB = object.Equals(num2, null)
                 ;
        
            if (nullA && nullB) // both null -> true
                return true;
            else if (nullA ^ nullB) // only one null -> false
                return false;

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

        public static bool operator <=(eeNumber num1, eeNumber num2)
        { return num1 == num2 || num1 < num2; }

        public static bool operator >=(eeNumber num1, eeNumber num2)
        { return num1 == num2 || num1 > num2; }

        #endregion

        #region String Conversion

        /* Returns the absolute value of the numerator as a string */
        public string NumeratorToString()
        {
            string str = "";

            foreach (byte b in this.bytes.Reverse())
                str += b.ToString();

            var chrs = str.ToCharArray();
            Array.Reverse(chrs);
            return new string(chrs);
        }

        /* Returns an approximation of this number as a string (or an exact value if this number is rational) */
        public override string ToString()
        {
            string s = negative ? "-" : "";
            return s + (this.denominator != null
                ? this.ApproximateDivision(DEFAULTMAXDECIMALPLACE) // is a frac
                : this.NumeratorToString());                       // regular integer
        }

        #endregion

        #region Utility Methods

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
                    carry = (byte)(nums[i] / 10);
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

        private bool IsFrac()
        { return denominator != null; }

        // removes the denominator of this number and returns it
        private eeNumber PopDenominator()
        {
            var den = this.denominator;
            this.denominator = null;
            return den;
        }

        private eeNumber IntegerDivision(eeNumber divisor, out eeNumber mod)
        {
            // base case
            if (this < divisor)
            {
                mod = this;
                return new eeNumber(0);
            }

            Queue<byte> bytesQue = new Queue<byte>(this.bytes); // a que of the dividend as bytes
            List<eeNumber> quotient = new List<eeNumber>();     // quotient

            eeNumber divPart = null;

            while (bytesQue.Count() > 0)
            {
                /* Grab the next digit from dividend */
                var asList = divPart == null ? new List<byte>() : divPart.bytes.ToList();
                var nextByte = bytesQue.Dequeue();
                asList.Add(nextByte);

                /* Keep grabbing until we run out or need more */
                divPart = new eeNumber(asList);
                while (bytesQue.Count() != 0 && divPart < divisor)
                {
                    quotient.Add(new eeNumber(0));

                    asList.Add(bytesQue.Dequeue());
                    divPart = new eeNumber(asList);
                    divPart.TrimZeros();
                }

                divPart.TrimZeros();

                /* Divide it and append the division to the quotient */
                eeNumber q = new eeNumber(1); // number of times divisor fits into this divPart
                while (divisor * q <= divPart)
                    q += ONE;
                q -= ONE;

                quotient.Add(q); // keep track of quotient

                /* Figure out the remainder*/
                divPart = divPart - (divisor * q);
            }

            // remainder
            mod = divPart;

            var intergerQuotient = new eeNumber(quotient);
            intergerQuotient.TrimZeros();

            return intergerQuotient;
        }

        private string ApproximateDivision(long accurateTo)
        {
            if (this.denominator == null)
                throw new Exception("Cannot approximate division on an integer");

            eeNumber denom = this.PopDenominator(),
                     remainder,
                     integerQuotient = this.IntegerDivision(denom, out remainder);

            string approx = integerQuotient.NumeratorToString();

            if (remainder == ZERO)
                return approx;

            const string ZEROSTR = "0";
            string decimalAprx = "";

            // If the dividend is longer than the max accuracy, iterate to that, otherwise, to max accuracy
            long c = denom.bytes.LongCount();
            if (c > accurateTo)
                accurateTo += c;

            for (int i = 1; i <= accurateTo; i++)
            {
                eeNumber dec = new eeNumber(
                    remainder.NumeratorToString() + ZEROSTR
                );
                decimalAprx += dec.IntegerDivision(denom, out eeNumber rem).NumeratorToString();
                remainder = rem;
            }
            decimalAprx = decimalAprx.TrimEnd('0');

            this.denominator = denom;

            return $"{approx}.{decimalAprx}";
        }

        #endregion
    }
}
using Antlr4.Runtime.Misc;
using Eeloo.Grammar;
using Eeloo.Objects;
using Eeloo.Objects.ParserObjects;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;

namespace Eeloo.Evaluator
{
    public class StringMathHelpers
    {
        public static eeObject Add(eeObject exp1, eeObject exp2)
        {
            string left = exp1.AsString(), right = exp2.AsString();
            return eeObject.newStringObject(left + right);
        }

        /* String Subtraction:
         * If right is not a substring(s) of left, then the subtraction is invalid.
         * If it is, then said substring will be removed from the lefthand string in its last occurence.
         * eg: "abcde123fgh123" - "123" -> abcde123fgh"
         * eg: "abc" - "d" -> Error: invalid string subtraction
         * 
         * This function will return null if it is an invalid subtraction, 
         * otherwise it will return the "difference"
         */
        public static eeObject Subtract(eeObject exp1, eeObject exp2)
        {
            string left = exp1.AsString(), right = exp2.AsString();

            if (!left.Contains(right))
                throw new Exception("Cannot subtract two strings where one is not a substring of the other");

            var chArry = right.ToCharArray();
            int pos = left.LastIndexOfAny(chArry);

            return eeObject.newStringObject(left.Remove(pos - chArry.Length + 1, chArry.Length));
        }

        public static eeObject Multiply(eeObject exp1, eeObject exp2)
        {
            // Figure out which one is a string and which one is a num
            string str = exp1.type == eeObjectType.NUMBER ? exp2.AsString() : exp1.AsString();
            eeNumber num = exp1.type == eeObjectType.NUMBER ? exp1.AsNumber() : exp2.AsNumber();

            // Copy string n times
            string concat = "";
            for (eeNumber i = eeNumber.ZERO; i < num; i += eeNumber.ONE)
                concat += str;

            return eeObject.newStringObject(concat);
        }

        /* First will check for length, the longer one will win.
         * If they're the same length, the one alphabetically earlier will win.
         * Returns bool as opposed to a eeBoolObject because the eeObject method also returns a bool directly.
         */
        public static bool GreaterThan(eeObject exp1, eeObject exp2)
        {
            return exp1.AsString().CompareTo(exp2.AsString()) < 0;
        }
    }
}
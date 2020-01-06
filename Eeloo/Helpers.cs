using System;
using System.Collections.Generic;
using System.Text;

using Eeloo.Objects;

namespace Eeloo
{
    public class Helpers
    {
        public static bool EvaluateComparison(string comparasonOperator, eeObject val1, eeObject val2)
        {
            switch (comparasonOperator)
            {
                case "==":
                    return val1.AsNumber() == val2.AsNumber(); 
                case ">=":
                    return val1.AsNumber() >= val2.AsNumber(); 
                case "<=":
                    return val1.AsNumber() <= val2.AsNumber(); 
                case "!=":
                    return val1.AsNumber() != val2.AsNumber(); 
                case "<":
                    return val1.AsNumber() < val2.AsNumber(); 
                case ">":
                    return val1.AsNumber() > val2.AsNumber();
                default:
                    return false;
            }
        }
    }
}

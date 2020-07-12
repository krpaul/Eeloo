using System;
using System.Collections.Generic;
using System.Text;

using Eeloo.Objects.ParserObjects;

namespace Eeloo.Objects
{
    partial class eeObject
    {
        // small macro to make code more concise
        public static eeObject NegOne()
        { return newNumberObject(new eeNumber(-1)); }

        //public static eeObject newNumberObject(long value, string mod=null)
        //{
        //    var ob = new eeObject(new eeNumber(value))
        //    {
        //        type = eeObjectType.NUMBER,
        //        modifier = mod,
        //        attributes = new Dictionary<string, dynamic>()
        //        {
        //        },
        //        methods = new Dictionary<string, Func<eeObject, ICollection<eeObject>, eeObject>>()
        //        {
        //                    /*
        //            { "increment", (self, args) => 
        //                {
        //                    var incAmount = args.AsList()[0].AsNumber();
        //                    self.value += incAmount;
        //                    return None;
        //                }
        //            }
        //                    */
        //        }
        //    };

        //    return ob;
        //}

        //public static eeObject newNumberObject(double value, string modifier = null)
        //{ 
        //    return new eeObject(new eeNumber(value.ToString()))
        //    {
        //        type = eeObjectType.NUMBER,
        //        modifier = modifier,
        //    }; 
        //}

        public static eeObject newNumberObject(eeNumber value, string modifier = null)
        {
            return new eeObject(value)
            {
                type = eeObjectType.NUMBER,
                modifier = modifier,
            };
        }
    }
}

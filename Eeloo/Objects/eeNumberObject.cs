using System;
using System.Collections.Generic;
using System.Text;

namespace Eeloo.Objects
{
    partial class eeObject
    {
        public static eeObject newNumberObject(long value, string mod=null)
        {
            var ob = new eeObject(value)
            {
                type = eeObjectType.NUMBER,
                modifier = mod,
                attributes = new Dictionary<string, dynamic>()
                {
                    { "maxCount", (ulong) 0}
                },
                methods = new Dictionary<string, Func<eeObject, ICollection<eeObject>, eeObject>>()
                {
                            /*
                    { "increment", (self, args) => 
                        {
                            var incAmount = args.AsList()[0].AsNumber();
                            self.value += incAmount;
                            return None;
                        }
                    }
                            */
                }
            };

            return ob;
        }

        public static eeObject newNumberObject(double value, string modifier = null)
        { 
            return new eeObject(value)
            {
                type = eeObjectType.DECIMAL,
                modifier = modifier,
            }; 
        }
    }
}

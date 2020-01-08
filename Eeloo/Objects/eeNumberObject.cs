using System;
using System.Collections.Generic;
using System.Text;

namespace Eeloo.Objects
{
    partial class eeObject
    {
        public static eeObject newNumberObject(long value)
        {
            return new eeObject(value)
            {
                type = eeObjectType.NUMBER
            };
        }

        public static eeObject newNumberObject(long value, bool multiplier)
        {
            return new eeObject(value)
            {
                type = eeObjectType.NUMBER,
                attributes = new Dictionary<string, dynamic>()
                {
                    { "multiplier", multiplier ? 1 : -1 },
                    { "maxCount", (ulong) 0}
                },
                methods = new Dictionary<string, Func<eeObject, eeObject, eeObject>>()
                {
                    { "increment", (self, args) => 
                        {
                            var incAmount = args.AsList()[0].AsNumber();
                            self.value += incAmount;
                            return None;
                        }
                    }
                }
            };
        }

        public static eeObject newNumberObject(double value)
        { 
            return new eeObject(value)
            {
                type = eeObjectType.DECIMAL
            }; 
        }
    }
}

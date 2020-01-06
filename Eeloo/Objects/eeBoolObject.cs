using System;
using System.Collections.Generic;
using System.Text;

namespace Eeloo.Objects
{
    partial class eeObject
    {
        public static eeObject TRUE = newBoolObject(true);
        public static eeObject FALSE = newBoolObject(false);
        public static eeObject newBoolObject(bool val)
        {
            return new eeObject(val)
            {
                type = eeObjectType.BOOL
            };
        }
    }
}

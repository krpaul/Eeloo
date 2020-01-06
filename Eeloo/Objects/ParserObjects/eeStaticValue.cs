using System;
using System.Collections.Generic;
using System.Text;

namespace Eeloo.Objects.ParserObjects
{
    /* TO DO: Figure out if this is even needed? */
    
    // Give this class a unique integer identifier and it will act as a unique static value for comparing NONE and EMPTY values;
    class eeStaticValue : eeObject
    {
        public eeStaticValue(object val)
        {
            this.value = val;
        }

        public override bool Equals(object other)
        {
            return this.value == ((eeStaticValue) other).value;
        }
    }
}

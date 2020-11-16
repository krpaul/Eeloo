using System;
using System.Collections.Generic;
using System.Text;

using Eeloo.Objects.ParserObjects;
using Eeloo.Objects;

namespace Eeloo.Helpers
{
    class RangeGenerator
    {
        public static ICollection<eeObject> Generate(eeNumber start, eeNumber stop, eeNumber step)
        {
            ICollection<eeObject> rangeObj = new List<eeObject>();

            if (start < stop)
            {
                for (eeNumber i = start; i <= stop; i += step.Copy())
                    rangeObj.Add(eeObject.newNumberObject(i.Copy()));
            }
            else if (stop < start)
            {
                for (eeNumber i = start; i >= stop; i -= step.Copy())
                    rangeObj.Add(eeObject.newNumberObject(i.Copy()));
            }
            else // they're equal
                rangeObj.Add(eeObject.newNumberObject(start.Copy()));

            return rangeObj;
        }
    }
}

using Eeloo.Objects;
using Eeloo.Objects.ParserObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Eeloo.Functions
{
    partial class BuiltInFunctions
    {
        /* Return Unix Time as Number*/
        public static eeObject Time(ICollection<eeObject> args)
        {
            var utime = ((long)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalMilliseconds) * 1000;
            return eeObject.newNumberObject(
                new eeNumber(utime) / new eeNumber(1000000)
            );
        }
    }
}

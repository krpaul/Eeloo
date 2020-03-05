using System;
using System.Collections.Generic;
using System.Linq;

namespace Eeloo.Objects
{
    partial class eeObject
    {
        private static Dictionary<string, Func<eeObject, ICollection<eeObject>, eeObject>> decimalDefaultMethods
            = new Dictionary<string, Func<eeObject, ICollection<eeObject>, eeObject>>()
            { };
    }
}

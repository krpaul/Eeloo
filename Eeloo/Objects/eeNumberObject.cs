using System;
using System.Collections.Generic;
using System.Text;

using Eeloo.Objects.ParserObjects;
using Eeloo.Methods;
using Eeloo.Functions;

namespace Eeloo.Objects
{
    partial class eeObject
    {
        // small macro to make code more concise
        public static eeObject NegOne()
        { return newNumberObject(new eeNumber(-1)); }

        public static eeObject newNumberObject(eeNumber value, string modifier = null)
        {
            return new eeObject(value)
            {
                type = eeObjectType.NUMBER,
                modifier = modifier,
                methods = BuiltInMethods.numberBuiltInMethods
            };
        }
    }
}

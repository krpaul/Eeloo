using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using Eeloo.Objects.ParserObjects;
using System.Reflection;
using Eeloo.Methods;
using Eeloo.Functions;

namespace Eeloo.Objects
{
    partial class eeObject
    {
        public static eeObject newStringObject(string str)
        {
            var newObj = new eeObject(str) {
                type = eeObjectType.STRING,
                methods = BuiltInMethods.stringBuiltInMethods
            };

            newObj.attributes.Add(
                "length", newNumberObject(new eeNumber(str.Length))
            );

            return newObj;
        }
    }
}

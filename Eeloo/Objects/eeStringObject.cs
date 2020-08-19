﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using Eeloo.Objects.ParserObjects;
using System.Reflection;

namespace Eeloo.Objects
{
    partial class eeObject
    {
        private static Dictionary<string, Func<eeObject, ICollection<eeObject>, eeObject>> stringDefaultMethods
            = new Dictionary<string, Func<eeObject, ICollection<eeObject>, eeObject>>()
            {
                {
                    "concatenate",
                    (eeObject self, ICollection<eeObject> strings) =>
                    {
                        //// Alter value
                        //foreach (eeObject obj in strings.AsList())
                        //    self.value += (string) obj.value;

                        //// As well as return new value
                        //return eeObject.newStringObject(self.AsString());
                        return self;
                    }
                },
            };

        public static eeObject newStringObject(string str)
        {
            var newObj = new eeObject(str) {
                type = eeObjectType.STRING,
                methods = stringDefaultMethods
            };

            newObj.attributes.Add("length", newNumberObject(new eeNumber(str.Length)));

            return newObj;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Eeloo.Objects.ParserObjects;

namespace Eeloo.Objects
{
    partial class eeObject
    {
        private static Dictionary<string, Func<eeObject, ICollection<eeObject>, eeObject>> listDefaultMethods 
            = new Dictionary<string, Func<eeObject, ICollection<eeObject>, eeObject>>()
            {
                {
                    "add", // appends new values to the end of the list
                    (eeObject self, ICollection<eeObject> newVals) => 
                    {
                        foreach (eeObject obj in newVals)
                        {
                            ((List<eeObject>) self.value).Add(obj);
                        }

                        var lenAttr = self.attributes["length"];
                        lenAttr.value = lenAttr.AsNumber() + new eeNumber(newVals.Count());

                        return eeObject.None;
                    }
                },
            };

        /* Note: the eeObject.value for eeListObject must always be a List<eeObject> */

        // Constructor passed an internal_EXPRLIST eeObject
        public static eeObject newListObject(eeObject exprlist, string modifier=null)
        {
            ICollection<eeObject> expressions;

            if (exprlist == null)
                expressions = new List<eeObject>();
            else
                // Extract the expressions
                expressions = exprlist.AsEXPRLIST();

            return newListObject(expressions);
        }

        public static eeObject newListObject(ICollection<eeObject> expressions, string modifier=null)
        {
            // Encapsulate the List object into an eeObject
            var newObj = new eeObject(expressions.ToList())
            {
                type = eeObjectType.LIST,
                methods = listDefaultMethods,
                modifier = modifier,
            };

            newObj.attributes.Add(
                "length", newNumberObject(new eeNumber(expressions.Count))
            );

            return newObj;
        }
    }
}

using Eeloo.Objects.ParserObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using Eeloo.Functions;

namespace Eeloo.Objects
{
    partial class eeObject
    {
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
                methods = DefaultMethods.listBuiltInMethods,
                modifier = modifier,
            };

            newObj.attributes.Add(
                "length", newNumberObject(new eeNumber(expressions.Count))
            );

            return newObj;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

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

                        /*
                        if (self.value == null)
                            self.value = newVals.AsList();
                        else
                        {
                            foreach (eeObject obj in newVals.AsEXPRLIST())
                                ((List<eeObject>) self.value).Add(obj);
                        }*/

                        return eeObject.None;
                    }
                },
            };

        /* Note: the eeObject.value for eeListObject must always be a List<eeObject> */

        /* For future reference: Lazy List Implementation ->
         
        public static eeObject newListObject(List<EelooParser.ExpContext> objects)
        {
            // Visit each element of list and create a List objec out of it
            var rawList = objects != null ? (from obj in objects select Antlr.visitor.Visit(obj)).ToList() : null;

            // Encapsulate the List object into an eeObject
            var newObj = new eeObject(
                rawList != null ? rawList : new List<eeObject>()
            )
            {
                type = eeObjectType.LIST,
                methods = listDefaultMethods,
            };

            newObj.attributes.Add(
                "length",
                 objects == null ? 0 : objects.Count
            );

            return newObj;
        }
        */

        /*
        public static eeObject newListObject(List<eeObject> objects)
        {
            // Encapsulate the List object into an eeObject
            var newObj = new eeObject(objects)
            {
                type = eeObjectType.LIST,
                methods = listDefaultMethods,
            };

            newObj.attributes.Add(
                "length",
                 objects == null ? 0 : objects.Count
            );

            return newObj;
        }
        */

        // Constructor passed an internal_EXPRLIST eeObject
        public static eeObject newListObject(eeObject exprlist)
         {
            // Extract the expressions
            ICollection<eeObject> expressions = exprlist.AsEXPRLIST();

            // Encapsulate the List object into an eeObject
            var newObj = new eeObject(expressions.ToList())
            {
                type = eeObjectType.LIST,
                methods = listDefaultMethods,
            };

            newObj.attributes.Add(
                "length", expressions.Count
            );

            return newObj;
        }

        // When passed eeObjects directly
        /*
        public static eeObject newListObject(params eeObject[] objects)
        {
            // Send to primary constructor 
            return newListObject(objects.ToList());
        }
        */
        /*
        public static eeObject newListObject() // for empty lists
        {
            // Encapsulate the List object into an eeObject
            var newObj = new eeObject()
            {
                type = eeObjectType.LIST,
                methods = listDefaultMethods,
            };

            newObj.attributes.Add(
                "length", 0
            );

            return newObj;
        }
        */
    }
}

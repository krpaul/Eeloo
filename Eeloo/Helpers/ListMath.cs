using Eeloo.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Text;
using System.ComponentModel.Design;
using Eeloo.Objects.ParserObjects;

namespace Eeloo.Helpers
{
    class ListMathHelpers
    {
        public static eeObject Add(eeObject exp1, eeObject exp2)
        {
            // if both are lists
            if (exp1.type == eeObjectType.LIST && exp2.type == eeObjectType.LIST)
            {
                // concat the lists

                // TO DO: Make this use our already defined eeListObject methods.
                ((List<eeObject>) exp1.value).AddRange(exp2.AsList());
                return exp1;
            }
            // if at least one is a list
            else if (exp1.type == eeObjectType.LIST ^ exp2.type == eeObjectType.LIST)
            {
                // append the nonlist to the list

                // find the nonlist and list
                eeObject nonlist = exp1.type == eeObjectType.LIST ? exp2 : exp1;
                eeObject list = exp1.type == eeObjectType.LIST ? exp1 : exp2;

                // TO DO: Make this use our already defined eeListObject methods.
                ((List<eeObject>) list.value).Add(nonlist);
                return list;
            }
            else {
                throw new Exception("This shouldn't happen");
            }
        }

        /* List Subtraction:
         * If left and right are both lists, and every element in b is contained in a, all common elements (including duplicates) are removed from a.
         * If left is a list and right is a nonlist, right is removed from left assuming it exists, errors otherwise.
         * Errors if left is a nonlist and right is a list
        */
        public static eeObject Subtract(eeObject exp1, eeObject exp2)
        {
            // if both are lists
            if (exp1.type == eeObjectType.LIST && exp2.type == eeObjectType.LIST)
            {
                List<eeObject> left = exp1.AsList(),
                               right = exp2.AsList();

                // make sure every element in right is contained in left
                if (right.All(i => left.Contains(i)))
                {
                    // uses a linq query to filter out all matches
                    return eeObject.newListObject(
                        (from elmt in left
                         where !right.Contains(elmt)
                         select elmt).ToList()
                    );
                }
                else
                { throw new Exception("Cannot subtract two lists where one is not a subset of the other or where the subtrahend is longer than the minuend"); }
            }
            // if left is list and right is nonlist
            else if (exp1.type == eeObjectType.LIST)
            {
                var left = (List<eeObject>) exp1.value;

                if (!left.Contains(exp2))
                { throw new Exception("Cannot subtract an object from a list which it is not contained in."); }

                // remove all matches
                left.RemoveAll(new Predicate<eeObject>((a) => a == exp2));
                return exp1;
            }
            else
            { throw new Exception("This shouldn't happen"); }
        }

        public static eeObject Multiply(eeObject exp1, eeObject exp2)
        {
            bool isExp1 = false;

            List<eeObject> list;
            eeObject nonlist;

            if (exp1.type == eeObjectType.LIST && exp2.type == eeObjectType.NUMBER)
            {
                isExp1 = true;
                list = (List<eeObject>) exp1.value;
                nonlist = exp2;
            }
            else if (exp1.type == eeObjectType.NUMBER && exp2.type == eeObjectType.LIST)
            {
                list = (List<eeObject>) exp2.value;
                nonlist = exp1;
            }
            else { throw new Exception("This shouldn't happen");  }

            var listCount = list.Count();

            for (eeNumber i = new eeNumber(0); i < nonlist.AsNumber(); i += new eeNumber(1))
            {
                for (int j = 0; j < listCount; j++)
                    list.Add(list[j]);
            }
            return (isExp1 ? exp1 : exp2);
        }
        
        // Returns bool as opposed to a eeBoolObject because the eeObject method also returns a bool directly.
        public static bool GreaterThan(eeObject exp1, eeObject exp2)
        {
            return exp1.AsList().Count > exp2.AsList().Count;
        }
    }
}

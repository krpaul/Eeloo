using Eeloo.Objects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Eeloo.Functions
{
    partial class BuiltInFunctions
    {
        /* Prints given string to command line and accepts one line of input */
        public static eeObject Input(ICollection<eeObject> stringQueries)
        {
            // if multiple queries, iteratively gather inputs
            if (stringQueries.Count > 1)
            {
                ICollection<eeObject> returns = new List<eeObject>();
                foreach (var input in stringQueries)
                {
                    Console.WriteLine(input.ToPrintableString());
                    returns.Add(eeObject.newStringObject(Console.ReadLine()));
                }

                return eeObject.newListObject( // Return a list of the user's inputs
                    new eeObject(returns) { type = eeObjectType.internal_EXPRLIST, }
                );
            }
            else if (stringQueries.Count == 1) // If only one query
            {
                Console.WriteLine(stringQueries.ElementAt(0).ToPrintableString()); // print the query
            }

            // Then, return the user's input string
            return eeObject.newStringObject(Console.ReadLine());
        }
    }
}

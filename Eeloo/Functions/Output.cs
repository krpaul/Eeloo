using Eeloo.Objects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Eeloo.Functions
{
    partial class BuiltInFunctions
    {
        /* Prints all given arguments to command line */
        public static eeObject Output(ICollection<eeObject> exprlist)
        {
            var lineToPrint =
                string.Join(", ",
                    (from node in exprlist select node.ToPrintableString())
                );

            Console.WriteLine(lineToPrint);

            return eeObject.None;
        }
    }
}

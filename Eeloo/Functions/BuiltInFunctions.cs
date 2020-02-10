using System;
using System.Collections.Generic;
using System.Text;

using Eeloo.Evaluator.Exceptions;
using System.Linq;
using Eeloo.Objects;

namespace Eeloo.Functions
{
    // ICollection<eeObject> is an internal_EXPRLIST
    partial class BuiltInFunctions
    {
        public static Dictionary<string, Delegate> functionMap 
            = new Dictionary<string, Delegate>() 
            {
                { "say",  new Func<ICollection<eeObject>, eeObject>(BuiltInFunctions.say)},
            };

        /* Prints all given arguments to command line */
        public static eeObject say(ICollection<eeObject> exprlist)
        {
            var a = exprlist;
            var lineToPrint =
                string.Join(", ", 
                    (from node in exprlist select node.ToPrintableString())
                );

            Console.WriteLine(lineToPrint);

            return eeObject.None;
        }

        /* Prints given string to command line and accepts one line of input */
        public static eeObject ask(eeObject input)
        {
            //if (input.AsString() == null)
            //    throw new ArgumentError("Argument for function \"ask\" must be of type string or similar");

            Console.WriteLine(input.ToPrintableString());

            return eeObject.newStringObject(Console.ReadLine());
        }
    }
}

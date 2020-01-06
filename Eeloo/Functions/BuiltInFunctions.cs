using System;
using System.Collections.Generic;
using System.Text;

using Eeloo.Evaluator.Exceptions;
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

        public static eeObject say(ICollection<eeObject> exprlist)
        {
            foreach (var node in exprlist)
            {
                Console.WriteLine(
                    node.ToPrintableString()
                );
            }

            return eeObject.None;
        }

        public static eeObject ask(eeObject input)
        {
            //if (input.AsString() == null)
            //    throw new ArgumentError("Argument for function \"ask\" must be of type string or similar");

            Console.WriteLine(input.ToPrintableString());
            return eeObject.newStringObject(Console.ReadLine());
        }
    }
}

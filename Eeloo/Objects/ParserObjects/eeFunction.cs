using Eeloo.Grammar;
using Eeloo.Errors;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Eeloo.Objects.ParserObjects
{
    // This class will be the value field in the eeFunctionObject
    public class eeFunction
    {
        public string name;
        public EelooParser.LinesContext codeblock;
        public Scope scope;
        public List<string> argNames 
            = new List<string>();

        public eeFunction(
            string name,
            EelooParser.LinesContext codeblock,
            Dictionary<string, eeObject> defaultArgs
        )
        {
            this.name = name;
            this.codeblock = codeblock;
            this.scope = new Scope(null, codeblock);
            
            // Assign each argument to a variable in the scope
            foreach (var arg in defaultArgs)
            {
                scope.assignVar(
                    arg.Key,  // argument name      
                    arg.Value
                );

                argNames.Add(arg.Key);
            }
        }

        public eeObject invoke(ICollection<eeObject> args)
        {
            // scope to this
            this.scope.parent = Interpreter.currentScope;
            Interpreter.currentScope = this.scope;

            // Assign each positional argument
            if (args != null)
            {
                // Make sure args line up
                long argCount = args.Count(),
                     thisCount = this.argNames.Count();

                if (argCount != thisCount)
                {
                    throw new ArgumentMismatchError(this, thisCount, argCount);
                }

                for (int i = 0; i < args.Count(); i++)
                    scope.assignVar(this.argNames[i], args.ElementAt(i));
            }

            // Execute the function and remember the return value
            var returnVal = Interpreter.visitor.Visit(this.codeblock);

            // unscope
            Interpreter.currentScope = this.scope.parent;

            /* Return the return value
            If Lines return nothing, add an implicit None obj
            Otherwise, return the actual return value */
            return returnVal == null ? eeObject.None : returnVal;
        }


        // returns the name of the arguments for this function
        public string getArgNames()
        {
            string ret = "";
            foreach (string s in this.argNames)
                ret += $"{s}, ";

            return ret.TrimEnd(',', ' ');
        }   
    }
}

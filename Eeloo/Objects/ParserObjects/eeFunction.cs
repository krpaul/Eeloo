using System;
using System.Collections.Generic;
using System.Text;
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
            
            // Create scope
            this.scope = new Scope(Antlr.visitor.scope);
            
            // Assign each argument to a variable in the scope
            foreach (var arg in defaultArgs)
            {
                scope.assignVar(
                    arg.Key,  // argument name      
                    arg.Value // default value
                );

                argNames.Add(arg.Key);
            }
        }

        public eeObject invoke(IEnumerable<eeObject> args)
        {
            // Assign each positional argument
            for (int i = 0; i < args.Count(); i++)
                scope.assignVar(argNames[i], args.ElementAt(i));

            // Reassign the scope while the function is running
            Antlr.visitor.scope = this.scope;

            // Execute the function and remember the return value
            var returnVal = Antlr.visitor.Visit(this.codeblock);//.AsList()[0];
                                    
            // Exit the scope
            Antlr.visitor.scope = Antlr.visitor.scope.parent;

            /* Return the return value
            If Lines return nothing, add an implicit None obj
            Otherwise, return the actual return value */
            return returnVal == null ? eeObject.None : returnVal;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Eeloo.Grammar;
using Eeloo.Objects.ParserObjects;

using Antlr4.Runtime;

namespace Eeloo.Objects
{
    public class Scope
    {
        public Scope parent;
        public ParserRuleContext scopeCtx;

        Dictionary<string, eeObject> scopeVars 
            = new Dictionary<string, eeObject>();

        public Scope(Scope parent, ParserRuleContext ctx)
        {
            this.parent = parent;
            this.scopeCtx = ctx;
        }
        
        // sets current scope to this scope
        public void ScopeThis()
        { Interpreter.currentScope = this; }

        // puts the scope back up to the parent
        public static void unScope(Scope scopeObj)
        {
            if (scopeObj.parent == null)
                throw new Exception("Internal Error: Cannot unscope the top-most scope");

            // set the current scope to the parent
            Interpreter.currentScope = scopeObj.parent;
        }


        public void assignVar(string name, eeObject val)
        {
            // Var doesn't exist
            if (!scopeVars.ContainsKey(name))
            {
                scopeVars.Add(name, val);
            }

            // Var exists; reassign
            else
            {
                scopeVars[name] = val;
            }
        }

        public eeObject resolveVar(string name)
        {
            // If var is in current scope
            if (scopeVars.ContainsKey(name))
            {
                // return it
                return scopeVars[name];
            }
            // Else, check if it is in a higher scope
            else
            {
                // If this is the top level scope and the variable hasn't been found, return null
                if (this.parent == null && !scopeVars.ContainsKey(name))
                    return null;
                else
                    return this.parent.resolveVar(name);
            }
        }
    }
}

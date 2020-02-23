using System;
using System.Collections.Generic;
using System.Text;
using Eeloo.Grammar;

namespace Eeloo.Objects
{
    public class Scope
    {
        public Scope parent;
        Dictionary<string, eeObject> scopeVars 
            = new Dictionary<string, eeObject>();

        public Scope(Scope parent)
        {
            this.parent = parent;
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

﻿using Eeloo.Objects;
using System;
using System.Collections.Generic;


namespace Eeloo.Functions
{
    using FnSignature = Func<ICollection<eeObject>, eeObject>;

    // ICollection<eeObject> is an internal_EXPRLIST
    partial class BuiltInFunctions
    {
        public static readonly Dictionary<List<string>, FnSignature> functionMap 
            = new Dictionary<List<string>, FnSignature>() 
            {
                { new List<string>() { "output", "print", "say"  },  
                    new FnSignature(BuiltInFunctions.Output)},
                { new List<string>() { "input", "query", "ask" }, 
                    new FnSignature(BuiltInFunctions.Input)},
                { new List<string>() { "time", "getTime", "getUnixTime" },
                    new FnSignature(BuiltInFunctions.Time)}
            };

        public static Delegate ResolveFunc(string name)
        {
            foreach (List<string> aliases in functionMap.Keys)
            {
                if (aliases.Contains(name))
                {
                    return functionMap[aliases];
                }
            }
            return null;
        }
    }
}

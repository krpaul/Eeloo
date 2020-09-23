using System;
using System.Collections.Generic;
using System.Text;
using Antlr4.Runtime.Misc;
using Eeloo.Functions;
using Eeloo.Grammar;
using Eeloo.Objects;
using Antlr4;
using Antlr4.Runtime;

namespace Eeloo.Errors
{
    class NoFunctionError : BaseError
    {
        public NoFunctionError(ParserRuleContext error_context, string fnName) 
            : base(error_context, "NoFunctionError", $"Function with name {fnName} does not exist.")
        {}
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Antlr4.Runtime;

namespace Eeloo.Errors
{
    class InvalidOperationError : BaseError
    {
        public InvalidOperationError(ParserRuleContext error_context, string operation, string type)
            : base(error_context, "InvalidOperationError", $"Cannot perform {operation} operation on type {type}.")
        { }
    }
}

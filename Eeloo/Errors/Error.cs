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
    class Error
    {
        public Error(ParserRuleContext error_context)
        {
            string err_msg = $"on line {error_context} ";

        }
    }
}

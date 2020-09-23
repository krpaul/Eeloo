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
    [Serializable()]
    abstract class BaseError : Exception
    {
        protected string ErrorName,
                         Message;

        ParserRuleContext Context;

        public BaseError() : base() {}
        public BaseError(string message) : base(message) { }

        public BaseError(ParserRuleContext error_context, string errName, string message)
            : base($"{errName} occured on line {error_context.Start.Line} column {error_context.Start.Column}: {message}")
        {
            ErrorName = errName;
            Context = error_context;
        }

        public override string ToString()
        { return Message; }
    }
}

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

        public BaseError(string errName, string message)
            : base(
                $"{errName} occured on line {Interpreter.currentScope.scopeCtx.Start.Line} column {Interpreter.currentScope.scopeCtx.Start.Column}: {message}"
                + Environment.NewLine + Environment.NewLine +
                "Function Traceback:"
                + Environment.NewLine
                + Interpreter.globalStack.ToString()
            )
        {
            ErrorName = errName;
            Context = Interpreter.currentScope.scopeCtx;
        }

        public override string ToString()
        { return Message; }
    }
}

using Eeloo.Objects;
using Antlr4.Runtime.Misc;
using System;
using Eeloo.Grammar;

namespace Eeloo.Evaluator
{
    public partial class EvalVisitor : EelooBaseVisitor<eeObject>
    {
        /* Asserts that an expression is true. Otherwise throw an error */
        public override eeObject VisitAssert_stmt([NotNull] EelooParser.Assert_stmtContext ctx)
        {
            // add this to scope
            Interpreter.currentScope.scopeCtx = ctx;

            var bool_exp = Visit(ctx.exp());

            // make sure it's true
            if (bool_exp.DynamicBoolConvert() == eeObject.TRUE)
            {
                throw new Exception($"Line {ctx.start.Line} in {Interpreter.filename}: Assertion statement failed");
            }

            return null;
        }
    }
}

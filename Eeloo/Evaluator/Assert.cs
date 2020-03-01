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
            var bool_exp = Visit(ctx.exp());

            // make sure it's true
            if (!bool_exp.AsBool())
            {
                throw new Exception("Assertion statment failed");
            }

            return null;
        }
    }
}

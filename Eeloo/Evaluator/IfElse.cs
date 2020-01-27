using Eeloo.Objects;
using Antlr4.Runtime.Misc;
using System;

namespace Eeloo.Evaluator
{
    public partial class EvalVisitor : EelooBaseVisitor<eeObject>
    {
        public override eeObject VisitIf_partial([NotNull] EelooParser.If_partialContext ctx)
        {
            if (Visit(ctx.exp()).AsBool())
            {
                return Visit(ctx.lines()) ?? eeObject.None;
            }

            return null;
        }

        public override eeObject VisitElse_if_partial([NotNull] EelooParser.Else_if_partialContext ctx)
        {
            if (Visit(ctx.exp()).AsBool())
            {
                return Visit(ctx.lines()) ?? eeObject.None;
            }

            return null;
        }

        public override eeObject VisitElse_partial([NotNull] EelooParser.Else_partialContext ctx)
        {
            return Visit(ctx.lines()) ?? eeObject.None;
        }

        public override eeObject VisitIf_stmt([NotNull] EelooParser.If_stmtContext ctx)
        {
            // Execute first if statment
            var ifstmt = Visit(ctx.if_partial());

            // Return if nescessary
            if (ifstmt != null)
                return ifstmt;

            // Array of else if blocks
            var elseifstmts = ctx.else_if_partial();
            foreach (var block in elseifstmts) // Execute each block
            {
                var retval = Visit(block);
                if (retval != null) // Return if necessary
                    return retval;
            }

            // Otherwise, execute and return the else statment or null if there isn't one;
            var elsestmt = ctx.else_partial();
            return elsestmt != null ? Visit(elsestmt) : null;
        }
    }
}

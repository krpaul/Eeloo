using Antlr4.Runtime.Misc;
using Eeloo.Grammar;
using Eeloo.Objects;
using System;

namespace Eeloo.Evaluator
{
    public partial class EvalVisitor : EelooBaseVisitor<eeObject>
    {
        public override eeObject VisitIf_partial([NotNull] EelooParser.If_partialContext ctx)
        {
            if (Visit(ctx.exp()).AsBool())
            {
                var scope = new Scope(Interpreter.currentScope, ctx);
                scope.ScopeThis();
                var result = Visit(ctx.lines());
                Scope.unScope(scope);

                if (result != null)
                    return result;
            }

            return null;
        }

        public override eeObject VisitElse_if_partial([NotNull] EelooParser.Else_if_partialContext ctx)
        {
            if (Visit(ctx.exp()).AsBool())
            {
                var scope = new Scope(Interpreter.currentScope, ctx);
                scope.ScopeThis();
                var result = Visit(ctx.lines()) ?? null;
                Scope.unScope(scope);

                if (result != null)
                    return result;
            }

            return null;
        }

        public override eeObject VisitElse_partial([NotNull] EelooParser.Else_partialContext ctx)
        {
            var scope = new Scope(Interpreter.currentScope, ctx);
            scope.ScopeThis();
            var result = Visit(ctx.lines());
            Scope.unScope(scope);

            return result;
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
            if (elsestmt != null)
                return Visit(elsestmt);

            return null;
        }
    }
}

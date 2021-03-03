﻿using Antlr4.Runtime.Misc;
using Eeloo.Grammar;
using Eeloo.Objects;

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
                var result = Visit(ctx.lines()) ?? eeObject.None;
                Scope.unScope(scope);

                return result;
            }

            return eeObject.None;
        }

        public override eeObject VisitElse_if_partial([NotNull] EelooParser.Else_if_partialContext ctx)
        {
            if (Visit(ctx.exp()).AsBool())
            {
                var scope = new Scope(Interpreter.currentScope, ctx);
                scope.ScopeThis();
                var result = Visit(ctx.lines()) ?? eeObject.None;
                Scope.unScope(scope);

                return result;
            }

            return eeObject.None;
        }

        public override eeObject VisitElse_partial([NotNull] EelooParser.Else_partialContext ctx)
        {
            var scope = new Scope(Interpreter.currentScope, ctx);
            scope.ScopeThis();
            var result = Visit(ctx.lines()) ?? eeObject.None;
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
            return elsestmt != null ? Visit(elsestmt) : eeObject.None;
        }
    }
}

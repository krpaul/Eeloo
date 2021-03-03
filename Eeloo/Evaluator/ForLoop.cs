using Antlr4.Runtime.Misc;
using Eeloo.Grammar;
using Eeloo.Objects;
using Eeloo.Errors;
using System;

namespace Eeloo.Evaluator
{
    public partial class EvalVisitor : EelooBaseVisitor<eeObject>
    {
        public override eeObject VisitFor_stmt([NotNull] EelooParser.For_stmtContext ctx)
        {
            var scope = new Scope(Interpreter.currentScope, ctx);
            scope.ScopeThis();

            var iterVar = ctx.var().GetText();
            var enumExp = Visit(ctx.exp()).AsEnumerable();

            if (enumExp == null)
                throw new UniterableValueError();

            foreach (var iteration in enumExp)
            {
                scope.assignVar(iterVar, iteration);
                var codeblock = Visit(ctx.lines());
                if (codeblock != null)
                {
                    Scope.unScope(scope);
                    return codeblock;
                }
            }

            Scope.unScope(scope);
            return eeObject.None;
        }
    }
}

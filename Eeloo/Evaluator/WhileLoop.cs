using Antlr4.Runtime.Misc;
using Eeloo.Grammar;
using Eeloo.Objects;

namespace Eeloo.Evaluator
{
    public partial class EvalVisitor : EelooBaseVisitor<eeObject>
    {
        public override eeObject VisitWhile_stmt([NotNull] EelooParser.While_stmtContext ctx)
        {
            bool until = ctx.UNTIL() != null;

            var scope = new Scope(Interpreter.currentScope, ctx);
            scope.ScopeThis();

            while (
                until == false ? // If this is a while loop
                Visit(ctx.exp()).AsBool() // While condition true
                : !Visit(ctx.exp()).AsBool() // If until loop, until condition false
            )
            {
                var codeblock = Visit(ctx.lines());

                // If there is a return statement inside the loop
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

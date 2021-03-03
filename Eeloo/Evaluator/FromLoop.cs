using Antlr4.Runtime.Misc;
using Eeloo.Grammar;
using Eeloo.Objects;
using Eeloo.Objects.ParserObjects;

namespace Eeloo.Evaluator
{
    public partial class EvalVisitor : EelooBaseVisitor<eeObject>
    {
        public override eeObject VisitFrom_loop([NotNull] EelooParser.From_loopContext ctx)
        {
            var scope = new Scope(Interpreter.currentScope, ctx);
            scope.ScopeThis();

            eeObject start = Visit(ctx.exp(0)),
                     stop  = Visit(ctx.exp(1));

            eeNumber step  = ctx.RANGE_2() != null 
                        ? Visit(ctx.exp(2)).AsNumber()
                        : 1;

            // If range is reversed
            if (stop.IsLessThan(start))
            {
                // Switch start and stop
                var b = start;
                start = stop;
                stop = b;

                // Reverse step
                step *= new eeNumber(1);
            }

            var iterVar = ctx.IDENTIFIER().GetText();

            for (
                eeObject iter = start;
                iter.IsLessThanOrEqualTo(stop);
                iter.value = (iter.AsNumber() + step)
            ) {
                scope.assignVar(iterVar, iter);
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

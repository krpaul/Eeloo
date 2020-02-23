using Antlr4.Runtime.Misc;
using Eeloo.Grammar;
using Eeloo.Objects;

namespace Eeloo.Evaluator
{
    public partial class EvalVisitor : EelooBaseVisitor<eeObject>
    {
        public override eeObject VisitFrom_loop([NotNull] EelooParser.From_loopContext ctx)
        {
            eeObject start = Visit(ctx.exp(0)),
                     stop = Visit(ctx.exp(1));

            long step  = ctx.RANGE_2() != null 
                        ? (long) Visit(ctx.exp(2)).value 
                        : 1;

            // If range is reversed
            if (stop.IsLessThan(start))
            {
                // Switch start and stop
                var b = start;
                start = stop;
                stop = b;

                // Reverse step
                step *= 1;
            }

            var iterVar = ctx.IDENTIFIER().GetText();

            for (
                eeObject iter = start;
                iter.IsLessThanOrEqualTo(stop);
                iter.value = (iter.AsInteger() + step)
            ) {
                scope.assignVar(iterVar, iter);
                var codeblock = Visit(ctx.lines());
                if (codeblock != null)
                    return codeblock;
            }

            return eeObject.None;
        }
    }
}

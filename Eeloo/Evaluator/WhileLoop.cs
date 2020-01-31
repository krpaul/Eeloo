using Eeloo.Objects;
using Antlr4.Runtime.Misc;

namespace Eeloo.Evaluator
{
    public partial class EvalVisitor : EelooBaseVisitor<eeObject>
    {
        public override eeObject VisitWhile_stmt([NotNull] EelooParser.While_stmtContext ctx)
        {
            bool until = ctx.UNTIL() != null;

            while (
                until == false ? // If this is a while loop
                Visit(ctx.exp()).AsBool() // While condition true
                : !Visit(ctx.exp()).AsBool() // If until loop, until condition false
            )
            {
                var codeblock = Visit(ctx.lines());

                // If there is a return statement inside the loop
                if (codeblock != null)
                    return codeblock;
            }

            return eeObject.None;
        }
    }
}

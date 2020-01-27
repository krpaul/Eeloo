using Eeloo.Objects;
using Antlr4.Runtime.Misc;

namespace Eeloo.Evaluator
{
    public partial class EvalVisitor : EelooBaseVisitor<eeObject>
    {
        public override eeObject VisitWhile_stmt([NotNull] EelooParser.While_stmtContext ctx)
        {
            var a = Visit(ctx.exp());
            var b = a.AsBool();
            while (b)
            {
                var codeblock = Visit(ctx.lines());

                // If there is a return statement inside the loop
                if (codeblock != null)
                    return codeblock;
            }

            return null;
        }
    }
}

using Eeloo.Objects;
using Antlr4.Runtime.Misc;
using System;

namespace Eeloo.Evaluator
{
    public partial class EvalVisitor : EelooBaseVisitor<eeObject>
    {
        public override eeObject VisitFor_stmt([NotNull] EelooParser.For_stmtContext ctx)
        {
            var iterVar = ctx.var().GetText();
            var enumExp = Visit(ctx.exp()).AsEnumerable();

            if (enumExp == null)
                throw new Exception("TO DO");

            foreach (var iteration in enumExp)
            {
                scope.assignVar(iterVar, iteration);
                var codeblock = Visit(ctx.lines());
                if (codeblock != null)
                    return codeblock;
            }

            return eeObject.None;
        }
    }
}

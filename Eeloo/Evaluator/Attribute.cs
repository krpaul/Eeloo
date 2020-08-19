using Eeloo.Objects;
using Antlr4.Runtime.Misc;
using System;
using Eeloo.Grammar;

namespace Eeloo.Evaluator
{
    public partial class EvalVisitor : EelooBaseVisitor<eeObject>
    {
        public override eeObject VisitAttributeRefExp([NotNull] EelooParser.AttributeRefExpContext ctx)
        {
            return Visit(ctx.exp()).GetAttribute(ctx.IDENTIFIER().GetText());
        }
    }
}

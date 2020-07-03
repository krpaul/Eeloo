using Eeloo.Objects;
using Antlr4.Runtime.Misc;
using System;
using Eeloo.Grammar;
using Eeloo.Objects.ParserObjects;

namespace Eeloo.Evaluator
{
    public partial class EvalVisitor : EelooBaseVisitor<eeObject>
    {
        public override eeObject VisitNumExp([NotNull] EelooParser.NumExpContext ctx)
        { return Visit(ctx.num()); }

        public override eeObject VisitInt([NotNull] EelooParser.IntContext ctx)
        {
            return eeObject.newNumberObject(new eeNumber(ctx.NUMBER().GetText()));
        }

        public override eeObject VisitDec([NotNull] EelooParser.DecContext ctx)
        {
            return eeObject.newNumberObject(new eeNumber($"{ctx.NUMBER(0).GetText()}.{ctx.NUMBER(1).GetText()}"));
        }
    }
}

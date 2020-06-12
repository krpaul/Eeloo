using Antlr4.Runtime.Misc;
using Eeloo.Grammar;
using Eeloo.Objects;
using Eeloo.Objects.ParserObjects;
using System;

namespace Eeloo.Evaluator
{
    partial class EvalVisitor : EelooBaseVisitor<eeObject>
    {
        public override eeObject VisitFunctionCallExp([NotNull] EelooParser.FunctionCallExpContext ctx)
        { return Visit(ctx.fn_call()); }

        public override eeObject VisitVarExp([NotNull] EelooParser.VarExpContext ctx)
        { return Visit(ctx.var()); }

        public override eeObject VisitStrExp([NotNull] EelooParser.StrExpContext ctx)
        { return Visit(ctx.@string()); }

        public override eeObject VisitListExp([NotNull] EelooParser.ListExpContext ctx)
        { return Visit(ctx.list()); }
    }
}
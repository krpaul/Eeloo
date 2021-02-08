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
        {
            // add this to scope
            Interpreter.currentScope.scopeCtx = ctx;

            bool negate = ctx.MINUS() != null;
            var num = Visit(ctx.num());

            if (negate)
                num = num.Multiply(eeObject.NegOne());
            
            return num; 
        }

        public override eeObject VisitInt([NotNull] EelooParser.IntContext ctx)
        {
            // add this to scope
            Interpreter.currentScope.scopeCtx = ctx;

            return eeObject.newNumberObject(new eeNumber(ctx.NUMBER().GetText()));
        }

        public override eeObject VisitDec([NotNull] EelooParser.DecContext ctx)
        {
            // add this to scope
            Interpreter.currentScope.scopeCtx = ctx;

            return eeObject.newNumberObject(new eeNumber($"{ctx.NUMBER(0).GetText()}.{ctx.NUMBER(1).GetText()}"));
        }
    }
}

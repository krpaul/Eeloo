using Eeloo.Objects;
using Antlr4.Runtime.Misc;
using System;

namespace Eeloo.Evaluator
{
    public partial class EvalVisitor : EelooBaseVisitor<eeObject>
    {
        public override eeObject VisitBoolExp([NotNull] EelooParser.BoolExpContext ctx)
        {
            return Visit(ctx.bool_stmt());
        }

        public override eeObject VisitPrefixedInlineBool([NotNull] EelooParser.PrefixedInlineBoolContext ctx)
        {
            var boolExp = Visit(ctx.exp());

            if (boolExp.type != eeObjectType.BOOL)
                throw new Exception("Expression not a boolean");

            return boolExp;
        }
    }
}

using Antlr4.Runtime.Misc;
using Eeloo.Grammar;
using Eeloo.Objects;

namespace Eeloo.Evaluator
{
    public partial class EvalVisitor : EelooBaseVisitor<eeObject>
    {
        public override eeObject VisitEqualityExp([NotNull] EelooParser.EqualityExpContext ctx)
        {
            // Get both expressions
            eeObject obj1 = Visit(ctx.exp(0)),
                     obj2 = Visit(ctx.exp(1));

            // Return equality operation
            return eeObject.newBoolObject(obj1.IsEqualTo(obj2));
        }

        public override eeObject VisitInequalityExp([NotNull] EelooParser.InequalityExpContext ctx)
        {
            // Get both expressions
            eeObject obj1 = Visit(ctx.exp(0)),
                    obj2 = Visit(ctx.exp(1));

            // Return inequality operation
            return eeObject.newBoolObject(obj1.IsNotEqualTo(obj2));
        }
    }
}

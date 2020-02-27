using Antlr4.Runtime.Misc;
using Eeloo.Grammar;
using Eeloo.Objects;

namespace Eeloo.Evaluator
{
    public partial class EvalVisitor : EelooBaseVisitor<eeObject>
    {
        public override eeObject VisitList([NotNull] EelooParser.ListContext ctx)
        {
            // Get expressions
            var expressions = Visit(ctx.exps());

            // Evaluate them
            var listObj = eeObject.newListObject(expressions);

            // Return the object
            return listObj;
        }
    }
}

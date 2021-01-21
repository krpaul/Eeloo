using Antlr4.Runtime.Misc;
using Eeloo.Grammar;
using Eeloo.Objects;
using System.ComponentModel.DataAnnotations;

namespace Eeloo.Evaluator
{
    public partial class EvalVisitor : EelooBaseVisitor<eeObject>
    {
        public override eeObject VisitList([NotNull] EelooParser.ListContext ctx)
        {
            // add this to scope
            scope.scopeCtx = ctx;

            var e = ctx.exps();
            eeObject listObj;
            if (e != null)
            {
                // Get expressions
                var expressions = Visit(ctx.exps());

                // Evaluate them
                listObj = eeObject.newListObject(expressions);
            }
            else
            {
                listObj = eeObject.newListObject((eeObject) null);
            }

            // Return the object
            return listObj;
        }
    }
}

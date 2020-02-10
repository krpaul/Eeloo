using Antlr4.Runtime.Misc;
using Eeloo.Objects;
using System;

namespace Eeloo.Evaluator
{
    public partial class EvalVisitor : EelooBaseVisitor<eeObject>
    {
        public override eeObject VisitAssignment([NotNull] EelooParser.AssignmentContext ctx)
        {
            // Get value of right hand side
            eeObject assignVal = Visit(ctx.exp());

            // get the var
            eeObject oldVar = Visit(ctx.var());

            // If this var doesn't exist already
            if (oldVar == null)
            {
                // get alternative possibilities
                var a = (ctx.children[0] as EelooParser.VariableContext);
                var b = (ctx.children[0] as EelooParser.ArrayIndexContext);

                // get iden of non null alternative
                string iden = a != null ? a.IDENTIFIER().GetText() : b.IDENTIFIER().GetText();

                scope.assignVar(iden, assignVal);

                return eeObject.None;
            }
            // if this var already exists
            else
            {
                oldVar.OverrideSelf(assignVal);
                return null;
            }
        }
    }
}


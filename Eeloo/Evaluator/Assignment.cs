using Antlr4.Runtime.Misc;
using Eeloo.Functions;
using Eeloo.Objects;
using System;
using System.Collections.Generic;

namespace Eeloo.Evaluator
{
    public partial class EvalVisitor : EelooBaseVisitor<eeObject>
    {
        public override eeObject VisitAssignment([NotNull] EelooParser.AssignmentContext ctx)
        {
            // Get value of right hand side
            eeObject assignVal = Visit(ctx.exp());

            // Assign it to the current scope
            scope.assignVar(ctx.IDENTIFIER().GetText(), assignVal);

            // TO DO: Maybe this statement should return the value of the variable?
            return null;
        }
    }
}

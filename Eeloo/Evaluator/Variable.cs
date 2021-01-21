using Antlr4.Runtime.Misc;
using Eeloo.Grammar;
using Eeloo.Objects;
using Eeloo.Objects.ParserObjects;
using System;
using System.Collections.Generic;

namespace Eeloo.Evaluator
{
    public partial class EvalVisitor : EelooBaseVisitor<eeObject>
    {
        public override eeObject VisitVariable([NotNull] EelooParser.VariableContext ctx)
        {
            // add this to scope
            scope.scopeCtx = ctx;

            // Get the name of the variable
            string iden = ctx.IDENTIFIER().GetText();

            // Get the value
            var val = scope.resolveVar(iden);

            // return the value
            return val;
        }

    }
}

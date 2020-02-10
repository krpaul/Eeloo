using System;
using System.Collections.Generic;
using System.Text;

using Eeloo.Objects;
using Antlr4.Runtime.Misc;

namespace Eeloo.Evaluator
{
    /* This evaluator goes down the parse tree before the main evaluator in 
     * order to find all function definitions and store them into eeFunction
     * objects, that way allowing functions to be called anywhere in the code
     * regardless of where they're defined 
     */
    class FunctionEvaluator : EelooBaseVisitor<eeObject>
    {
        public Scope scope; // This func's scope

        public FunctionEvaluator(Scope scope)
        { this.scope = scope; }

        public override eeObject VisitFn_def([NotNull] EelooParser.Fn_defContext ctx)
        {
            scope.assignVar(
                ctx.IDENTIFIER().GetText(),
                eeObject.newFunctionObject(
                    ctx.IDENTIFIER().GetText(),
                    (Dictionary<string, eeObject>)Antlr.visitor.Visit(ctx.fn_args()).value,
                    ctx.lines()
                )
            );
            return null;
        }
    }
}

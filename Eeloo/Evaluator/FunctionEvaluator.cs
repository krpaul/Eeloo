using System;
using System.Collections.Generic;
using System.Text;

using Eeloo.Objects;
using Antlr4.Runtime.Misc;

namespace Eeloo
{
    class FunctionEvaluator : EelooBaseVisitor<eeObject>
    {
        public Scope scope;
        public FunctionEvaluator(Scope scope)
        {
            this.scope = scope;
        }

        public override eeObject VisitFn_def([NotNull] EelooParser.Fn_defContext ctx)
        {
            Console.WriteLine("this should not execute");
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

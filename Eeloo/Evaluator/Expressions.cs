using Antlr4.Runtime.Misc;
using Eeloo.Grammar;
using Eeloo.Objects;
using System.Collections.Generic;

namespace Eeloo.Evaluator
{
    partial class EvalVisitor : EelooBaseVisitor<eeObject>
    {
        public override eeObject VisitPlainExps([NotNull] EelooParser.PlainExpsContext ctx)
        {
            // add this to scope
            Interpreter.currentScope.scopeCtx = ctx;

            ICollection<eeObject> rawListObj = new List<eeObject>();

            foreach (var exp in ctx.exp())
                rawListObj.Add(Visit(exp));

            eeObject newListObj = new eeObject(rawListObj)
            {
                type = eeObjectType.internal_EXPRLIST,
            };

            return newListObj;
        }

        public override eeObject VisitBrackExps([NotNull] EelooParser.BrackExpsContext ctx)
        {
            // add this to scope
            Interpreter.currentScope.scopeCtx = ctx;

            return Visit(ctx.exps()); 
        }
    }
}
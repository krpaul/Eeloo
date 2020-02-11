using System;
using System.Collections.Generic;
using System.Text;
using Antlr4.Runtime.Misc;
using Eeloo.Objects;

namespace Eeloo.Evaluator
{
    partial class EvalVisitor : EelooBaseVisitor<eeObject>
    {
        public override eeObject VisitPlainExps([NotNull] EelooParser.PlainExpsContext ctx)
        {
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
        { return Visit(ctx.exps()); }
    }
}
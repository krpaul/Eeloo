using Eeloo.Objects;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using Eeloo.Grammar;

namespace Eeloo.Evaluator
{
    public partial class EvalVisitor : EelooBaseVisitor<eeObject>
    {
        //// find the first loop in the parent syntax tree. returns null if not found.
        //private ParserRuleContext findLoop(EelooParser.Continue_stmtContext ctx)
        //{
        //    dynamic parent = ctx.parent;
        //    while (allowedContexts.FindIndex(parent.GetType()) == -1)
        //        parent = parent.parent;

        //    return parent;
        //}

        //private static readonly List<Type> allowedContexts = new List<Type>()
        //{
        //    typeof(EelooParser.For_stmtContext),
        //    typeof(EelooParser.While_stmtContext),
        //    typeof(EelooParser.Repeat_loopContext),
        //    typeof(EelooParser.From_loopContext)
        //};
        
        public override eeObject VisitContinue_stmt([NotNull] EelooParser.Continue_stmtContext ctx)
        {
            return new eeObject() { type = eeObjectType.internal_CONTINUE_STMT };
        }
    }
}

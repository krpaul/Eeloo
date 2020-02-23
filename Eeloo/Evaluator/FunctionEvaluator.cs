﻿using Antlr4.Runtime.Misc;
using Eeloo.Grammar;
using Eeloo.Objects;
using System.Collections.Generic;

namespace Eeloo.Evaluator
{
    using ArgList = Dictionary<string, eeObject>;

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
            ArgList args = ctx.fn_args() != null ?
                (ArgList)Interpreter.visitor.Visit(ctx.fn_args()).value : 
                new ArgList()
                ;

            scope.assignVar(
                ctx.IDENTIFIER().GetText(),
                eeObject.newFunctionObject(
                    ctx.IDENTIFIER().GetText(),
                    args,
                    ctx.lines()
                )
            );

            return null;
        }
    }
}

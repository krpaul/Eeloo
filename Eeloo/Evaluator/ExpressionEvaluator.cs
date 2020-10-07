﻿using Antlr4.Runtime.Misc;
using Eeloo.Grammar;
using Eeloo.Objects;
using Eeloo.Objects.ParserObjects;
using System;
using Eeloo.Errors;

namespace Eeloo.Evaluator
{
    partial class EvalVisitor : EelooBaseVisitor<eeObject>
    {
        public override eeObject VisitFunctionCallExp([NotNull] EelooParser.FunctionCallExpContext ctx)
        { 
            bool negate = ctx.MINUS() != null;
            var exp = Visit(ctx.fn_call());

            if (negate && exp.type == eeObjectType.NUMBER)
                exp = exp.Multiply(ctx, eeObject.NegOne());
            else if (negate && exp.type != eeObjectType.NUMBER)
                throw new NegationError(ctx, eeObjectType.NUMBER, true);

            return exp;
        }

        public override eeObject VisitVarExp([NotNull] EelooParser.VarExpContext ctx)
        {
            bool negate = ctx.MINUS() != null;
            var exp = Visit(ctx.var());

            if (negate && exp.type == eeObjectType.NUMBER)
                exp = exp.Multiply(ctx, eeObject.NegOne());
            else if (negate && exp.type != eeObjectType.NUMBER)
                throw new NegationError(ctx, eeObjectType.NUMBER, true);

            return exp;
        }

        public override eeObject VisitStrExp([NotNull] EelooParser.StrExpContext ctx)
        { return Visit(ctx.@string()); }

        public override eeObject VisitListExp([NotNull] EelooParser.ListExpContext ctx)
        { return Visit(ctx.list()); }
    }
}
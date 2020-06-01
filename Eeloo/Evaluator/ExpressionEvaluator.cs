﻿using Antlr4.Runtime.Misc;
using Eeloo.Grammar;
using Eeloo.Objects;
using Eeloo.Objects.ParserObjects;
using System;

namespace Eeloo.Evaluator
{
    partial class EvalVisitor : EelooBaseVisitor<eeObject>
    {
        public override eeObject VisitAdditiveOprExp([NotNull] EelooParser.AdditiveOprExpContext ctx)
        {
            eeObject exp1 = Visit(ctx.exp(0)),
                     exp2 = Visit(ctx.exp(1));

            switch (ctx.opr.Type)
            {
                case EelooLexer.PLUS:
                    return eeObject.newNumberObject(exp1.AsNumber() + exp2.AsNumber());
                case EelooLexer.MINUS:
                    return eeObject.newNumberObject(exp1.AsNumber() - exp2.AsNumber());
                default:
                    throw new Exception($"Invalid operation: {ctx.opr.Text}");
            }
        }

        public override eeObject VisitMultiplicativeOprExp([NotNull] EelooParser.MultiplicativeOprExpContext ctx)
        {
            eeObject exp1 = Visit(ctx.exp(0)),
                     exp2 = Visit(ctx.exp(1));

            switch (ctx.opr.Type)
            {
                case EelooLexer.MULTIPLY:
                    return eeObject.newNumberObject(exp1.AsNumber() * exp2.AsNumber());
                case EelooLexer.DIVIDE:
                    return eeObject.newNumberObject(exp1.AsNumber() / exp2.AsNumber());
                case EelooLexer.MOD:
                    return eeObject.newNumberObject(exp1.AsNumber() % exp2.AsNumber());
                default:
                    throw new Exception($"Invalid operation: {ctx.opr.Text}");
            }
        }

        public override eeObject VisitComparisonExp([NotNull] EelooParser.ComparisonExpContext ctx)
        {
            eeObject obj1 = Visit(ctx.exp(0)),
                     obj2 = Visit(ctx.exp(1));

            switch (ctx.opr.Type)
            {
                case EelooLexer.LESS_EQL:
                    return eeObject.newBoolObject(obj1.IsLessThanOrEqualTo(obj2));
                case EelooLexer.GRT_EQL:
                    return eeObject.newBoolObject(obj1.IsGreaterThanOrEqualTo(obj2));
                case EelooLexer.LESS:
                    return eeObject.newBoolObject(obj1.IsLessThan(obj2));
                case EelooLexer.GRT:
                    return eeObject.newBoolObject(obj1.IsGreaterThan(obj2));
                default:
                    throw new Exception("default case for VisitComparisonExp");
            }
        }

        public override eeObject VisitPwrExp([NotNull] EelooParser.PwrExpContext ctx)
        {
            eeObject exp1 = Visit(ctx.exp(0)),
                     exp2 = Visit(ctx.exp(1));

            double result = exp1.AsNumber() ^ exp2.AsNumber();

            if (result % 1 == 0) // If is int, cast result
                return eeObject.newNumberObject((int)result);
            else // If is decimal, keep it as a double
                return eeObject.newNumberObject(result);
        }

        public override eeObject VisitBracketedExp([NotNull] EelooParser.BracketedExpContext ctx)
        {
            return Visit(ctx.exp());
        }

        public override eeObject VisitAndExp([NotNull] EelooParser.AndExpContext ctx)
        {
            eeObject obj1 = Visit(ctx.exp(0)),
                     obj2 = Visit(ctx.exp(1));

            // If we are concatenating strings
            if (obj1.type == eeObjectType.STRING && obj2.type == eeObjectType.STRING)
                return eeObject.newStringObject(obj1.AsString() + obj2.AsString());

            bool? exp1 = Visit(ctx.exp(0)).AsBool(),
                  exp2 = Visit(ctx.exp(1)).AsBool();

            if (exp1 == null || exp2 == null)
                throw new Exception("TO DO");

            return eeObject.newBoolObject((bool)exp1 && (bool)exp2);
        }

        public override eeObject VisitOrExp([NotNull] EelooParser.OrExpContext ctx)
        {
            bool? exp1 = Visit(ctx.exp(0)).AsBool(),
                  exp2 = Visit(ctx.exp(1)).AsBool();

            if (exp1 == null || exp2 == null)
                throw new Exception("TO DO");

            return eeObject.newBoolObject((bool)exp1 || (bool)exp2);
        }

        public override eeObject VisitFunctionCallExp([NotNull] EelooParser.FunctionCallExpContext ctx)
        { return Visit(ctx.fn_call()); }

        public override eeObject VisitNegationExp([NotNull] EelooParser.NegationExpContext ctx)
        { return eeObject.newNumberObject(Visit(ctx.exp()).AsNumber() * eeNumber.NEG_ONE); }

        public override eeObject VisitVarExp([NotNull] EelooParser.VarExpContext ctx)
        { return Visit(ctx.var()); }

        public override eeObject VisitStrExp([NotNull] EelooParser.StrExpContext ctx)
        { return Visit(ctx.@string()); }

        public override eeObject VisitListExp([NotNull] EelooParser.ListExpContext ctx)
        { return Visit(ctx.list()); }
    }
}
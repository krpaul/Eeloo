﻿using Antlr4.Runtime.Misc;
using Eeloo.Errors;
using Eeloo.Grammar;
using Eeloo.Objects;
using Eeloo.Objects.ParserObjects;
using System;

namespace Eeloo.Evaluator
{
    partial class EvalVisitor : EelooBaseVisitor<eeObject>
    {
        /* This function checks if the requested operation involves strings. 
         * If so, the handlers in StringMath.cs will take the operation and
         * this function will return true.
        */
        public override eeObject VisitAdditiveOprExp([NotNull] EelooParser.AdditiveOprExpContext ctx)
        {
            // add this to scope
            Interpreter.currentScope.scopeCtx = ctx;

            eeObject exp1 = Visit(ctx.exp(0)),
                     exp2 = Visit(ctx.exp(1));

            switch (ctx.opr.Type)
            {
                case EelooLexer.PLUS:
                    return exp1.Add(exp2);
                case EelooLexer.MINUS:
                    return exp1.Subtract(exp2);
                default:
                    throw new Exception($"Invalid operation: {ctx.opr.Text}");
            }
        }

        public override eeObject VisitMultiplicativeOprExp([NotNull] EelooParser.MultiplicativeOprExpContext ctx)
        {
            // add this to scope
            Interpreter.currentScope.scopeCtx = ctx;

            eeObject exp1 = Visit(ctx.exp(0)),
                     exp2 = Visit(ctx.exp(1));

            switch (ctx.opr.Type)
            {
                case EelooLexer.MULTIPLY:
                    return exp1.Multiply(exp2);
                case EelooLexer.DIVIDE:
                    return exp1.Divide(exp2);
                case EelooLexer.MOD:
                    return eeObject.newNumberObject(exp1.AsNumber() % exp2.AsNumber());
                default:
                    throw new Exception($"Invalid operation: {ctx.opr.Text}");
            }
        }

        public override eeObject VisitComparisonExp([NotNull] EelooParser.ComparisonExpContext ctx)
        {
            // add this to scope
            Interpreter.currentScope.scopeCtx = ctx;

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
            // add this to scope
            Interpreter.currentScope.scopeCtx = ctx;

            eeObject exp1 = Visit(ctx.exp(0)),
                     exp2 = Visit(ctx.exp(1));

            return eeObject.newNumberObject(
                eeNumber.Power(exp1.AsNumber(), exp2.AsNumber())
            );
        }

        public override eeObject VisitBracketedExp([NotNull] EelooParser.BracketedExpContext ctx)
        {
            // add this to scope
            Interpreter.currentScope.scopeCtx = ctx;

            return Visit(ctx.exp()); 
        }

        public override eeObject VisitFactorialExp([NotNull] EelooParser.FactorialExpContext ctx)
        {
            // add this to scope
            Interpreter.currentScope.scopeCtx = ctx;

            var num = Visit(ctx.exp());

            if (num.type != eeObjectType.NUMBER)
                throw new Exception();

            eeNumber fact = num.AsNumber().Factorial();

            return eeObject.newNumberObject(fact);
        }

        public override eeObject VisitSinglePwrExp([NotNull] EelooParser.SinglePwrExpContext ctx)
        {
            // add this to scope
            Interpreter.currentScope.scopeCtx = ctx;

            var num = Visit(ctx.exp());

            if (num.type != eeObjectType.NUMBER)
                throw new InvalidOperationError(ctx.opr.Text, num.type);

            switch (ctx.opr.Type)
            {
                case EelooLexer.SQUARED:
                    return eeObject.newNumberObject(eeNumber.Power(num.AsNumber(), eeNumber.TWO));
                case EelooLexer.CUBED:
                    return eeObject.newNumberObject(eeNumber.Power(num.AsNumber(), eeNumber.THREE));
                default:
                    throw new InternalError($"SinglePwrExp got unknown value {ctx.opr.Text}");
            }
        }
    }
}
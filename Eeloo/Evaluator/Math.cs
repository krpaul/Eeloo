using Antlr4.Runtime.Misc;
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

            return eeObject.newNumberObject(
                eeNumber.Power(exp1.AsNumber(), exp2.AsNumber())
            );
        }

        public override eeObject VisitBracketedExp([NotNull] EelooParser.BracketedExpContext ctx)
        { return Visit(ctx.exp()); }

        public override eeObject VisitNegationExp([NotNull] EelooParser.NegationExpContext ctx)
        { return eeObject.newNumberObject(Visit(ctx.exp()).AsNumber() * eeNumber.NEG_ONE); }
    }
}
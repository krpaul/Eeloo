using Antlr4.Runtime.Misc;
using Eeloo.Grammar;
using Eeloo.Objects;
using Eeloo.Objects.ParserObjects;
using System;
using Eeloo.Errors;

namespace Eeloo.Evaluator
{
    partial class EvalVisitor : EelooBaseVisitor<eeObject>
    {
        public override eeObject VisitAndExp([NotNull] EelooParser.AndExpContext ctx)
        {
            // add this to scope
            scope.scopeCtx = ctx;

            eeObject obj1 = Visit(ctx.exp(0)),
                     obj2 = Visit(ctx.exp(1));

            // If we are concatenating strings
            if (obj1.type == eeObjectType.STRING && obj2.type == eeObjectType.STRING)
                return eeObject.newStringObject(obj1.AsString() + obj2.AsString());

            bool? exp1 = Visit(ctx.exp(0)).AsBool(),
                  exp2 = Visit(ctx.exp(1)).AsBool();

            if (exp1 == null || exp2 == null)
                throw new Exception("TO DO");

            return eeObject.newBoolObject((bool) exp1 && (bool) exp2);
        }

        public override eeObject VisitOrExp([NotNull] EelooParser.OrExpContext ctx)
        {
            // add this to scope
            scope.scopeCtx = ctx;

            bool? exp1 = Visit(ctx.exp(0)).AsBool(),
                  exp2 = Visit(ctx.exp(1)).AsBool();

            if (exp1 == null || exp2 == null)
                throw new InvalidOperationError("or", eeObjectType.BOOL);
            
            return eeObject.newBoolObject((bool) exp1 || (bool) exp2);
        }

        public override eeObject VisitNotExp([NotNull] EelooParser.NotExpContext ctx)
        {
            // add this to scope
            scope.scopeCtx = ctx;

            return eeObject.newBoolObject(!Visit(ctx.exp()).AsBool());
        }
    }
}
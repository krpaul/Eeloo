using Antlr4.Runtime.Misc;
using Eeloo.Grammar;
using Eeloo.Objects;
using System.Collections.Generic;
using System;

namespace Eeloo.Evaluator
{
    public partial class EvalVisitor : EelooBaseVisitor<eeObject>
    {
        public override eeObject VisitAttributeRefExp([NotNull] EelooParser.AttributeRefExpContext ctx)
        {
            // add this to scope
            Interpreter.currentScope.scopeCtx = ctx;

            return Visit(ctx.exp()).GetAttribute(ctx.IDENTIFIER().GetText());
        }

        public override eeObject VisitVerboseAttributeExp([NotNull] EelooParser.VerboseAttributeExpContext ctx)
        {
            // add this to scope
            Interpreter.currentScope.scopeCtx = ctx;

            string iden = ctx.IDENTIFIER().GetText();
            eeObject obj = Visit(ctx.exp());

            if (obj.attributes.ContainsKey(iden))
            {
                return obj.attributes[iden];
            }
            // A method can be called without parentheses (e.g. `length of x`), however the method will be passed 0 parameters (method may error if it requires >0)
            else if (obj.methods.ContainsKey(iden))
            {
                return obj.CallMethod(iden,
                    new eeObject(new List<eeObject>())
                    { type=eeObjectType.internal_EXPRLIST }
                );
            }
            else
            {
                throw new Exception($"No attribute or method name {iden} exists");
            }
        }
    }
}

using Eeloo.Objects;
using Antlr4.Runtime.Misc;
using System;
using Eeloo.Grammar;

namespace Eeloo.Evaluator
{
    public partial class EvalVisitor : EelooBaseVisitor<eeObject>
    {
        public override eeObject VisitTypecastExpression([NotNull] EelooParser.TypecastExpressionContext ctx)
        {
            var obj = Visit(ctx.exp());
            if (ctx.STRING_TOK() != null)
            {
                return eeObject.newStringObject(obj.value.ToString());
            }
            else if (ctx.NUMBER_TOK() != null)
            {
                return obj.DynamicNumConvert();
            }
            else if (ctx.BOOL_TOK() != null)
            {
                return eeObject.newBoolObject(obj.AsBool());
            }
            else if (ctx.LIST_TOK() != null)
            {
                return eeObject.newListObject(obj.AsList());
            }
            else throw new Exception("Typecast error");
        }
    }
}
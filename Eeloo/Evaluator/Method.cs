using Antlr4.Runtime.Misc;
using Eeloo.Objects;
using System;

namespace Eeloo.Evaluator
{
    public partial class EvalVisitor : EelooBaseVisitor<eeObject>
    {
        public override eeObject VisitMethod_call([NotNull] EelooParser.Method_callContext ctx)
        {
            var hostObj = Visit(ctx.exp());
            string methodName = ctx.fn_call().IDENTIFIER().GetText();
            var methodParams = Visit(ctx.fn_call().exps());

            if (!hostObj.methods.ContainsKey(methodName))
                throw new Exception($"Method {methodName} does not exist");

            return hostObj.CallMethod(methodName, methodParams);
        }
    }
}

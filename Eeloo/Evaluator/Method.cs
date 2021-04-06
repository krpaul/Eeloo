using Antlr4.Runtime.Misc;
using Eeloo.Grammar;
using Eeloo.Objects;
using Eeloo.Methods;
using Eeloo.Errors;
using System;

namespace Eeloo.Evaluator
{
    public partial class EvalVisitor : EelooBaseVisitor<eeObject>
    {
        //public override eeObject VisitMethod_call([NotNull] EelooParser.Method_callContext ctx)
        //{
        //    var hostObj = Visit(ctx.exp());
        //    string methodName = ctx.fn_call().IDENTIFIER().GetText();
        //    var methodParams = Visit(ctx.fn_call().exps());

        //    if (!hostObj.methods.ContainsKey(methodName))
        //        throw new Exception($"Method {methodName} does not exist");

        //    return hostObj.CallMethod(methodName, methodParams);
        //}

        //public override eeObject VisitMethd

        //public override eeObject VisitMethodCallExp([NotNull] EelooParser.MethodCallExpContext ctx)
        //{
        //    // add this to scope
        //    Interpreter.currentScope.scopeCtx = ctx;

        //    var hostObj = Visit(ctx.exp());
        //    string methodName = ctx.fn_call().IDENTIFIER().GetText();

        //    var passedParams = ctx.fn_call().exps();
        //    var methodParams = passedParams != null ? Visit(passedParams) : null;

        //    if (!hostObj.methods.ContainsKey(methodName))
        //        throw new NoMethodError(methodName, hostObj.type);

        //    return hostObj.CallMethod(methodName, methodParams);
        //}

        public override eeObject VisitMethod_standardSyntax([NotNull] EelooParser.Method_standardSyntaxContext ctx)
        {
            // add this to scope
            Interpreter.currentScope.scopeCtx = ctx;

            eeObject obj = Visit(ctx.exp());
            string alias = ctx.fn_call().IDENTIFIER().GetText();

            var passedParams = ctx.fn_call().exps();
            var methodParams = passedParams != null ? Visit(passedParams) : null;

            Method m = Method.Find()
        }

        public override eeObject VisitMethod_expandedSyntax([NotNull] EelooParser.Method_expandedSyntaxContext ctx)
        {
            // add this to scope
            Interpreter.currentScope.scopeCtx = ctx;

        }

        public override eeObject VisitMethod_expandedSyntaxNoBrackets([NotNull] EelooParser.Method_expandedSyntaxNoBracketsContext ctx)
        {
            // add this to scope
            Interpreter.currentScope.scopeCtx = ctx;

        }
    }
}

using Antlr4.Runtime.Misc;
using Eeloo.Grammar;
using Eeloo.Objects;

namespace Eeloo.Evaluator
{
    partial class EvalVisitor : EelooBaseVisitor<eeObject>
    {
        //public override eeObject VisitReturn_stmt([NotNull] EelooParser.Return_stmtContext ctx)
        //{
        //    // get the object
        //    var obj = Visit(ctx.exps());

        //    // wrap it in a return object
        //    eeObject ret = new eeObject(obj) { type = eeObjectType.internal_RETURN_VALUE };

        //    return ret;
        //}

        // when passed one expressions
        public override eeObject VisitExpReturn([NotNull] EelooParser.ExpReturnContext ctx)
        {
            // add this to scope
            scope.scopeCtx = ctx;

            // get the object
            var obj = Visit(ctx.exp());

            // wrap it in a return object
            eeObject ret = new eeObject(obj) { type = eeObjectType.internal_RETURN_VALUE };

            return ret;
        }

        // When passed a list of expressions
        public override eeObject VisitMultiExpReturn([NotNull] EelooParser.MultiExpReturnContext ctx)
        {
            // add this to scope
            scope.scopeCtx = ctx;

            // get the expression list
            var exprs = Visit(ctx.exps());

            // make it a list
            var obj = eeObject.newListObject(exprs);

            // wrap it in a return object
            eeObject ret = new eeObject(obj) { type = eeObjectType.internal_RETURN_VALUE };

            return ret;
        }
    }
}

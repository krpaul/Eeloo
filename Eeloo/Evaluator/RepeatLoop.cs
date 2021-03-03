using Antlr4.Runtime.Misc;
using Eeloo.Grammar;
using Eeloo.Objects;
using Eeloo.Objects.ParserObjects;
using System;

namespace Eeloo.Evaluator
{
    public partial class EvalVisitor : EelooBaseVisitor<eeObject>
    {
        public override eeObject VisitRepeat_loop([NotNull] EelooParser.Repeat_loopContext ctx)
        {
            var count = Visit(ctx.exp());
            if (count.type != eeObjectType.NUMBER || count.AsNumber() < eeNumber.ONE)
                throw new Exception("Cannot repeat a non-positive integer number of times.");


            var scope = new Scope(Interpreter.currentScope, ctx);
            scope.ScopeThis();
            for (eeNumber i = new eeNumber(0); i < count.AsNumber(); i += eeNumber.ONE)
            {
                var codeblock = Visit(ctx.lines());

                // If there is a return statement inside the loop
                if (codeblock != null)
                {
                    Scope.unScope(scope);
                    return codeblock;
                }
            }

            Scope.unScope(scope);
            return eeObject.None;
        }
    }
}

using Eeloo.Objects;
using Antlr4.Runtime.Misc;
using System;

namespace Eeloo.Evaluator
{
    public partial class EvalVisitor : EelooBaseVisitor<eeObject>
    {
        public override eeObject VisitRepeat_loop([NotNull] EelooParser.Repeat_loopContext ctx)
        {
            var count = Visit(ctx.exp());
            if (count.type != eeObjectType.NUMBER || count.AsNumber() < 1)
                throw new Exception("Cannot repeat a non-positive integer number of times.");

            for (long i = 0; i < count.AsNumber(); i++)
            {
                var codeblock = Visit(ctx.lines());

                // If there is a return statement inside the loop
                if (codeblock != null)
                    return codeblock;
            }

            return null;
        }
    }
}

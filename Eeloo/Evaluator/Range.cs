using Antlr4.Runtime.Misc;
using Eeloo.Functions;
using Eeloo.Objects;
using System;
using System.Collections.Generic;

namespace Eeloo.Evaluator
{
    partial class EvalVisitor : EelooBaseVisitor<eeObject>
    {
        private ICollection<eeObject> getRange(long start, long stop, long step=1)
        {
            ICollection<eeObject> rangeObj = new List<eeObject>();

            if (start < stop)
            {
                for (long i = start; i <= stop; i += step)
                    rangeObj.Add(eeObject.newNumberObject(i));
            }
            else if (stop < start)
            {
                for (long i = start; i >= stop; i -= step)
                    rangeObj.Add(eeObject.newNumberObject(i));
            }
            else
                rangeObj.Add(eeObject.newNumberObject(start));

            return rangeObj;
        }

        public override eeObject VisitRangeExp([NotNull] EelooParser.RangeExpContext ctx)
        {
            eeObject exp1 = Visit(ctx.exp(0)),
                     exp2 = Visit(ctx.exp(1));


            if (exp1.AsNumber() == null || exp2.AsNumber() == null)
                throw new Exception("TO DO");

            long start = exp1.AsNumber(),
                 stop = exp2.AsNumber();

            ICollection<eeObject> rangeObj = getRange(start, stop);

            eeObject exprList = new eeObject(rangeObj)
            { type = eeObjectType.internal_EXPRLIST };

            return eeObject.newListObject(exprList);
        }

        public override eeObject VisitRangeExtendedExp([NotNull] EelooParser.RangeExtendedExpContext ctx)
        {
            eeObject exp1 = Visit(ctx.exp(0)),
                     exp2 = Visit(ctx.exp(1)),
                     exp3 = Visit(ctx.exp(2));

            if (exp1.AsNumber() == null || exp2.AsNumber() == null || exp3.AsNumber() == null)
                throw new Exception("TO DO");

            long start = exp1.AsNumber(),
                 stop = exp2.AsNumber(),
                 step = exp3.AsNumber();

            if (step > Math.Abs(start - stop))
                throw new Exception("TO DO");

            ICollection<eeObject> rangeObj = getRange(start, stop, step);

            eeObject exprList = new eeObject(rangeObj)
            { type = eeObjectType.internal_EXPRLIST };

            return eeObject.newListObject(exprList);
        }
    }
}

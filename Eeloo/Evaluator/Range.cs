using Antlr4.Runtime.Misc;
using Eeloo.Grammar;
using Eeloo.Objects;
using Eeloo.Objects.ParserObjects;
using System;
using System.Collections.Generic;

namespace Eeloo.Evaluator
{
    partial class EvalVisitor : EelooBaseVisitor<eeObject>
    {
        private ICollection<eeObject> getRange(eeNumber start, eeNumber stop, eeNumber step)
        {
            ICollection<eeObject> rangeObj = new List<eeObject>();

            if (start < stop)
            {
                for (eeNumber i = start; i <= stop; i += step)
                    rangeObj.Add(eeObject.newNumberObject(i.Copy()));
            }
            else if (stop < start)
            {
                for (eeNumber i = start; i >= stop; i -= step)
                    rangeObj.Add(eeObject.newNumberObject(i.Copy()));
            }
            else
                rangeObj.Add(eeObject.newNumberObject(start.Copy()));

            return rangeObj;
        }

        
        public override eeObject VisitRangeExp([NotNull] EelooParser.RangeExpContext ctx)
        {
            EelooParser.ExpContext[] exps = ctx.exp();
            
            eeObject exp1 = Visit(exps[0]),
                     exp2 = Visit(exps[1]);

            if (exp1.AsNumber() == null || exp2.AsNumber() == null)
                throw new Exception("TO DO");

            eeNumber start = exp1.AsNumber(),
                     stop = exp2.AsNumber();
            
            ICollection<eeObject> rangeObj;
            if (exps.Length == 2)
                rangeObj = getRange(start, stop, eeNumber.ONE);
            else
                rangeObj = getRange(start, stop, Visit(exps[2]).AsNumber());

            eeObject exprList = new eeObject(rangeObj)
            { type = eeObjectType.internal_EXPRLIST };

            return eeObject.newListObject(exprList);
        }
    }
}

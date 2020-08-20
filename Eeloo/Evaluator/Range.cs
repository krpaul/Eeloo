﻿using Antlr4.Runtime.Misc;
using Eeloo.Grammar;
using Eeloo.Objects;
using System;
using System.Collections.Generic;
using Eeloo.Objects.ParserObjects;
using Microsoft.Win32.SafeHandles;

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
            eeObject exp1 = Visit(ctx.exp(0)),
                     exp2 = Visit(ctx.exp(1));


            if (exp1.AsNumber() == null || exp2.AsNumber() == null)
                throw new Exception("TO DO");

            eeNumber start = exp1.AsNumber(),
                     stop = exp2.AsNumber();

            ICollection<eeObject> rangeObj = getRange(start, stop, eeNumber.ONE);

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

            eeNumber start = exp1.AsNumber(),
                     stop  = exp2.AsNumber(),
                     step  = exp3.AsNumber();

            if (step > eeNumber.AsboluteValue(start - stop))
                throw new Exception("TO DO");

            ICollection<eeObject> rangeObj = getRange(start, stop, step);

            eeObject exprList = new eeObject(rangeObj)
            { type = eeObjectType.internal_EXPRLIST };

            return eeObject.newListObject(exprList);
        }
    }
}

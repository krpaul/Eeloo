using Antlr4.Runtime.Misc;
using Eeloo.Grammar;
using Eeloo.Objects;
using Eeloo.Objects.ParserObjects;
using System;
using System.Collections.Generic;
using Eeloo.Helpers;

namespace Eeloo.Evaluator
{
    partial class EvalVisitor : EelooBaseVisitor<eeObject>
    {
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
            if (exps.Length == 2) // only two numbers provided, assume step is 1
                rangeObj = RangeGenerator.Generate(start, stop, eeNumber.ONE);
            else
                rangeObj = RangeGenerator.Generate(start, stop, Visit(exps[2]).AsNumber());

            eeObject exprList = new eeObject(rangeObj)
            { type = eeObjectType.internal_EXPRLIST };

            return eeObject.newListObject(exprList);
        }
    }
}

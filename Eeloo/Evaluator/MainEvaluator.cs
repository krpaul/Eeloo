using Antlr4.Runtime.Misc;
using Eeloo.Grammar;
using Eeloo.Objects;
using System;
using System.Collections.Generic;

namespace Eeloo.Evaluator
{
    public partial class EvalVisitor : EelooBaseVisitor<eeObject>
    {
        public override eeObject VisitLines([NotNull] EelooParser.LinesContext ctx)
        {
            var stmtArr = ctx.stmt();

            foreach (var stmt in stmtArr)
            {
                var val = Visit(stmt);

                /* val might be equal to eeObject.None, because expressions like 
                 * function calls and that will return eeObjects and not null. However, stmts 
                 * will return null instead. We can't pass down eeObject.None as a return value
                 * so we'll have to treat it like a null and skip it.
                 */

                if (val == null || val == eeObject.None)
                    continue;

                return val;
            }

            return null;
        }

        /* Statments */

        public override eeObject VisitString([NotNull] EelooParser.StringContext ctx)
        {
            // Return new str obj
            return eeObject.newStringObject(

                // Get text from string
                ctx.STR().GetText() 
                
                // Remove the quotes
                .Replace("\"", string.Empty) 

           );
        }


        public override eeObject VisitBool_stmt([NotNull] EelooParser.Bool_stmtContext ctx)
        { return eeObject.newBoolObject(ctx.TRUE() != null ? true : false); }
        
        public override eeObject VisitInExp([NotNull] EelooParser.InExpContext ctx)
        {
            eeObject exp1 = Visit(ctx.exp(0)),
                     exp2 = Visit(ctx.exp(1));

            var enumObj = exp2.AsEnumerable();
            if (enumObj == null)
                throw new Exception("TO DO");

            foreach (eeObject obj in enumObj)
            {
                if (obj.IsEqualTo(exp1))
                    return eeObject.TRUE;
            }

            return eeObject.FALSE;
        }
    }
}


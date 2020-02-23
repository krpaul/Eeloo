using Antlr4.Runtime.Misc;
using Eeloo.Grammar;
using Eeloo.Objects;
using System;
using System.Collections.Generic;

namespace Eeloo.Evaluator
{
    public partial class EvalVisitor : EelooBaseVisitor<eeObject>
    {
        public Scope scope;

        public EvalVisitor(Scope scope, IEnumerable<string> functions)
        {
            this.scope = scope;

            // Scan through and look for all function definitions
            //var functionEval = new FunctionEvaluator(this.scope);
        }

        public override eeObject VisitLines([NotNull] EelooParser.LinesContext ctx)
        {
            var stmtArr = ctx.stmt();

            foreach (var stmt in stmtArr)
            {
                var val = Visit(stmt);

                if (val == null)
                    continue;

                // If it's a return value
                if (val.type == eeObjectType.internal_RETURN_VALUE)
                {
                    // return it
                    return (eeObject) val.value;
                }
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

        public override eeObject VisitList([NotNull] EelooParser.ListContext ctx)
        {
            // Get expressions
            var expressions = Visit(ctx.exps());

            // Evaluate them
            var listObj = eeObject.newListObject(expressions);

            // Return the object
            return listObj;
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


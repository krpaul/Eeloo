using Antlr4.Runtime.Misc;
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
                var returnVal = Visit(stmt);
                if (stmt.return_stmt() != null)
                {
                    // If there is only one return value, return the generic eeObject
                    var asList = returnVal.AsList();
                    if (asList.Count == 1)
                    {
                        return asList[0];
                    }

                    // Otherwise, if there is more than one return value, return it as a list
                    /* A return val is an eeList diguised as internal_EXPRLIST. So, we translate it to a list at this phase (keeping it as an internal object while it's making its way through the tree is better for debugging and general clarity) */
                    returnVal.type = eeObjectType.LIST;

                    // And return
                    return returnVal;
                }
            }
            return null;
        }

        /* Statments */

        public override eeObject VisitReturn_stmt([NotNull] EelooParser.Return_stmtContext ctx)
        {
            return Visit(ctx.exps());
        }


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


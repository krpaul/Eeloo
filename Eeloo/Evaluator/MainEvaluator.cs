using Antlr4.Runtime.Misc;
using Eeloo.Functions;
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
        public override eeObject VisitFn_def([NotNull] EelooParser.Fn_defContext ctx)
        {
            scope.assignVar(
                // Function's name
                ctx.IDENTIFIER().GetText(),

                // Create function object
                eeObject.newFunctionObject(
                    // Name
                    ctx.IDENTIFIER().GetText(),

                    // Argument hash
                    (Dictionary<string, eeObject>) Antlr.visitor.Visit(ctx.fn_args()).value,

                    // Codeblock context
                    ctx.lines()
                )
            );
            return null;
        }

        public override eeObject VisitReturn_stmt([NotNull] EelooParser.Return_stmtContext ctx)
        {
            return Visit(ctx.exps());
        }

        public override eeObject VisitFn_call([NotNull] EelooParser.Fn_callContext ctx)
        {
            // Get function's name
            string iden = ctx.IDENTIFIER().GetText();

            // If function is built-in
            if (BuiltInFunctions.functionMap.ContainsKey(iden))
            {
                // Evaluate function's arguments
                ICollection<eeObject> arguments = Visit(ctx.exps()).AsEXPRLIST();

                return (eeObject) 
                    BuiltInFunctions.functionMap[iden].DynamicInvoke(
                        arguments
                    );
            }

            // Check if function is user-defined 
            var fn = scope.resolveVar(iden);
            if (fn != null && fn.type == eeObjectType.FUNCTION)
            {
                IEnumerable<eeObject> args = Visit(ctx.exps()).AsEnumerable();
                return fn.AsFunction().invoke(args);
            }
            else
            {
                throw new NotImplementedException("TO DO");
            }
        }

        public override eeObject VisitMethod_call([NotNull] EelooParser.Method_callContext ctx)
        {
            var hostObj = Visit(ctx.exp());
            string methodName = ctx.fn_call().IDENTIFIER().GetText();
            var methodParams = Visit(ctx.fn_call().exps());

            if (!hostObj.methods.ContainsKey(methodName))
                throw new Exception("To Do");

            return hostObj.CallMethod(methodName, methodParams);
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

        // This method actually returns a Dictionary<string, eeObject>, but uses eeObject as a vehicle as to conform to the visitor's uniform return type
        public override eeObject VisitFn_args([NotNull] EelooParser.Fn_argsContext ctx)
        {
            // key is argument name, value is the default value if one is provided
            Dictionary<string, eeObject> arguments
                = new Dictionary<string, eeObject>();

            var args = ctx.fn_arg();

            foreach (var arg in args)
            {
                if (arg.exp() == null)
                    arguments.Add(arg.IDENTIFIER().GetText(), null);
                else
                    arguments.Add(arg.IDENTIFIER().GetText(), Visit(arg.exp()));
            }

            return new eeObject(arguments)
            {
                type = eeObjectType.internal_FN_ARG_LIST
            };
        }
    }
}


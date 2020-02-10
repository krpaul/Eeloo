﻿using Eeloo.Objects;
using Antlr4.Runtime.Misc;
using System;
using System.Collections.Generic;
using Eeloo.Functions;

namespace Eeloo.Evaluator
{
    public partial class EvalVisitor : EelooBaseVisitor<eeObject>
    {
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

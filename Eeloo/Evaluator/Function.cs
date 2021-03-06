﻿using Antlr4.Runtime.Misc;
using Eeloo.Functions;
using Eeloo.Grammar;
using Eeloo.Objects;
using Eeloo.Errors;
using System;
using System.Collections.Generic;

namespace Eeloo.Evaluator
{
    using ArgList = Dictionary<string, eeObject>;

    public partial class EvalVisitor : EelooBaseVisitor<eeObject>
    {
        public override eeObject VisitFn_call([NotNull] EelooParser.Fn_callContext ctx)
        {
            // add this to scope
            Interpreter.currentScope.scopeCtx = ctx;

            // Get function's name
            string iden = ctx.IDENTIFIER().GetText();

            // If function is built-in
            var func = BuiltInFunctions.ResolveFunc(iden);
            if (func != null)
            {
                // Evaluate function's arguments
                ICollection<eeObject> arguments = ctx.exps() != null ? Visit(ctx.exps()).AsEXPRLIST() : new List<eeObject>();

                var scope = new Scope(Interpreter.currentScope, ctx);
                var result = (eeObject) func.DynamicInvoke(arguments);
                Scope.unScope(scope);
                
                return result;
            }

            // Check if function is user-defined 
            var fn = Interpreter.currentScope.resolveVar(iden);
            if (fn != null && fn.type == eeObjectType.FUNCTION)
            {
                ICollection<eeObject> args = ctx.exps() != null ? Visit(ctx.exps()).AsEXPRLIST() : null;
                var funcObj = fn.AsFunction();

                var scope = new Scope(Interpreter.currentScope, ctx);
                Interpreter.globalStack.AddCall(ctx, funcObj);
                
                var result = funcObj.invoke(args);

                Interpreter.globalStack.RemoveCall(funcObj);
                Scope.unScope(scope);

                return result;
            }
            else
            { throw new NoFunctionError(iden); }
        }

        // This method actually returns a Dictionary<string, eeObject>, but uses eeObject as a vehicle as to conform to the visitor's uniform return type
        public override eeObject VisitFn_args([NotNull] EelooParser.Fn_argsContext ctx)
        {
            // add this to scope
            Interpreter.currentScope.scopeCtx = ctx;

            // key is argument name, value is the default value if one is provided
            ArgList arguments
                = new ArgList();

            var args = ctx.fn_arg();

            if (args != null)
            {
                foreach (var arg in args)
                {
                    if (arg.exp() == null)
                        arguments.Add(arg.IDENTIFIER().GetText(), null);
                    else
                        arguments.Add(arg.IDENTIFIER().GetText(), Visit(arg.exp()));
                }
            }

            return new eeObject(arguments)
            {
                type = eeObjectType.internal_FN_ARG_LIST
            };
        }


        public override eeObject VisitFn_def([NotNull] EelooParser.Fn_defContext ctx)
        {
            // add this to scope
            Interpreter.currentScope.scopeCtx = ctx;

            ArgList args = ctx.fn_args() != null ?
                (ArgList)Interpreter.visitor.Visit(ctx.fn_args()).value :
                new ArgList()
                ;

            Interpreter.currentScope.assignVar(
                ctx.IDENTIFIER().GetText(),
                eeObject.newFunctionObject(
                    ctx.IDENTIFIER().GetText(),
                    args,
                    ctx.lines()
                )
            );

            return null;
        }
    }
}

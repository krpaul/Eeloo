using Antlr4.Runtime.Misc;
using Eeloo.Grammar;
using Eeloo.Objects;

using System;

namespace Eeloo.Evaluator
{
    public partial class EvalVisitor : EelooBaseVisitor<eeObject>
    {
        /* Assignment to regular named vars withouth modifiers will 
         * always remain without a modifier and variables defined with 
         * modifiers will always retain their first given modifier.
         * Array items will always be modifier less (for now)
         */
        public override eeObject VisitRegAssign([NotNull] EelooParser.RegAssignContext ctx)
        {
            // add this to scope
            scope.scopeCtx = ctx;

            // Get value of right hand side
            eeObject assignVal = Visit(ctx.exp());

            // get the var
            eeObject oldVar = Visit(ctx.var());

            // get alternative possibilities. this can either be an array index we're assigning to or a regular variable.
            var asVar = (ctx.children[0] as EelooParser.VariableContext);
            var asArr = (ctx.children[0] as EelooParser.ArrayIndexContext);

            // If this var doesn't exist already
            if (oldVar == null)
            {
                // get iden of non null alternative
                string iden = asVar != null ? asVar.IDENTIFIER().GetText() : asArr.var().GetText();

                scope.assignVar(iden, assignVal);

                return eeObject.None;
            }
            // if this var already exists
            else
            {
                if (asVar != null) // is a var
                {
                    var newVar = Visit(asVar);
                    // If they're the same type
                    if (oldVar.type == newVar.type)
                    {
                        oldVar.OverrideNew(assignVal);
                    }
                    else // if different types
                    {
                        // Just override the old object
                        scope.assignVar(asVar.IDENTIFIER().GetText(), newVar);
                    }
                }
                else if (asArr != null) // is a list item
                {
                    oldVar.CopyFrom(assignVal, true);
                }

                return null;
            }
        }

        public override eeObject VisitAugAssign([NotNull] EelooParser.AugAssignContext ctx)
        {
            // add this to scope
            scope.scopeCtx = ctx;

            var oldVar = Visit(ctx.var());
            var exp = Visit(ctx.exp());
            var opr = ctx.opr.Text;

            // get alternative possibilities. this can either be an array index we're assigning to or a regular variable.
            var asVar = (ctx.children[0] as EelooParser.VariableContext);
            var asArr = (ctx.children[0] as EelooParser.ArrayIndexContext);

            // If this var doesn't exist already
            if (oldVar == null)
                throw new Exception("Cannot augment a non-existent variable");
            // if this var already exists
            else
            {
                if (asVar != null) // is a var
                {
                    var newVar = Visit(asVar);

                    // If they're the same type
                    if (oldVar.type == newVar.type)
                    {
                        switch (opr)
                        {
                            case "+=":
                                oldVar.OverrideNew(oldVar.Add(exp));
                                break;
                            case "-=":
                                oldVar.OverrideNew(oldVar.Subtract(exp));
                                break;
                            case "*=":
                                oldVar.OverrideNew(oldVar.Multiply(exp));
                                break;
                            case "/=":
                                oldVar.OverrideNew(oldVar.Divide(exp));
                                break;
                            default: throw new Exception();
                        }
                    }
                    else // if different types
                    {
                        // Just override the old object
                        scope.assignVar(asVar.IDENTIFIER().GetText(), newVar);
                    }
                }
                else if (asArr != null) // is a list item
                {
                    switch (opr)
                    {
                        case "+=":
                            oldVar.CopyFrom(oldVar.Add(exp), true);
                            break;
                        case "-=":
                            oldVar.CopyFrom(oldVar.Subtract(exp), true);
                            break;
                        case "*=":
                            oldVar.CopyFrom(oldVar.Multiply(exp), true);
                            break;
                        case "/=":
                            oldVar.CopyFrom(oldVar.Divide(exp), true);
                            break;
                        default: throw new Exception();
                    }
                }

                return null;
            }
        }
    }
}


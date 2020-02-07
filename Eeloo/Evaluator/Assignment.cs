using Antlr4.Runtime.Misc;
using Eeloo.Functions;
using Eeloo.Objects;
using System;
using System.Collections.Generic;

namespace Eeloo.Evaluator
{
    public partial class EvalVisitor : EelooBaseVisitor<eeObject>
    {
        public override eeObject VisitAssignment([NotNull] EelooParser.AssignmentContext ctx)
        {
            // Get value of right hand side
            eeObject assignVal = Visit(ctx.exp());

            // variable name
            string iden = ctx.IDENTIFIER().GetText();
            
            // Check if this variable already has a value, and it's the same type
            eeObject oldVar = scope.resolveVar(iden);
            if (oldVar != null && oldVar.type == assignVal.type)
            {
                // Carry over modifiers
                assignVal.modifier = oldVar.modifier;
            }

            // Verifiy validity
            assignVal.Verify();

            // Assign it to the current scope
            scope.assignVar(ctx.IDENTIFIER().GetText(), assignVal);
            
            // Maybe this statement returns a none value
            return eeObject.None;
        }


        /*
        public override eeObject VisitExpAssign([NotNull] EelooParser.ExpAssignContext ctx)
        {
            // Get value of right hand side
            eeObject assignVal = Visit(ctx.exp());

            // Assign it to the current scope
            scope.assignVar(ctx.IDENTIFIER().GetText(), assignVal);

            // Maybe this statement returns a none value
            return eeObject.None;
        }

        public override eeObject VisitCreatorAssign([NotNull] EelooParser.CreatorAssignContext ctx)
        {
            string[] creator = ctx.CREATOR().GetText().Split(" ");

            string type, modifier = null;

            type = creator[0];
            if (creator.Length == 2)
            {
                modifier = creator[0];
                type = creator[1];
            }

            eeObject obj;

            switch (type)
            {
                case "list":
                    obj = eeObject.newListObject(null, modifier);
                    break;
                case "string":
                    obj = eeObject.newStringObject(""); // Strings don't have modifiers
                    break;
                case "number":
                    obj = eeObject.newNumberObject(0, modifier);
                    break;
                default:
                    throw new Exception("No such type " + type);
            }

            scope.assignVar(ctx.IDENTIFIER().GetText(), obj);

            return null;
        }
        */
    }
}

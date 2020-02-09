using Antlr4.Runtime.Misc;
using Eeloo.Functions;
using Eeloo.Objects;
using System;
using System.Collections.Generic;

namespace Eeloo.Evaluator
{
    public partial class EvalVisitor : EelooBaseVisitor<eeObject>
    {
        public override eeObject VisitVarAssign([NotNull] EelooParser.VarAssignContext ctx)
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

        public override eeObject VisitIdxAssign([NotNull] EelooParser.ArrAssignContext ctx)
        {
            // Get value of right hand side
            eeObject assignVal = Visit(ctx.exp(1));

            // get the list var name
            string iden = ctx.IDENTIFIER().GetText();

            // Get the value
            var variableVal = scope.resolveVar(iden);

            // Make sure it exists
            if (variableVal == null)
                throw new Exception($"{iden} is not a variable.");
            // Make sure it's an array or string variable
            else if (variableVal.type != eeObjectType.LIST && variableVal.type != eeObjectType.STRING)
                throw new Exception($"{iden} is not a string or array.");

            // Bring it into a C# List<eeObject> type
            List<eeObject> array = variableVal.AsList();

            // get the array index
            var idxExp = Visit(ctx.exp(0));

            // First, Make sure the requested index is a number
            if (idxExp.type != eeObjectType.NUMBER)
                throw new Exception("TO DO: Index is not a number");

            int idx = (int) idxExp.AsInteger();

            
        }
    }
}

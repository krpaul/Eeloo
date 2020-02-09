using Eeloo.Objects;
using Antlr4.Runtime.Misc;
using System;
using System.Collections.Generic;

namespace Eeloo.Evaluator
{
    public partial class EvalVisitor : EelooBaseVisitor<eeObject>
    {
        public override eeObject VisitVariable([NotNull] EelooParser.VariableContext ctx)
        {
            // Get the name of the variable
            string iden = ctx.IDENTIFIER().GetText();

            // Get the value
            var val = scope.resolveVar(iden);

            // If the value does not exist
            if (val == null)
                throw new Exception($"Variable with name {iden} does not exist");

            // return the value
            return val;
        }

        public override eeObject VisitArrayIndex([NotNull] EelooParser.ArrayIndexContext ctx)
        {
            // Get the name of the array variable
            string iden = ctx.IDENTIFIER().GetText();

            // Get the value
            var variableVal = scope.resolveVar(iden);

            // Make sure it exists
            if (variableVal == null)
            {
                throw new Exception($"{iden} is not a variable.");
            }
            // Make sure it's an array or string variable
            else if (variableVal.type != eeObjectType.LIST && variableVal.type != eeObjectType.STRING)
            {
                throw new Exception($"{iden} is not a string or array.");
            }

            // Bring it into a C# List<eeObject> type
            List<eeObject> array = variableVal.AsList();

            // Evaluate the index
            var index = Visit(ctx.exp());

            // First, Make sure the requested index is a number
            if (index.type != eeObjectType.NUMBER)
            {
                throw new Exception("TO DO: Index is not a number");
            }

            int indexValue = (int)index.AsInteger();
            bool reversed = false;

            // If the index is a negative number
            // -1 will be last element, -2 second last elem, -0 will be first element though
            if (indexValue < 0)
            {
                indexValue = Math.Abs(indexValue);
                indexValue--; // Decrement to conform to regular indexes

                array.Reverse();
                reversed = true;
            }

            // Then make sure the index is in range
            if (!(indexValue >= 0 && indexValue < array.Count))
                throw new Exception("TO DO: Index out of range");

            // return the value at the index
            var valAtIdx = array[indexValue];

            // Reverse the array back to its orignal state if it was reversed
            if (reversed)
                array.Reverse();

            return valAtIdx;
        }

    }
}

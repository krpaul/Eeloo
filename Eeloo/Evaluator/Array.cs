using Antlr4.Runtime.Misc;
using Eeloo.Grammar;
using Eeloo.Objects;
using Eeloo.Objects.ParserObjects;
using System;
using System.Collections.Generic;

namespace Eeloo.Evaluator
{
    public partial class EvalVisitor : EelooBaseVisitor<eeObject>
    {
        public override eeObject VisitArrayIndex([NotNull] EelooParser.ArrayIndexContext ctx)
        {
            // Get the name of the array variable
            string iden = ctx.var().GetText();

            // Get the value
            var variableVal = scope.resolveVar(iden);

            // if it doesnt exist
            if (variableVal == null)
            {
                return null;
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

            // workaround for now to get eeNumbers to work with c#'s arrays
            eeNumber indexValue_ee = index.AsNumber();
            int indexValue;
            for (indexValue = 0; new eeNumber(indexValue) < indexValue_ee; ++indexValue) ;

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

        public override eeObject VisitArraySlice([NotNull] EelooParser.ArraySliceContext ctx)
        {
            EelooParser.ExpContext[] exps = ctx.exp();
            
            eeObject exp1 = Visit(exps[1]),
                     exp2 = Visit(exps[2]);

            int start = Convert_eeNum(exp1.AsNumber()),
                stop = Convert_eeNum(exp2.AsNumber()),
                step = exps.Length > 2 ? Convert_eeNum(Visit(exps[3]).AsNumber()) : 1;

            List <eeObject> list = Visit(exps[0]).AsList();
            List<eeObject> slice = new List<eeObject>();

            for (int i = start; i <= stop; i += step)
            {
                slice.Add(list[i]);
            }

            return eeObject.newListObject(slice);
        }

        private int Convert_eeNum(eeNumber num)
        {
            // workaround for now to get eeNumbers to work with c#'s arrays
            int indexValue;
            for (indexValue = 0; new eeNumber(indexValue) < num; ++indexValue) ;

            return indexValue;
        }

    }
}


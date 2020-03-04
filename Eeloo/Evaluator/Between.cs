using Antlr4.Runtime.Misc;
using Eeloo.Grammar;
using Eeloo.Objects;
using System.Linq;
using System.Collections.Generic;
using System;

namespace Eeloo.Evaluator
{
    public partial class EvalVisitor : EelooBaseVisitor<eeObject>
    {
        public override eeObject VisitBetweenExp([NotNull] EelooParser.BetweenExpContext ctx)
        {
            var exps = ctx.exp();

            var toCheck = Visit(exps[0]);
            var bounds = new eeObject[] { Visit(exps[1]), Visit(exps[2]), };

            if (!(bounds[0].type == bounds[1].type && bounds[1].type == toCheck.type))
                throw new Exception("Types mismatch for \"between\" comparason");

            bool between;
            switch (toCheck.type)
            {
                case eeObjectType.DECIMAL:
                    var d_val = (double) toCheck.value;

                    between = d_val > (double) bounds[0].value 
                        && d_val < (double) bounds[1].value;

                    if (between)
                        return eeObject.TRUE;
                    else return eeObject.FALSE;
                case eeObjectType.NUMBER:
                    var l_val = (long) toCheck.value;

                    between = l_val > (long) bounds[0].value
                        && l_val < (long) bounds[1].value;

                    if (between)
                        return eeObject.TRUE;
                    else return eeObject.FALSE;
                case eeObjectType.STRING:
                    string s_chk  = (string)toCheck.value,
                           s_val1 = (string)bounds[0].value,
                           s_val2 = (string)bounds[1].value;

                    // Make sure they're chars
                    bool isChar = s_chk.Length == 1 && s_val1.Length == 1 && s_val2.Length == 1;
                    if (!isChar)
                        throw new Exception("Between comparason for strings must be single characters");

                    // Cast them all to chars
                    char chk = s_chk.ToCharArray()[0],
                           val1 = s_val1.ToCharArray()[0],
                           val2 = s_val2.ToCharArray()[0];

                    // compare
                    between = chk > val1 && chk < val2;

                    if (between)
                        return eeObject.TRUE;
                    else return eeObject.FALSE;
                default:
                    throw new Exception($"Cannot do \"between\" comparason for object \"{toCheck.value}\"");
            }
        }
    }
}

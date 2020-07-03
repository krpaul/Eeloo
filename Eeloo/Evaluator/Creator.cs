using Antlr4.Runtime.Misc;
using Eeloo.Grammar;
using Eeloo.Objects;
using Eeloo.Objects.ParserObjects;

namespace Eeloo.Evaluator
{
    public partial class EvalVisitor : EelooBaseVisitor<eeObject>
    {
        public override eeObject VisitCreator([NotNull] EelooParser.CreatorContext ctx)
        {
            var creator = ctx;
            return null;
            /*
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

            return obj;
            */
        }

        public override eeObject VisitListCreator([NotNull] EelooParser.ListCreatorContext ctx)
        {
            var modNode = ctx.LIST_MODIFIER();
            string modifier = modNode == null ? null : modNode.GetText(); 
            return eeObject.newListObject((eeObject) null, modifier);
        }

        public override eeObject VisitNumberCreator([NotNull] EelooParser.NumberCreatorContext ctx)
        {
            var modNode = ctx.NUMBER_MODIFIER();
            string modifier = modNode == null ? null : modNode.GetText(); 
            return eeObject.newNumberObject(
                modifier == "odd" ? new eeNumber(1) : new eeNumber(0), // Pass it a value of 1 if it has an "odd" modifier instead of the default 0
                modifier                   // Pass the modifier
            ); 
        }

        public override eeObject VisitStringCreator([NotNull] EelooParser.StringCreatorContext ctx)
        {
            return eeObject.newStringObject("");
        }

        public override eeObject VisitCreatorExpression([NotNull] EelooParser.CreatorExpressionContext ctx)
        {
            return Visit(ctx.creator());
        }
    }
}

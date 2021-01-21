using Antlr4.Runtime.Misc;
using Eeloo.Grammar;
using Eeloo.Objects;
using Eeloo.Objects.ParserObjects;

namespace Eeloo.Evaluator
{
    public partial class EvalVisitor : EelooBaseVisitor<eeObject>
    {
        public override eeObject VisitListCreator([NotNull] EelooParser.ListCreatorContext ctx)
        {
            // add this to scope
            scope.scopeCtx = ctx;

            var modNode = ctx.LIST_MODIFIER();
            string modifier = modNode == null ? null : modNode.GetText(); 
            return eeObject.newListObject((eeObject) null, modifier);
        }

        public override eeObject VisitNumberCreator([NotNull] EelooParser.NumberCreatorContext ctx)
        {
            var modNode = ctx.NUMBER_MODIFIER();
            string modifier = modNode == null ? null : modNode.GetText(); 
            return eeObject.newNumberObject(
                modifier == "odd" ? new eeNumber(1)         // Pass it a value of 1 if it has an "odd" modifier instead of the default 0
                : modifier == "negative" ? new eeNumber(-1) // Pass it a value of -11 if it has a "negative" modifier instead of the default 0
                : new eeNumber(0),                          // Default 0
                modifier                                    // Pass the modifier
            ); 
        }

        public override eeObject VisitStringCreator([NotNull] EelooParser.StringCreatorContext ctx)
        { return eeObject.newStringObject(""); }

        public override eeObject VisitCreatorExpression([NotNull] EelooParser.CreatorExpressionContext ctx)
        { return Visit(ctx.creator()); }
    }
}

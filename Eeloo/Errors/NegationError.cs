using Antlr4.Runtime;
using Eeloo.Objects;
using Eeloo.Helpers;

namespace Eeloo.Errors
{
    class NegationError : BaseError
    {
        public NegationError(ParserRuleContext error_context, eeObjectType type)
            : base(error_context, "NegationError", $"Cannot negate object of type {ObjectTypeHelpers.ObjectTypeToString(type)}.")
        { }
    }
}

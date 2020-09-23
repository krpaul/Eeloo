using Antlr4.Runtime;
using Eeloo.Objects;
using Eeloo.Helpers;

namespace Eeloo.Errors
{
    class NegationError : BaseError
    {
        public NegationError(ParserRuleContext error_context, eeObjectType type, bool not = false)
            : base(error_context, "NegationError", $"Cannot negate object {(not ? "that is not" : "")} of type {ObjectTypeHelpers.ObjectTypeToString(type)}.")
        { }
    }
}

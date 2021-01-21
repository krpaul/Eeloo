using Antlr4.Runtime;
using Eeloo.Objects;
using Eeloo.Helpers;

namespace Eeloo.Errors
{
    class NegationError : BaseError
    {
        public NegationError(eeObjectType type, bool not = false)
            : base("NegationError", $"Cannot negate object {(not ? "that is not" : "")} of type {ObjectTypeHelpers.ObjectTypeToString(type)}.")
        { }
    }
}

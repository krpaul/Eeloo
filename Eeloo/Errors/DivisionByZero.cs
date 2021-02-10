using Eeloo.Helpers;
using Eeloo.Objects.ParserObjects;

namespace Eeloo.Errors
{
    class DivisionByZeroError : BaseError
    {
        public DivisionByZeroError(eeNumber num1)
            : base("NegationError", $"Cannot divide number {num1} by 0.")
        { }
    }
}

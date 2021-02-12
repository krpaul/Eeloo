using Eeloo.Helpers;
using Eeloo.Objects.ParserObjects;

namespace Eeloo.Errors
{
    class ArgumentMismatchError : BaseError
    {
        public ArgumentMismatchError(eeFunction func, long thisCount, long argCount)
            : base("ArgumentMismatchError", $"Function \'{func.name}\' takes {thisCount} argument{(thisCount != 1 ? "s" : "")} but was given {argCount} argument{(argCount != 1 ? "s" : "")}")
        { }
    }
}

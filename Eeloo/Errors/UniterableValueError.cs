using Antlr4.Runtime;

namespace Eeloo.Errors
{
    class UniterableValueError : BaseError
    {
        public UniterableValueError(ParserRuleContext error_context)
            : base(error_context, "UniterableValueError", $"Iterable type not provided")
        { }
    }
}

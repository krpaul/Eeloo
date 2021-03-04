namespace Eeloo.Errors
{
    class NoFunctionError : BaseError
    {
        public NoFunctionError(string fnName) 
            : base("NoFunctionError", $"Function with name {fnName} does not exist in this context.")
        {}
    }
}

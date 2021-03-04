namespace Eeloo.Errors
{
    class UniterableValueError : BaseError
    {
        public UniterableValueError()
            : base("UniterableValueError", $"Iterable type not provided")
        { }
    }
}

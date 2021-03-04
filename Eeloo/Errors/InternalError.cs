namespace Eeloo.Errors
{
    class InternalError : BaseError
    {
        public InternalError(string msg)
            : base("InternalError", msg)
        { }
    }
}

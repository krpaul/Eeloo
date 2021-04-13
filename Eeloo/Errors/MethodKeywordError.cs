namespace Eeloo.Errors
{
    class MethodKeywordError : BaseError
    {
        public MethodKeywordError(string mAlias, string keywordUsed, string[] validKeywords)
            : base("MethodKeywordError", 
                  $"Used wrong keyword \"{keywordUsed}\" for method {mAlias}, valid keywords are {string.Join(", ", validKeywords)}"
            )
        { }
    }
}

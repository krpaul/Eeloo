namespace Eeloo.Errors
{
    class MethodKeywordError : BaseError
    {
        public MethodKeywordError(string mAlias, string keywordUsed, string[] validKeywords)
            : base("MethodKeywordError", 
                  $"Used wrong keyword \"{keywordUsed}\" for method '{mAlias}', valid keyword{(validKeywords.Length != 1 ? "s" : "")} {(validKeywords.Length != 1 ? "are" : "is")}: \"{string.Join("\", \"", validKeywords)}\""
            )
        { }
    }
}

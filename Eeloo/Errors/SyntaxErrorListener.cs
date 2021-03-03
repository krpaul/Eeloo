using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using System;

namespace Eeloo.Errors
{
    public class SyntaxErrorListener : BaseErrorListener
    {
        public override void SyntaxError(IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
       {
            var a = offendingSymbol.StartIndex;
            var b = offendingSymbol.StopIndex;

            Interval interval = new Interval(a, b);

            string tokenMsg = offendingSymbol.InputStream.GetText(interval);

            // Make message more friendly
            msg = msg
                .Replace(" expecting", ", expecting")
                .Replace("NL", "a new line")
                .Replace("missing IDENTIFIER", "missing a keyword or variable name")
                .Replace("mismatched input", "unexpected keyword");

            throw new Exception(
                $"Line {line} Column {charPositionInLine}: {msg}"
                
            );
       }
    }
}

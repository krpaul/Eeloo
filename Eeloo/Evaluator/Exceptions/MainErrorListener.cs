using System;
using System.Collections.Generic;
using System.Text;

using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;

namespace Eeloo.Evaluator.Exceptions
{
    public class ThrowingErrorListener : BaseErrorListener
    {
       public override void SyntaxError(IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
       {
            var a = offendingSymbol.StartIndex;
            var b = offendingSymbol.StopIndex;

            Interval interval = new Interval(a, b);

            string tokenMsg = offendingSymbol.InputStream.GetText(interval);

            throw new Exception($"Line {line} Column {charPositionInLine}: {msg}");
       }
    }
}

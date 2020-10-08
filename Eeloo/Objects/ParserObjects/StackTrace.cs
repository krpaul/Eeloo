using System;
using System.Collections.Generic;
using System.Text;
using Antlr4.Runtime;

namespace Eeloo.Objects.ParserObjects
{
    class StackTrace
    {
        private List<ParserRuleContext> trace = new List<ParserRuleContext>();

        public void AddCall(ParserRuleContext context)
        {
            trace.Add(context);
        }

        public override string ToString()
        {
            StringBuilder build = new StringBuilder(); 
            foreach (var ctx in this.trace)
            {
                build.Append($"Line {ctx.Start.Line} column {ctx.Start.Column} {Environment.NewLine}");
            }

            return build.ToString();
        }
    }
}

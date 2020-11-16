using System;
using System.Collections.Generic;
using System.Text;
using Antlr4.Runtime;

namespace Eeloo.Objects.ParserObjects
{
    public class CallStack
    {
        private List<ParserRuleContext> stack = new List<ParserRuleContext>();

        public void AddCall(ParserRuleContext context)
        {
            stack.Add(context);
        }

        public override string ToString()
        {
            StringBuilder build = new StringBuilder(); 
            foreach (var ctx in this.stack)
            {
                build.Append($"Line {ctx.Start.Line} column {ctx.Start.Column} {Environment.NewLine}");
            }

            return build.ToString();
        }
    }
}

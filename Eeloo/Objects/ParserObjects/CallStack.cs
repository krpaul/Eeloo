using System.Collections.Generic;
using System.Text;
using System;
using Antlr4.Runtime;
using System.Linq;

namespace Eeloo.Objects.ParserObjects
{
    public class CallStack
    {
        class Call
        {
            ParserRuleContext callContext;
            public eeFunction calledFunction;

            public Call(ParserRuleContext ctx, eeFunction func)
            {
                callContext = ctx;
                calledFunction =  func;
            }

            public string ToString()
            {
                return $"Function {this.calledFunction.name} on line {callContext.Start.Line}";
            }
        }

        private List<Call> stack = new List<Call>();

        public void AddCall(ParserRuleContext context, eeFunction callingFunction)
        {
            stack.Add(new Call(context, callingFunction));
        }

        public void RemoveCall(eeFunction closingFunction)
        {
            this.stack.RemoveAll(c => c.calledFunction == closingFunction);
        }

        public override string ToString()
        {

            StringBuilder build = new StringBuilder();
            var cnt = this.stack.Count;
            for (int i = cnt - 1; i >= 0; i--)
            {
                var call = stack.ElementAt(i);
                build.Append($"{String.Concat(Enumerable.Repeat("  ", cnt - i))}» " + call.ToString() + Environment.NewLine);
            }

            return build.ToString();
        }
    }
}

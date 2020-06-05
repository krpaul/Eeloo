using System;
using Xunit;
using Eeloo;

namespace InterpreterTests
{
    public class SyntaxTests
    {
        [Fact]
        public void TestMath()
        {
            Interpreter.Main(new string[] { "./Tests/math.ee" });
        }
    }
}

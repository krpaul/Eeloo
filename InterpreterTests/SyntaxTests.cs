using System;
using System.IO;
using Xunit;
using Eeloo;

namespace InterpreterTests
{
    public class SyntaxTests
    {
        [Fact]
        public void TestMath()
        {
            // Get file text
            string input = File.ReadAllText("../../../Tests/math.ee");
             
            Interpreter.Interpret(input);
        }
    }
}

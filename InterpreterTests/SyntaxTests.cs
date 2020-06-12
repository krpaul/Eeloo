using System;
using System.IO;
using Xunit;
using Eeloo;

namespace InterpreterTests
{
    public class SyntaxTests
    {
        private string ReadTestFile(string name)
        {
            // Get & return file text
            return File.ReadAllText($"../../../Tests/{ name }.ee");
        }

        [Fact]
        public void TestMath()
        { Interpreter.Interpret(ReadTestFile("math")); }

        [Fact]
        public void TestStrings()
        { Interpreter.Interpret(ReadTestFile("strings")); }
    }
}

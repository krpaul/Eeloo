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
        public void TestString()
        { Interpreter.Interpret(ReadTestFile("string")); }

        [Fact]
        public void TestCast()
        { Interpreter.Interpret(ReadTestFile("cast")); }

        [Fact]
        public void TestBool()
        { Interpreter.Interpret(ReadTestFile("bool")); }

        [Fact]
        public void TestList()
        { Interpreter.Interpret(ReadTestFile("list")); }

        [Fact]
        public void TestLoop()
        { Interpreter.Interpret(ReadTestFile("loop")); }
}
}

using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Linq;
using System.IO;
using System.Collections.Generic;

using Antlr4.Runtime;
using Antlr4.Runtime.Tree;

using Eeloo.Objects;
using Eeloo.Objects.ParserObjects;
using Eeloo.Functions;
using Eeloo.Evaluator;
using Eeloo.Errors;
using Eeloo.Grammar;

using System.Diagnostics;

namespace Eeloo
{
    public class Interpreter
    { 
        public static EvalVisitor visitor;
        public static string filename = "";
        public static CallStack globalStack = new CallStack();
        public static Scope currentScope = new Scope(null, null);

        public static void Main(string[] args) // passing filenames to read
        {
            // Get file text
            string input = File.ReadAllText(args[0]);
            filename = args[0].Split('/').Last();

            Interpret(input);
        }

        public static void Interpret(string input)
        {
            try
            {
                // Add newline to stream
                input += Environment.NewLine;

                // Convert to CharStream
                var stream = new AntlrInputStream(input);

                // Feed into lexer
                var lexer = new EelooLexer(stream);

                // Convert to TokenStream
                var tokens = new CommonTokenStream(lexer);

                // Create parser and feed it tokens
                var parser = new EelooParser(tokens);
                parser.AddErrorListener(new SyntaxErrorListener());

                // Top Level Rule
                var tree = parser.program();

                // Create visitor object
                EvalVisitor evalVisitor = new EvalVisitor();

                // Give public access to the visitor and the global scope
                Interpreter.visitor = evalVisitor;

                // Visit tree
                evalVisitor.Visit(tree);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw e;
            }
            finally { }
        }
    }
}

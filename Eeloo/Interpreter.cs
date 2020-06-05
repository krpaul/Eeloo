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
using Eeloo.Evaluator.Exceptions;
using Eeloo.Grammar;

namespace Eeloo
{
    public class Interpreter
    {
        public static EvalVisitor visitor;
        public static string filename = "";

        public static void Main(string[] args)
        {
            // Run tests
#if DEBUG
            foreach (string testfile in Directory.GetFiles("../../../Tests/"))
            {
                filename = testfile.Split('/').Last();

                string input = File.ReadAllText(testfile) + Environment.NewLine;
                var stream = new AntlrInputStream(input);
                var lexer = new EelooLexer(stream);
                var tokens = new CommonTokenStream(lexer);
                var parser = new EelooParser(tokens); parser.AddErrorListener(new ThrowingErrorListener());
                var tree = parser.program(); Scope globalScope = new Scope(null); var builtIns = (from fn in typeof(BuiltInFunctions).GetMethods() where char.IsLower(fn.Name[0]) select fn.Name).Distinct(); FunctionEvaluator func = new FunctionEvaluator(globalScope); EvalVisitor evalVisitor = new EvalVisitor(globalScope, builtIns); Interpreter.visitor = evalVisitor; func.Visit(tree);
                evalVisitor.Visit(tree);
            }
#endif  

            try
            {
                // Get file text
                string input = File.ReadAllText(args[0]);

                filename = args[0].Split('/').Last();

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
                parser.AddErrorListener(new ThrowingErrorListener());

                // Top Level Rule
                var tree = parser.program();

                // Create global scope object
                Scope globalScope = new Scope(null);

                // List of builtins
                var builtIns = (
                    from fn in typeof(BuiltInFunctions).GetMethods()
                    where char.IsLower(fn.Name[0])
                    select fn.Name
                ).Distinct();

                // Create function visitor
                FunctionEvaluator func = new FunctionEvaluator(globalScope);

                // Create visitor object
                EvalVisitor evalVisitor = new EvalVisitor(globalScope, builtIns);

                // Give public access to the visitor and the global scope
                Interpreter.visitor = evalVisitor;

                // Find all functions
                func.Visit(tree);

                // Visit tree
                evalVisitor.Visit(tree);
            }
            //catch (Exception e)
            //{
            //    Console.WriteLine("Error on " + e.Message);
            //}
            finally
            {
                // Exit on keyPress
                Console.ReadKey();
            }
        }
    }
}

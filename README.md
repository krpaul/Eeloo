# The Eeloo Language
Eeloo aims to be a high-level, abstract, and human-readable programming language. This language experiments with how efficient having multiple ways to express an idea is. Currently, it is most syntactically similar with Ruby with elements from Python.

Each statment, grammatical structure, and function has many ways to be expressed. One might think that this creates clutter and confusion within a programming language. However, the language strives to avoid this ambiguity on the premise that any written program can read like English. The goal is not to create something poetic, but to creating something that is widely understandable, and functional.

This language is implemented entirely in the C# language, using ANTLR v4 as a lexer/parser generator. Currently, this language has only been tested on Windows and does not guarantee its functionality on a Unix platform, although it is technically implemented on the .NET Core framework, allowing portability.

If you wish to run this language as it is right now (very bare-bones and in development) clone this git repo, open it in Visual Studio, and type your test program into the program.ee file. (If you're simply testing this language in development mode with no intention to compile it, Visual Studio should handle the platform compilition and this should therefore work on almost every platform).

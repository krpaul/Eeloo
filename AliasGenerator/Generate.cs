using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using YamlDotNet.Serialization;

namespace AliasGenerator
{
    using YObjType = Dictionary<string, Dictionary<string, List<object>>>;

    // these are the various types that the `object` in the above type can represent:
    using ModAlias = Dictionary<string, string>; // string 1 is alias name, string 2 is property name, string 3 is property value.
    using KWList = List<string>;
    using Property = Dictionary<string, string>;

    class ConstructLexer
    {
        public const string FOLDERPREFIX = "./Grammar/";
        public readonly static List<string> AllowedProperties = new List<string>() { "allowStandardSyntax", "requireBrackets", } ;
        readonly static Deserializer deserializer = new Deserializer();
        readonly static Newtonsoft.Json.JsonSerializer js = new Newtonsoft.Json.JsonSerializer();

        static void Main()
        {
            List<string> tokensFromMethodAliases;
            ParseMethodAliases(out tokensFromMethodAliases); return;

            string grammar = File.ReadAllText(FOLDERPREFIX + "LexerAliasesGrammar");
            string aliases = File.ReadAllText(FOLDERPREFIX + "Aliases.yml");


            var aliasDict = deserializer.Deserialize<Dictionary<string, List<string>>>(aliases);

            Regex rx = new Regex(@"\<\<\w+\>\>");
            Regex comments_rx = new Regex(@"(?s)\/\*.*?\*\/");

            MatchCollection matches = rx.Matches(grammar);
            MatchCollection comments = comments_rx.Matches(grammar);

            string nl = Environment.NewLine;

            grammar += nl + nl + "/* Auto-generated tokens */" + nl;
            int i = 0;

            List<string> tokens = new List<string>();

            // Replace each
            foreach (Match match in matches)
            {
                string aliasID = match.Value;
                aliasID = aliasID.Substring(2, aliasID.Length - 4);

                var al = aliasDict[aliasID];
                string flag = string.Empty;

                // Check for flags
                if (al[0].StartsWith("__") && al[0].EndsWith("__"))
                {
                    // Store flag and remove as an alias
                    flag = al[0].Replace("__", "");
                    al.RemoveAt(0);
                }

                string wsIdenBounding = 
                    flag == "OptionalBoundingWhitespace" ? 
                        "WS?" : // Optional whitespace
                        "WS" ; // Mandatory whitespace

                foreach (string alias in al)
                {
                    foreach (string word in alias.Split(' '))
                    {
                        if (tokens.Contains(word))
                            continue;

                        var tokenLabel = $"AUTOTOKEN_{i:D4}";
                        grammar += $"{tokenLabel} : '{word}' ;";
                        grammar += Environment.NewLine;
                        
                        i++;
                        tokens.Add(word);
                    }
                }

                string expanded = string.Join(" | ",
                    from syn in al
                    select 
                    $"{wsIdenBounding} " + string.Join(" WS ",
                        from s in syn.Split(" ") select $"'{s}'"
                    ) + $" {wsIdenBounding}"
                );

                grammar = grammar.Replace(match.Value, expanded);
            }

            // delete comments
            foreach (Match match in comments)
                grammar = grammar.Replace(match.Value, "");

            grammar = "lexer grammar GeneratedLexer;" + Environment.NewLine + grammar;
            File.WriteAllText(FOLDERPREFIX + "GeneratedLexer.g4", grammar);
        }
        
        static void ParseMethodAliases(out List<string> neededTokens)
        {
            var methodAliases = File.ReadAllText("../../../../Eeloo/Grammar/ObjectMethodAliases.yml");
            Dictionary<string, Dictionary<string, List<object>>> aliasDict = deserializer.Deserialize<YObjType>(methodAliases);
            neededTokens = new List<string>();

            var w = new StringWriter();
            js.Serialize(w, aliasDict);
            string jsonText = w.ToString();

            Console.WriteLine(jsonText);
            Console.WriteLine(aliasDict);
        }
    }
}

// Dictionary<string, List<string>>
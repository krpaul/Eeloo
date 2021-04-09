using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using YamlDotNet.Serialization;

namespace AliasGenerator
{
    using YObjType = Dictionary<string, Dictionary<string, Dictionary<string, List<string>>>>;

    class ConstructLexer
    {
        public const string FOLDERPREFIX = "./Grammar/";
        public static readonly Deserializer deserializer = new Deserializer();
        
        static void Main()
        {
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

                // Special treatment for keywords since it's define in the method alias file
                if (aliasID == "<<__keywords__>>")
                {
                    var kwds = GetMethodAliasKeywords();
                    grammar = grammar.Replace(
                        match.Value, string.Join(" | ", 
                            from k in kwds select $"WS \'{k}\' WS")
                    );
                    tokens.AddRange(kwds);
                    continue;
                }

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

        private static IEnumerable<string> GetMethodAliasKeywords()
        {
            var methodAliases = File.ReadAllText(FOLDERPREFIX + "ObjectMethodAliases.yml");
            YObjType aliasDict = deserializer.Deserialize<YObjType>(methodAliases);
            var keywords = new List<string>();

            foreach (var objType in aliasDict)
            {
                foreach (var MethodID in objType.Value)
                {
                    foreach (var property in MethodID.Value)
                    {
                        if (property.Key == "keywords")
                        {
                            keywords.AddRange(property.Value);
                        }
                    }
                }
            }

            // remove duplicates and return keywords
            return keywords.Distinct();
        }
    }
}

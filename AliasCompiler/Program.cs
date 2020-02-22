using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace BuildAliases
{
    class ConstructLexer
    {
        public const string FOLDERPREFIX = "./";
        static void Main(string[] args)
        {
            string grammar = File.ReadAllText(FOLDERPREFIX + "LexerAliases.gram");
            string aliases = File.ReadAllText(FOLDERPREFIX + "Aliases.yml");

            var deserializer = new YamlDotNet.Serialization.Deserializer();

            var aliasDict = deserializer.Deserialize<Dictionary<string, List<string>>>(aliases);

            Regex rx = new Regex(@"\<\<\w+\>\>");

            MatchCollection matches = rx.Matches(grammar);

            // Replace each
            foreach (Match match in matches)
            {
                string aliasID = match.Value;
                aliasID = aliasID.Substring(2, aliasID.Length - 4);

                string expanded = string.Join(" | ",
                    from syn in aliasDict[aliasID]
                    select string.Join(" WS ",
                        from s in syn.Split(" ") select $"'{s}'"
                    )
                );

                grammar = grammar.Replace(match.Value, expanded);
            }

            File.WriteAllText(FOLDERPREFIX + "LexerAliases.g4", grammar);
        }
    }
}

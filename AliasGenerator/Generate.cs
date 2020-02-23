﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AliasGenerator
{
    class ConstructLexer
    {
        public const string FOLDERPREFIX = "./Grammar/";
        static void Main()
        {
            string grammar = File.ReadAllText(FOLDERPREFIX + "LexerAliasesGrammar");
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
                    flag == "OPTIONAL_BOUNDING_WS" ? 
                        "WS?" : 
                        "WS";

                string expanded = string.Join(" | ",
                    from syn in al
                    select 
                    $"{wsIdenBounding} " + string.Join(" WS ",
                        from s in syn.Split(" ") select $"'{s}'"
                    ) + $" {wsIdenBounding}"
                );

                grammar = grammar.Replace(match.Value, expanded);
            }

            grammar = "lexer grammar GeneratedLexer;" + Environment.NewLine + grammar;
            File.WriteAllText(FOLDERPREFIX + "GeneratedLexer.g4", grammar);
        }
    }
}

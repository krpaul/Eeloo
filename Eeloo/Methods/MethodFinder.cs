using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.IO;
using YamlDotNet.Serialization;

namespace Eeloo.Methods
{
    using YObjType = Dictionary<string, Dictionary<string, List<object>>>;
    public class MethodFinder
    {
        public readonly static List<string> AllowedProperties = new List<string>() { "allowStandardSyntax", "requireBrackets", };
        public static readonly Deserializer deserializer = new Deserializer();
        public static readonly JsonSerializer js = new JsonSerializer();

        public static void ParseMethodAliases(out List<string> neededTokens)
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

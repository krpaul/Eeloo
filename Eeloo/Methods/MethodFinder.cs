using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using YamlDotNet.Serialization;
using Eeloo.Errors;
using Eeloo.Helpers;

namespace Eeloo.Methods
{
    using YObjType = Dictionary<string, Dictionary<string, Dictionary<string, List<string>>>>;
    public class MethodFinder
    {
        public readonly static List<string> AllowedProperties = new List<string>() { "allowStandardSyntax", "requireBrackets", };
        public static readonly Deserializer deserializer = new Deserializer();

        public static void ParseMethodAliases(out List<string> neededTokens)
        {
            var methodAliases = File.ReadAllText("../../../../Eeloo/Grammar/ObjectMethodAliases.yml");
            YObjType aliasDict = deserializer.Deserialize<YObjType>(methodAliases);
            neededTokens = new List<string>();

            foreach (var objType in aliasDict)
            {
                foreach (var MethodID in objType.Value)
                {
                    Method m = new Method(MethodID.Key, ObjectTypeHelpers.StringToObjectType(objType.Key), true);
                    foreach (var property in MethodID.Value)
                    {
                        switch (property.Key)
                        {
                            case "aliases":
                                m.AssignAliases(property.Value);
                                break;
                            case "keywords":
                                m.Keywords = property.Value;
                                break;
                            case "generalProperties":
                                foreach (string gprop in property.Value)
                                    m.GeneralProperties.Add(Method.StringToMethodFlag(gprop));
                                break;
                            default:
                                throw new InternalError($"On method specification parsing: unknown property \"{property.Key}\"");
                        }
                    }

                    // add method to collective list
                    Method.AllMethods.Add(m);
                }
            }
        }
    }
}

﻿using Eeloo.Errors;
using Eeloo.Helpers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using YamlDotNet.Serialization;

namespace Eeloo.Methods
{
    using YObjType = Dictionary<string, Dictionary<string, Dictionary<string, List<string>>>>;
    public class MethodFinder
    {
        public readonly static List<string> AllowedProperties = new List<string>() { "allowStandardSyntax", "requireBrackets", };
        public static readonly Deserializer deserializer = new Deserializer();

        public static void ParseMethodAliases()
        {
            var methodAliases = File.ReadAllText("../../../../Eeloo/Grammar/ObjectMethodAliases.yml");
            YObjType aliasDict = deserializer.Deserialize<YObjType>(methodAliases);

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
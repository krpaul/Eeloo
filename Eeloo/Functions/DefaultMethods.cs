using Eeloo.Objects;
using Eeloo.Objects.ParserObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Eeloo.Functions
{
    class DefaultMethods
    {
        public static Dictionary<string, Func<eeObject, ICollection<eeObject>, eeObject>> listBuiltInMethods
            = new Dictionary<string, Func<eeObject, ICollection<eeObject>, eeObject>>()
            {
                {
                    "add", // appends new values to the end of the list
                    (eeObject self, ICollection<eeObject> newVals) =>
                    {
                        foreach (eeObject obj in newVals)
                        {
                            ((List<eeObject>) self.value).Add(obj);
                        }

                        var lenAttr = self.attributes["length"];
                        lenAttr.value = lenAttr.AsNumber() + new eeNumber(newVals.Count());

                        return eeObject.None;
                    }
                },
                {
                    "remove", // appends new values to the end of the list
                    (eeObject self, ICollection<eeObject> valsToRem) =>
                    {
                        foreach (eeObject obj in valsToRem)
                        {
                            ((List<eeObject>) self.value).Remove(obj);
                        }

                        var lenAttr = self.attributes["length"];
                        lenAttr.value = lenAttr.AsNumber() - new eeNumber(valsToRem.Count());

                        return eeObject.None;
                    }
                },
                {
                    "removeAll", // appends new values to the end of the list
                    (eeObject self, ICollection<eeObject> valsToRem) =>
                    {
                        foreach (eeObject obj in valsToRem)
                        {
                            ((List<eeObject>) self.value).RemoveAll((o) => o.IsEqualTo(obj));
                        }

                        var lenAttr = self.attributes["length"];
                        lenAttr.value = new eeNumber(self.AsList().Count());

                        return eeObject.None;
                    }
                },
            };

        public static Dictionary<string, Func<eeObject, ICollection<eeObject>, eeObject>> stringBuiltInMethods
            = new Dictionary<string, Func<eeObject, ICollection<eeObject>, eeObject>>() {
                {
                    "concatenate",
                    (eeObject self, ICollection<eeObject> strings) =>
                    {
                        // Alter value
                        foreach (eeObject obj in strings)
                            self.value += (string) obj.value;

                        // As well as return new value
                        return eeObject.newStringObject(self.AsString());
                    }
                },
                {
                    "uppercase",
                    (eeObject self, ICollection<eeObject> strings) =>
                    {
                        if (strings.Count() > 0)
                            throw new Exception("This method does not take any arguments");

                        return eeObject.newStringObject(self.AsString().ToUpper());

                    }
                },
                {
                    "lowercase",
                    (eeObject self, ICollection<eeObject> strings) =>
                    {
                        if (strings.Count() > 0)
                            throw new Exception("This method does not take any arguments");

                        return eeObject.newStringObject(self.AsString().ToLower());

                    }
                },
                {
                    "reverse",
                    (eeObject self, ICollection<eeObject> strings) =>
                    {
                        if (strings.Count() > 0)
                            throw new Exception("This method does not take any arguments");

                        char[] reversedString = self.AsString().ToCharArray();
                        Array.Reverse(reversedString);

                        return eeObject.newStringObject(new string(reversedString));

                    }
                },
            };

        public static Dictionary<string, Func<eeObject, ICollection<eeObject>, eeObject>> numberBuiltInMethods
            = new Dictionary<string, Func<eeObject, ICollection<eeObject>, eeObject>>()
            {

            };
    }
}

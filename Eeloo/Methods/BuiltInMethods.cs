using Eeloo.Objects;
using Eeloo.Objects.ParserObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;


namespace Eeloo.Methods
{
    using MethodDict = Dictionary<string, Func<eeObject, ICollection<eeObject>, eeObject>>;
    class BuiltInMethods
    {
        public static MethodDict listBuiltInMethods
            = new MethodDict()
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
                    "remove", // removes first matching value
                    (eeObject self, ICollection<eeObject> valsToRem) =>
                    {
                        List<eeObject> rawPtr = (List<eeObject>) self.value;
                        foreach (eeObject obj in valsToRem)
                        {
                            var itemtoremove = rawPtr.FirstOrDefault(item => item.IsEqualTo(obj));
                            rawPtr.Remove(itemtoremove);
                        }

                        var lenAttr = self.attributes["length"];
                        lenAttr.value = lenAttr.AsNumber() - new eeNumber(valsToRem.Count());

                        return eeObject.None;
                    }
                },
                {
                    "removeAll", // removes all matching values
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
                {
                    "sum", // sums the values of the list
                    (eeObject self, ICollection<eeObject> args) =>
                    {
                        if (args.Count() > 0)
                            throw new Exception("This method takes no arguments");

                        eeNumber sum = new eeNumber(0);
                        foreach (eeObject obj in self.AsList())
                        {
                            var n = obj.AsNumber();
                            if (n == null) throw new Exception("List must have all number types in order to be summed");

                            sum += n;
                        }

                        return eeObject.newNumberObject(sum);
                    }
                },
                { 
                    "average", // get the average value of the list
                    (eeObject self, ICollection<eeObject> args) =>
                    {
                        if (args.Count() > 0)
                            throw new Exception("This method takes no arguments");

                        eeNumber sum = new eeNumber(0);
                        var l = self.AsList();
                        foreach (eeObject obj in l)
                        {
                            var n = obj.AsNumber();
                            if (n == null) throw new Exception("List must have all number types in order to be averaged");

                            sum += n;
                        }

                        return eeObject.newNumberObject(sum / new eeNumber(l.Count()));
                    }
                },
                {
                    "slice", // slices the list
                    (eeObject self, ICollection<eeObject> args) =>
                    {
                        var argCount = args.Count();
                        if (argCount != 2)
                            throw new Exception("This method takes 2 arguments");

                        List<eeObject> list = self.AsList();
                        List<eeObject> newList = new List<eeObject>();
                        bool collect = false; 

                        for (eeNumber i = new eeNumber(0); i < new eeNumber(list.Count()); i += eeNumber.ONE)
                        {
                            if (i == args.ElementAt(0).AsNumber())
                            {
                                collect = true;
                            }

                            //if (collect)
                                //newList.Add(list.ElementAt());
                        }

                        //list = list.GetRange(args.ElementAt(0).AsNumber(, args[1] - args[0]);
                        return null;
                    }
                },
                {
                    "reverse", // reverses the values of the list
                    (eeObject self, ICollection<eeObject> args) =>
                    {
                        if (args.Count() > 0)
                            throw new Exception("This method takes no arguments");

                        var list = self.AsList();
                        
                        var rev = list.ToArray();
                        Array.Reverse(rev);

                        return eeObject.newListObject(rev.ToList());
                    }
                },
            };

        public static MethodDict stringBuiltInMethods
            = new MethodDict() {
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
                    (eeObject self, ICollection<eeObject> args) =>
                    {
                        if (args.Count() > 0)
                            throw new Exception("This method does not take any arguments");

                        return eeObject.newStringObject(self.AsString().ToUpper());

                    }
                },
                {
                    "lowercase",
                    (eeObject self, ICollection<eeObject> args) =>
                    {
                        if (args.Count() > 0)
                            throw new Exception("This method does not take any arguments");

                        return eeObject.newStringObject(self.AsString().ToLower());

                    }
                },
                {
                    "reverse", // reverses a string
                    (eeObject self, ICollection<eeObject> strings) =>
                    {
                        if (strings.Count() > 0)
                            throw new Exception("This method does not take any arguments");

                        char[] reversedString = self.AsString().ToCharArray();
                        Array.Reverse(reversedString);

                        return eeObject.newStringObject(new string(reversedString));

                    }
                },
                {
                    "slice", // slices a string
                    (eeObject self, ICollection<eeObject> args) =>
                    {
                        if (args.Count() == 0)
                            throw new Exception("This method takes at least 1 argument");
                        
                        return self;
                    }
                },
            };

        public static MethodDict numberBuiltInMethods
            = new MethodDict()
            {
                {
                    "binary", // returns the binary representation of this num
                    (eeObject self, ICollection<eeObject> args) =>
                    {
                        if (args.Count() > 0)
                            throw new Exception("This method takes no arguments");

                        var bin = new List<eeObject>();

                        foreach (bool b in self.AsNumber().ToBinary())
                        {
                            bin.Add(
                                eeObject.newNumberObject(b ? eeNumber.ONE.Copy() : eeNumber.ZERO.Copy())
                            );
                        }
                        
                        return eeObject.newListObject(bin);
                    }
                },
                {
                    "digits", // returns a list of digits of this num
                    (eeObject self, ICollection<eeObject> args) =>
                    {
                        if (args.Count() > 0)
                            throw new Exception("This method takes no arguments");

                        return eeObject.newListObject(
                            ((((List<eeNumber>)(((eeNumber)self.AsNumber()).Digits())).Select(num => eeObject.newNumberObject(num)))).ToList()
                        );
                    }
                },
                {
                    "factorial", // returns the factorial of this num
                    (eeObject self, ICollection<eeObject> args) =>
                    {
                        if (args.Count() > 0)
                            throw new Exception("This method takes no arguments");

                        return eeObject.newNumberObject(self.AsNumber().Factorial());
                    }
                },
            };
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using Eeloo.Objects.ParserObjects;

namespace Eeloo.Objects
{
    public enum eeObjectType
    {
        // Internal types (used only in the language's parser implementation) are negative-valued
        internal_RETURN_VAL = -4,
        internal_FN_ARG_LIST = -3,
        internal_RETURN_VALUE = -2,
        internal_EXPRLIST = -1, 
        NUMBER = 1,  // Avoiding 0 as first value since type's default value will take 0 and we'll know if we forgot to assign it somwhere in the case of a 0
        DECIMAL,
        STRING,
        BOOL,
        LIST,
        FUNCTION
    }

    public partial class eeObject
    {
        //public readonly List<string> NUMBER_MODS = new List<string>() { "negative", "even",  "odd"};
        //public readonly List<string> LIST_MODS = new List<string>() { "unique" };

        public eeObjectType type;

        private object _value;

        public object value
        {
            get { return _value;  }
            set { _value = value; this.Verify(); }
        }

        // Values in this dictionary will be regular object values
        public Dictionary<string, dynamic> attributes
            = new Dictionary<string, dynamic>();

        /* Values in this dictionary will be various Func<>'s
         * First param is always the eeObject itself
         * Second param is an eeListObject of all the params passed to the method
         * Third param is the method's return type
         */
        public Dictionary<string, Func<eeObject, ICollection<eeObject>, eeObject>> methods
            = new Dictionary<string, Func<eeObject, ICollection<eeObject>, eeObject>>();

        public string modifier = null;

        /* Some static values */

        // For none values
        public static eeObject NONE = new eeObject();

        // For empty strings and iterables (equivalent to eeObject.NONE essentially, but is more precise)
        public static eeObject EMPTY = new eeObject();

        public eeObject(object val)
        { this._value = val; }

        public eeObject()
        { this._value = null; }

        public string AsString()
        { return value as string; }

        public string ToPrintableString()
        {
            switch (this.type)
            {
                case eeObjectType.LIST:
                    return  this.value == null ? 
                        "[]" // Empty list
                        : $"[{string.Join(", ", (from elem in this.AsList() select elem.ToPrintableString()))}]"; 
                case eeObjectType.STRING:
                    return "\"" + this.AsString() + "\""; 
                case eeObjectType.NUMBER:
                    return this.AsNumber().ToString(); 
                case eeObjectType.DECIMAL:
                    return this.AsDecimal().ToString();  
                case eeObjectType.BOOL:
                    return this.AsBool() ? "true" : "false";
                case eeObjectType.FUNCTION:
                    eeFunction func = this.AsFunction();
                    return $"Function \"{func.name}\", Arguments: {func.argNames.ToString()}";
                default:
                    throw new Exception("default case for ToPrintableString");
            }
        }

        public bool AsBool()
        {
            // If object is a type of primitive
            switch (this.type)
            {
                case eeObjectType.BOOL:
                    return (bool) value;
                case eeObjectType.NUMBER:
                    return this.AsNumber() != 0;
                case eeObjectType.DECIMAL:
                    return this.AsDecimal() != 0.0;
                case eeObjectType.STRING:
                    return this.AsString().Length != 0;
            }

            // If object is an enumerable
            var enumObj = this.AsEnumerable();
            if (enumObj != null)
            {
                // Return false if empty and true if not
                return enumObj.Count() != 0;
            }

            // Return false if anything else
            return false;
        }

        public long AsInteger()
        { return (long) value; }

        public double AsDecimal()
        { return (double) value; }

        public dynamic AsNumber()
        {
            if (value is double)
                return AsDecimal();
            else if (value is long)
                return AsInteger();
            else throw new Exception();
        }

        public List<eeObject> AsList()
        { return (List<eeObject>) value;  }

        public eeFunction AsFunction()
        { return (eeFunction) value; }

        public IEnumerable<eeObject> AsEnumerable()
        {
            // If this object is a string, return an enumerable of eeStringObjects for each character
            if (this.type == eeObjectType.STRING)
                return (
                    from char c in this.AsString().ToCharArray()
                    select eeObject.newStringObject(c.ToString())
                ) as IEnumerable<eeObject>;
            else return this.value as IEnumerable<eeObject>;
        }

        // Method internally used for expression lists
        public ICollection<eeObject> AsEXPRLIST()
        {
            if (this.type != eeObjectType.internal_EXPRLIST)
            {
                throw new Exception("internal type mismatch");
            }

            // Return a generic enumerable interface, because we'll only need to read and iterate over expression values
            // If using an expression list as part of a list object type, use the AsList or AsEnumerable methods
            return this.value as ICollection<eeObject>;
        }

        public bool IsEqualTo(eeObject obj)
        { return this.value.Equals(obj.value); }

        public bool IsNotEqualTo(eeObject obj)
        { return !IsEqualTo(obj); }

        public bool IsGreaterThan(eeObject obj)
        {
            switch (this.type)
            {
                case eeObjectType.NUMBER:
                    return this.AsNumber() > obj.AsNumber();
                case eeObjectType.LIST:
                    return this.AsList().Count > obj.AsList().Count;
                case eeObjectType.STRING:
                    return this.AsString().Length > obj.AsString().Length;
                case eeObjectType.DECIMAL:
                    return this.AsDecimal() > obj.AsDecimal();
                case eeObjectType.BOOL:
                default:
                    throw new InvalidOperationException("TO DO");
            }
        }

        public bool IsGreaterThanOrEqualTo(eeObject obj)
        { return IsGreaterThan(obj) || IsEqualTo(obj);  }

        public bool IsLessThan(eeObject obj)
        { return !IsGreaterThanOrEqualTo(obj); }

        public bool IsLessThanOrEqualTo(eeObject obj)
        { return !IsGreaterThan(obj); }

        // Methods in Eeloo will be passed as an internal_EXPRLIST ICollection object to the method handler.
        public eeObject CallMethod(string name, eeObject parameters)
        {
            // Extract the expressions
            ICollection<eeObject> expressions = parameters.AsEXPRLIST();

            // Run the method
            var returnVal = this.methods[name](this, expressions);

            // Verify this object
            Verify();

            // Return method's return value
            return returnVal;
        }

        // Reads the value of this eeObject, cross references the type and the modifier, and returns true if all checks out properly
        public bool Verify()
        {
            if (this.modifier == null)
                return true;

            bool valid = true;
            switch (this.type)
            {
                case eeObjectType.NUMBER:
                    var num = AsNumber();
                    switch (modifier)
                    {
                        case "negative":
                            if (num > 0)
                                valid = false;
                            break;
                        case "positive":
                            if (num < 0)
                                valid = false;
                            break;
                        case "even":
                            if (num % 2 != 0)
                                valid = false;
                            break;
                        case "odd":
                            if (num % 2 != 1)
                                valid = false;
                            break;
                        default:
                            throw new Exception("Unknown modifier: " + modifier);
                    }
                    break;
                case eeObjectType.LIST:
                    var list = this.AsList();
                    switch (modifier)
                    {
                        case "unique":
                            var disct = list.Select(s => s.value).Distinct();
                            if (disct.Count() != list.Count())
                                valid = false;
                            break;

                        default:
                            throw new Exception("Unknown modifier: " + modifier);
                    }
                    break;
            }

            if (valid)
                return valid;
            else
                throw new Exception($"Constraint '{modifier}' violated with value of {this.ToPrintableString()}");
        }

        // Workaround to override this object with another object
        public void OverrideSelf(eeObject newObj)
        {
            modifier = newObj.modifier;
            attributes = newObj.attributes;
            methods = newObj.methods;
            type = newObj.type;
            value = newObj.value;
        }
    }
}


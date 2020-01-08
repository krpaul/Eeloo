using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using Eeloo.Objects.ParserObjects;

namespace Eeloo.Objects
{
    public enum eeObjectType
    {
        internal_FN_ARG_LIST = -2,
        internal_EXPRLIST = -1,  // Internal types (used only in the language implementation) are negative 
        NUMBER = 1,  // Avoiding 0 as first value since type's default value will take 0 and we'll know if we forgot to assign it somwhere in the case of a 0
        DECIMAL,
        STRING,
        BOOL,
        LIST,
        FUNCTION
    }

    public partial class eeObject
    {
        public eeObjectType type;
        public object value;

        // Values in this dictionary will be regular object values
        public Dictionary<string, dynamic> attributes
            = new Dictionary<string, dynamic>();

        /* Values in this dictionary will be various Func<>'s
         * First param is always the eeObject itself
         * Second param is an eeListObject of all the params passed to the method
         * Third param is the method's return type
         */
        public Dictionary<string, Func<eeObject, eeObject, eeObject>> methods
            = new Dictionary<string, Func<eeObject, eeObject, eeObject>>();

        /* Some static values */

        // For none values
        public static eeObject NONE = new eeObject();

        // For empty strings and iterables (equivalent to eeObject.NONE essentially, but is more precise)
        public static eeObject EMPTY = new eeObject();

        public eeObject(object value)
        { this.value = value; }

        public eeObject()
        { value = null; }

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

        public eeObject CallMethod(string name, eeObject parameters)
        {
            if (parameters.AsEnumerable() == null) // If not a list of params
                this.methods[name](this,
                    eeObject.newListObject(new List<eeObject>() { parameters })
                );

            return this.methods[name](this, parameters);
        }
    }
}

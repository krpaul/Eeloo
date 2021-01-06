using System;
using System.Collections.Generic;
using System.Linq;
using Eeloo.Evaluator;
using Eeloo.Evaluator.Exceptions;
using Eeloo.Helpers;
using Eeloo.Objects.ParserObjects;
using Eeloo.Errors;
using Eeloo.Grammar;

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
        public Dictionary<string, eeObject> attributes
            = new Dictionary<string, eeObject>();

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

        #region Casts
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
                    return this.AsString();
                case eeObjectType.NUMBER:
                    return this.AsNumber().ToString(); 
                case eeObjectType.BOOL:
                    return this.AsBool() ? "true" : "false";
                case eeObjectType.FUNCTION:
                    eeFunction func = this.AsFunction();
                    return $"Function \"{func.name}\", Arguments: {func.argNames.ToString()}";
                default:
                    throw new Exception("default case for ToPrintableString");
            }
        }

        // tries to coax an eeObject into a number, if compatible
        public readonly List<char> CharNums = new List<char>() { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '.' };
        public eeObject DynamicNumConvert()
        {
            switch (this.type)
            {
                case eeObjectType.NUMBER:
                    return this;
                case eeObjectType.STRING:
                    // go through the entire string and look for any numbers
                    List<char> nums = new List<char>();
                    foreach (char c in this.AsString().ToCharArray())
                    {
                        if (CharNums.Contains(c)) {
                            nums.Add(c);
                        }
                    }

                    // If no nums found
                    if (nums.Count == 0)
                        return eeObject.newNumberObject(new eeNumber(0));

                    string agg = new string(nums.ToArray());
                    
                    // If its a decimal
                    if (nums.Contains('.')) {
                        return eeObject.newNumberObject(new eeNumber(agg));
                    }
                    // Otherwise
                    return eeObject.newNumberObject(new eeNumber(agg));
                case eeObjectType.BOOL:
                    // Use standard bool to int conversion
                    return eeObject.newNumberObject(this.AsBool() ? new eeNumber(1) : new eeNumber(0));
                case eeObjectType.LIST:
                    // If all the elements in the list are number objects, return their sums
                    List<eeObject> elems = this.AsList();
                    bool allNums = true;
                    foreach (eeObject obj in elems)
                    {
                        if (obj.type != eeObjectType.NUMBER)
                        {
                            allNums = false;
                            break;
                        }
                    }

                    if (allNums)
                    {
                        // For now I'm using double as to retain decimal accuracy, but in the future we'll have to split it up into respectice long and double types
                        double sum = 0; 
                        foreach (eeObject obj in elems)
                        {
                            sum += obj.AsNumber();
                        }
                        return eeObject.newNumberObject(new eeNumber(sum.ToString()));
                    }
                    // If not all nums, return the list's length
                    else
                    {
                        return eeObject.newNumberObject(new eeNumber(elems.Count()));
                    }
                default: // Otherwise it's not a type we should concern ourselves with converting into a number;
                    return eeObject.newNumberObject(new eeNumber(0));
            }
        }

        public eeObject DynamicListConvert()
        {
            List<eeObject> val = new List<eeObject>();
            switch (this.type)
            {
                case eeObjectType.LIST:
                    return this;
                case eeObjectType.STRING: // Return list of chars
                    string str = this.AsString();
                    val = str.ToCharArray().Select(c => eeObject.newStringObject(c.ToString())).ToList();
                    break;
                case eeObjectType.NUMBER:
                case eeObjectType.BOOL: // Return list where this is the first object
                    val = new List<eeObject>() { this };
                    break;
            }

            return eeObject.newListObject(val);
        }

        public eeObject DynamicBoolConvert()
        {
            bool val;
            switch (this.type)
            {
                case eeObjectType.BOOL:
                    return this;
                case eeObjectType.NUMBER:
                    val = !this.AsNumber().IsZero();
                    break;
                case eeObjectType.LIST:
                    val = this.AsList().Count() != 0;
                    break;
                case eeObjectType.STRING:
                    // check if the string itself is "true" or "false"
                    string str = this.AsString().ToUpper(); // ToUpper because it's faster than ToLower for comparisons
                    if (str == "TRUE") {
                        val = true;
                        break;
                    } 
                    else if (str == "FALSE") {
                        val = false;
                        break;
                    }
                    else {
                        val = str.Length != 0;
                        break;
                    }
                default:
                    val = false;
                    break;
            }

            return newBoolObject(val);
        }

        public bool AsBool()
        {
            // If object is a type of primitive
            switch (this.type)
            {
                case eeObjectType.BOOL:
                    return (bool) value;
                case eeObjectType.NUMBER:
                    return this.AsNumber().IsZero();
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

        public dynamic AsNumber()
        {
            //if (value is double)
            //    return AsDecimal();
            //else if (value is long)
            //    return AsInteger();
            //else throw new Exception();
            return value as eeNumber;
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

        #endregion

        #region Comparisons

        public bool IsEqualTo(eeObject obj)
        {
            // eeNumber has a separate object comparason 
            if (this.type == eeObjectType.NUMBER && obj.type == eeObjectType.NUMBER)
            {
                return ((eeNumber) this.value) == ((eeNumber) obj.value);
            }
            else if (this.type == eeObjectType.LIST && obj.type == eeObjectType.LIST)
            {
                List<eeObject> listA = this.AsList(),
                               listB = obj.AsList();

                if (listA.Count() != listB.Count()) return false;

                for (int i = 0; i < listA.Count(); i++)
                {
                    if (listA[i].IsNotEqualTo(listB[i]))
                        return false;
                }

                return true;
            }
            else // all other underlying native c# objects can be compared as usual
            {
                return this.type == obj.type && this.value.Equals(obj.value);
            }
        }

        public bool IsNotEqualTo(eeObject obj)
        { return !IsEqualTo(obj); }


        public bool IsGreaterThan(eeObject obj)
        {
            switch (this.type)
            {
                case eeObjectType.NUMBER:
                    return this.AsNumber() > obj.AsNumber();
                case eeObjectType.LIST:
                    return ListMathHelpers.GreaterThan(this, obj);
                case eeObjectType.STRING:
                    return StringMathHelpers.GreaterThan(this, obj);
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

        #endregion

        #region Operations
        public eeObject Add(EelooParser.AdditiveOprExpContext context, eeObject exp)
        {
            if (this.type == eeObjectType.STRING || exp.type == eeObjectType.STRING) // string math
                return StringMathHelpers.Add(this, exp);
            else if (this.type == eeObjectType.LIST || exp.type == eeObjectType.LIST) // list math
                return ListMathHelpers.Add(this, exp);
            else if (this.type == eeObjectType.NUMBER && exp.type == eeObjectType.NUMBER) // regular arithmetic
                return eeObject.newNumberObject(this.AsNumber() + exp.AsNumber());
            else
                throw new InvalidOperationError(context, context.opr.Text, type, exp.type);
        }

        public eeObject Subtract(EelooParser.AdditiveOprExpContext context, eeObject exp)
        {
            if (this.type == eeObjectType.STRING || exp.type == eeObjectType.STRING) // string math
                return StringMathHelpers.Subtract(this, exp);
            else if (this.type == eeObjectType.LIST || exp.type == eeObjectType.LIST) // list math
                return ListMathHelpers.Subtract(this, exp);
            else if (this.type == eeObjectType.NUMBER && exp.type == eeObjectType.NUMBER) // regular arithmetic
                return eeObject.newNumberObject(this.AsNumber() - exp.AsNumber());
            else
                throw new InvalidOperationError(context, context.opr.Text, type, exp.type);

        }

        public eeObject Multiply(Antlr4.Runtime.ParserRuleContext context, eeObject exp)
        {
            if (this.type == eeObjectType.STRING || exp.type == eeObjectType.STRING)
                return StringMathHelpers.Multiply(this, exp);
            else if (this.type == eeObjectType.LIST || exp.type == eeObjectType.LIST)
                return ListMathHelpers.Multiply(this, exp);
            else if (this.type == eeObjectType.NUMBER && exp.type == eeObjectType.NUMBER) // regular arithmetic
                return eeObject.newNumberObject(this.AsNumber() * exp.AsNumber());
            else
                throw new InvalidOperationError(context, "multiplication", type, exp.type);

        }
        #endregion

        #region Methods
        // Methods in Eeloo will be passed as an internal_EXPRLIST ICollection object to the method handler.
        public eeObject CallMethod(string name, eeObject parameters)
        {
            // Extract the expressions
            ICollection<eeObject> expressions = parameters != null ? parameters.AsEXPRLIST() : new List<eeObject>();

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
                    eeNumber num = AsNumber();
                    switch (modifier)
                    {
                        case "negative":
                            if (num > eeNumber.ZERO)
                                valid = false;
                            break;
                        case "positive":
                            if (num < eeNumber.ZERO)
                                valid = false;
                            break;
                        case "even":
                            if (num % eeNumber.TWO != eeNumber.ZERO)
                                valid = false;
                            break;
                        case "odd":
                            if (num % eeNumber.TWO != eeNumber.ONE)
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

        // If this object is an element of a list, this method with replace it with another value
        public void CopyFrom(eeObject fromObj, bool asArrayElement=false)
        {
            switch (fromObj.type)
            {
                case eeObjectType.NUMBER:
                    this.value = fromObj.AsNumber().Copy();
                    break;
                default: // all other types are natively constants so we dont need to explicity copy them
                    this.value = fromObj.value;
                    break;
            }

            this.attributes = fromObj.attributes;
            this.methods = fromObj.methods;
            this.type = fromObj.type;

            // Array elements do not get modifiers
            if (asArrayElement)
                modifier = null;
        }

        // returns a copy of this object; using CopyFrom because it's already implemented
        public eeObject Copy()
        {
            eeObject newObject = new eeObject();
            newObject.CopyFrom(this);

            return newObject;
        }

        // If this object is a variable, set it's value to newObj but retain the old modifier
        public void OverrideNew(eeObject newObj)
        {
            // the two objects should be the same type, but this function doesn't handle that directly.
            this.value = newObj.value;
            this.methods = newObj.methods;
            this.attributes = newObj.attributes;
        }

        public eeObject GetAttribute(string name)
        {
            if (!this.attributes.ContainsKey(name))
                throw new Exception($"No attribute named {name} in object");

            return this.attributes[name];
        }

        #endregion
    }
}


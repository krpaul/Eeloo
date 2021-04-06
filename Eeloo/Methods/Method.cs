using System;
using System.Collections.Generic;
using System.Text;

using Eeloo.Objects;
using Eeloo.Errors;
using Eeloo.Helpers;

namespace Eeloo.Methods
{
                                     
    using MethodImplementation = Func<eeObject, ICollection<eeObject>, eeObject>;
    /* Func<> Parameters:
     * eeObject: the 'self' object. is set to the object that this method is called on
     * ICollection<eeObject>: the parameters. A list of objects this method is passed.
     * eeObject: the return object. this is the object that the method returns.
    */

    using MethodDict = Dictionary<string, Func<eeObject, ICollection<eeObject>, eeObject>>;

    public enum MethodFlag
    {
        NoStandardSyntax,
        DontRequireBrackets,
    }

    class Alias
    {
        string aliasStr;
        bool allowStandardSyntax;

        public Alias(string aliasStr, bool allowStandardSyntax)
        {
            this.aliasStr = aliasStr;
            this.allowStandardSyntax = allowStandardSyntax;
        }
    }

    class Method
    {
        // Static List of all methods
        public static List<Method> AllMethods;

        // Method properties
        private readonly string MethodID;
        bool BuiltIn;
        eeObjectType MethodObjectType;
        public List<MethodFlag> GeneralProperties = new List<MethodFlag>();
        public List<string> Keywords;
        private MethodImplementation Implementation;
        
        public List<Alias> Aliases;

        public Method(string MethodID, eeObjectType objType, bool BuiltIn)
        {
            // set props
            this.MethodID = MethodID;
            this.BuiltIn = BuiltIn;
            this.MethodObjectType = objType;

            /* locate the method in the list of built-ins */
            // get list of methods for this type
            ref MethodDict FetchMethodsForType(eeObjectType type)
            {
                switch (objType)
                {
                    case eeObjectType.STRING:
                        return ref BuiltInMethods.stringBuiltInMethods;
                    case eeObjectType.LIST:
                        return ref BuiltInMethods.listBuiltInMethods;
                    case eeObjectType.NUMBER:
                        return ref BuiltInMethods.numberBuiltInMethods;
                    default:
                        throw new InternalError($"Unsupported method object type: No methods exist for object of type {ObjectTypeHelpers.ObjectTypeToString(objType)}.");
                }
            }

            // assign ref
            ref MethodDict listOfBuiltIns = ref FetchMethodsForType(objType);

            // find implementation
            foreach (var m in listOfBuiltIns)
            {
                if (m.Key == MethodID)
                { 
                    this.Implementation = m.Value;
                    break;
                }
            }
        }

        public void AssignAliases(List<string> rawAliases)
        {
            bool nss = false;
            foreach (string a in rawAliases)
            {
                if (a == "__NoStandardSyntax__") // after this flag, all next aliases will be nss-only (non-standard syntax)
                {
                    nss = true;
                    continue;
                }

                this.Aliases.Add(new Alias(a, !nss));
            }
        }

        #region Static Functions

        public static MethodFlag StringToMethodFlag(string flag)
        {
            switch (flag)
            {
                case "__NoStandardSyntax__":
                    return MethodFlag.NoStandardSyntax;
                case "__DontRequireBrackets__":
                    return MethodFlag.DontRequireBrackets;
                default:
                    return (MethodFlag) (-1); // for debugging
            }
        }

        //public static eeObject CallMethod(string methodName, eeObject methodParams)
        //{
        //    //foreach (Method m in Method.AllMethods)
        //    //{
        //    //    if ()
        //    //}
        //}

        #endregion
    }
}

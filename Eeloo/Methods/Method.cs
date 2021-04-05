using System;
using System.Collections.Generic;
using System.Text;

using Eeloo.Objects;

namespace Eeloo.Methods
{
                                     
    using MethodImplementation = Func<eeObject, ICollection<eeObject>, eeObject>;
                                /* Func<> Parameters:
                                 * eeObject: the 'self' object. is set to the object that this method is called on
                                 * ICollection<eeObject>: the parameters. A list of objects this method is passed.
                                 * eeObject: the return object. this is the object that the method returns.
                                */

    public enum MethodFlags
    {
        NoStandardSyntax,
        RequireBrackets,
    }

    class Method
    {
        readonly string MethodID;
        List<string> Aliases;
        eeObjectType MethodObjectType;
        List<MethodFlags> MethodFlags = new List<MethodFlags>();
        MethodImplementation implementation;

    }
}

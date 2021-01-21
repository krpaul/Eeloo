using System;
using System.Collections.Generic;
using System.Text;
using Antlr4.Runtime;
using Eeloo.Objects;
using Eeloo.Helpers;

namespace Eeloo.Errors
{
    class InvalidOperationError : BaseError
    {
        public InvalidOperationError(string operation, params eeObjectType[] type)
            : base("InvalidOperationError", $"Cannot perform {operation} on type(s) {ConcatTypes(type)}.")
        { }

        private static string ConcatTypes(eeObjectType[] types)
        {
            if (types.Length == 1)
                return ObjectTypeHelpers.ObjectTypeToString(types[0]);
            else
                return string.Join(", ", types);
        }
    }
}

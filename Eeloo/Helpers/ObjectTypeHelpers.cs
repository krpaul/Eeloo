using System;
using System.Collections.Generic;
using System.Text;

using Eeloo.Objects;

namespace Eeloo.Helpers
{
    class ObjectTypeHelpers
    {
        public static string ObjectTypeToString(eeObjectType type)
        {
            switch (type)
            {
                case eeObjectType.BOOL:
                    return "Boolean";
                case eeObjectType.FUNCTION:
                    return "Function";
                case eeObjectType.LIST:
                    return "List";
                case eeObjectType.NUMBER:
                    return "Number";
                case eeObjectType.STRING:
                    return "String";
                default:
                    return "<internal type>";
            }
        }
    }
}

using Eeloo.Objects;
using Eeloo.Helpers;

namespace Eeloo.Errors
{
    class NoMethodError : BaseError
    {
        public NoMethodError(string methodName, eeObjectType objType)
            : base("NoMethodError", $"Method with name '{methodName}' does not exist for object of type {ObjectTypeHelpers.ObjectTypeToString(objType)}")
        { }
    }
}


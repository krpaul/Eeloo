using Eeloo.Objects;
using Eeloo.Helpers;

namespace Eeloo.Errors
{
    class MethodNotFoundError : BaseError
    {
        public MethodNotFoundError(string mName, eeObjectType mObjType)
            : base("MethodNotFoundError", $"Method with name {mName} does not exist for object of type {ObjectTypeHelpers.ObjectTypeToString(mObjType)}.")
        { }
    }
}

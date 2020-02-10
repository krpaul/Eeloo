using Eeloo.Objects.ParserObjects;
using System.Collections.Generic;

namespace Eeloo.Objects
{
    partial class eeObject
    {
        public static eeObject newFunctionObject(
            string fnName, 
            Dictionary<string, eeObject> defaultArgs, 
            EelooParser.LinesContext codeblock
        )
        {
            // Create function obj
            eeFunction newFnObject = new eeFunction(fnName, codeblock, defaultArgs);
                
            // Create eeObject
            return new eeObject(newFnObject)
            {
                type = eeObjectType.FUNCTION,
            };
        }
    }
}

﻿using System;
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
                    return "BOOLEAN";
                case eeObjectType.FUNCTION:
                    return "FUNCTION";
                case eeObjectType.LIST:
                    return "LIST";
                case eeObjectType.NUMBER:
                    return "NUMBER";
                case eeObjectType.STRING:
                    return "STRING";
                default:
                    return "<internal type>";
            }
        }
    }
}

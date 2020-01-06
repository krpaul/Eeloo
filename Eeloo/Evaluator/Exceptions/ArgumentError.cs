using System;
using System.Collections.Generic;
using System.Text;

namespace Eeloo.Evaluator.Exceptions
{
    class ArgumentError : Exception
    {
        public ArgumentError(string msg) : base(msg) { }
    }
}

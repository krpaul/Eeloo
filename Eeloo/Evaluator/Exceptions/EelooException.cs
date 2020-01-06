using System;
using System.Collections.Generic;
using System.Text;

namespace Eeloo.Evaluator.Exceptions
{
    class EelooException : Exception
    {
        public EelooException(string msg) : base(msg)
        { }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Antlr4.Runtime.Misc;
using Eeloo.Functions;
using Eeloo.Grammar;
using Eeloo.Objects;
using Antlr4;
using Antlr4.Runtime;

namespace Eeloo.Errors
{
    class Error
    {
        protected string err_msg; 
        public Error(ParserRuleContext error_context)
        {
            err_msg = $"occured on line {error_context.Start.Line} column {error_context.Start.Column}";
        }

        public void PrependSpecificError(SpecificError err)
        { err_msg = err.name + err_msg; }
    }

    abstract class SpecificError : Error
    {
        public string name {
            get ; 
            set ; 
        }

        public SpecificError(SpecificError err, ParserRuleContext error_context) : base(error_context)
        {
            this.PrependSpecificError(this);
        }
    }
}

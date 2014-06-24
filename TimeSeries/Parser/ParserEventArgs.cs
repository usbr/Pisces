using System;
using System.Collections.Generic;
using System.Text;

namespace Reclamation.TimeSeries.Parser
{
    class ParserEventArgs: EventArgs
    {

        public ParserResult ParserResult;
        public string VariableName;
        public ParserEventArgs(ParserResult e, string variableName)
        {
            this.ParserResult = e;
            this.VariableName = variableName;
        }


    }
}

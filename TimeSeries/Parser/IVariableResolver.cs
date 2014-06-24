using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Reclamation.TimeSeries.Parser
{
    public interface IVariableResolver
    {
        ParserResult Lookup(string name);
    }
}

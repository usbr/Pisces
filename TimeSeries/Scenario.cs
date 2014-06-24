using System;
using System.Collections.Generic;
using System.Text;

namespace Reclamation.TimeSeries
{
    /// <summary>
    /// Scenairo contains scenario name and optional path.
    /// </summary>
    public class Scenario
    {

        string m_name;

        public string Name
        {
            get { return m_name; }
            set { m_name = value; }
        }
        string m_path;

        public string Path
        {
            get { return m_path; }
            set { m_path = value; }
        }
        public Scenario(string name)
        {
            m_name = name;
            m_path = "";
        }
        public Scenario( string name, string path)
        {
            m_name = name;
            m_path = path;
        }


    }
}

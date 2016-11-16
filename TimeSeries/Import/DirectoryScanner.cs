using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Reclamation.TimeSeries.Import
{
    /// <summary>
    /// Looks for time series data in a given directory tree
    /// manages lists of files with optional 
    /// scenario, and siteid
    /// </summary>
    public class DirectoryScanner
    {
        string[] m_files;
        string[] m_siteid;

        public string[] Siteid
        {
            get { return m_siteid; }
            set { m_siteid = value; }
        }
        string[] m_scenario;

        public string[] Scenario
        {
            get { return m_scenario; }
            set { m_scenario = value; }
        }

        public string[] UniqueScenarios()
        {
            return m_scenario.Distinct().ToArray();
        }

        Regex re = null;
        public DirectoryScanner(string path, string filter, string regexPattern)
        {
            var dirs = new List<string>{path};
            dirs.AddRange(Directory.EnumerateDirectories(path, "*"));

            var files = new List<string>();
            foreach (var dir in dirs)
            {
                try
                {
                    files.AddRange(Directory.EnumerateFiles(dir, filter, SearchOption.AllDirectories));
                }
                catch (UnauthorizedAccessException UAEx)
                {
                    continue;
                }
                catch (PathTooLongException PathEx)
                {
                    continue;
                }
            }

            m_files = files.ToArray();
            m_scenario = Enumerable.Repeat<string>("", m_files.Length).ToArray();
            m_siteid = Enumerable.Repeat<string>("", m_files.Length).ToArray();

            if( regexPattern != "")
            {
                FindMatches(regexPattern);
            }
        }

        public void FindMatches(string regexPattern)
        {
            re = new Regex(regexPattern, RegexOptions.IgnoreCase);

            for (int i = 0; i < m_files.Length; i++)
            {
                if (re.IsMatch(m_files[i]))
                {
                    m_scenario[i] = "";
                    m_siteid[i] = "";
                    var m = re.Matches(m_files[i])[0];
                    
                    if (re.GetGroupNames().Contains("scenario"))
                    {
                        m_scenario[i] = m.Groups["scenario"].Value;
                    }
                    
                    if (re.GetGroupNames().Contains("siteid"))
                    {
                        m_siteid[i] = m.Groups["siteid"].Value;
                    }
                }
            }
        }


        public int Count
        {
            get { return m_files.Length; }
        }

        public string[] Files
        {
            get { return m_files; }
        }


    }
}

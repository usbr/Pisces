using System;
using System.Collections.Generic;
using System.Text;

namespace Reclamation.TimeSeries
{
    public class SeriesListText
    {
        SeriesList seriesList1;
        public SeriesListText(SeriesList items)
        {
            this.seriesList1 = items;
        }


        /// <summary>
        ///  Find something unique for the title.  
        /// </summary>
        public string TitleText()
        {
            if (UniqueSites.Length == 1)
             {
                 return UniqueSites[0];
             }
             //if (UniqueScenarios.Length == 1)
             //{
             //    return UniqueScenarios[0];
             //}
            return "";
        }

        internal string[] UniqueSites
        {
            get
            {
                List<string> list = new List<string>();
                foreach (Series s in seriesList1)
                {
                    if (!list.Contains(s.SiteName))
                    {
                        list.Add(s.SiteName);
                    }
                }
                return list.ToArray();
            }
        }

        /// <summary>
        /// Text for legends on graphs, or titles in tables
        /// for single or non-scenario cases the series.Appearance.LegendText is used
        /// for multiple scenarios, and a single location using scenarioname
        /// otherwise using scenarioName + LegendText
        /// </summary>
        public string[] Text
        {
            get
            {
                string[] scenarioNames = UniqueScenarios;
                string[] names = UniqueNames;
                string[] legends = UniqueLegendText;
                bool mScenairo = scenarioNames.Length > 1;
                bool mNames = names.Length > 1;
                bool mSiteNames = UniqueSites.Length > 1;
                bool mLegends = legends.Length > 1;
                bool legendsAllDifferent = (UniqueLegendText.Length == seriesList1.Count );

                if (legendsAllDifferent)
                {
                    return legends;
                }

                List<string> rval = new List<string>();

                if (mScenairo)
                { // try prefixing with scenario.
                    rval.Clear();
                    for (int i = 0; i < seriesList1.Count; i++)
                    {
                        rval.Add(seriesList1[i].ScenarioName + " " + seriesList1[i].Appearance.LegendText);
                    }

                    if (IsListUnique(rval))
                        return rval.ToArray();
                }

                if (mSiteNames)
                { // try prefixing with Name.
                    rval.Clear();
                    for (int i = 0; i < seriesList1.Count; i++)
                    {
                        rval.Add(seriesList1[i].SiteName + " " + seriesList1[i].Appearance.LegendText);
                    }

                    if (IsListUnique(rval))
                        return rval.ToArray();
                }

                if (mNames)
                { // try prefixing with Name.
                    rval.Clear();
                    for (int i = 0; i < seriesList1.Count; i++)
                    {
                        rval.Add(seriesList1[i].Name + " " + seriesList1[i].Appearance.LegendText);
                    }

                    if (IsListUnique(rval))
                        return rval.ToArray();
                }


                if (mNames && mScenairo)
                {// try prefixing with location and scenario
                    rval.Clear();
                    for (int i = 0; i < seriesList1.Count; i++)
                    {
                        rval.Add(seriesList1[i].Provider + " " + seriesList1[i].Name + " " + seriesList1[i].Appearance.LegendText);
                    }

                    if (IsListUnique(rval))
                        return rval.ToArray();
                }

                    rval.Clear();
                    for (int i = 0; i < seriesList1.Count; i++)
                    {
                        string nameAndLegend = seriesList1[i].Name + " " + seriesList1[i].Appearance.LegendText;
                        if( nameAndLegend.Trim() == "") // scenario name is all we have.
                        rval.Add(seriesList1[i].Provider + " " +nameAndLegend);
                        else
                            rval.Add(nameAndLegend);
                    }

                    if (IsListUnique(rval))
                        return rval.ToArray();


                    else
                    {
                        for (int i = 0; i < rval.Count; i++)
                        {
                            rval[i] = rval[i] + i.ToString();
                        }
                    }

                
                return rval.ToArray();

            }
        }


        private bool IsListUnique(List<string> items)
        {
            List<string> list = new List<string>();
            foreach (string s in items)
            {
                if (!list.Contains(s))
                {
                    list.Add(s);
                }
            }

            return (list.Count == items.Count);
        }

        private string[] UniqueLegendText
        {
            get
            {
                List<string> list = new List<string>();
                foreach (Series s in seriesList1)
                {
                    if (!list.Contains(s.Appearance.LegendText))
                    {
                        list.Add(s.Appearance.LegendText);
                    }
                }
                return list.ToArray();
            }
        }
       

        private string[] UniqueScenarios
        {
            get
            {
                List<string> list = new List<string>();
                foreach (Series s in seriesList1)
                {
                    if (!list.Contains(s.ScenarioName))
                    {
                        list.Add(s.Provider);
                    }
                }
                return list.ToArray();
            }
        }
        private string[] UniqueNames
        {
            get
            {
                List<string> list = new List<string>();
                foreach (Series s in seriesList1)
                {
                    if (!list.Contains(s.Name))
                    {
                        list.Add(s.Name);
                    }
                }
                return list.ToArray();
            }
        }

        /// <summary>
        /// returns a unique list of units.
        /// usefull for setting up and labeling Yaxis
        /// </summary>
        public string[] UniqueUnits
        {
            get
            {
                List<string> list = new List<string>();
                foreach (Series s in seriesList1)
                {
                    if (!list.Contains(s.Units))
                    {
                        list.Add(s.Units);
                    }
                }
                return list.ToArray();
            }
        }

    }
}

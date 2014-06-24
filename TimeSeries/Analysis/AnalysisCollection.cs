using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Reclamation.Core;
using System.Collections;
namespace Reclamation.TimeSeries.Analysis
{
    public class AnalysisCollection
    {
        List<BaseAnalysis> m_list;
        PiscesSettings Explorer;
        static string s_analysisTypes = "TimeSeriesAnalysis,ExceedanceAnalysis,ProbabilityAnalysis,WaterYearsAnalysis,SummaryHydrographAnalysis,CorrelationAnalysis,MonthlySummaryAnalysis,MovingAverageAnalysis";
        public AnalysisCollection(PiscesSettings explorer)
        {
         m_list = new List<BaseAnalysis>();
         Explorer = explorer;
        }

        private void BuildList()
        {
          try
          {
              string[] classNames = new string[] { };
              string s = System.Configuration.ConfigurationManager.AppSettings["8analysisTypes"];
              if (s != null && s.Trim() != "")
              {
                  classNames = s.Split(',');
              }
              else
              {
                  classNames = s_analysisTypes.Split(',');
              }

              m_list.Clear();
              Assembly asm = Assembly.GetExecutingAssembly();

              foreach (string c in classNames)
              {
                  string fullName = "Reclamation.TimeSeries.Analysis." + c;
                  object obj = asm.CreateInstance(fullName, false, BindingFlags.CreateInstance, null, new object[] { Explorer }, null, new object[] { });
                  BaseAnalysis analysis = obj as BaseAnalysis;
                  if (analysis == null)
                  {
                      Logger.WriteLine("Error creating analysis '" + c + "'");
                  }
                  else
                  {
                      m_list.Add(analysis);
                  }

              }
              
              //foreach (Type thisType in asm.GetTypes())
              //{
              //    string name = thisType.Namespace +"."+    thisType.Name;
              //    if( name.IndexOf("Reclamation.TimeSeries.Analysis.") ==0
              //        && thisType.IsSubclassOf(typeof(BaseAnalysis)) )
              //    {
              //        object obj = asm.CreateInstance(name, false, BindingFlags.CreateInstance, null, new object[] { Explorer }, null, new object[] { });
              //        BaseAnalysis analysis = obj as BaseAnalysis;
              //        m_list.Add(analysis);
              //    //Console.WriteLine(name);
              //    //Console.WriteLine(analysis.Description);
              //    }
                  //foreach (Type thisInterface in thisType.GetInterfaces())
                  //{
                  //    if ((Operators.CompareString(thisInterface.Name, "IPlugin", false) == 0) 
                  // && (Operators.CompareString(thisInterface.Namespace, "PluginInterface", false) == 0))
                  //    {
                  //        IPlugin thisPlugin = (IPlugin)RuntimeHelpers.GetObjectValue(
                  //asm.CreateInstance(thisType.Namespace + "." + thisType.Name));
                  //        thisPlugin.MenuItem.add_Click(new EventHandler(this.PluginMenu_Click));
                  //        this.plugins.Add(thisPlugin);
                  //        this.PluginsToolStripMenuItem.get_DropDownItems().Add(thisPlugin.MenuItem);
                  //    }
                  //}
              //}
          }
          catch (Exception exception2)
          {
              Logger.WriteLine(exception2.Message);  
          }


        }

        public BaseAnalysis this[AnalysisType analysis]
        {
            get
            {
                if (m_list.Count == 0)
                {
                    BuildList();
                }

                for (int i = 0; i < m_list.Count; i++)
                {
                    if (m_list[i].AnalysisType == analysis)
                    {
                        return m_list[i];
                    }
                }
                throw new NotSupportedException("the analysis type "+analysis.ToString()+" is not supported ");
            }
        }
        public BaseAnalysis this[int index]
        {
            get
            {
                if (m_list.Count == 0)
                {
                    BuildList();
                }
                return m_list[index];                
            }
        }
        
        public int Count
        {
            get 
                {
                    if (m_list.Count == 0)
                    {
                        BuildList();
                    }

                return m_list.Count; }
        }

    }
}

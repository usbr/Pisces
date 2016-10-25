using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace Reclamation.TimeSeries.Forms.Graphing
{
    public class GraphSettings
    {
         
         Dictionary<string, SeriesSettings> dict;

         public GraphSettings()
        {
         dict = new Dictionary<string, SeriesSettings>();
        }

        public bool Contains(string label)
         {
             return dict.ContainsKey(label);
         }
         public SeriesSettings Get(string label)
        {
            if( dict.ContainsKey(label))
            {
                return dict[label];
            }
            return new SeriesSettings();
        }

        public void Add(string label, Color c, int width=1)
        {
            SeriesSettings s = new SeriesSettings(c, width);
            dict.Add(label, s);
        }

        public int Count
        {
            get
            {
                return dict.Count;
            }
        }

        public void Read(string filename)
        {
            try
            {
                dict.Clear();
                var lines = File.ReadAllLines(filename);
                foreach (var item in lines)
                {
                    var tokens = item.Split(',');
                    var c = Color.FromArgb(Convert.ToInt32(tokens[1]));
                    int width = Convert.ToInt32(tokens[2]);
                    var s = new SeriesSettings(c,width);
                    dict.Add(tokens[0], s);
                }
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message);
                File.Delete(filename);
            }

        }
        public void Save(string filename)
        {
            StreamWriter sw = new StreamWriter(filename);
            foreach (var item in dict)
            {
                sw.WriteLine(item.Key + "," + item.Value.Color.ToArgb().ToString()+","+item.Value.Width);
            }
            sw.Close();
        }

        /// <summary>
        /// Add or overwrite settings
        /// </summary>
        /// <param name="graphSettings"></param>
        public void Merge(GraphSettings graphSettings)
        {
            foreach (var item in graphSettings.dict)
            {
                if( this.Contains(item.Key))
                {// update
                    var x = this.Get(item.Key);
                    x.Color = item.Value.Color;
                    x.Width = item.Value.Width;
                }
                else
                {// add setting
                    this.Add(item.Key,item.Value.Color,item.Value.Width);
                }
            }
            
        }
    }
}

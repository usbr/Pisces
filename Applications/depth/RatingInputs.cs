using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace Depth
{
    public partial class RatingInputs : UserControl
    {
        public RatingInputs()
        {
            InitializeComponent();
        }


        public RatingTable RatingTable
        {
            get
            {
                var rval = new RatingTable();

                if (Lookup("scaling").ToLower() == "loglog")
                    rval.Scaling = Scaling.LogLog;
                else if (Lookup("scaling").ToLower() == "linear")
                    rval.Scaling = Scaling.Linear;

                rval.Title = Lookup("Title");
                rval.Offset = ReadDouble("offset", 0);
                rval.Points = ReadPoints();
                

                return rval;
            }
        }

        private double ReadDouble(string name, double value =0 )
        {
            var s = Lookup("offset", "");
            var rval = value;
            double x;
            if (double.TryParse(s, out x))
            {
                rval = x;
            }
            return rval;
        }

        private string Lookup(string name, string defaultValue="")
        {
            var pattern = "^" + name + @"\s+([\s0-9a-zA-Z]+)";
            Regex re = new Regex(pattern,RegexOptions.IgnoreCase);
            foreach  (string s in textBoxPoints.Lines)
            {
               var m = re.Match(s);
                if(m.Success)
                {
                    return m.Groups[1].Value;
                }
            }

            return defaultValue;
        }

        /// <summary>
        /// # stage    flow     label
        ///1.76   40.4   1995
        ///9.89	9390  1980
        ///10.64   11500  2017
        /// </summary>
        /// <returns></returns>
        private ObservationPoint[] ReadPoints()
        {
            var rval = new List<ObservationPoint>();

            foreach (string s in textBoxPoints.Lines)
            {
                var tokens = s.Trim().Split(new char[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);

                double x, y;
                if (tokens.Length >= 2 && double.TryParse(tokens[0],out x) 
                    && double.TryParse(tokens[1],out y)
                  ) 
                {
                    ObservationPoint pt = new ObservationPoint();
                    pt.x = x;
                    pt.y = y;
                    if (tokens.Length >= 3)
                        pt.tag = tokens[2];
                    rval.Add(pt);
                }
            }

            return rval.ToArray();
        }

        public string[] Lines {
            get { return this.textBoxPoints.Lines; }
            set { this.textBoxPoints.Lines = value; }
        }
    }
}

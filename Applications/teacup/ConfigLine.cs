using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Teacup
{
    class ConfigLine
    {
        string line;
        public Int32 row = 0;
        public Int32 col = 0;
        public string cbtt = "";
        public string pcode = "";
        public Int32 size = 0;
        public double capacity = 0;
        public string ResName = "";
        public string type = "";
        public string units = "";
//This class looks into the type of config line that is being read and responds accordingly
        public ConfigLine(string line)
        {
            this.line = line;
            line = line.Replace("\t", "    ");
            string[] x = System.Text.RegularExpressions.Regex.Split(line.Trim(), @"\s+");
            type = x[0];
            col = Convert.ToInt32(x[1]); //x
            row = Convert.ToInt32(x[2]); //y
            cbtt = x[3];
            pcode = x[4];

            if (IsTeacup)
            {
                size = Convert.ToInt32(x[5]);
                capacity = Convert.ToDouble(x[6]);
                if (x.Length >= 9)
                {
                    ResName = x[7] + " " + x[8];
                }
                else
                {
                    ResName = x[7];
                }

            }

            if (IsLine)
            {
                ResName = line.Substring(31);
                units = x[5];
            }
        }

        public bool IsTeacup
        {
            get
            {
                return line.ToLower().IndexOf("teacup") == 0;
            }
        }
        
        public bool IsCFS
        {
            get
            {
                return line.ToLower().IndexOf("cfs") == 0;
            }
        }
        
        public bool IsLine
        {
            get
            {
                return line.ToLower().IndexOf("line") == 0;
            }
        }

    }
}

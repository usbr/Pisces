using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Reclamation.TimeSeries.Forms
{
    public partial class TimeSelectorBeginEndWaterYear : UserControl
    {
        public TimeSelectorBeginEndWaterYear()
        {
            InitializeComponent();
        }


        /// <summary>DateTime of beginning</summary>
        public DateTime T1
        {
            get { // 2011 in text box then return 10/1/2010 
                int y = 2011;
                if (!int.TryParse(textBoxT1.Text.Trim(), out y))
                {
                    y = WaterYear.CurrentWaterYear();
                }
                return new DateTime(y-1,10, 1);
                
            }
            set { // given 10/1/2010  enter 2011 in text box
                textBoxT1.Text = ( WaterYear.BeginningOfWaterYear(value).Year + 1).ToString();
            }
        }
        /// <summary>DateTime of ending</summary>
        public DateTime T2
        {
            get
            {
                int y = 2011;
                if (!int.TryParse(textBoxT2.Text.Trim(), out y))
                {
                    y = DateTime.Now.Year;
                }
                return new DateTime(y , 9,30);

            }
            set
            {
                textBoxT2.Text = WaterYear.EndOfWaterYear(value).Year.ToString();
            }
        }

    }
}

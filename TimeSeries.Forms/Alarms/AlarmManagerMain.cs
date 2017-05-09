using Reclamation.Core;
using Reclamation.TimeSeries.Alarms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Reclamation.TimeSeries.Forms.Alarms
{
    public partial class AlarmManagerMain : Form
    {
        public AlarmManagerMain()
        {
            InitializeComponent();
        }
        
        public AlarmManagerMain(BasicDBServer svr)
        {
            AlarmManagerControl c = new AlarmManagerControl(svr);
            this.Controls.Add(c);
            c.Dock = DockStyle.Fill;

        }

        

    }
}

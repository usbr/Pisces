using Reclamation.Core;
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
        BasicDBServer m_svr;
        public AlarmManagerMain(BasicDBServer svr)
        {
            m_svr = svr;
            InitializeComponent();
            SoundFiles s = new SoundFiles(svr);

            this.tabPageSounds.Controls.Add(s);
            s.Dock = DockStyle.Fill;

        }

        

    }
}

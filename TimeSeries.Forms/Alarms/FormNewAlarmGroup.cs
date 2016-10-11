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
    public partial class FormNewAlarmGroup : Form
    {
        public FormNewAlarmGroup()
        {
            InitializeComponent();
        }

        public string AlarmGroupText
        {
            get
            {
                return this.textBox1.Text;
            }
        }
            
    }
}

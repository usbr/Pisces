using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Reclamation.Core;
using Reclamation.TimeSeries.Hydromet;

namespace HydrometTools.Advanced
{
    public partial class DayfileInsert : UserControl
    {
        public DayfileInsert()
        {
            InitializeComponent();
            //this.dateTimePicker1.MinDate = DateTime.Now.AddMonths(-6);
            this.dateTimePicker1.MaxDate = DateTime.Now.AddDays(1);
            this.dateTimePicker1.Value = DateTime.Now.Date; // midnight.

            //this.dateTimePicker2.MinDate = DateTime.Now.AddMonths(-6);
            this.dateTimePicker2.MaxDate = DateTime.Now.AddDays(1);
            dateTimePicker2.Value = DateTime.Now.Date.AddHours(23).AddMinutes(59);
            domainUpDown1.SelectedIndex = 2;
        }

        DateTime T1
        {
            get { return dateTimePicker1.Value; }
        }

        DateTime T2
        {
            get { return dateTimePicker2.Value; }
        }
        string Cbtt { get { return textBoxCbtt.Text; } }


        private void buttonRun_Click(object sender, EventArgs e)
        {
              Login login = new Login();
             if (login.ShowDialog() != DialogResult.OK)
                 return;

             try
             {
                 Cursor = Cursors.WaitCursor;

                 int increment = Convert.ToInt32(this.domainUpDown1.SelectedItem.ToString());
                 string status = HydrometEditsVMS.InsertDayFileValue(login.Username, login.Password, T1, T2,textBoxCbtt.Text,textBoxPcode.Text, 0,increment);
                 FormStatus.ShowStatus(status);
                 Logger.WriteLine("", "ui");
             }
             finally
             {
                 Cursor = Cursors.Default;
             }
         
        }
    }
}

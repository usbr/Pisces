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

namespace HydrometTools
{
    /// <summary>
    /// UI to input range of dates and cbtt to reprocess
    /// satellite data.  Limted to last 6 months.
    /// </summary>
    public partial class RawDataInput : UserControl
    {
        public RawDataInput()
        {
            InitializeComponent();
            this.dateTimePicker1.MinDate = DateTime.Now.AddMonths(-6);
            this.dateTimePicker1.MaxDate = DateTime.Now.AddDays(1);
            this.dateTimePicker1.Value = DateTime.Now.Date; // midnight.

            this.dateTimePicker2.MinDate = DateTime.Now.AddMonths(-6);
            this.dateTimePicker2.MaxDate = DateTime.Now.AddDays(1);
            dateTimePicker2.Value = DateTime.Now.Date.AddHours(23).AddMinutes(59);
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

        private void buttonRunRawData_Click(object sender, EventArgs e)
         {
             Login login = new Login();
             if (login.ShowDialog() != DialogResult.OK)
                 return;

             try
             {
                 Cursor = Cursors.WaitCursor;

                 string status = HydrometEditsVMS.RunRawData(login.Username, login.Password, Cbtt, T1, T2);
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

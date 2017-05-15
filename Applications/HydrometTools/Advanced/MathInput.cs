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
    /// satellite data.  
    /// </summary>
    public partial class MathInput : UserControl
    {
        public MathInput()
        {
            InitializeComponent();
            //this.dateTimePicker1.MinDate = DateTime.Now.AddMonths(-6);
            this.dateTimePicker1.MinDate = DateTime.Now.AddYears(-60); // for Chuck GP REgion
            this.dateTimePicker1.MaxDate = DateTime.Now.AddDays(1);
            this.dateTimePicker1.Value = DateTime.Now.Date; // midnight.

            this.dateTimePicker2.MinDate = DateTime.Now.AddYears(-60);
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
        string PcodeIn { get { return textBoxPcodeIn.Text; } }
        string PcodeOut { get { return textBoxPcodeOut.Text; } }

        private void buttonRunMath_Click(object sender, EventArgs e)
        {
            Login login = new Login();
            if (login.ShowDialog() != DialogResult.OK)
                return;

            try
            {
                Cursor = Cursors.WaitCursor;

                string status = HydrometEditsVMS.RunRatingTableMath(login.Username, login.Password, Cbtt, PcodeIn, PcodeOut, T1, T2,checkBoxACE.Checked);

                if (checkBoxArchiver.Checked)
                {
                    status += "\n" + HydrometEditsVMS.RunArchiver(login.Username, login.Password, new string[] { Cbtt }, PcodeOut, T1, T2, false);
                }
                
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

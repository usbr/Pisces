﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Reclamation.Core;
using Reclamation.TimeSeries.Hydromet;
using System.IO;


namespace HydrometTools.Advanced
{
    /// <summary>
    /// UI to input range of dates and cbtt to reprocess
    /// satellite data.  Limted to last 6 months.
    /// </summary>
    public partial class ArchiverFileList : UserControl
    {
        public ArchiverFileList()
        {
            InitializeComponent();
            this.dateTimePicker1.MinDate = new DateTime(1980, 1, 1);
            this.dateTimePicker1.MaxDate = DateTime.Now.Date.AddDays(-1);
            this.dateTimePicker1.Value = DateTime.Now.Date.AddDays(-1); // midnight.

            this.dateTimePicker2.MinDate = new DateTime(1980, 1, 1);
            this.dateTimePicker2.MaxDate = DateTime.Now.Date.AddDays(-1);
            dateTimePicker2.Value = DateTime.Now.Date.AddDays(-1);
        }

         DateTime T1
        {
            get { return dateTimePicker1.Value; }
        }

         DateTime T2
        {
            get { return dateTimePicker2.Value; }
        }
         string Cbtt { get { return textBoxCbttFilename.Text; } }

         string Pcode { get { return this.textBoxPcode.Text; } }


        private void buttonRunRawData_Click(object sender, EventArgs e)
         {

             if (!File.Exists(this.textBoxCbttFilename.Text))
             {
                 MessageBox.Show("File does not exist");
                 return;
             }

             TextFile tf = new TextFile(this.textBoxCbttFilename.Text);


             Login login = new Login();
             if (login.ShowDialog() != DialogResult.OK)
                 return;

             try
             {
                 Cursor = Cursors.WaitCursor;
                 string pc = Pcode;
                 if (checkBoxAllPcodes.Checked)
                     pc = "ALL";

                 string status = HydrometEditsVMS.RunArchiver(login.Username, login.Password, tf.FileData, pc, T1, T2,checkBoxPreview.Checked);
                 FormStatus.ShowStatus(status);
                 Logger.WriteLine("", "ui");
             }
             finally
             {
                 Cursor = Cursors.Default;
                 checkBoxAllPcodes.Checked = false;
             }
         }

        private void buttonHelpArchiver_Click(object sender, EventArgs e)
        {
            var f = new ArchiverPcodeHelp();
            f.Show();
        }

        private void checkBoxAllPcodes_CheckedChanged(object sender, EventArgs e)
        {
            textBoxPcode.Enabled = !checkBoxAllPcodes.Checked;
        }

        private void buttonOpenFile_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
               this.textBoxCbttFilename.Text =   openFileDialog1.FileName;
            }
        }

    }
}

using Reclamation.Core;
using Reclamation.TimeSeries.Import;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Reclamation.TimeSeries.Forms
{
    public partial class BulkImportForm : Form
    {
        public BulkImportForm()
        {
            InitializeComponent();
            
            Logger.OnLogEvent += Logger_OnLogEvent;
        }

        void Logger_OnLogEvent(object sender, StatusEventArgs e)
        {
            var msg = e.Message;
            if (msg.Contains("importing ["))
            {
                var s = msg.Substring(11, msg.IndexOf("]") - 11);

                int idx = 0;
                dataGridView1.ClearSelection();
                if( int.TryParse(s,out idx))
                {
                    dataGridView1.CurrentCell = dataGridView1.Rows[idx].Cells[0];
                    dataGridView1.Rows[idx].Cells[0].Value = "ok";
                    //dataGridView1.Rows[idx].Selected = true;
                    Application.DoEvents();
                }
            }
        }

        public event EventHandler ImportClick;

        private void linkLabelSelectDirectory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            var path = UserPreference.Lookup("bulk_import_folder");
            if (Directory.Exists(path))
                dlg.SelectedPath = path;

            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.linkLabelSelectDirectory.Text = dlg.SelectedPath;
                buttonApplyFilter_Click(this, EventArgs.Empty);
            }
        }

        public string SelectedPath
        {
            get { return linkLabelSelectDirectory.Text; }
            set { linkLabelSelectDirectory.Text = value; }
        }


        private void buttonApplyFilter_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(SelectedPath))
                return;

            DirectoryScanner di = new DirectoryScanner(SelectedPath, this.textBoxFilter.Text, this.comboBoxRegex.Text);
            this.dataGridView1.Columns.Clear();
            if (this.comboBoxRegex.Text.Trim() == "")
                SetupGridBasic(di.Files);
            else
                SetupGridRegex(di);
            if (di.Files.Length > 0)
                buttonImport.Enabled = true;
        }

        private void SetupGridBasic(string[] files)
        {
            dataGridView1.Columns.Add("status", "status");
            dataGridView1.Columns.Add("filename", "filename");
            dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            for (int i = 0; i < files.Length; i++)
            {
                dataGridView1.Rows.Add(new object[] { "", files[i] });
            }
        }

        private void SetupGridRegex(DirectoryScanner di )
        {
            dataGridView1.Columns.Add("status", "status");
            dataGridView1.Columns.Add("scenario", "scenario");
            dataGridView1.Columns.Add("siteid", "siteid");
            dataGridView1.Columns.Add("filename", "filename");
            dataGridView1.Columns["filename"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            for (int i = 0; i < di.Files.Length; i++)
            {
              dataGridView1.Rows.Add(new object[] { "",di.Scenario[i],
                                                     di.Siteid[i], 
                                                     di.Files[i] });
            }
        }

        public string Filter
        {
            get {return this.textBoxFilter.Text ;}
            set {this.textBoxFilter.Text = value;}
        }

        public string RegexFilter
        {
            get { return this.comboBoxRegex.Text; }
        }


        private void buttonImport_Click(object sender, EventArgs e)
        {
             EventHandler handler = ImportClick;
            if( handler != null)
            {
                handler(this, e);
            }
        }

        private void BulkImportForm_Load(object sender, EventArgs e)
        {
            buttonApplyFilter_Click(this, EventArgs.Empty);
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://regexstorm.net/");
        }
    }
}

using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Reclamation.TimeSeries;
using Reclamation.TimeSeries.Hydromet;
using Reclamation.Core;
using System.Text.RegularExpressions;

namespace HydrometPisces
{
    public partial class HydrometCommandLine : UserControl
    {

        string historyFilename = System.IO.Path.GetTempPath() + "\\pisces_expert_history.txt";
        public HydrometCommandLine()
        {
            InitializeComponent();
            comboBoxServer.SelectedIndex = 0;
            ReadHistory();
        }

        public event EventHandler OnSubmit;

        private void button1_Click(object sender, EventArgs e)
        {
            if (OnSubmit != null)
            {
                OnSubmit(this, EventArgs.Empty);
            }
        }

        private HydrometHost Server
        {
            get
            {
                if (comboBoxServer.SelectedIndex == 0)
                    return HydrometHost.PN;
                if (comboBoxServer.SelectedIndex == 1)
                    return HydrometHost.Yakima;

                return HydrometHost.GreatPlains;
            }
    }

       
        public Series[] SelectedSeries
        {
            get
            {
                string query = HydrometInfoUtility.ExpandQuery(this.textBox1.Text.Trim(), TimeInterval.Daily);
                CommandLine cmd = new CommandLine(query, TimeInterval.Daily);

                var rval = cmd.CreateSeries(Server).ToArray();

                if (rval.Length > 0)
                {
                   AddToHistory(this.textBox1.Text);
                }

                return rval;
            }
        }


        /// <summary>
        /// adds user input to history,unless allready there.
        /// </summary>
        /// <param name="s"></param>
        private void AddToHistory(string s)
        {
            if (!listBox1.Items.Contains(s))
            {
                listBox1.Items.Insert(0, s);
            }
            string[] vals = new string[listBox1.Items.Count];

            listBox1.Items.CopyTo(vals, 0);

            File.WriteAllLines(historyFilename, vals);
        }

        

        private void ReadHistory()
        {
            this.listBox1.Items.Clear();
            if( System.IO.File.Exists(historyFilename))
            {
                this.listBox1.Items.AddRange(System.IO.File.ReadAllLines(historyFilename));
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex >= 0)
            {
                this.textBox1.Text = listBox1.SelectedItem.ToString();
            }
        }

       

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                button1_Click(this, EventArgs.Empty);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Clear History?", "Clear History?", MessageBoxButtons.OKCancel) != DialogResult.OK)
            {
                return;
            }
            if( File.Exists(historyFilename))
            {
                System.IO.File.Delete(historyFilename);
            }
            this.listBox1.Items.Clear();
        }
    }
}

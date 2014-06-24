using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Reclamation.TimeSeries;

namespace Reclamation.TimeSeries.Forms
{
    public partial class SeriesListSumForm : Form
    {
        public DateTime t1;
        public DateTime t2;
        public string sName;
        public SeriesListSumForm(DateTime bt, DateTime et, string[] seriesList)
        {
            InitializeComponent();
            sumTimeSelector.T1 = bt;
            sumTimeSelector.T2 = et;
            for(int i=0; i<seriesList.Length; i++)
            {
            ListOfSeries.Items.Add(seriesList[i]);
            }
        }
        private void buttonSum_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("Name for the SeriesList sum is needed");
            }
            sName = textBox1.Text;
            t1 = sumTimeSelector.T1;
            t2 = sumTimeSelector.T2;
            Close();
        }

        private void button1Cancel_Click(object sender, EventArgs e)
        {
            Close();
        }

    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Reclamation.Core
{
    public partial class LoggerView : Form
    {
        public LoggerView()
        {
            InitializeComponent();
        }

        private void richTextBox1_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            try
            {
                //System.Diagnostics.Process.Start(e.LinkText);
                //this.richTextBox1.AutoWordSelection = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void LoggerView_Load(object sender, EventArgs e)
        {
            buttonRefresh_Click(this, EventArgs.Empty);
        }

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            this.richTextBox1.Lines = Reclamation.Core.Logger.LogHistory.ToArray();
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            Reclamation.Core.Logger.LogHistory.Clear();
            buttonRefresh_Click(this, EventArgs.Empty);

        }

        private void richTextBox1_Click(object sender, EventArgs e)
        {
        }
    }
}
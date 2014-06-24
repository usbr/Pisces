using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace Reclamation.TimeSeries.Forms.Calculations
{
    /// <summary>
    /// TextBox that allows dragging Pisces object into it.
    /// user uses this to define simple algebra using the mouse
    /// </summary>
    public partial class PiscesObjectTextBox : UserControl
    {
        int m_id = -1;
        Series m_series = null;

        public Series Series
        {
            get { return m_series; }
        }
        public int ID
        {
            get { return m_id; }
        }

        public bool HasData
        {
            get { return this.textBox1.Text.Trim() != ""; }
        }

        public PiscesObjectTextBox()
        {
            InitializeComponent();
        }

        private void textBox1_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
            if (e.Data.GetDataPresent("Aga.Controls.Tree.TreeNodeAdv[]"))
            {
                e.Effect = DragDropEffects.Move;
            }
        }


        private void textBox1_DragDrop(object sender, DragEventArgs e)
        {
            Aga.Controls.Tree.TreeNodeAdv[] nodes = e.Data.GetData("Aga.Controls.Tree.TreeNodeAdv[]") as Aga.Controls.Tree.TreeNodeAdv[];

            var po = nodes[0].Tag as PiscesObject;

            if (po is Series)
            {
                m_series = po as Series;
                m_id = po.ID;
                this.textBox1.Text = " [" + m_id + "]" + po.Name;
            }
            else
            {
                textBox1.Text = "";
            }

        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Reclamation.TimeSeries.Parser;
using System.Linq;

namespace Reclamation.TimeSeries.Forms.Calculations
{
    public partial class BasicEquation : UserControl
    {

        List<string> functionHelp = new List<string>();
        public BasicEquation()
        {
            InitializeComponent();
            this.listBoxFunctions.Items.Clear();
            this.labelToolTip.Text = "";
            var fa = ParserFunction.GetPiscesFunctionAttributes();

            listBoxFunctions.Items.AddRange((from a in fa select a.Example).ToArray<string>());
            functionHelp.AddRange((from a in fa select a.Description).ToArray<string>());
            this.comboBoxInterval.SelectedItem = TimeInterval.Daily.ToString();
        }

        public string SeriesExpression
        {
            get { return this.textBoxMath.Text; }
            set { this.textBoxMath.Text = value; }
        }

        public bool Calculate
        {
            get { return this.checkBoxCompute.Checked; }
        }
        public string SeriesName
        {
            get { return this.textBoxSeriesName.Text; }
            set { this.textBoxSeriesName.Text = value; }
        }
        public string Units
        {
            get { return this.comboBoxUnits.Text; }
            set { this.ComboBoxUnits.Text = value; }
        }
        public ComboBox ComboBoxUnits
        {
            get { return this.comboBoxUnits; } 
        }


        public TimeInterval TimeInterval
        {
            get
            {
                TimeInterval rval = (TimeInterval)System.Enum.Parse(typeof(TimeInterval), comboBoxInterval.SelectedItem.ToString());
                return rval;
            }
            set
            {
                comboBoxInterval.SelectedValue = value.ToString();
            }
        }

        private void textBoxMath_DragEnter_1(object sender, DragEventArgs e)
        {

            e.Effect = DragDropEffects.Move;
            if (e.Data.GetDataPresent("Aga.Controls.Tree.TreeNodeAdv[]"))
            {
                e.Effect = DragDropEffects.Move;
            }
        }

        

        private void textBoxMath_DragDrop_1(object sender, DragEventArgs e)
        {

            string fmt = "Aga.Controls.Tree.TreeNodeAdv[]";
            if (e.Data.GetDataPresent(fmt))
            {
                Aga.Controls.Tree.TreeNodeAdv[] nodes = e.Data.GetData(fmt) as Aga.Controls.Tree.TreeNodeAdv[];
                
                var po = nodes[0].Tag as PiscesObject;
                if (po.Name.Length > 0 && Char.IsDigit(po.Name[0]))
                {
                    MessageBox.Show("Error: Names cannot begin with a number when used in equations.");
                    return;
                }
                textBoxMath.SelectionLength = 0;
                textBoxMath.SelectedText = SurroundWithQuotesIfNeeded(po.Name);
            }
            else // copy function call
                if (e.Data.GetDataPresent(DataFormats.StringFormat))
                {
                    string str = (string)e.Data.GetData( DataFormats.StringFormat);
                    textBoxMath.SelectionLength = 0;
                    textBoxMath.SelectedText = str;
                }

        }

        private string SurroundWithQuotesIfNeeded(string str)
        {
            if (str.Trim().IndexOf(' ')>0)
                return "'" + str + "'";
            return str;
        }

        /// <remarks>
        /// GetCharIndexFromPosition is missing one caret position, as there is one extra caret
        /// position than there are characters (an extra one at the end).
        /// </remarks>
        private int GetCaretIndexFromPoint(TextBox tb, int x, int y) 
        {

            System.Drawing.Point p = tb.PointToClient(new System.Drawing.Point(x, y)); 
            int i = tb.GetCharIndexFromPosition(p); 
            if (i == tb.Text.Length - 1) 
            { 
                System.Drawing.Point c = tb.GetPositionFromCharIndex(i); 
                if (p.X > c.X)                 
                    i++; 
            } 
            return i; 
        }


        /// <summary>
        /// Gives visual feedback where the dragged text will be dropped.
        /// </summary>
        private void textBoxMath_DragOver(Object sender, System.Windows.Forms.DragEventArgs e)
        {
            // fake moving the text caret
            textBoxMath.SelectionStart = GetCaretIndexFromPoint(textBoxMath, e.X, e.Y);
            textBoxMath.SelectionLength = 0;
            // don't forget to set focus to the text box to make the caret visible!
            textBoxMath.Focus();
        }

        private void listBoxFunctions_MouseDown(object sender, MouseEventArgs e)
        {
            if (listBoxFunctions.Items.Count == 0)
                return;

            int index = listBoxFunctions.IndexFromPoint(e.X, e.Y);
            if (index >= 0 && index <= listBoxFunctions.Items.Count)
            {
                string s = listBoxFunctions.Items[index].ToString();
                DragDropEffects dde1 = DoDragDrop(s,
                    DragDropEffects.All);

                labelToolTip.Text = functionHelp[index];
            }

        }


       
    }
}

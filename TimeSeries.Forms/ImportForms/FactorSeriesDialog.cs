using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Reclamation.TimeSeries.Forms.ImportForms
{
    public partial class FactorSeriesDialog : Form
    {
        public DateTime Time1
        {
            get { return this.dateTimePicker1.Value; }
            set { this.dateTimePicker1.Value = value; }
        }
        public DateTime Time2
        {
            get { return this.dateTimePicker2.Value; }
            set { this.dateTimePicker2.Value = value; }
        }
        public string SeriesName
        {
            get { return this.textBox1.Text; }
            set { this.textBox1.Text = value; }
        }
        public float Factor
        {
            get { return Convert.ToSingle(this.textBox2.Text);}
            set { this.textBox2.Text = value.ToString(); }
        }

        public FactorSeriesDialog(Series old)
        {
            InitializeComponent();
            dateTimePicker1.Value = old.MinDateTime;
            dateTimePicker2.Value = old.MaxDateTime;
            textBox1.Text = "Factored " + old.Name;
            textBox2.Text = Convert.ToString(1.0);
        }
    }
}
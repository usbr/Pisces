using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace HydrometTools.Graphing
{
    public partial class EditStringCollection : Form
    {
        public EditStringCollection()
        {
            InitializeComponent();
        }

        public string[] Items
        {

            get {
                return this.textBox1.Lines;
            }
            set { this.textBox1.Lines = value; }
        }
    }
}

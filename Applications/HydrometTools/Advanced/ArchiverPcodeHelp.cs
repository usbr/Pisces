using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace HydrometTools.Advanced
{
    public partial class ArchiverPcodeHelp : Form
    {
        public ArchiverPcodeHelp()
        {
            InitializeComponent();
            if (File.Exists("Archiver_help.rtf"))
                this.richTextBox1.LoadFile("Archiver_help.rtf");

        }
    }
}

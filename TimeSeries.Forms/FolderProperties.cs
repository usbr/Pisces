using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Reclamation.TimeSeries.Forms
{
    public partial class FolderProperties : Form
    {
        PiscesFolder folder;
        public FolderProperties()
        {
            InitializeComponent();
        }

        public FolderProperties(PiscesFolder folder)
        {
            InitializeComponent();
            this.folder = folder;
            this.Text = folder.Name + " Properties";
            this.textBoxName.Text = folder.Name;
            labelInfo.Text = "id = " + folder.ID + " parentid = " + folder.ParentID;
            
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            folder.Name = this.textBoxName.Text;
        }

    }
}

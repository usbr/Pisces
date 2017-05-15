using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Reclamation.Core;

namespace HydrometTools
{
    public partial class FormStatus : Form
    {
        public FormStatus()
        {
            InitializeComponent();
        }

        public string[] Lines
        {
            set { this.richTextBox1.Lines = value; }
        }

        public bool HideDialogNextTime
        {
            get { return this.checkBox1.Checked; }
        }


        public static void ShowStatus(string status, bool showAllways=false)
        {
            var lines = status.Split('\n');
            foreach (var item in lines)
            {
                Logger.WriteLine(item);
            }

            if (UserPreference.Lookup("HideStatusDialog") != "True"  || showAllways)
            {
                var f = new FormStatus();
                f.Lines = lines;
                f.ShowDialog();

                if (f.HideDialogNextTime)
                    UserPreference.Save("HideStatusDialog", "True");
                else
                    UserPreference.Save("HideStatusDialog", "False");
            }
        }

    }
}

using Reclamation.TimeSeries;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Look
{
    public partial class LookForm : Form
    {
        public LookForm()
        {
            InitializeComponent();
        }
        public string PassSearch
        {
            get
            {
                if (newLook1.PassSearch != "")
                {
                    return newLook1.PassSearch;
                }
                else
                {
                    return "";
                }
            }
        }
        public TimeInterval DataType
        {
            set 
            { 
                newLook1.DataType = value;
            }
        }
    }
}

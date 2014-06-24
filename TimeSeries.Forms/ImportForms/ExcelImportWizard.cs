using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Reclamation.TimeSeries.Forms.ImportForms
{
    public enum ExcelImportType { Standard, WaterYear, Database };

    public partial class ExcelImportWizard : Form
    {
        public ExcelImportWizard()
        {
            InitializeComponent();
        }

        public ExcelImportType ImportType
        {
            get
            {
                if (radioButtonDatabase.Checked)
                    return ExcelImportType.Database;
                if (radioButtonStandard.Checked)
                    return ExcelImportType.Standard;
                if (radioButtonWaterYear.Checked)
                    return ExcelImportType.WaterYear;

                throw new NotImplementedException();
            }
        }
    }
}

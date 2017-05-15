using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Reclamation.TimeSeries;

namespace HydrometTools
{
    public partial class TimeSeriesSpreadsheet : UserControl, ITimeSeriesSpreadsheet
    {
        public TimeSeriesSpreadsheet()
        {
            InitializeComponent();
        }

        public bool AutoFlagDayFiles
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                //throw new NotImplementedException();
            }
        }

        public void Clear()
        {

        }

        public void SetCellValue(int rowIndex, int columnIndex, double val)
        {
            //throw new NotImplementedException();
        }

        public void SetDataTable(DataTable tbl, TimeInterval db, bool scrollToTop)
        {
            dataGridView1.DataSource = tbl;
        }

        public bool SuspendUpdates
        {
            get { throw new NotImplementedException();}
        }

        public event EventHandler<EventArgs> UpdateCompleted;

        private void HandleUpdateCompleted()
        {
            if (UpdateCompleted != null)
            {

            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Reclamation.Core;

namespace Reclamation.TimeSeries.Forms
{
    public partial class MonthRangePicker : UserControl
    {
        int[] months;
        int[] monthIndex;
        public MonthRangePicker()
        {
            months = new int[12];
            monthIndex = new int[13];
            InitializeComponent();
            UpdatePicker();

            dataGridView1.SelectionChanged += new EventHandler(dataGridView1_SelectionChanged);
        }

        void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.ColumnCount < 12 || dataGridView1.RowCount <1)
                return;
            int idx1 = -1;
            int idx2 = -1;
            for (int i = 0; i < 12; i++)
            {
                if (dataGridView1[i, 0].Selected && idx1 == -1)
                {
                    idx1 = i;
                    idx2 = idx1;
                }
                else if( dataGridView1[i,0].Selected )
                {
                    idx2 = i;
                }
            }
            if(idx1 >=0)
            m_monthDayRange = new MonthDayRange(this.months[idx1], 1,
                                                this.months[idx2], DateTime.DaysInMonth(2000, months[idx2]));
        }

        int m_beginningMonth = 10;
        public int BeginningMonth
        {
            set
            {
                if (value != m_beginningMonth)
                {
                    m_beginningMonth = value;
                    UpdatePicker();
                }
            }
            get
            {
                return m_beginningMonth;
            }
        }

        private void UpdatePicker()
        {
            DateTime t = new DateTime(2004, m_beginningMonth, 1);
            this.dataGridView1.Columns.Clear();
            for (int i = 0; i < 12; i++)
            {
                DataGridViewTextBoxColumn c = new DataGridViewTextBoxColumn();
                c.HeaderText = i.ToString();
                c.SortMode = DataGridViewColumnSortMode.NotSortable;
                this.dataGridView1.Columns.Add(c);
            }
            t = new DateTime(2004, m_beginningMonth, 1);
            this.dataGridView1.Rows.Add();
            for (int i = 0; i < 12; i++)
            {
                this.dataGridView1[i, 0].Value = t.ToString("MMM");
                this.dataGridView1[i, 0].Selected = true;
                months[i] = t.Month;
                monthIndex[t.Month] = i;
                t = t.AddMonths(1);
            }
            dataGridView1_SelectionChanged(this, EventArgs.Empty);
        }

        public string ReportRange()
        {
            string rval = "";
            DataGridViewSelectedCellCollection col= dataGridView1.SelectedCells;
            foreach (DataGridViewCell c in col)
            {
                Console.WriteLine(c.Value.ToString());
                rval += c.Value.ToString() + ", ";
            }
            return m_monthDayRange.ToString() ;
        }


        private MonthDayRange m_monthDayRange;

        public MonthDayRange MonthDayRange
        {
            get {
                
                return m_monthDayRange; 
            }
            set {
                for (int i = 0; i < 12; i++)
                {
                    dataGridView1[i, 0].Selected = false;
                }
               
                for (int i = monthIndex[value.Month1]; i <= monthIndex[value.Month2]; i++)
			{
                dataGridView1[i, 0].Selected = true;
			}

              m_monthDayRange = value; 
              }
        }



        private void dataGridView1_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            dataGridView1[e.ColumnIndex, 0].Selected = true;
        }

        private void dataGridView1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            
        }

        private void MonthRangePicker_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible)
                BeginningMonth = m_beginningMonth;
        }

    }
}

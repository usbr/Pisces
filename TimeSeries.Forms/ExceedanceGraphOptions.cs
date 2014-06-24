using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;


namespace Reclamation.TimeSeries.Forms
{
    public partial class ExceedanceGraphOptions : UserControl
    {
        public ExceedanceGraphOptions()
        {
            InitializeComponent();
        }


        //public MonthPicker MonthPicker
        //{
        //    get { return this.monthPicker1; }
        //}

        
        public AggregateOption AggregateOptions
        {
            get { return this.aggregateOptions1; }
        }

       
       
    }
}

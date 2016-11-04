using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Reclamation.TimeSeries.RatingTables;

namespace Reclamation.TimeSeries.Forms.RatingTables
{
    public partial class RatingEquationBuilder : UserControl
    {
        HydrographyDataSet m_ds;
        public RatingEquationBuilder(HydrographyDataSet ds,int equation_id)
        {
            InitializeComponent();
            m_ds = ds;
        }
        public RatingEquationBuilder()
        {
            InitializeComponent();
            throw new Exception("Invalid Constructor");
        }
    }
}

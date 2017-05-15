using System;
using System.Windows.Forms;

namespace Reclamation.TimeSeries.Graphing
{
    public partial class RatingTableGraph : UserControl
    {
        public RatingTableGraph()
        {
            InitializeComponent();
        }

        public TimeSeriesDatabaseDataSet.RatingTableDataTable[] RatingTable
        {

            set
            {
                LoadRatingTable(value);
            }
        }

        private void LoadRatingTable(TimeSeriesDatabaseDataSet.RatingTableDataTable[] ratingTables)
        {
            tChart1.Series.Clear();
            
            var ratingTable = ratingTables[0];
            // label axis using first series only
            tChart1.Axes.Bottom.Title.Text = ratingTable.XUnits;
            tChart1.Axes.Left.Title.Text = ratingTable.YUnits;
            tChart1.Legend.Visible = true;
            for (int j = 0; j < ratingTables.Length; j++)
            {
                var s = new Steema.TeeChart.Styles.Line();
                tChart1.Series.Add(s);
                s.Title = ratingTables[j].Name;
                s.ShowInLegend = true;
                ratingTable = ratingTables[j];
                for (int i = 0; i < ratingTable.Count; i++)
                {
                    double x = ratingTable[i].x;
                    double y = ratingTable[i].y;
                    s.Add(x, y);
                }
            }

        }

        public void ShowEditor()
        {
            Steema.TeeChart.Editor ed = new Steema.TeeChart.Editor(this.tChart1);
            ed.Title = "Editor";
            ed.ShowModal();
        }
        
    }
}

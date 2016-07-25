using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Reclamation.TimeSeries.Forms.RatingTables
{
    public partial class BasicMeasurementView : UserControl, IExplorerView
    {
        public BasicMeasurementView()
        {
            InitializeComponent();
        }

        public BasicMeasurement Measurement { get; set; }



        SeriesList IExplorerView.SeriesList
        {
            get
            {
                return new SeriesList();
            }
            set
            {
            }
        }

        string IExplorerView.Title
        {
            set { ; }
        }

        string IExplorerView.SubTitle
        {
            set { ; }
        }

        bool IExplorerView.MultipleYAxis
        {
            set { ; }
        }

        DataTable IExplorerView.DataTable
        {
            get
            {
                return new DataTable();
            }
            set
            {
                ;
            }
        }

        AnalysisType IExplorerView.AnalysisType
        {
            set {  ; }
        }

        List<string> IExplorerView.Messages
        {
            get
            {
                return new List<string>();
            }
            set
            {
                ;
            }
        }

        void IExplorerView.Draw()
        {
            this.labelTitle.Text = Measurement.SiteID;
            //this.textBox_Discharge.Text = Measurement.

        }

        bool IExplorerView.UndoZoom
        {
            set {  ; }
        }

        void IExplorerView.Clear()
        {
             ;
        }

    }
}

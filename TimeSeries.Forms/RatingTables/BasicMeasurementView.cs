using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Reclamation.Core;
using Reclamation.TimeSeries.RatingTables;

namespace Reclamation.TimeSeries.Forms.RatingTables
{
    public partial class BasicMeasurementView : UserControl
    {
        public BasicMeasurementView()
        {
            InitializeComponent();
        }

        public BasicMeasurement Measurement { get; set; }

        

        public void Draw()
        {
            this.labelTitle.Text = Measurement.SiteID;
            Logger.WriteLine("Measurement: " + Measurement.SiteID + " " + Measurement.ID);
            if( Measurement.MeasurementRow == null)
                Console.WriteLine("OOPS" +Measurement.MeasurementRow);
            this.textBox_Memo.Text = Measurement.MeasurementRow.notes;
            this.textBox_Party.Text = Measurement.MeasurementRow.party;
            this.textBox_Prim_Gage.Text = Measurement.MeasurementRow.stage.ToString("F2");
            this.textBox_Discharge.Text = Measurement.MeasurementRow.discharge.ToString("F2");
            //this.textBox_Discharge.Text = Measurement.

        }

         

    }
}

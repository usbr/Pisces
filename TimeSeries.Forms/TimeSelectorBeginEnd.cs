using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace Reclamation.TimeSeries.Forms
{
	/// <summary>
	/// TimeSelector us a UserControl that contains two drop down date selectors.
	/// </summary>
	public class TimeSelectorBeginEnd : System.Windows.Forms.UserControl
	{
    public static string TimeFormat = "MM/dd/yyyy HH:mm:ss";
    public static string DateFormat = "MM/dd/yyyy";
    
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.DateTimePicker dateTimePicker2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.DateTimePicker dateTimePicker1;

		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		bool showTime;

		public TimeSelectorBeginEnd()
		{
			InitializeComponent();
			ShowTime = false;
		}

		public bool ShowTime
		{
			get
			{
				return (showTime);
			}
			set
			{
				showTime = value;

				if( value)
				{
					this.dateTimePicker1.CustomFormat = TimeFormat;
					this.dateTimePicker2.CustomFormat = TimeFormat;
				}
				else
				{
					this.dateTimePicker1.CustomFormat = DateFormat;
					this.dateTimePicker2.CustomFormat = DateFormat;
				}
			}
		}

    /// <summary>DateTime of beginning</summary>
		public DateTime T1
		{
			get { return dateTimePicker1.Value;}
			set { dateTimePicker1.Value = value;}
		}
    /// <summary>DateTime of ending</summary>
		public DateTime T2
		{
  		  get { return dateTimePicker2.Value;}
		  set { dateTimePicker2.Value = value;}
		}
		


		/// <summary>
		/// rounds dates set in time pickers 
		/// so the beginning date starts at midnight
		/// and the ending date ends at 1 second before midnight.
		/// useful when not displaying time
		/// </summary>
		public void SetInclusiveDates()
		{
			// set the date to the beginning of the day (midnight)
			DateTime time = dateTimePicker1.Value;
			time = new DateTime(time.Year,time.Month,time.Day,0,0,0);
			dateTimePicker1.Value = time;

			//set the date to the end of the day. (just before midnight)
			 time = dateTimePicker2.Value;
			time = new DateTime(time.Year,time.Month,time.Day,23,59,59);
			dateTimePicker2.Value = time;
		

		}

		public void SetDataTime(DateTime endTime, int daysBetween) // number of days difference between time selectors
		{
			this.dateTimePicker1.Value = endTime.AddDays(-daysBetween);
			this.dateTimePicker2.Value = endTime; 
		}
		

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.label2 = new System.Windows.Forms.Label();
            this.dateTimePicker2 = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(8, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(30, 16);
            this.label2.TabIndex = 24;
            this.label2.Text = "End";
            // 
            // dateTimePicker2
            // 
            this.dateTimePicker2.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker2.Location = new System.Drawing.Point(56, 24);
            this.dateTimePicker2.Name = "dateTimePicker2";
            this.dateTimePicker2.Size = new System.Drawing.Size(130, 20);
            this.dateTimePicker2.TabIndex = 23;
            this.dateTimePicker2.ValueChanged += new System.EventHandler(this.dateTime_ValueChanged);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(8, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 16);
            this.label1.TabIndex = 22;
            this.label1.Text = "Start";
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker1.Location = new System.Drawing.Point(56, 2);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(130, 20);
            this.dateTimePicker1.TabIndex = 21;
            this.dateTimePicker1.ValueChanged += new System.EventHandler(this.dateTime_ValueChanged);
            // 
            // TimeSelectorBeginEnd
            // 
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dateTimePicker2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dateTimePicker1);
            this.Name = "TimeSelectorBeginEnd";
            this.Size = new System.Drawing.Size(195, 46);
            this.ResumeLayout(false);

    }
		#endregion

    private void dateTime_ValueChanged(object sender, System.EventArgs e)
    {
    }


		

		


	}
}

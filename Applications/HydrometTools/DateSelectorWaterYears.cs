using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace HydrometTools
{
	/// <summary>
	/// Summary description for TimeSelectorWaterYears.
	/// </summary>
	public class TimeSelectorWaterYears : System.Windows.Forms.UserControl
	{
		int[] waterYears;
		private System.Windows.Forms.TextBox textBoxWaterYears;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public TimeSelectorWaterYears()
		{
			InitializeComponent();
			// Begin with most current two water years.
			DateTime date = DateTime.Now;

			int yr = date.Year;
			if( date.Month >9)
			{
				yr++;
			}
			this.textBoxWaterYears.Text = (yr-1) +" "+yr;
			Parse();
		}

		/// <summary>
		/// Array of integer water years.
		/// </summary>
		public int[] WaterYears
		{
			get {
				Parse(); 
				return	 waterYears; 
			    }
			set {
				this.waterYears = value;
				int sz = waterYears.Length;
				this.textBoxWaterYears.Text = "";
				for(int i=0; i<sz; i++)
					this.textBoxWaterYears.Text += waterYears[i].ToString()+" ";
			    }
		}

		/// <summary>
		/// parses contents of TextBox 
		/// to build an array of integer water years
		/// </summary>
		void Parse()
		{
			string[] text = this.textBoxWaterYears.Text.Trim().Split(' ');
			ArrayList list = new ArrayList();

			int sz = text.Length;
			for(int i=0; i<sz; i++)
			{
				string s = text[i].Trim();
				if(s!= "")
				{
				int yr = Convert.ToInt32(s);
					list.Add(yr);
				}
			}
			waterYears = new int[list.Count];
			list.CopyTo(waterYears);
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
			this.textBoxWaterYears = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// textBoxWaterYears
			// 
			this.textBoxWaterYears.Anchor = (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.textBoxWaterYears.Name = "textBoxWaterYears";
			this.textBoxWaterYears.Size = new System.Drawing.Size(312, 20);
			this.textBoxWaterYears.TabIndex = 0;
			this.textBoxWaterYears.Text = "";
			// 
			// TimeSelectorWaterYears
			// 
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.textBoxWaterYears});
			this.Name = "TimeSelectorWaterYears";
			this.Size = new System.Drawing.Size(312, 20);
			this.ResumeLayout(false);

		}
		#endregion
	}
}

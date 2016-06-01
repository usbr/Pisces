using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Reclamation.TimeSeries.Forms
{
	/// <summary>
	/// Dialog window showing a <see cref="TimeSelector"/> and an OK Button
	/// </summary>
	public class TimeSelectorForm : System.Windows.Forms.Form
	{
    private TimeSelectorBeginEnd timeSelector1;
    private System.Windows.Forms.Button buttonOk;
    private System.Windows.Forms.Button buttonCancel;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

    public DateTime BeginningTime
    {
        get{ return this.timeSelector1.T1;}
        set{ this.timeSelector1.T1 = value;}
    }
    public DateTime EndingTime
    {
      get{ return this.timeSelector1.T2;}
      set{ this.timeSelector1.T2 = value;}
    }

		public TimeSelectorForm()
		{
			InitializeComponent();
			//this.timeSelector1.Reset(5);
		}

    public void ShowTime()
    {
      this.timeSelector1.ShowTime = true;
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

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.timeSelector1 = new Reclamation.TimeSeries.Forms.TimeSelectorBeginEnd();
            this.buttonOk = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // timeSelector1
            // 
            this.timeSelector1.Location = new System.Drawing.Point(8, 8);
            this.timeSelector1.Name = "timeSelector1";
            this.timeSelector1.ShowTime = false;
            this.timeSelector1.Size = new System.Drawing.Size(192, 48);
            this.timeSelector1.T1 = new System.DateTime(2003, 7, 10, 12, 34, 9, 921);
            this.timeSelector1.T2 = new System.DateTime(2003, 7, 10, 12, 34, 9, 921);
            this.timeSelector1.TabIndex = 0;
            // 
            // buttonOk
            // 
            this.buttonOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOk.Location = new System.Drawing.Point(200, 16);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(75, 23);
            this.buttonOk.TabIndex = 1;
            this.buttonOk.Text = "Ok";
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(200, 48);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 2;
            this.buttonCancel.Text = "Cancel";
            // 
            // TimeSelectorForm
            // 
            this.AcceptButton = this.buttonOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(288, 78);
            this.ControlBox = false;
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.timeSelector1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MinimizeBox = false;
            this.Name = "TimeSelectorForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Dates";
            this.ResumeLayout(false);

    }
		#endregion

    private void buttonOk_Click(object sender, System.EventArgs e)
    {
      Close();
    }
	}
}

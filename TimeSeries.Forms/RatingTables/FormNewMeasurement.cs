using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Reclamation.TimeSeries.Forms
{
  public enum MeasurementType {Manual,AquaCalc,RiverCat};
	/// <summary>
	/// Summary description for FormNewMeasurment.
	/// </summary>
	public class FormNewMeasurment : System.Windows.Forms.Form
	{
    //private string _siteDesc="";
    //MeasurementType measType;
    //private int _measurementNumber;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.ComboBox comboBoxGageIds;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Button buttonSave;
    private System.Windows.Forms.DateTimePicker dateTimePicker1;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.ToolTip toolTip1;
    private System.Windows.Forms.Button buttonCancel;
    private System.ComponentModel.IContainer components;
    private GroupBox groupBox1;
    private Label label5;
    private Label label6;
    private TextBox textBox_Discharge;
    private TextBox textBox_Prim_Gage;
    private Label label3;
    private TextBox textBox_Party;
    private TextBox textBox_Memo;
    private Label label_Party;
    private Label label7;
    private TextBox textBoxQuality;

    TimeSeriesDatabaseDataSet.sitecatalogDataTable m_siteCatalog;

		public FormNewMeasurment( TimeSeriesDatabaseDataSet.sitecatalogDataTable siteCatalog)
		{
			InitializeComponent();
            buttonSave.Enabled = false;
            m_siteCatalog = siteCatalog;
            Prepare();
            this.Text="Input New Measurement";
		}

        public double Flow
        {
            get
            {
                var rval = 0.0;
                double.TryParse(this.textBox_Discharge.Text, out rval);
                return rval;
            }
        }
        public double Stage
        {
            get
            {
                var rval = 0.0;
                double.TryParse(this.textBox_Prim_Gage.Text, out rval);
                return rval;
            }
        }
        public string Notes
        {
            get { return this.textBox_Memo.Text; }
        }

        public string Party
        {
            get { return this.textBox_Party.Text; }
        }

        public string Quality
        {
            get { return this.textBoxQuality.Text; }
        }
        //public string 
    //public string SiteDescription
    //{
    //  get { return this._siteDesc;}
    //  set { 
    //        this._siteDesc = value;
    //        this.comboBoxGageIds.Text =this._siteDesc;
    //      }
    //}
    public DateTime DateTime
    {
      get { return this.dateTimePicker1.Value;}
    }

    public string SiteID
    {
      get { return this.comboBoxGageIds.SelectedValue.ToString();}
    }
    
    private void Prepare()
    {
      this.dateTimePicker1.Value = DateTime.Now;
      this.comboBoxGageIds.DataSource = m_siteCatalog; ;
      this.comboBoxGageIds.DisplayMember="description";
      this.comboBoxGageIds.ValueMember="siteid";
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
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxGageIds = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonSave = new System.Windows.Forms.Button();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.buttonCancel = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.textBox_Discharge = new System.Windows.Forms.TextBox();
            this.textBox_Prim_Gage = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox_Party = new System.Windows.Forms.TextBox();
            this.textBox_Memo = new System.Windows.Forms.TextBox();
            this.label_Party = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.textBoxQuality = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(8, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(280, 44);
            this.label1.TabIndex = 0;
            this.label1.Text = "Enter the following information to create a new measurement record";
            // 
            // comboBoxGageIds
            // 
            this.comboBoxGageIds.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxGageIds.Location = new System.Drawing.Point(118, 63);
            this.comboBoxGageIds.Name = "comboBoxGageIds";
            this.comboBoxGageIds.Size = new System.Drawing.Size(292, 21);
            this.comboBoxGageIds.TabIndex = 1;
            this.comboBoxGageIds.SelectedIndexChanged += new System.EventHandler(this.comboBoxGageIds_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(8, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(104, 20);
            this.label2.TabIndex = 2;
            this.label2.Text = "Location";
            // 
            // buttonSave
            // 
            this.buttonSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSave.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonSave.Location = new System.Drawing.Point(608, 424);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(75, 23);
            this.buttonSave.TabIndex = 3;
            this.buttonSave.Text = "OK";
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.CustomFormat = "MM/dd/yyyy HH:mm";
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker1.Location = new System.Drawing.Point(118, 90);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(136, 20);
            this.dateTimePicker1.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(8, 90);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(92, 20);
            this.label4.TabIndex = 6;
            this.label4.Text = "Date/Time";
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(608, 460);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 7;
            this.buttonCancel.Text = "Cancel";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.textBoxQuality);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.textBox_Party);
            this.groupBox1.Controls.Add(this.textBox_Memo);
            this.groupBox1.Controls.Add(this.label_Party);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.textBox_Discharge);
            this.groupBox1.Controls.Add(this.textBox_Prim_Gage);
            this.groupBox1.Location = new System.Drawing.Point(20, 116);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(492, 339);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "details";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(10, 27);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(64, 16);
            this.label5.TabIndex = 87;
            this.label5.Text = "discharge";
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(10, 51);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(76, 16);
            this.label6.TabIndex = 88;
            this.label6.Text = "stage";
            // 
            // textBox_Discharge
            // 
            this.textBox_Discharge.Location = new System.Drawing.Point(108, 25);
            this.textBox_Discharge.Name = "textBox_Discharge";
            this.textBox_Discharge.Size = new System.Drawing.Size(80, 20);
            this.textBox_Discharge.TabIndex = 85;
            this.textBox_Discharge.Text = "0";
            // 
            // textBox_Prim_Gage
            // 
            this.textBox_Prim_Gage.Location = new System.Drawing.Point(108, 51);
            this.textBox_Prim_Gage.Name = "textBox_Prim_Gage";
            this.textBox_Prim_Gage.Size = new System.Drawing.Size(80, 20);
            this.textBox_Prim_Gage.TabIndex = 86;
            this.textBox_Prim_Gage.Text = "0";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(11, 160);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 16);
            this.label3.TabIndex = 104;
            this.label3.Text = "notes";
            // 
            // textBox_Party
            // 
            this.textBox_Party.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_Party.Location = new System.Drawing.Point(61, 112);
            this.textBox_Party.Name = "textBox_Party";
            this.textBox_Party.Size = new System.Drawing.Size(380, 20);
            this.textBox_Party.TabIndex = 101;
            // 
            // textBox_Memo
            // 
            this.textBox_Memo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_Memo.Location = new System.Drawing.Point(61, 160);
            this.textBox_Memo.Multiline = true;
            this.textBox_Memo.Name = "textBox_Memo";
            this.textBox_Memo.Size = new System.Drawing.Size(384, 171);
            this.textBox_Memo.TabIndex = 102;
            // 
            // label_Party
            // 
            this.label_Party.Location = new System.Drawing.Point(6, 115);
            this.label_Party.Name = "label_Party";
            this.label_Party.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label_Party.Size = new System.Drawing.Size(32, 16);
            this.label_Party.TabIndex = 103;
            this.label_Party.Text = "Party";
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(11, 78);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(76, 16);
            this.label7.TabIndex = 106;
            this.label7.Text = "quality note:";
            // 
            // textBoxQuality
            // 
            this.textBoxQuality.Location = new System.Drawing.Point(109, 78);
            this.textBoxQuality.Name = "textBoxQuality";
            this.textBoxQuality.Size = new System.Drawing.Size(146, 20);
            this.textBoxQuality.TabIndex = 105;
            this.toolTip1.SetToolTip(this.textBoxQuality, "fair, poor, good, etc...");
            // 
            // FormNewMeasurment
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(688, 490);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.dateTimePicker1);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.comboBoxGageIds);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "FormNewMeasurment";
            this.Text = "Input New Measurement";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

    }
		#endregion

    private void buttonSave_Click(object sender, System.EventArgs e)
    {
    }

    private void comboBoxGageIds_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (comboBoxGageIds.SelectedIndex >= 0)
            buttonSave.Enabled = true;
    }
	}
}

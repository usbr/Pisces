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
    private string _siteDesc="";
    MeasurementType measType;
    private int _measurementNumber;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.ComboBox comboBoxGageIds;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Button buttonSave;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.DateTimePicker dateTimePicker1;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.ToolTip toolTip1;
    private System.Windows.Forms.Button buttonCancel;
    private System.ComponentModel.IContainer components;

    TimeSeriesDatabaseDataSet.sitecatalogDataTable m_siteCatalog;

		public FormNewMeasurment(MeasurementType measType,  TimeSeriesDatabaseDataSet.sitecatalogDataTable siteCatalog)
		{
			InitializeComponent();
            buttonSave.Enabled = false;
            m_siteCatalog = siteCatalog;
            this.measType = measType;
            Prepare();
            this.Text="Input New "+measType.ToString()+" Measurement";
		}

    public string SiteDescription
    {
      get { return this._siteDesc;}
      set { 
            this._siteDesc = value;
            this.comboBoxGageIds.Text =this._siteDesc;
          }
    }
    public DateTime Date
    {
      get { return this.dateTimePicker1.Value;}
    }

    public string SiteID
    {
      get { return this.comboBoxGageIds.SelectedValue.ToString();}
    }
    public int MeasurementNumber
    {
      get { return _measurementNumber;}
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
            this.label3 = new System.Windows.Forms.Label();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.buttonCancel = new System.Windows.Forms.Button();
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
            this.comboBoxGageIds.Location = new System.Drawing.Point(120, 112);
            this.comboBoxGageIds.Name = "comboBoxGageIds";
            this.comboBoxGageIds.Size = new System.Drawing.Size(292, 21);
            this.comboBoxGageIds.TabIndex = 1;
            this.comboBoxGageIds.SelectedIndexChanged += new System.EventHandler(this.comboBoxGageIds_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(124, 84);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(104, 20);
            this.label2.TabIndex = 2;
            this.label2.Text = "Location";
            // 
            // buttonSave
            // 
            this.buttonSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSave.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonSave.Location = new System.Drawing.Point(404, 224);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(75, 23);
            this.buttonSave.TabIndex = 3;
            this.buttonSave.Text = "OK";
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(100, 216);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(280, 56);
            this.label3.TabIndex = 4;
            this.label3.Text = "Click OK to create the record.  You will then be able to input detailed readings";
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.CustomFormat = "MM/dd/yyyy HH:mm";
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker1.Location = new System.Drawing.Point(124, 164);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(136, 20);
            this.dateTimePicker1.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(124, 136);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(92, 20);
            this.label4.TabIndex = 6;
            this.label4.Text = "Date";
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(404, 260);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 7;
            this.buttonCancel.Text = "Cancel";
            // 
            // FormNewMeasurment
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(484, 290);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.dateTimePicker1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.comboBoxGageIds);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "FormNewMeasurment";
            this.Text = "Input New Measurement";
            this.ResumeLayout(false);

    }
		#endregion

    private void buttonSave_Click(object sender, System.EventArgs e)
    {
      // Create a new measurement...
     string site_code = this.comboBoxGageIds.SelectedValue.ToString();
      if( this.measType == MeasurementType.Manual)
      {
       // _measurementNumber = Database.NewManualMeasurement(site_code,this.dateTimePicker1.Value);
      }
      else
        if( this.measType==MeasurementType.RiverCat)
      {
        //_measurementNumber = Database.NewRiverCatMeasurement(site_code,this.dateTimePicker1.Value);
      }

    }

    private void comboBoxGageIds_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (comboBoxGageIds.SelectedIndex >= 0)
            buttonSave.Enabled = true;
    }
	}
}

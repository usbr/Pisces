using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using Reclamation.TimeSeries;
using Reclamation.Core;
namespace FlowMeasurement
{
  /// <summary>
  /// Summary description for RatingEquationEditor.
  /// </summary>
  public class RatingEquationEditor : System.Windows.Forms.UserControl
  {
    private int EquationNumber=-1;
    DataTable tblEquation;
    DataTable tblEquationMeasurement;
    //  PolynomialFitter curveFitter = null;
    double mouseX, mouseY;
    int colIndexQ;
    int colIndexS;
    int prevRowSelected = -1;

    private System.Windows.Forms.Label labelTitle;
    private System.Windows.Forms.DataGrid dataGrid1;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Button buttonSave;
    private System.Windows.Forms.NumericUpDown numericUpDownPolynomialOrder;
    private System.Windows.Forms.TextBox textBoxNote;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.TextBox textBoxDateCreated;
    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.Button buttonAdd;
    private System.Windows.Forms.Label label6;
    private System.Windows.Forms.ImageList imageList1;
    private System.Windows.Forms.Button buttonAddAll;
    private System.Windows.Forms.ToolTip toolTip1;
    private System.Windows.Forms.Panel panel1;
    private System.Windows.Forms.Button buttonCompute;
    private System.Windows.Forms.TextBox textBoxEquation;
    private System.Windows.Forms.Label label7;
    private System.Windows.Forms.TextBox textBoxName;
    private System.Windows.Forms.TextBox textBoxMinStage;
    private System.Windows.Forms.TextBox textBoxMaxStage;
    private System.Windows.Forms.Button buttonSelect;
    private System.Windows.Forms.TextBox textBoxOffset;
    private System.Windows.Forms.Label label8;
    private CheckBox checkBoxZeroIntercept;
      private TabControl tabControl1;
      private TabPage tabPage1;
      private TabPage tabPage2;
      private GroupBox groupBox2;
    private System.ComponentModel.IContainer components;

    public RatingEquationEditor()
    {
      InitializeComponent();
    }
    public RatingEquationEditor(int EquationNumber)
    {
      InitializeComponent();
      this.EquationNumber=EquationNumber;
        
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RatingEquationEditor));
            this.labelTitle = new System.Windows.Forms.Label();
            this.dataGrid1 = new System.Windows.Forms.DataGrid();
            this.label1 = new System.Windows.Forms.Label();
            this.numericUpDownPolynomialOrder = new System.Windows.Forms.NumericUpDown();
            this.textBoxMinStage = new System.Windows.Forms.TextBox();
            this.textBoxMaxStage = new System.Windows.Forms.TextBox();
            this.buttonSave = new System.Windows.Forms.Button();
            this.textBoxNote = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxDateCreated = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.checkBoxZeroIntercept = new System.Windows.Forms.CheckBox();
            this.textBoxOffset = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.textBoxName = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.buttonCompute = new System.Windows.Forms.Button();
            this.textBoxEquation = new System.Windows.Forms.TextBox();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.buttonAddAll = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.buttonSelect = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPolynomialOrder)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelTitle
            // 
            this.labelTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTitle.Location = new System.Drawing.Point(20, 11);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(388, 23);
            this.labelTitle.TabIndex = 80;
            this.labelTitle.Text = "site des";
            // 
            // dataGrid1
            // 
            this.dataGrid1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.dataGrid1.DataMember = "";
            this.dataGrid1.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.dataGrid1.Location = new System.Drawing.Point(8, 48);
            this.dataGrid1.Name = "dataGrid1";
            this.dataGrid1.Size = new System.Drawing.Size(424, 299);
            this.dataGrid1.TabIndex = 82;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(281, 83);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 20);
            this.label1.TabIndex = 84;
            this.label1.Text = "polynomial order";
            // 
            // numericUpDownPolynomialOrder
            // 
            this.numericUpDownPolynomialOrder.Location = new System.Drawing.Point(375, 80);
            this.numericUpDownPolynomialOrder.Maximum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.numericUpDownPolynomialOrder.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownPolynomialOrder.Name = "numericUpDownPolynomialOrder";
            this.numericUpDownPolynomialOrder.Size = new System.Drawing.Size(56, 20);
            this.numericUpDownPolynomialOrder.TabIndex = 85;
            this.numericUpDownPolynomialOrder.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // textBoxMinStage
            // 
            this.textBoxMinStage.Location = new System.Drawing.Point(158, 503);
            this.textBoxMinStage.Name = "textBoxMinStage";
            this.textBoxMinStage.Size = new System.Drawing.Size(56, 20);
            this.textBoxMinStage.TabIndex = 88;
            // 
            // textBoxMaxStage
            // 
            this.textBoxMaxStage.Location = new System.Drawing.Point(86, 503);
            this.textBoxMaxStage.Name = "textBoxMaxStage";
            this.textBoxMaxStage.Size = new System.Drawing.Size(56, 20);
            this.textBoxMaxStage.TabIndex = 89;
            // 
            // buttonSave
            // 
            this.buttonSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSave.Location = new System.Drawing.Point(851, 593);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(75, 23);
            this.buttonSave.TabIndex = 90;
            this.buttonSave.Text = "Save";
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // textBoxNote
            // 
            this.textBoxNote.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxNote.Location = new System.Drawing.Point(11, 141);
            this.textBoxNote.Multiline = true;
            this.textBoxNote.Name = "textBoxNote";
            this.textBoxNote.Size = new System.Drawing.Size(393, 63);
            this.textBoxNote.TabIndex = 91;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(8, 122);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(36, 16);
            this.label4.TabIndex = 92;
            this.label4.Text = "notes";
            // 
            // textBoxDateCreated
            // 
            this.textBoxDateCreated.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxDateCreated.Location = new System.Drawing.Point(251, 503);
            this.textBoxDateCreated.Name = "textBoxDateCreated";
            this.textBoxDateCreated.ReadOnly = true;
            this.textBoxDateCreated.Size = new System.Drawing.Size(215, 20);
            this.textBoxDateCreated.TabIndex = 94;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Controls.Add(this.checkBoxZeroIntercept);
            this.groupBox1.Controls.Add(this.textBoxOffset);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.textBoxName);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.buttonCompute);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.numericUpDownPolynomialOrder);
            this.groupBox1.Controls.Add(this.textBoxNote);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.textBoxEquation);
            this.groupBox1.Location = new System.Drawing.Point(448, 20);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(455, 358);
            this.groupBox1.TabIndex = 95;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Rating Details";
            // 
            // checkBoxZeroIntercept
            // 
            this.checkBoxZeroIntercept.AutoSize = true;
            this.checkBoxZeroIntercept.Location = new System.Drawing.Point(6, 210);
            this.checkBoxZeroIntercept.Name = "checkBoxZeroIntercept";
            this.checkBoxZeroIntercept.Size = new System.Drawing.Size(102, 17);
            this.checkBoxZeroIntercept.TabIndex = 105;
            this.checkBoxZeroIntercept.Text = "set intercept = 0";
            this.checkBoxZeroIntercept.UseVisualStyleBackColor = true;
            // 
            // textBoxOffset
            // 
            this.textBoxOffset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxOffset.Location = new System.Drawing.Point(375, 328);
            this.textBoxOffset.Name = "textBoxOffset";
            this.textBoxOffset.Size = new System.Drawing.Size(56, 20);
            this.textBoxOffset.TabIndex = 104;
            this.toolTip1.SetToolTip(this.textBoxOffset, "offset applied to Scada/Logger data during computations");
            // 
            // label8
            // 
            this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label8.Location = new System.Drawing.Point(331, 332);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(40, 16);
            this.label8.TabIndex = 103;
            this.label8.Text = "offset";
            // 
            // textBoxName
            // 
            this.textBoxName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxName.Location = new System.Drawing.Point(88, 16);
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.Size = new System.Drawing.Size(359, 20);
            this.textBoxName.TabIndex = 101;
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(8, 16);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(64, 12);
            this.label7.TabIndex = 100;
            this.label7.Text = "name";
            // 
            // buttonCompute
            // 
            this.buttonCompute.Location = new System.Drawing.Point(11, 266);
            this.buttonCompute.Name = "buttonCompute";
            this.buttonCompute.Size = new System.Drawing.Size(75, 23);
            this.buttonCompute.TabIndex = 96;
            this.buttonCompute.Text = "Compute";
            this.buttonCompute.Click += new System.EventHandler(this.buttonCompute_Click);
            // 
            // textBoxEquation
            // 
            this.textBoxEquation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxEquation.Location = new System.Drawing.Point(11, 233);
            this.textBoxEquation.Name = "textBoxEquation";
            this.textBoxEquation.ReadOnly = true;
            this.textBoxEquation.Size = new System.Drawing.Size(431, 20);
            this.textBoxEquation.TabIndex = 99;
            // 
            // buttonAdd
            // 
            this.buttonAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonAdd.Location = new System.Drawing.Point(272, 355);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(48, 23);
            this.buttonAdd.TabIndex = 96;
            this.buttonAdd.Text = "Add";
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label6.Location = new System.Drawing.Point(4, 355);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(88, 23);
            this.label6.TabIndex = 95;
            this.label6.Text = "Measurements";
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "");
            this.imageList1.Images.SetKeyName(1, "");
            this.imageList1.Images.SetKeyName(2, "");
            this.imageList1.Images.SetKeyName(3, "");
            this.imageList1.Images.SetKeyName(4, "");
            this.imageList1.Images.SetKeyName(5, "");
            this.imageList1.Images.SetKeyName(6, "");
            this.imageList1.Images.SetKeyName(7, "");
            this.imageList1.Images.SetKeyName(8, "");
            this.imageList1.Images.SetKeyName(9, "");
            this.imageList1.Images.SetKeyName(10, "");
            // 
            // buttonAddAll
            // 
            this.buttonAddAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonAddAll.Location = new System.Drawing.Point(324, 355);
            this.buttonAddAll.Name = "buttonAddAll";
            this.buttonAddAll.Size = new System.Drawing.Size(52, 23);
            this.buttonAddAll.TabIndex = 98;
            this.buttonAddAll.Text = "Add All";
            this.buttonAddAll.Click += new System.EventHandler(this.buttonAddAll_Click);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.buttonAddAll);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.labelTitle);
            this.panel1.Controls.Add(this.dataGrid1);
            this.panel1.Controls.Add(this.buttonAdd);
            this.panel1.Controls.Add(this.buttonSelect);
            this.panel1.Location = new System.Drawing.Point(3, 5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(907, 391);
            this.panel1.TabIndex = 99;
            // 
            // buttonSelect
            // 
            this.buttonSelect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonSelect.Location = new System.Drawing.Point(384, 355);
            this.buttonSelect.Name = "buttonSelect";
            this.buttonSelect.Size = new System.Drawing.Size(52, 23);
            this.buttonSelect.TabIndex = 99;
            this.buttonSelect.Text = "select ...";
            this.buttonSelect.Click += new System.EventHandler(this.buttonSelect_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(923, 547);
            this.tabControl1.TabIndex = 102;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.panel1);
            this.tabPage1.Controls.Add(this.textBoxMaxStage);
            this.tabPage1.Controls.Add(this.textBoxMinStage);
            this.tabPage1.Controls.Add(this.textBoxDateCreated);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(915, 521);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Rating Equation";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(915, 529);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Rating Table";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Location = new System.Drawing.Point(11, 40);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(264, 79);
            this.groupBox2.TabIndex = 106;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "curve fit type";
            // 
            // RatingEquationEditor
            // 
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.tabControl1);
            this.Name = "RatingEquationEditor";
            this.Size = new System.Drawing.Size(929, 619);
            this.Load += new System.EventHandler(this.RatingEquation_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPolynomialOrder)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.ResumeLayout(false);

    }
    #endregion

    private void RatingEquation_Load(object sender, System.EventArgs e)
    {
    
      if(! DesignMode)
        LoadData();
    }


    //RatingTableSpreadsheet ratingTableSpreadsheet1;
    private void LoadData()
    {

    }

    private void LoadEquationMeasurements()
    {
    }

    private void SetupMeasurementComboBox()
    {
    }
    private void SaveData()
    {
      LoadData();
    }

    private void buttonSave_Click(object sender, System.EventArgs e)
    {
      SaveData();
      
    }


    /// <summary>
    /// Updates the min and max stage values
    /// based on current measurement table.
    /// </summary>
    private void UpdateMinMax()
    {
      DataRow[] rows = this.tblEquationMeasurement.Select("","",DataViewRowState.CurrentRows);

      double min = double.MaxValue;
      double max  = double.MinValue;

      for(int i=0; i<rows.Length; i++)
      {
        if( rows[i]["Stage"] == DBNull.Value)
          continue;
        double stage = Convert.ToDouble(rows[i]["Stage"]);
        if( stage > max)
          max = stage;
        if( stage <  min)
          min = stage;         
      }
      this.textBoxMaxStage.Text=max.ToString("F2");
      this.textBoxMinStage.Text=min.ToString("F2");
    //  ratingTableSpreadsheet1.Save();
    }

    /// <summary>
    /// Add record to EquationMeasurements Table.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void buttonAdd_Click(object sender, System.EventArgs e)
    {
      //AddMeasurement(this.imageComboBox1.SelectedIndex);
      UpdateMinMax();
    }

    private void buttonAddAll_Click(object sender, System.EventArgs e)
    {
      UpdateMinMax();
    }

    private void AddMeasurement(int index)
    {
    }


    /// <summary>
    /// Hide columns not necessary for user.
    /// 
    /// </summary>
    private void FormatGrid()
    {

    
    }

    private void buttonCompute_Click(object sender, System.EventArgs e)
    {
      
      ComputeRegression();

    }

    private void ComputeRegression()
    {
    }

    private void buttonSelect_Click(object sender, System.EventArgs e)
    {

    }


    private void graph1_OnPointFound(object sender,  System.EventArgs e ) //FlowMeasurement.XYEventArgs e)
    {
    //  // try to find and highlight this point in the table of Stage Discharge.
    //  if( mouseX == e.X && mouseY == e.Y)
    //  {
    //    Console.WriteLine("found previous x,y (do nothing) :"+e.X.ToString() +" "+ e.Y);
    //  }
    //  else
    //{
    //  Console.WriteLine(e.X.ToString() +" "+ e.Y);
    // int sz = this.tblEquationMeasurement.Rows.Count;   
    //    for(int rowIndex=0; rowIndex<sz; rowIndex++)
    //    {
    //     double Q =  Convert.ToDouble(this.dataGrid1[rowIndex,colIndexQ]);
    //     double S = Convert.ToDouble(this.dataGrid1[rowIndex,colIndexS]);
    //      if( System.Math.Abs(e.X -S) < 0.01 && System.Math.Abs(e.Y - Q) <0.1)
    //      {
    //        if( prevRowSelected >=0)
    //        {
    //        this.dataGrid1.UnSelect(prevRowSelected);
    //        }
    //        this.dataGrid1.Select(rowIndex);
    //        this.dataGrid1.CurrentRowIndex = rowIndex;
    //        prevRowSelected = rowIndex;
    //        break;
    //      }
    //    }
    //}
    //  mouseX = e.X;
    //  mouseY = e.Y;

    }
  }
}
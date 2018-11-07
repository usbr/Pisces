using System;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.IO;
using Reclamation.Core;
using Reclamation.TimeSeries;
using System.Web.Security;
using System.Collections.Generic;
using Reclamation.TimeSeries.Hydromet;
using System.Text.RegularExpressions;
using Reclamation.TimeSeries.Graphing;
using Reclamation.AgriMet;
using System.Diagnostics;
using Reclamation.TimeSeries.AgriMet;
using Reclamation.TimeSeries.Reports;
namespace HydrometTools
{

	/// <summary>
	/// A table and graph for Viewing and editing hydromet data
	/// </summary>
	public class TimeSeriesEditor : System.Windows.Forms.UserControl
	{
	
		private Steema.TeeChart.Tools.DragPoint dragPoint1;
        private ITimeSeriesSpreadsheet timeSeriesSpreadsheet1;

        string originalDataXmlFilename;
		DataTable hydrometDataTable;
		bool GraphDrawNeeded=false;
		private System.Windows.Forms.ComboBox comboBoxInputs;
        private System.Windows.Forms.Button buttonUpload;
		private System.Windows.Forms.LinkLabel linkLabelChartDetails;
        private System.Windows.Forms.Button buttonDownload;
		private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.Panel panelGraphTable;
        private Steema.TeeChart.TChart tChart1;
        private Splitter splitter1;
		private System.ComponentModel.IContainer components;
        private Reclamation.TimeSeries.Forms.TimeSelectorBeginEnd timeSelector2;
        private Label labelFileName;
        private Label label2;
        private Button buttonOpenFile;
        private CheckBox checkBoxShowBadData;
        TimeInterval m_interval;
        private CheckBox checkBoxShowPoints;
        private Reclamation.TimeSeries.Forms.TimeSelectorBeginEndWaterYear timeSelectorBeginEndWaterYear1;
        private LinkLabel linkLabelUsgs;
        private ComboBox comboBoxEnableDragPoint;
        private Label label1;
        private Steema.TeeChart.Styles.Line line1;
        private ComboBox comboBoxInterval;
        private Label labelFillGap;
        private Button buttonForward;
        private Button buttonBack;
        private GroupBox groupBoxMonthlyReports;
        private LinkLabel MonthlyInventory;
        private LinkLabel USGSMonthlyReport;
        private CheckBox includeMonthlyFlags;
        private Button Lookbutton;
        private LinkLabel linkLabelOwrd;
        private LinkLabel linkLabelIdahoPower;
        private LinkLabel linkLabelPrint;
        Steema.TeeChart.Tools.Annotation annotation1;


		public TimeSeriesEditor(TimeInterval db)
		{
			InitializeComponent();
#if SpreadsheetGear
            var uc = new TimeSeriesSpreadsheetSG();
#else
            var uc = new TimeSeriesSpreadsheet();
#endif
            uc.Parent = this.panelGraphTable;
            uc.BringToFront();
            uc.Dock = DockStyle.Fill;
            timeSeriesSpreadsheet1 = uc;
            uc.Dock = DockStyle.Fill;
            uc.BringToFront();

            m_interval = db;
            SetupTimeSelector(db);

            this.checkBoxShowPoints.Checked = UserPreference.Lookup("ShowPoints") == "True";

            annotation1 = new Steema.TeeChart.Tools.Annotation(tChart1.Chart);
            
            originalDataXmlFilename = Path.Combine(FileUtility.GetTempPath(), db.ToString() +DateTime.Now.Ticks+ "_download.xml");

            checkBoxShowBadData.Visible = false;
            T1 = DateTime.Now.AddDays(-5);
            T2 = DateTime.Now;

            Logger.WriteLine(m_interval.ToString(),"ui");
            if (m_interval == TimeInterval.Monthly)
            {
                T1 = WaterYear.BeginningOfWaterYear(DateTime.Now);
                T2 = WaterYear.EndOfWaterYear(DateTime.Now);
               BackColor = Color.AliceBlue;
               groupBoxMonthlyReports.Visible = true;
            }
           if (m_interval == TimeInterval.Daily)
           {
                T2 = DateTime.Now.Date.AddDays(-1);
                BackColor = Color.Lavender;
           }

           if (m_interval == TimeInterval.Irregular)
           {
               checkBoxShowBadData.Visible = true;
               comboBoxInterval.Visible = true;
               labelFillGap.Visible = true;
               comboBoxInterval.SelectedIndex = 0;
           }
            
			dragPoint1 = new Steema.TeeChart.Tools.DragPoint();
			this.dragPoint1.Style = Steema.TeeChart.Tools.DragPointStyles.Y;
			this.tChart1.Tools.Add(this.dragPoint1);
			this.dragPoint1.Drag += new Steema.TeeChart.Tools.DragPointEventHandler(this.dragPoint1_Drag);


            tChart1.MouseMove += new MouseEventHandler(tChart1_MouseMove);
            var nearest = new Steema.TeeChart.Tools.NearestPoint(tChart1.Chart);

			LoadSiteList();
            this.comboBoxInputs.Text = UserPreference.Lookup("Inputs" + m_interval.ToString());

            
			this.dragPoint1.Active = false;
			this.dragPoint1.Series = null;
        //    HydrometEdits.Progress += new ProgressEventHandler(HydrometEdits_Progress);

            timeSeriesSpreadsheet1.UpdateCompleted += new EventHandler<EventArgs>(timeSeriesSpreadsheet1_UpdateCompleted);
		}

        private DateTime T1
        {
            get
            {
                if (m_interval == TimeInterval.Monthly)
                    return timeSelectorBeginEndWaterYear1.T1;
                return timeSelector2.T1;
            }
            set
            {
                if (m_interval == TimeInterval.Monthly)
                {
                    timeSelectorBeginEndWaterYear1.T1 = value;
                }
                else
                {
                    timeSelector2.T1 = value;
                }
            }
        }

        private DateTime T2
        {
            get
            {
                if (m_interval == TimeInterval.Monthly)
                    return timeSelectorBeginEndWaterYear1.T2;
                return timeSelector2.T2;
            }
            set
            {
                if (m_interval == TimeInterval.Monthly)
                {
                    timeSelectorBeginEndWaterYear1.T2 = value;
                }
                else
                {
                    timeSelector2.T2 = value;
                }
            }
        }

        private void SetupTimeSelector(TimeInterval db)
        {
            if (db == TimeInterval.Monthly)
            {
                timeSelector2.Visible = false;
            }
            else
            {
                timeSelectorBeginEndWaterYear1.Visible = false;
            }
        }

        void tChart1_MouseMove(object sender, MouseEventArgs e)
        {
            
            for (int i = 0; i < tChart1.Series.Count; i++)
            {
                int idx = tChart1[i].Clicked(e.X, e.Y);
                if (idx != -1)
                {
                    DrawAnnotation(i, idx);
                    return;
                }
            }
            annotation1.Active = false;
        }

        private void DrawAnnotation(int seriesIndex, int pointIndex)
        {
            annotation1.Active = true;
            var s = tChart1[seriesIndex];

            var t = DateTime.FromOADate(s.XValues[pointIndex]);

            string tip = s.Title + " " + t.ToString("yyyyMMMdd HHmm") + " " + s.YValues[pointIndex].ToString();
            annotation1.Text = tip;

        }



        void tips_GetText(Steema.TeeChart.Tools.MarksTip sender, Steema.TeeChart.Tools.MarksTipGetTextEventArgs e)
        {
            //sender.
        }

        void timeSeriesSpreadsheet1_UpdateCompleted(object sender, EventArgs e)
        {
            Graph();
        }

		public void Print()
		{
		this.tChart1.Printer.Preview();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TimeSeriesEditor));
            this.comboBoxInputs = new System.Windows.Forms.ComboBox();
            this.buttonUpload = new System.Windows.Forms.Button();
            this.linkLabelChartDetails = new System.Windows.Forms.LinkLabel();
            this.buttonDownload = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.checkBoxShowBadData = new System.Windows.Forms.CheckBox();
            this.checkBoxShowPoints = new System.Windows.Forms.CheckBox();
            this.buttonForward = new System.Windows.Forms.Button();
            this.buttonBack = new System.Windows.Forms.Button();
            this.panelGraphTable = new System.Windows.Forms.Panel();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.tChart1 = new Steema.TeeChart.TChart();
            this.line1 = new Steema.TeeChart.Styles.Line();
            this.labelFileName = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonOpenFile = new System.Windows.Forms.Button();
            this.linkLabelUsgs = new System.Windows.Forms.LinkLabel();
            this.comboBoxEnableDragPoint = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxInterval = new System.Windows.Forms.ComboBox();
            this.labelFillGap = new System.Windows.Forms.Label();
            this.groupBoxMonthlyReports = new System.Windows.Forms.GroupBox();
            this.includeMonthlyFlags = new System.Windows.Forms.CheckBox();
            this.MonthlyInventory = new System.Windows.Forms.LinkLabel();
            this.USGSMonthlyReport = new System.Windows.Forms.LinkLabel();
            this.Lookbutton = new System.Windows.Forms.Button();
            this.linkLabelOwrd = new System.Windows.Forms.LinkLabel();
            this.linkLabelIdahoPower = new System.Windows.Forms.LinkLabel();
            this.timeSelectorBeginEndWaterYear1 = new Reclamation.TimeSeries.Forms.TimeSelectorBeginEndWaterYear();
            this.timeSelector2 = new Reclamation.TimeSeries.Forms.TimeSelectorBeginEnd();
            this.linkLabelPrint = new System.Windows.Forms.LinkLabel();
            this.panelGraphTable.SuspendLayout();
            this.groupBoxMonthlyReports.SuspendLayout();
            this.SuspendLayout();
            // 
            // comboBoxInputs
            // 
            this.comboBoxInputs.Location = new System.Drawing.Point(3, 28);
            this.comboBoxInputs.Name = "comboBoxInputs";
            this.comboBoxInputs.Size = new System.Drawing.Size(313, 21);
            this.comboBoxInputs.TabIndex = 23;
            this.toolTip1.SetToolTip(this.comboBoxInputs, "example:  JCK AF, AMF AF");
            this.comboBoxInputs.SelectedIndexChanged += new System.EventHandler(this.comboBoxInputs_SelectedIndexChanged);
            this.comboBoxInputs.KeyDown += new System.Windows.Forms.KeyEventHandler(this.comboBoxInputs_KeyDown);
            // 
            // buttonUpload
            // 
            this.buttonUpload.BackColor = System.Drawing.SystemColors.Control;
            this.buttonUpload.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonUpload.ForeColor = System.Drawing.SystemColors.ControlText;
            this.buttonUpload.Location = new System.Drawing.Point(424, 77);
            this.buttonUpload.Name = "buttonUpload";
            this.buttonUpload.Size = new System.Drawing.Size(75, 23);
            this.buttonUpload.TabIndex = 21;
            this.buttonUpload.Text = "&Save";
            this.buttonUpload.UseVisualStyleBackColor = false;
            this.buttonUpload.Click += new System.EventHandler(this.ButtonSaveClick);
            // 
            // linkLabelChartDetails
            // 
            this.linkLabelChartDetails.Location = new System.Drawing.Point(266, 67);
            this.linkLabelChartDetails.Name = "linkLabelChartDetails";
            this.linkLabelChartDetails.Size = new System.Drawing.Size(72, 29);
            this.linkLabelChartDetails.TabIndex = 18;
            this.linkLabelChartDetails.TabStop = true;
            this.linkLabelChartDetails.Text = "chart details";
            this.linkLabelChartDetails.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelChartDetails_LinkClicked);
            // 
            // buttonDownload
            // 
            this.buttonDownload.ForeColor = System.Drawing.SystemColors.ControlText;
            this.buttonDownload.Location = new System.Drawing.Point(343, 77);
            this.buttonDownload.Name = "buttonDownload";
            this.buttonDownload.Size = new System.Drawing.Size(75, 23);
            this.buttonDownload.TabIndex = 16;
            this.buttonDownload.Text = "Refresh";
            this.buttonDownload.Click += new System.EventHandler(this.RefreshClick);
            // 
            // checkBoxShowBadData
            // 
            this.checkBoxShowBadData.AutoSize = true;
            this.checkBoxShowBadData.Checked = true;
            this.checkBoxShowBadData.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxShowBadData.Location = new System.Drawing.Point(351, 39);
            this.checkBoxShowBadData.Name = "checkBoxShowBadData";
            this.checkBoxShowBadData.Size = new System.Drawing.Size(115, 17);
            this.checkBoxShowBadData.TabIndex = 32;
            this.checkBoxShowBadData.Text = "graph flagged data";
            this.checkBoxShowBadData.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip1.SetToolTip(this.checkBoxShowBadData, "show data that has been \'flagged\' bad");
            this.checkBoxShowBadData.UseVisualStyleBackColor = true;
            this.checkBoxShowBadData.CheckedChanged += new System.EventHandler(this.checkBoxShowBadData_CheckedChanged);
            // 
            // checkBoxShowPoints
            // 
            this.checkBoxShowPoints.AutoSize = true;
            this.checkBoxShowPoints.Location = new System.Drawing.Point(351, 57);
            this.checkBoxShowPoints.Name = "checkBoxShowPoints";
            this.checkBoxShowPoints.Size = new System.Drawing.Size(82, 17);
            this.checkBoxShowPoints.TabIndex = 33;
            this.checkBoxShowPoints.Text = "show points";
            this.checkBoxShowPoints.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip1.SetToolTip(this.checkBoxShowPoints, "show point on graph for each timestamp");
            this.checkBoxShowPoints.UseVisualStyleBackColor = true;
            this.checkBoxShowPoints.CheckedChanged += new System.EventHandler(this.checkBoxShowPoints_CheckedChanged);
            // 
            // buttonForward
            // 
            this.buttonForward.Location = new System.Drawing.Point(688, 32);
            this.buttonForward.Name = "buttonForward";
            this.buttonForward.Size = new System.Drawing.Size(22, 24);
            this.buttonForward.TabIndex = 40;
            this.buttonForward.Text = ">";
            this.toolTip1.SetToolTip(this.buttonForward, "move forward in time");
            this.buttonForward.UseVisualStyleBackColor = true;
            this.buttonForward.Click += new System.EventHandler(this.buttonForward_Click);
            // 
            // buttonBack
            // 
            this.buttonBack.Location = new System.Drawing.Point(688, 55);
            this.buttonBack.Name = "buttonBack";
            this.buttonBack.Size = new System.Drawing.Size(22, 24);
            this.buttonBack.TabIndex = 41;
            this.buttonBack.Text = "<";
            this.toolTip1.SetToolTip(this.buttonBack, "move back in time");
            this.buttonBack.UseVisualStyleBackColor = true;
            this.buttonBack.Click += new System.EventHandler(this.buttonBack_Click);
            // 
            // panelGraphTable
            // 
            this.panelGraphTable.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelGraphTable.Controls.Add(this.splitter1);
            this.panelGraphTable.Controls.Add(this.tChart1);
            this.panelGraphTable.Location = new System.Drawing.Point(0, 115);
            this.panelGraphTable.Name = "panelGraphTable";
            this.panelGraphTable.Size = new System.Drawing.Size(918, 459);
            this.panelGraphTable.TabIndex = 27;
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(510, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(6, 459);
            this.splitter1.TabIndex = 4;
            this.splitter1.TabStop = false;
            // 
            // tChart1
            // 
            // 
            // 
            // 
            this.tChart1.Aspect.View3D = false;
            this.tChart1.Aspect.ZOffset = 0D;
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChart1.Axes.Bottom.AxisPen.Width = 1;
            // 
            // 
            // 
            this.tChart1.Axes.Bottom.Title.Transparent = true;
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChart1.Axes.Depth.Title.Transparent = true;
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChart1.Axes.DepthTop.Title.Transparent = true;
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChart1.Axes.Left.AxisPen.Width = 1;
            this.tChart1.Axes.Left.EndPosition = 99D;
            this.tChart1.Axes.Left.StartPosition = 1D;
            // 
            // 
            // 
            this.tChart1.Axes.Left.Title.Transparent = true;
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChart1.Axes.Right.AxisPen.Transparency = 77;
            this.tChart1.Axes.Right.AxisPen.Width = 0;
            // 
            // 
            // 
            this.tChart1.Axes.Right.Grid.Transparency = 80;
            // 
            // 
            // 
            this.tChart1.Axes.Right.Title.Transparent = true;
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChart1.Axes.Top.Title.Transparent = true;
            this.tChart1.Axes.Top.Visible = false;
            this.tChart1.BackColor = System.Drawing.Color.Transparent;
            this.tChart1.Dock = System.Windows.Forms.DockStyle.Left;
            // 
            // 
            // 
            this.tChart1.Header.Lines = new string[] {
        ""};
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChart1.Legend.Font.Brush.Color = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(102)))), ((int)(((byte)(163)))));
            this.tChart1.Legend.FontSeriesColor = true;
            this.tChart1.Location = new System.Drawing.Point(0, 0);
            this.tChart1.Name = "tChart1";
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChart1.Panel.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            // 
            // 
            // 
            this.tChart1.Panel.Brush.Color = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            // 
            // 
            // 
            this.tChart1.Panel.Brush.Gradient.Visible = false;
            this.tChart1.Series.Add(this.line1);
            this.tChart1.Size = new System.Drawing.Size(510, 459);
            this.tChart1.TabIndex = 3;
            // 
            // 
            // 
            this.tChart1.Walls.Visible = false;
            this.tChart1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tChart1_MouseDown);
            this.tChart1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.tChart1_MouseUp);
            // 
            // line1
            // 
            // 
            // 
            // 
            this.line1.Brush.Color = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(102)))), ((int)(((byte)(163)))));
            this.line1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(102)))), ((int)(((byte)(163)))));
            this.line1.ColorEach = false;
            // 
            // 
            // 
            this.line1.LinePen.Color = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(61)))), ((int)(((byte)(98)))));
            // 
            // 
            // 
            // 
            // 
            // 
            this.line1.Marks.Callout.ArrowHead = Steema.TeeChart.Styles.ArrowHeadStyles.None;
            this.line1.Marks.Callout.ArrowHeadSize = 8;
            // 
            // 
            // 
            this.line1.Marks.Callout.Brush.Color = System.Drawing.Color.Black;
            this.line1.Marks.Callout.Distance = 0;
            this.line1.Marks.Callout.Draw3D = false;
            this.line1.Marks.Callout.Length = 10;
            this.line1.Marks.Callout.Style = Steema.TeeChart.Styles.PointerStyles.Rectangle;
            this.line1.Marks.Callout.Visible = false;
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            this.line1.Pointer.Brush.Gradient.Transparency = 100;
            this.line1.Pointer.Style = Steema.TeeChart.Styles.PointerStyles.Rectangle;
            this.line1.Title = "line1";
            // 
            // 
            // 
            this.line1.XValues.DataMember = "X";
            this.line1.XValues.Order = Steema.TeeChart.Styles.ValueListOrder.Ascending;
            // 
            // 
            // 
            this.line1.YValues.DataMember = "Y";
            // 
            // labelFileName
            // 
            this.labelFileName.AutoSize = true;
            this.labelFileName.Location = new System.Drawing.Point(101, 7);
            this.labelFileName.Name = "labelFileName";
            this.labelFileName.Size = new System.Drawing.Size(35, 13);
            this.labelFileName.TabIndex = 29;
            this.labelFileName.Text = "label2";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(49, 7);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 13);
            this.label2.TabIndex = 30;
            this.label2.Text = "filename";
            // 
            // buttonOpenFile
            // 
            this.buttonOpenFile.Image = ((System.Drawing.Image)(resources.GetObject("buttonOpenFile.Image")));
            this.buttonOpenFile.Location = new System.Drawing.Point(10, 6);
            this.buttonOpenFile.Name = "buttonOpenFile";
            this.buttonOpenFile.Size = new System.Drawing.Size(35, 18);
            this.buttonOpenFile.TabIndex = 31;
            this.buttonOpenFile.UseVisualStyleBackColor = true;
            this.buttonOpenFile.Click += new System.EventHandler(this.buttonOpenFile_Click);
            // 
            // linkLabelUsgs
            // 
            this.linkLabelUsgs.Location = new System.Drawing.Point(6, 51);
            this.linkLabelUsgs.Name = "linkLabelUsgs";
            this.linkLabelUsgs.Size = new System.Drawing.Size(130, 17);
            this.linkLabelUsgs.TabIndex = 35;
            this.linkLabelUsgs.TabStop = true;
            this.linkLabelUsgs.Text = "usgs";
            this.linkLabelUsgs.Visible = false;
            this.linkLabelUsgs.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelUsgs_LinkClicked);
            // 
            // comboBoxEnableDragPoint
            // 
            this.comboBoxEnableDragPoint.FormattingEnabled = true;
            this.comboBoxEnableDragPoint.Location = new System.Drawing.Point(151, 64);
            this.comboBoxEnableDragPoint.Name = "comboBoxEnableDragPoint";
            this.comboBoxEnableDragPoint.Size = new System.Drawing.Size(109, 21);
            this.comboBoxEnableDragPoint.TabIndex = 36;
            this.comboBoxEnableDragPoint.SelectedIndexChanged += new System.EventHandler(this.comboBoxEnableDragPointSelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(157, 51);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 13);
            this.label1.TabIndex = 37;
            this.label1.Text = "drag points";
            // 
            // comboBoxInterval
            // 
            this.comboBoxInterval.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxInterval.FormattingEnabled = true;
            this.comboBoxInterval.Items.AddRange(new object[] {
            "0  (don\'t fill gaps)",
            "5 minutes",
            "15 minutes",
            "60 minutes"});
            this.comboBoxInterval.Location = new System.Drawing.Point(716, 43);
            this.comboBoxInterval.Name = "comboBoxInterval";
            this.comboBoxInterval.Size = new System.Drawing.Size(115, 21);
            this.comboBoxInterval.TabIndex = 38;
            this.comboBoxInterval.Visible = false;
            // 
            // labelFillGap
            // 
            this.labelFillGap.AutoSize = true;
            this.labelFillGap.Location = new System.Drawing.Point(716, 22);
            this.labelFillGap.Name = "labelFillGap";
            this.labelFillGap.Size = new System.Drawing.Size(74, 13);
            this.labelFillGap.TabIndex = 39;
            this.labelFillGap.Text = "fill gap interval";
            this.labelFillGap.Visible = false;
            // 
            // groupBoxMonthlyReports
            // 
            this.groupBoxMonthlyReports.Controls.Add(this.includeMonthlyFlags);
            this.groupBoxMonthlyReports.Controls.Add(this.MonthlyInventory);
            this.groupBoxMonthlyReports.Controls.Add(this.USGSMonthlyReport);
            this.groupBoxMonthlyReports.Location = new System.Drawing.Point(716, 24);
            this.groupBoxMonthlyReports.Name = "groupBoxMonthlyReports";
            this.groupBoxMonthlyReports.Size = new System.Drawing.Size(147, 58);
            this.groupBoxMonthlyReports.TabIndex = 42;
            this.groupBoxMonthlyReports.TabStop = false;
            this.groupBoxMonthlyReports.Text = "reports";
            this.groupBoxMonthlyReports.Visible = false;
            // 
            // includeMonthlyFlags
            // 
            this.includeMonthlyFlags.AutoSize = true;
            this.includeMonthlyFlags.Location = new System.Drawing.Point(93, 17);
            this.includeMonthlyFlags.Name = "includeMonthlyFlags";
            this.includeMonthlyFlags.Size = new System.Drawing.Size(48, 17);
            this.includeMonthlyFlags.TabIndex = 4;
            this.includeMonthlyFlags.Text = "flags";
            this.includeMonthlyFlags.UseVisualStyleBackColor = true;
            // 
            // MonthlyInventory
            // 
            this.MonthlyInventory.AutoSize = true;
            this.MonthlyInventory.Location = new System.Drawing.Point(10, 36);
            this.MonthlyInventory.Name = "MonthlyInventory";
            this.MonthlyInventory.Size = new System.Drawing.Size(50, 13);
            this.MonthlyInventory.TabIndex = 3;
            this.MonthlyInventory.TabStop = true;
            this.MonthlyInventory.Text = "inventory";
            this.MonthlyInventory.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.MonthlyInventory_LinkClicked);
            // 
            // USGSMonthlyReport
            // 
            this.USGSMonthlyReport.AutoSize = true;
            this.USGSMonthlyReport.Location = new System.Drawing.Point(6, 18);
            this.USGSMonthlyReport.Name = "USGSMonthlyReport";
            this.USGSMonthlyReport.Size = new System.Drawing.Size(63, 13);
            this.USGSMonthlyReport.TabIndex = 2;
            this.USGSMonthlyReport.TabStop = true;
            this.USGSMonthlyReport.Text = "USGS table";
            this.USGSMonthlyReport.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.USGSMonthlyReport_LinkClicked);
            // 
            // Lookbutton
            // 
            this.Lookbutton.Location = new System.Drawing.Point(317, 28);
            this.Lookbutton.Name = "Lookbutton";
            this.Lookbutton.Size = new System.Drawing.Size(28, 21);
            this.Lookbutton.TabIndex = 43;
            this.Lookbutton.Text = "...";
            this.Lookbutton.UseVisualStyleBackColor = true;
            this.Lookbutton.Click += new System.EventHandler(this.Lookbutton_Click);
            // 
            // linkLabelOwrd
            // 
            this.linkLabelOwrd.Location = new System.Drawing.Point(6, 68);
            this.linkLabelOwrd.Name = "linkLabelOwrd";
            this.linkLabelOwrd.Size = new System.Drawing.Size(130, 17);
            this.linkLabelOwrd.TabIndex = 44;
            this.linkLabelOwrd.TabStop = true;
            this.linkLabelOwrd.Text = "owrd";
            this.linkLabelOwrd.Visible = false;
            this.linkLabelOwrd.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelOwrd_LinkClicked);
            // 
            // linkLabelIdahoPower
            // 
            this.linkLabelIdahoPower.Location = new System.Drawing.Point(5, 79);
            this.linkLabelIdahoPower.Name = "linkLabelIdahoPower";
            this.linkLabelIdahoPower.Size = new System.Drawing.Size(130, 17);
            this.linkLabelIdahoPower.TabIndex = 45;
            this.linkLabelIdahoPower.TabStop = true;
            this.linkLabelIdahoPower.Text = "idaho power";
            this.linkLabelIdahoPower.Visible = false;
            this.linkLabelIdahoPower.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelIdahoPower_LinkClicked);
            // 
            // timeSelectorBeginEndWaterYear1
            // 
            this.timeSelectorBeginEndWaterYear1.Location = new System.Drawing.Point(543, 22);
            this.timeSelectorBeginEndWaterYear1.Margin = new System.Windows.Forms.Padding(4);
            this.timeSelectorBeginEndWaterYear1.Name = "timeSelectorBeginEndWaterYear1";
            this.timeSelectorBeginEndWaterYear1.Size = new System.Drawing.Size(131, 62);
            this.timeSelectorBeginEndWaterYear1.T1 = new System.DateTime(2009, 10, 1, 0, 0, 0, 0);
            this.timeSelectorBeginEndWaterYear1.T2 = new System.DateTime(2011, 9, 30, 0, 0, 0, 0);
            this.timeSelectorBeginEndWaterYear1.TabIndex = 34;
            // 
            // timeSelector2
            // 
            this.timeSelector2.Location = new System.Drawing.Point(500, 31);
            this.timeSelector2.Name = "timeSelector2";
            this.timeSelector2.ShowTime = false;
            this.timeSelector2.Size = new System.Drawing.Size(195, 46);
            this.timeSelector2.T1 = new System.DateTime(2010, 5, 10, 11, 44, 21, 531);
            this.timeSelector2.T2 = new System.DateTime(2010, 5, 10, 11, 44, 21, 531);
            this.timeSelector2.TabIndex = 28;
            // 
            // linkLabelPrint
            // 
            this.linkLabelPrint.Location = new System.Drawing.Point(266, 88);
            this.linkLabelPrint.Name = "linkLabelPrint";
            this.linkLabelPrint.Size = new System.Drawing.Size(50, 24);
            this.linkLabelPrint.TabIndex = 46;
            this.linkLabelPrint.TabStop = true;
            this.linkLabelPrint.Text = "print";
            this.linkLabelPrint.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelPrint_LinkClicked);
            // 
            // TimeSeriesEditor
            // 
            this.Controls.Add(this.linkLabelPrint);
            this.Controls.Add(this.linkLabelOwrd);
            this.Controls.Add(this.linkLabelIdahoPower);
            this.Controls.Add(this.Lookbutton);
            this.Controls.Add(this.groupBoxMonthlyReports);
            this.Controls.Add(this.buttonBack);
            this.Controls.Add(this.buttonForward);
            this.Controls.Add(this.labelFillGap);
            this.Controls.Add(this.comboBoxInterval);
            this.Controls.Add(this.comboBoxEnableDragPoint);
            this.Controls.Add(this.linkLabelUsgs);
            this.Controls.Add(this.timeSelectorBeginEndWaterYear1);
            this.Controls.Add(this.checkBoxShowPoints);
            this.Controls.Add(this.checkBoxShowBadData);
            this.Controls.Add(this.buttonOpenFile);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.labelFileName);
            this.Controls.Add(this.timeSelector2);
            this.Controls.Add(this.panelGraphTable);
            this.Controls.Add(this.buttonUpload);
            this.Controls.Add(this.comboBoxInputs);
            this.Controls.Add(this.linkLabelChartDetails);
            this.Controls.Add(this.buttonDownload);
            this.Controls.Add(this.label1);
            this.Name = "TimeSeriesEditor";
            this.Size = new System.Drawing.Size(918, 574);
            this.panelGraphTable.ResumeLayout(false);
            this.groupBoxMonthlyReports.ResumeLayout(false);
            this.groupBoxMonthlyReports.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		void LoadSiteList()
		{
            string fn = "site.txt";
            if (m_interval == TimeInterval.Irregular)
                fn = "day_site.txt";
            else if (m_interval == TimeInterval.Daily)
                fn = "arc_site.txt";
            else if (m_interval == TimeInterval.Monthly)
                fn = "mpoll_site.txt";

            string property = m_interval.ToString() + "FileName";
            UserPreference.SetDefault(property, fn, false);
            
            fn = UserPreference.Lookup(property);
            ReadFile(fn);
		}

        private void buttonOpenFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.FileName = this.labelFileName.Text;

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                ReadFile(dlg.FileName);
            }
        }

        private void ReadFile(string filename)
        {
            labelFileName.Text = filename;
            if (!File.Exists(filename))
                return;
            comboBoxInputs.Items.Clear();
            comboBoxInputs.Items.AddRange(File.ReadAllLines(filename));
            labelFileName.Text = filename;
            UserPreference.Save(m_interval.ToString() + "FileName", filename);
        }

        CommandLine cmd; 
		private void RefreshClick(object sender, System.EventArgs e)
		{
            bool ctrl = (Control.ModifierKeys & Keys.Control) != 0;
            Performance perf = new Performance();
			UserPreference.Save("Inputs"+m_interval.ToString(),this.comboBoxInputs.Text);

			this.dragPoint1.Active = false;
			Cursor = Cursors.WaitCursor;
            timeSeriesSpreadsheet1.Clear();
            Application.DoEvents();
			try
			{
                HydrometHost svr = HydrometInfoUtility.HydrometServerFromPreferences();
                string query = "";

                Logger.WriteLine("svr = " + svr.ToString());
                //if (svr == HydrometHost.PNLinux)
                //{// lookup parameters from database. 
                //    //PiscesDatabase.Info.Parameters()
                //    //m_db  == HydrometDataBase.
                //}
                //else
               // {// parameters from flat files.
                if( NetworkUtility.Intranet)
                  query = HydrometInfoUtility.ExpandQuery(comboBoxInputs.Text, m_interval,Database.DB());
                else // at home 
                    query = HydrometInfoUtility.ExpandQuery(comboBoxInputs.Text, m_interval, null);
                //}
                cmd = new CommandLine(query, m_interval);
				GetTimeSeries();
                referenceData = ReadReferenceData();
                hydrometDataTable.AcceptChanges();
                hydrometDataTable.RowChanged += new DataRowChangeEventHandler(dataTable_RowChanged);
                Logger.WriteLine(UserPreference.Lookup("HydrometServer"),"ui");
			}
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                Logger.WriteLine("error reading data from " + UserPreference.Lookup("HydrometServer") + " " + ex.ToString(),"ui");
                Logger.WriteLine(ex.ToString());
            }
            finally
            {
                Cursor = Cursors.Default;
            }
			if( hydrometDataTable == null)
				return;

            string msg = UserPreference.Lookup("HydrometServer") +" " +hydrometDataTable.Rows.Count + " rows of data read " + perf.ElapsedSeconds.ToString("F2") + "s";
            Logger.WriteLine(msg, "ui");

            Logger.WriteLine("using temporary file " + originalDataXmlFilename);
			hydrometDataTable.WriteXml(originalDataXmlFilename,XmlWriteMode.WriteSchema);

            Graph();

            SetupUsgsLink();
            

            timeSeriesSpreadsheet1.SetDataTable(hydrometDataTable, m_interval,ctrl);
            timeSeriesSpreadsheet1.AutoFlagDayFiles = UserPreference.Lookup("AutoFlagDayFiles") == "True";


		}

        

        string usgsUrl = "";
        string owrdUrl = "";
        private void SetupUsgsLink()
        {
            this.linkLabelUsgs.Visible = false;
            this.linkLabelOwrd.Visible = false;
            linkLabelIdahoPower.Visible = false;
            linkLabelUsgs.Text = "";
            usgsUrl = "";
            owrdUrl = "";
            if (hydrometDataTable != null && hydrometDataTable.Columns.Count > 1 && hydrometDataTable.Rows.Count >0)
            {
                string cbtt = this.comboBoxInputs.Text.Trim().Split(' ')[0];
                // check for USGS id..
                string altId = HydrometInfoUtility.LookupAltID(cbtt);
                if (altId.Trim().Length > 0 && Regex.IsMatch(altId,"[0-9]{7,10}"))
                {
                    linkLabelUsgs.Text = "usgs " + altId;
                    usgsUrl = "http://waterdata.usgs.gov/nwis/uv?format=html&period=7&site_no="+altId;
                    linkLabelUsgs.Visible = true;

                    linkLabelOwrd.Text = "owrd "+altId;
                    owrdUrl = "http://apps.wrd.state.or.us/apps/sw/hydro_near_real_time/display_hydro_graph.aspx?station_nbr="+altId;
                    this.linkLabelOwrd.Visible = true;

                    linkLabelIdahoPower.Visible = true;
                }

                
            }
        }

        private void linkLabelUsgs_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(usgsUrl);
        }

        private void linkLabelOwrd_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(owrdUrl);
        }

        private void linkLabelIdahoPower_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.idahopower.com/OurEnvironment/WaterInformation/StreamFlow/stationList/basinstationList.cfm?selectS=3");
        }
        private string GetHeaderTitle( out string subTitle)
        {

            subTitle = "";
            if (cmd.IsSingleCbtt  && cmd.Title == "")
            {
                subTitle = HydrometInfoUtility.LookupGroupDescription(cmd.CbttList[0]);
                return HydrometInfoUtility.LookupSiteDescription(cmd.CbttList[0]);

            }
            return cmd.Title;
        }


        void dataTable_RowChanged(object sender, DataRowChangeEventArgs e)
        {

            if( ! (timeSeriesSpreadsheet1.SuspendUpdates || pointDrag))
               Graph();
        }

		
		/// <summary>
		/// graphs data in dataTable
		/// </summary>
		void Graph()
		{
            UserPreference.Save("ShowPoints", this.checkBoxShowPoints.Checked.ToString());

            tChart1.Text = "";
			if (hydrometDataTable == null)
				return;
            Console.WriteLine("graph()");

			tChart1.Series.Clear();
            tChart1.Zoom.Undo();
			int sz = hydrometDataTable.Columns.Count;
            if (sz == 2 || (sz == 3 && m_interval == TimeInterval.Irregular) 
                || (sz == 3 && m_interval == TimeInterval.Monthly))  // single graph series.
			{
				this.tChart1.Legend.Visible = false;
			}
			else
			{
				this.tChart1.Legend.Visible = true;
			}
            string subTitle = "";
            this.tChart1.Header.Text = GetHeaderTitle(out subTitle);
            this.tChart1.SubHeader.Visible = false;
            if (subTitle.Trim() != "")
            {
                this.tChart1.SubHeader.Text = subTitle;
                this.tChart1.SubHeader.Visible = true;
            }
            int increment = 1;
            if (m_interval == TimeInterval.Irregular || m_interval == TimeInterval.Monthly)
                increment = 2;
            tChart1.Axes.Custom.RemoveAll();
            tChart1.Panel.MarginLeft = 3;
            tChart1.Axes.Left.Title.Text = "";
            tChart1.Axes.Right.Title.Text = "";
            comboBoxEnableDragPoint.Items.Clear();
            comboBoxEnableDragPoint.Items.Add("None");
            TChartDataLoader loader = new TChartDataLoader(this.tChart1);

            for (int i=1; i<sz; i+=increment)
			{
				try 
				{
					string columnName = hydrometDataTable.Columns[i].ColumnName;
                    // double avg = AverageOfColumn(hydrometDataTable, columnName);


                    //Steema.TeeChart.Styles.Line series = MakeSeries(hydrometDataTable, columnName, avg);
                    var series = loader.CreateSeries(hydrometDataTable, columnName, m_interval, checkBoxShowBadData.Checked);

                    series.VertAxis = Steema.TeeChart.Styles.VerticalAxis.Left;
                    series.Pointer.Visible = this.checkBoxShowPoints.Checked;

                    var tokens = TextFile.Split(columnName);
                    string pcode = "";
                    string cbtt = "";
                    if (tokens.Length == 2)
                    {
                        cbtt = tokens[0].Trim();
                        pcode = tokens[1].Trim();
                    }


                    string units = LookupUnits(pcode);


                    if( UserPreference.Lookup("MultipleYAxis") == "True")
                        TChartDataLoader.SetupMultiLeftAxis(tChart1, series, units);
                    else
                        TChartDataLoader.SetupAxisLeftRight(tChart1, series, units);

                comboBoxEnableDragPoint.Items.Add(series.ToString());

				tChart1.Series.Add(series);
				}
				catch(Exception e)
				{
                    MessageBox.Show(e.ToString()+ " series index "+i);
                    Logger.WriteLine(e.ToString(),"ui");
				}
			}
            comboBoxEnableDragPoint.SelectedIndex = 0;
			this.comboBoxEnableDragPointSelectedIndexChanged(null,null);

            GraphReferenceData();
            tChart1.Axes.Left.Automatic = true;
//            Application.DoEvents();
  //          this.tChart1.Zoom.Undo();
    //        this.tChart1.Zoom.ZoomPercent(99);

		}

        SeriesList referenceData;

        private void GraphReferenceData()
        {
            TChartDataLoader dl = new TChartDataLoader(this.tChart1);
            for (int i = 0; i < referenceData.Count; i++)
            {
                if( referenceData[i].Units == "acre-feet" && referenceData[i] is HydrometMonthlySeries)
                    HydrometMonthlySeries.ConvertFromAcreFeetToThousandAcreFeet(referenceData[i]);

                var tSeries = dl.CreateTChartSeries(referenceData[i].Name);
                dl.FillTimeSeries(referenceData[i], tSeries);

                TChartDataLoader.SetupAxisLeftRight(tChart1,tSeries,referenceData[i].Units);

                tChart1.Series.Add(tSeries);
            }
        }

        /// <summary>
        /// Refrerence data is plotted along with the data you are editing
        /// it is not shown in the table.
        /// </summary>
        /// <returns></returns>
        private SeriesList ReadReferenceData()
        {
            CommandLine cmd = new CommandLine(comboBoxInputs.Text, m_interval);
            HydrometHost svr = HydrometInfoUtility.HydrometServerFromPreferences();
            var list = cmd.CreateSeries(svr);
            SeriesList referenceData = new SeriesList();
            if (m_interval == TimeInterval.Daily)
            {
                referenceData.Add(list.FilteredList(TimeInterval.Irregular));
                referenceData.Add(list.FilteredList(TimeInterval.Monthly));
            }
            if (m_interval == TimeInterval.Monthly)
            {
                referenceData.Add(list.FilteredList(TimeInterval.Irregular));
                referenceData.Add(list.FilteredList(TimeInterval.Daily));
            }
            if (m_interval == TimeInterval.Irregular)
            {
                referenceData.Add(list.FilteredList(TimeInterval.Daily));
                referenceData.Add(list.FilteredList(TimeInterval.Monthly));
            }

            referenceData.Read(T1, T2);
            return referenceData;
        }

        private string LookupUnits(string pcode)
        {
            if (m_interval == TimeInterval.Daily)
                return HydrometInfoUtility.LookupDailyUnits(pcode);
            if (m_interval == TimeInterval.Irregular)
                return HydrometInfoUtility.LookupDayfileUnits(pcode);

            if( m_interval == TimeInterval.Monthly )
                return HydrometMonthlySeries.LookupUnits(pcode);

            return "";
        }


        private Steema.TeeChart.Styles.Line MakeSeries(DataTable table, string columnName, double avg)
        {
            TChartDataLoader loader = new TChartDataLoader(tChart1);
            var rval = loader.CreateSeries(table, columnName, m_interval, checkBoxShowBadData.Checked);

            if (m_interval == TimeInterval.Irregular && table.Rows.Count > 0)
            {
                DateTime maxDate = (DateTime)table.Rows[table.Rows.Count - 1][0];
                if (T2.Date == DateTime.Now.Date && maxDate.Date < DateTime.Now.Date)
                {// add a missing point to better detect missing data
                    rval.Add(DateTime.Now.ToOADate(), avg, Color.Transparent);
                }
            }
            return rval;
        }




		private void ButtonSaveClick(object sender, System.EventArgs e)
		{
            Performance perf = new Performance();
			if( hydrometDataTable == null)
				return;

			DataSet ds = new DataSet("old");
			ds.ReadXml(originalDataXmlFilename,XmlReadMode.ReadSchema);
			DataTable tblOld = ds.Tables[0];

            string editsFileNameVax = FileUtility.GetTempFileName(".txt");
            string editsFileNameLinux = FileUtility.GetTempFileName(".txt");
            int numRecordsWritten = 0;
            bool mpollPermanentMarkChanged = false;
            string[] arcCommands = null;
            string[] modifiedPcodes = new string[] { };
            string[] modifiedCbtt = new string[] { }; 
            DateRange range = new DateRange();
            if (m_interval == TimeInterval.Daily)
            {
                numRecordsWritten = HydrometDataUtility.WriteArchiveUpdateFile(hydrometDataTable, tblOld, editsFileNameVax, out modifiedCbtt, out modifiedPcodes, out range);
            }
            if (m_interval == TimeInterval.Irregular)
            {
                numRecordsWritten = DayFiles.WriteDayfileUpdateFile(hydrometDataTable, tblOld, editsFileNameVax,out arcCommands, out modifiedPcodes,out modifiedCbtt,out range);
            }
            if (m_interval == TimeInterval.Monthly)
            {
                numRecordsWritten = HydrometDataUtility.WriteMPollUpdateFile(hydrometDataTable, tblOld, editsFileNameVax, out mpollPermanentMarkChanged);
            }
            Logger.WriteLine(numRecordsWritten + " records written to " + m_interval.ToString() + " script","ui");

            File.Copy(editsFileNameVax, editsFileNameLinux, true);

            HydrometHost svr = HydrometInfoUtility.HydrometServerFromPreferences();

            bool SaveToVMS = true;
            if ( IsLinuxServer(svr)  && numRecordsWritten >0)
            { // saving to Postgresql/Linux

                if( Database.IsPasswordBlank())
                {
                    MessageBox.Show("Warning: the database password is blank.");
                    return;
                }

                SaveOptions o = new SaveOptions(m_interval);
                if (o.ShowDialog() == DialogResult.OK)
                {
                    SaveToVMS = o.SaveToVMS;
                    Logger.WriteLine("Pisces import: " + editsFileNameVax, "ui");
                    Database.ImportVMSTextFile(editsFileNameLinux,o.ComputeDependencies);
                    Logger.WriteLine("saved "+numRecordsWritten+" records ","ui");
                }
            }

			if(numRecordsWritten >0  && SaveToVMS)
			{
				Login login = new Login();
                bool computations = UserPreference.Lookup("EnableComputations") == "True";
                //bool agrimet = UserPreference.Lookup("AgrimetCalculations") == "True";
                bool admin = Login.AdminPasswordIsValid();

                bool anyAgrimetSites = AnyAgriMetSitesInList(modifiedCbtt);

                bool allowAgrimetCalculations =  anyAgrimetSites && m_interval != TimeInterval.Monthly;

                //login.AgrimetOptionsVisible = allowAgrimetCalculations;
                login.MpollPasswordGroupVisible = m_interval == TimeInterval.Monthly && mpollPermanentMarkChanged;
                login.RatingTableCalculationEnabled = !anyAgrimetSites && m_interval == TimeInterval.Irregular && computations && admin;
                login.AdvancedOptionsVisible = m_interval == TimeInterval.Irregular && computations && admin;


                bool allowACE = m_interval == TimeInterval.Irregular && svr == HydrometHost.GreatPlains;
                login.AceCheckboxVisible = allowACE;
                

                if (login.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        Cursor = Cursors.WaitCursor;
                        string remoteFilename = HydrometDataUtility.CreateRemoteFileName(login.Username, m_interval);

                        string status = "";
                        string mpollHash = "DE01B3A567AA4812F3056C6AACF742C9999116F8";
                        if (m_interval == TimeInterval.Daily)
                        {
                            status = HydrometEditsVMS.SaveDailyData(login.Username, login.Password, editsFileNameVax, remoteFilename,false,false);
                            if( allowAgrimetCalculations &&    Array.IndexOf(modifiedPcodes,"ET" ) >=0   )
                                status += AgriMetCropCharts(modifiedCbtt, range, login);
                        }
                        else
                            if (m_interval == TimeInterval.Irregular)
                            {
                                status = SaveDayfileEdits(editsFileNameVax, arcCommands, modifiedPcodes, modifiedCbtt, range, login, remoteFilename, status);
                                if (allowAgrimetCalculations )
                                   status += AgriMetCropCharts(modifiedCbtt, range, login);
                            }
                            else if (m_interval == TimeInterval.Monthly)
                            {
                                if (mpollPermanentMarkChanged &&
                                     mpollHash != FormsAuthentication.HashPasswordForStoringInConfigFile(login.MpollPassword.ToLower(), "sha1"))
                                {
                                    MessageBox.Show("mpoll Password required to change permanent marks (flags)");
                                }
                                else
                                {
                                    status = HydrometEditsVMS.RunMpollImport(login.Username, login.Password, editsFileNameVax, remoteFilename);
                                }
                            }


                        ShowVmsStatus(status);

                    }
                    catch (Exception aex)
                    {
                        string msg = aex.Message;

                        if( aex.InnerException != null)
                        {
                            msg += "inner\n" + aex.InnerException.Message;
                        }
                        MessageBox.Show(msg);
                        Logger.WriteLine("msg","ui");
                    }
                    finally
                    {
                        Cursor = Cursors.Default;
                        Logger.WriteLine("completed in "+perf.ElapsedSeconds.ToString("F2")+" seconds", "ui");
                    }                   
                }
			}
		}

        private bool IsLinuxServer(HydrometHost svr)
        {
            return svr == HydrometHost.PNLinux || svr == HydrometHost.YakimaLinux;
        }

        private static string SaveDayfileEdits(string hydrometScript, string[] arcCommands, string[] modifiedPcodes, string[] modifiedCbtt, DateRange range, Login login, string remoteFilename, string status)
        {
            // save dayfile changes
            status = HydrometEditsVMS.SaveInstantData(login.Username, login.Password, hydrometScript, remoteFilename);
            // items that depend on editing dayfile data

            // run math/rating tables for dependent variables.
            if (Array.IndexOf(modifiedPcodes, "fb") >= 0 && modifiedCbtt.Length == 1 && login.RatingTableMath)
            {
                string cbtt = modifiedCbtt[0];
                if (HydrometInfoUtility.LookupParameterSwitch(cbtt, "AF", HydrometInfoUtility.ParameterSwitch.ACTIVE))
                    status += HydrometEditsVMS.RunRatingTableMath(login.Username, login.Password, cbtt, "fb", "af", range.DateTime1, range.DateTime2);
            }
            if (Array.IndexOf(modifiedPcodes, "gh") >= 0 && modifiedCbtt.Length == 1 && login.RatingTableMath)
            {
                string cbtt = modifiedCbtt[0];
                if (HydrometInfoUtility.LookupParameterSwitch(cbtt, "Q", HydrometInfoUtility.ParameterSwitch.ACTIVE))
                    status += HydrometEditsVMS.RunRatingTableMath(login.Username, login.Password, cbtt, "gh", "q", range.DateTime1, range.DateTime2);
            }

            // run archiver.
            if (login.CalculateDailyValues && arcCommands.Length > 0)
            {
                status += HydrometEditsVMS.RunArchiveCommands(login.Username, login.Password, arcCommands);
            }

            


            return status;
        }

        private static string AgriMetCropCharts(string[] modifiedCbtt, DateRange range, Login login)
        {
            string status = "";

            for (int i = 0; i < modifiedCbtt.Length; i++)
            {
             if( Database.IsAgrimetSite(modifiedCbtt[i])  )
             {
                 Reclamation.TimeSeries.AgriMet.CropDatesDataSet.RunCropCharts();
                 status = "\nrunning crop charts on the server";
                 break;
             }
            }

            return status;
        }

        private bool AnyAgriMetSitesInList(string[] cbtt)
        {
            for (int i = 0; i < cbtt.Length; i++)
			{
                if (HydrometInfoUtility.LookupAcl(cbtt[i]) == "WMCO")
                    return true;
			}
            return false;
        }


        internal static void ShowVmsStatus(string status)
        {
            var lines = status.Split('\n');
            foreach (var item in lines)
            {
                Logger.WriteLine(item);
            }

            if (UserPreference.Lookup("HideStatusDialog") != "True")
            {
                var f = new FormStatus();
                f.Lines = lines;
                f.ShowDialog();

                if (f.HideDialogNextTime)
                    UserPreference.Save("HideStatusDialog", "True");
                else
                    UserPreference.Save("HideStatusDialog", "False");
            }
        }




		private void comboBoxInputs_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			this.RefreshClick(sender,e);
		}

		private void comboBoxEnableDragPointSelectedIndexChanged(object sender, System.EventArgs e)
		{
            if (comboBoxEnableDragPoint.SelectedIndex > 0)
            {
                dragPoint1.Series = tChart1[comboBoxEnableDragPoint.SelectedIndex - 1];
                dragPoint1.Active = true;
            }
            else
                dragPoint1.Active = false;

		}


		private void linkLabelChartDetails_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
            Steema.TeeChart.Editor.Show(tChart1);
		}


        private void tChart1_MouseDown(object sender, MouseEventArgs e)
        {
            Console.WriteLine("Mouse Down");
            pointDrag = false;
        }

        private void tChart1_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            Console.WriteLine("Mouse Up");
            if (pointDrag )
            {
                timeSeriesSpreadsheet1.SetCellValue(prevRowIndex, prevColIndex, newvalue);
                prevRowIndex = -1;
            }
            if (GraphDrawNeeded)
            {
                GraphDrawNeeded = false;
                Graph();
            }
        }

        int prevColIndex = -1;
        int prevRowIndex = -1;
        double newvalue = 0;
        bool pointDrag = false;
		private void dragPoint1_Drag(Steema.TeeChart.Tools.DragPoint sender, int Index)
		{

            Console.WriteLine("dragPoint1_Drag");
            int seriesIndex = comboBoxEnableDragPoint.SelectedIndex;
            if (seriesIndex > 0)
            {
                newvalue = tChart1[seriesIndex - 1].YValues[Index];
                newvalue = System.Math.Round(newvalue, 2);
                tChart1[seriesIndex - 1].YValues[Index] = newvalue;

                int colIndex = seriesIndex;
                if (m_interval == TimeInterval.Irregular)
                    colIndex = seriesIndex * 2 - 1;

                prevRowIndex = Index;
                prevColIndex = colIndex;
                pointDrag = true;
               // GraphDrawNeeded = true;
            }
            else
            {
                pointDrag = false;
                newvalue = -998877;
                prevColIndex = -1;
                prevRowIndex = -1;
            }
		}

		private void GetTimeSeries()
		{
            HydrometHost svr = HydrometInfoUtility.HydrometServerFromPreferences();

            string query = cmd.GetDefaultQuery();  // filters out extra reference series.

            if (query.Trim() == "")
                hydrometDataTable = new DataTable("empty");
            else
            if (m_interval == TimeInterval.Daily)
            {
                hydrometDataTable = HydrometDataUtility.ArchiveTable(svr, query, T1, T2);
            }
            else
                if (m_interval == TimeInterval.Irregular)
            {
                int interval = 0;
                var tokens = comboBoxInterval.SelectedItem.ToString().Split(' ');
                if (tokens.Length > 0)
                    int.TryParse(tokens[0], out interval);

                hydrometDataTable = HydrometDataUtility.DayFilesTable(svr, query, T1, T2, interval: interval);
            }
            else
                    if (m_interval == TimeInterval.Monthly)
            {

                hydrometDataTable = HydrometDataUtility.MPollTable(svr, query, T1, T2);
            }

		}

        

        private void checkBoxShowBadData_CheckedChanged(object sender, EventArgs e)
        {
            Graph();
        }

        private void checkBoxShowPoints_CheckedChanged(object sender, EventArgs e)
        {
            Graph();
        }

        private void buttonForward_Click(object sender, EventArgs e)
        {
            TimeSpan ts = new TimeSpan(T2.Ticks - T1.Ticks);

            T1 = T2;
            T2 = T1.AddDays(ts.Days);
            RefreshClick(this, EventArgs.Empty);
        }

        private void buttonBack_Click(object sender, EventArgs e)
        {
            TimeSpan ts = new TimeSpan(T2.Ticks - T1.Ticks);

            T2 = T1;
            T1 = T2.AddDays(-ts.Days);
            RefreshClick(this, EventArgs.Empty);
        }

        private void USGSMonthlyReport_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            CommandLine cmd = new CommandLine(HydrometInfoUtility.ExpandQuery( comboBoxInputs.Text,m_interval), m_interval);
               HydrometHost svr = HydrometInfoUtility.HydrometServerFromPreferences();
               var list = cmd.CreateSeries(svr);
            list.Read(T1, T2);

            

            List<string> lines = new List<string>();
            foreach (var s in list)
            {
                var txt = Usgs.UsgsMonthlyTextReport(s, includeMonthlyFlags.Checked);
                lines.AddRange(txt);
                
            }

             Usgs.DisplayLines(lines.ToArray());
        }

        private void MonthlyInventory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            var s = comboBoxInputs.Text.Split(' ');
            if (s.Length > 0)
            {
                var cbtt = s[0].Trim();

                var txt = HydrometInfoUtility.LookupMonthlyInventory(cbtt);

                Usgs.DisplayLines(txt);
            }
        }

        private void Lookbutton_Click(object sender, EventArgs e)
        {
            var f = new Look.LookForm();
            f.DataType = m_interval;
            if (f.ShowDialog() == DialogResult.OK)
            {
                comboBoxInputs.Text = f.PassSearch;
            }
        }

        private void comboBoxInputs_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                buttonDownload.PerformClick();
            }
        }

        private void linkLabelPrint_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            tChart1.Printer.Landscape = true;
            tChart1.Printer.Preview();
        }
    }
}
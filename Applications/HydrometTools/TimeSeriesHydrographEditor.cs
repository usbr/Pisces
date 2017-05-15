using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Diagnostics;
using System.Windows.Forms;
using System.IO;
using Reclamation.Core;
using Reclamation.TimeSeries;
using System.Web.Security;
using System.Collections.Generic;
using Reclamation.TimeSeries.Hydromet;
using Reclamation.TimeSeries.Forms;
using System.Text.RegularExpressions;
using Reclamation.TimeSeries.Analysis;
using Reclamation.TimeSeries.Graphing;

namespace HydrometTools
{

	/// <summary>
	/// A table and graph for Viewing and editing hydromet data
	/// </summary>
	public class TimeSeriesHydrographEditor : System.Windows.Forms.UserControl
	{
	
		private Steema.TeeChart.Tools.DragPoint dragPoint1;

        string originalDataXmlFilename;
		DataTable dataTable;
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
        private Label labelFileName;
        private Label label2;
        private Button buttonOpenFile;
        private CheckBox checkBoxShowBadData;
        TimeInterval m_db;
        private CheckBox checkBoxShowPoints;
        private LinkLabel linkLabelUsgs;
        private YearSelector yearSelector1;
        private MonthRangePicker monthRangePicker1;
        private CheckBox checkBoxWaterYear;
        private CheckBox checkBoxCelsius;
        private ITimeSeriesSpreadsheet timeSeriesSpreadsheet1;

        
        Steema.TeeChart.Tools.Annotation annotation1;


        public TimeSeriesHydrographEditor()
        {
            InitializeComponent();
            m_db = TimeInterval.Daily;
        }

        public TimeSeriesHydrographEditor(TimeInterval db)
		{
			InitializeComponent();

#if SpreadsheetGear
            var uc = new TimeSeriesSpreadsheetSG();
#else
            var uc = new TimeSeriesSpreadsheet();
#endif 
            uc.Parent = this.panelGraphTable;
            uc.Dock = DockStyle.Fill;
            uc.BringToFront();

            timeSeriesSpreadsheet1 = uc;

            m_db = db;
            //SetupTimeSelector(db);

            this.checkBoxShowPoints.Checked = UserPreference.Lookup("ShowPoints") == "True";


            annotation1 = new Steema.TeeChart.Tools.Annotation(tChart1.Chart);
            
            originalDataXmlFilename = Path.Combine(FileUtility.GetTempPath(), db.ToString() +DateTime.Now.Ticks+ "_download.xml");

            checkBoxShowBadData.Visible = false;

            Logger.WriteLine(m_db.ToString(),"ui");
           if (m_db == TimeInterval.Daily)
           {
                BackColor = Color.Lavender;
           }

            
			dragPoint1 = new Steema.TeeChart.Tools.DragPoint();
			this.dragPoint1.Style = Steema.TeeChart.Tools.DragPointStyles.Y;
			this.tChart1.Tools.Add(this.dragPoint1);
			this.dragPoint1.Drag += new Steema.TeeChart.Tools.DragPointEventHandler(this.dragPoint1_Drag);


            tChart1.MouseMove += new MouseEventHandler(tChart1_MouseMove);
            var nearest = new Steema.TeeChart.Tools.NearestPoint(tChart1.Chart);

			LoadSiteList();
            this.comboBoxInputs.Text = UserPreference.Lookup("Inputs" + m_db.ToString());

            
			this.dragPoint1.Active = false;
			this.dragPoint1.Series = null;
        //    HydrometEdits.Progress += new ProgressEventHandler(HydrometEdits_Progress);

            timeSeriesSpreadsheet1.UpdateCompleted += new EventHandler<EventArgs>(timeSeriesSpreadsheet1_UpdateCompleted);
		}

        //private DateTime T1
        //{
        //    get
        //    {
        //        if (m_db == HydrometDataBase.MPoll)
        //            return timeSelectorBeginEndWaterYear1.T1;
        //        return timeSelector2.T1;
        //    }
        //    set
        //    {
        //        if (m_db == HydrometDataBase.MPoll)
        //        {
        //            timeSelectorBeginEndWaterYear1.T1 = value;
        //        }
        //        else
        //        {
        //            timeSelector2.T1 = value;
        //        }
        //    }
        //}

        //private DateTime T2
        //{
        //    get
        //    {
        //        if (m_db == HydrometDataBase.MPoll)
        //            return timeSelectorBeginEndWaterYear1.T2;
        //        return timeSelector2.T2;
        //    }
        //    set
        //    {
        //        if (m_db == HydrometDataBase.MPoll)
        //        {
        //            timeSelectorBeginEndWaterYear1.T2 = value;
        //        }
        //        else
        //        {
        //            timeSelector2.T2 = value;
        //        }
        //    }
        //}

        //private void SetupTimeSelector(HydrometDataBase db)
        //{
        //    if (db == HydrometDataBase.MPoll)
        //    {
        //        timeSelector2.Visible = false;
        //    }
        //    else
        //    {
        //        timeSelectorBeginEndWaterYear1.Visible = false;
        //    }
        //}

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TimeSeriesHydrographEditor));
            Reclamation.Core.MonthDayRange monthDayRange1 = new Reclamation.Core.MonthDayRange();
            this.comboBoxInputs = new System.Windows.Forms.ComboBox();
            this.buttonUpload = new System.Windows.Forms.Button();
            this.linkLabelChartDetails = new System.Windows.Forms.LinkLabel();
            this.buttonDownload = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.checkBoxShowBadData = new System.Windows.Forms.CheckBox();
            this.checkBoxShowPoints = new System.Windows.Forms.CheckBox();
            this.panelGraphTable = new System.Windows.Forms.Panel();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.tChart1 = new Steema.TeeChart.TChart();
            this.labelFileName = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonOpenFile = new System.Windows.Forms.Button();
            this.linkLabelUsgs = new System.Windows.Forms.LinkLabel();
            this.yearSelector1 = new Reclamation.TimeSeries.Forms.YearSelector();
            this.monthRangePicker1 = new Reclamation.TimeSeries.Forms.MonthRangePicker();
            this.checkBoxWaterYear = new System.Windows.Forms.CheckBox();
            this.checkBoxCelsius = new System.Windows.Forms.CheckBox();
            this.panelGraphTable.SuspendLayout();
            this.SuspendLayout();
            // 
            // comboBoxInputs
            // 
            this.comboBoxInputs.Location = new System.Drawing.Point(3, 28);
            this.comboBoxInputs.Name = "comboBoxInputs";
            this.comboBoxInputs.Size = new System.Drawing.Size(349, 24);
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
            this.buttonUpload.Location = new System.Drawing.Point(352, 64);
            this.buttonUpload.Name = "buttonUpload";
            this.buttonUpload.Size = new System.Drawing.Size(75, 23);
            this.buttonUpload.TabIndex = 21;
            this.buttonUpload.Text = "Save";
            this.buttonUpload.UseVisualStyleBackColor = false;
            this.buttonUpload.Visible = false;
            // 
            // linkLabelChartDetails
            // 
            this.linkLabelChartDetails.Location = new System.Drawing.Point(196, 66);
            this.linkLabelChartDetails.Name = "linkLabelChartDetails";
            this.linkLabelChartDetails.Size = new System.Drawing.Size(72, 23);
            this.linkLabelChartDetails.TabIndex = 18;
            this.linkLabelChartDetails.TabStop = true;
            this.linkLabelChartDetails.Text = "chart details";
            this.linkLabelChartDetails.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelChartDetails_LinkClicked);
            // 
            // buttonDownload
            // 
            this.buttonDownload.ForeColor = System.Drawing.SystemColors.ControlText;
            this.buttonDownload.Location = new System.Drawing.Point(272, 64);
            this.buttonDownload.Name = "buttonDownload";
            this.buttonDownload.Size = new System.Drawing.Size(75, 23);
            this.buttonDownload.TabIndex = 16;
            this.buttonDownload.Text = "Refresh";
            this.buttonDownload.Click += new System.EventHandler(this.RefreshClick);
            this.buttonDownload.KeyDown += new System.Windows.Forms.KeyEventHandler(this.buttonDownload_KeyDown);
            // 
            // checkBoxShowBadData
            // 
            this.checkBoxShowBadData.AutoSize = true;
            this.checkBoxShowBadData.Checked = true;
            this.checkBoxShowBadData.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxShowBadData.Location = new System.Drawing.Point(358, 24);
            this.checkBoxShowBadData.Name = "checkBoxShowBadData";
            this.checkBoxShowBadData.Size = new System.Drawing.Size(150, 21);
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
            this.checkBoxShowPoints.Location = new System.Drawing.Point(358, 39);
            this.checkBoxShowPoints.Name = "checkBoxShowPoints";
            this.checkBoxShowPoints.Size = new System.Drawing.Size(104, 21);
            this.checkBoxShowPoints.TabIndex = 33;
            this.checkBoxShowPoints.Text = "show points";
            this.checkBoxShowPoints.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip1.SetToolTip(this.checkBoxShowPoints, "show point on graph for each timestamp");
            this.checkBoxShowPoints.UseVisualStyleBackColor = true;
            this.checkBoxShowPoints.CheckedChanged += new System.EventHandler(this.checkBoxShowPoints_CheckedChanged);
            // 
            // panelGraphTable
            // 
            this.panelGraphTable.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelGraphTable.Controls.Add(this.splitter1);
            this.panelGraphTable.Controls.Add(this.tChart1);
            this.panelGraphTable.Location = new System.Drawing.Point(0, 88);
            this.panelGraphTable.Name = "panelGraphTable";
            this.panelGraphTable.Size = new System.Drawing.Size(929, 393);
            this.panelGraphTable.TabIndex = 27;
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(510, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(6, 393);
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
            this.tChart1.Axes.Left.Title.Transparent = true;
            // 
            // 
            // 
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
            this.tChart1.Dock = System.Windows.Forms.DockStyle.Left;
            this.tChart1.Location = new System.Drawing.Point(0, 0);
            this.tChart1.Name = "tChart1";
            this.tChart1.Size = new System.Drawing.Size(510, 393);
            this.tChart1.TabIndex = 3;
            this.tChart1.Click += new System.EventHandler(this.tChart1_Click);
            this.tChart1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.tChart1_MouseUp);
            // 
            // labelFileName
            // 
            this.labelFileName.AutoSize = true;
            this.labelFileName.Location = new System.Drawing.Point(101, 7);
            this.labelFileName.Name = "labelFileName";
            this.labelFileName.Size = new System.Drawing.Size(46, 17);
            this.labelFileName.TabIndex = 29;
            this.labelFileName.Text = "label2";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(49, 7);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 17);
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
            this.linkLabelUsgs.Location = new System.Drawing.Point(17, 62);
            this.linkLabelUsgs.Name = "linkLabelUsgs";
            this.linkLabelUsgs.Size = new System.Drawing.Size(132, 23);
            this.linkLabelUsgs.TabIndex = 35;
            this.linkLabelUsgs.TabStop = true;
            this.linkLabelUsgs.Text = "usgs";
            this.linkLabelUsgs.Visible = false;
            this.linkLabelUsgs.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelUsgs_LinkClicked);
            // 
            // yearSelector1
            // 
            this.yearSelector1.Location = new System.Drawing.Point(479, 64);
            this.yearSelector1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.yearSelector1.Name = "yearSelector1";
            this.yearSelector1.SelectedYears = new int[] {
        1977,
        2001,
        2005};
            this.yearSelector1.Size = new System.Drawing.Size(289, 20);
            this.yearSelector1.TabIndex = 37;
            // 
            // monthRangePicker1
            // 
            this.monthRangePicker1.BeginningMonth = 10;
            this.monthRangePicker1.Location = new System.Drawing.Point(497, 24);
            this.monthRangePicker1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.monthRangePicker1.MonthDayRange = monthDayRange1;
            this.monthRangePicker1.Name = "monthRangePicker1";
            this.monthRangePicker1.Size = new System.Drawing.Size(428, 34);
            this.monthRangePicker1.TabIndex = 36;
            // 
            // checkBoxWaterYear
            // 
            this.checkBoxWaterYear.AutoSize = true;
            this.checkBoxWaterYear.Checked = true;
            this.checkBoxWaterYear.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxWaterYear.Location = new System.Drawing.Point(497, 1);
            this.checkBoxWaterYear.Name = "checkBoxWaterYear";
            this.checkBoxWaterYear.Size = new System.Drawing.Size(96, 21);
            this.checkBoxWaterYear.TabIndex = 38;
            this.checkBoxWaterYear.Text = "water year";
            this.checkBoxWaterYear.UseVisualStyleBackColor = true;
            this.checkBoxWaterYear.CheckedChanged += new System.EventHandler(this.checkBoxWaterYear_CheckedChanged);
            // 
            // checkBoxCelsius
            // 
            this.checkBoxCelsius.AutoSize = true;
            this.checkBoxCelsius.Location = new System.Drawing.Point(584, 1);
            this.checkBoxCelsius.Name = "checkBoxCelsius";
            this.checkBoxCelsius.Size = new System.Drawing.Size(257, 21);
            this.checkBoxCelsius.TabIndex = 39;
            this.checkBoxCelsius.Text = "display water temperature in Celsius";
            this.checkBoxCelsius.UseVisualStyleBackColor = true;
            // 
            // TimeSeriesHydrographEditor
            // 
            this.Controls.Add(this.checkBoxCelsius);
            this.Controls.Add(this.checkBoxWaterYear);
            this.Controls.Add(this.yearSelector1);
            this.Controls.Add(this.monthRangePicker1);
            this.Controls.Add(this.linkLabelUsgs);
            this.Controls.Add(this.checkBoxShowPoints);
            this.Controls.Add(this.checkBoxShowBadData);
            this.Controls.Add(this.buttonOpenFile);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.labelFileName);
            this.Controls.Add(this.panelGraphTable);
            this.Controls.Add(this.buttonUpload);
            this.Controls.Add(this.comboBoxInputs);
            this.Controls.Add(this.linkLabelChartDetails);
            this.Controls.Add(this.buttonDownload);
            this.Name = "TimeSeriesHydrographEditor";
            this.Size = new System.Drawing.Size(929, 481);
            this.Load += new System.EventHandler(this.TimeSeriesEditor_Load);
            this.panelGraphTable.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		void LoadSiteList()
		{
            string fn = "site.txt";
            if (m_db == TimeInterval.Irregular)
                fn = "day_site.txt";
            else if (m_db == TimeInterval.Daily)
                fn = "arc_site.txt";
            else if (m_db == TimeInterval.Monthly)
                fn = "mpoll_site.txt";

            string property = m_db.ToString() + "FileName";
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
            UserPreference.Save(m_db.ToString() + "FileName", filename);
        }

		private void RefreshClick(object sender, System.EventArgs e)
		{
			if( this.comboBoxInputs.Text.IndexOf(":") ==0)
				return;

			UserPreference.Save("Inputs"+m_db.ToString(),this.comboBoxInputs.Text);
			
			this.dragPoint1.Active = false;
			Cursor = Cursors.WaitCursor;
            timeSeriesSpreadsheet1.Clear();
            Application.DoEvents();
			try
			{
				dataTable = this.GetTimeSeries();
                dataTable.AcceptChanges();
                dataTable.RowChanged += new DataRowChangeEventHandler(dataTable_RowChanged);
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
			if( dataTable == null)
				return;


            Logger.WriteLine(dataTable.Rows.Count + " rows of data read", "ui");
            if( dataTable.Rows.Count >0)
			dataTable.WriteXml(originalDataXmlFilename,XmlWriteMode.WriteSchema);

            Graph();

            SetupUsgsLink();
            timeSeriesSpreadsheet1.SetDataTable(dataTable, m_db,true);
            timeSeriesSpreadsheet1.AutoFlagDayFiles = UserPreference.Lookup("AutoFlagDayFiles") == "True";

            //this.comboBoxEditSeries.Items.Clear();
            //this.comboBoxEditSeries.Items.Add("None");
            //for(int i=0; i<tChart1.Series.Count; i++)
            //{
            //    string columnName = tChart1.Series[i].Title;
            //    this.comboBoxEditSeries.Items.Add(columnName);
            //}

            //this.comboBoxEditSeries.SelectedIndex =0;

		}

        string usgsUrl = "";
        private void SetupUsgsLink()
        {
            this.linkLabelUsgs.Visible = false;
            usgsUrl = "";
            if (dataTable != null && dataTable.Columns.Count > 1 && dataTable.Rows.Count >0)
            {
                string cbtt = this.comboBoxInputs.Text.Trim().Split(' ')[0];
                // check for USGS id..
                string altId = HydrometInfoUtility.LookupAltID(cbtt);
                if (altId.Trim().Length > 0 && Regex.IsMatch(altId,"[0-9]{7,10}"))
                {
                    linkLabelUsgs.Text = "usgs " + altId;
                    usgsUrl = "http://waterdata.usgs.gov/nwis/uv?format=html&period=7&site_no="+altId;
                    linkLabelUsgs.Visible = true;
                }

                
            }
        }

        private void linkLabelUsgs_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(usgsUrl);
        }
        

        private string GetHeaderTitle( out string subTitle)
        {
            subTitle = "";
            var query = HydrometInfoUtility.ExpandQuery(this.comboBoxInputs.Text, TimeInterval.Daily);

            // single cbtt?
            var tokens = query.Split(',');

            var cbttList = new List<string>();
            foreach (var item in tokens)
            {
                string s = item.Trim().Split(' ')[0];
                if (!cbttList.Contains(s))
                    cbttList.Add(s);
            }

            if (cbttList.Count == 1)
            {
                subTitle = HydrometInfoUtility.LookupGroupDescription(cbttList[0]);
                return HydrometInfoUtility.LookupSiteDescription(cbttList[0]);

            }
            return "";
        }

        //private Color[] GetColors()
        //{
        //    var rval = new List<Color>();

        //    foreach (Steema.TeeChart.Styles.Series s in tChart1.Series)
        //    {
        //        rval.Add(s.Color);
        //    }
        //    return rval.ToArray();
        //}

        void dataTable_RowChanged(object sender, DataRowChangeEventArgs e)
        {
            
            if( !timeSeriesSpreadsheet1.SuspendUpdates)
               Graph();
        }

		
		/// <summary>
		/// graphs data in dataTable
		/// </summary>
		void Graph()
		{
            UserPreference.Save("ShowPoints", this.checkBoxShowPoints.Checked.ToString());

            tChart1.Text = "";
			if (dataTable == null)
				return;
            Console.WriteLine("graph()");

			tChart1.Series.Clear();
            tChart1.Zoom.Undo();
			int sz = dataTable.Columns.Count;
            if (sz == 2 || (sz == 3 && m_db == TimeInterval.Irregular) 
                || (sz == 3 && m_db == TimeInterval.Monthly))  // single graph series.
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
            if (m_db == TimeInterval.Irregular || m_db == TimeInterval.Monthly)
                increment = 2;
            tChart1.Axes.Custom.RemoveAll();
            tChart1.Panel.MarginLeft = 3;
            tChart1.Axes.Left.Title.Text = "";
            tChart1.Axes.Right.Title.Text = "";
            tChart1.Axes.Bottom.Labels.DateTimeFormat = "MM/dd";
			for(int i=1; i<sz; i+=increment)
			{
				try 
				{
					string columnName = dataTable.Columns[i].ColumnName;
                    double avg = AverageOfColumn(dataTable, columnName);
                    Steema.TeeChart.Styles.Line series = MakeSeries(dataTable, columnName, avg);

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

                /*    //if (i == 1)
                    //{
                    //    vertLabel1 = Hydromet.LookupMcfPcodeDescription(cbtt, pcode);
                    //    tChart1.Axes.Left.Title.Text = vertLabel1;
                    //    firstPcode = pcode;
                    //}
                    //else
                    //{ // determine if we should use right vertical axis.
                    //    if (firstPcode.ToLower().Trim() != pcode.Trim().ToLower())
                    //    {
                    //        series.VertAxis = Steema.TeeChart.Styles.VerticalAxis.Right;
                    //    }
                    //}
                 */


				tChart1.Series.Add(series);
				}
				catch(Exception e)
				{
                    MessageBox.Show(e.ToString()+ " series index "+i);
                    Logger.WriteLine(e.ToString(),"ui");
				}

                if (tChart1.Series.Count > 0)
                {
                    dragPoint1.Series = tChart1[0];
                    dragPoint1.Active = true;
                }
                else
                {
                    dragPoint1.Active = false;
                }
			}
            //tChart1.Zoom.ZoomPercent(94);
			this.comboBoxEditSeries_SelectedIndexChanged(null,null);
		}

        private string LookupUnits(string pcode)
        {
            if (m_db == TimeInterval.Daily)
                return HydrometInfoUtility.LookupDailyUnits(pcode);
            if (m_db == TimeInterval.Irregular)
                return HydrometInfoUtility.LookupDayfileUnits(pcode);

            if( m_db == TimeInterval.Monthly )
                return HydrometMonthlySeries.LookupUnits(pcode);

            return "";
        }


		private Steema.TeeChart.Styles.Line MakeSeries(DataTable table , string columnName, double avg)
		{
			Steema.TeeChart.Styles.Line series1 = new Steema.TeeChart.Styles.Line();

			series1.XValues.DateTime = true;
			series1.ShowInLegend = true;
			series1.Pointer.Visible = true;
			series1.Pointer.HorizSize = 2;
			series1.Pointer.VertSize = 2;
			Color[] colors = {Color.Red,Color.Green,Color.Blue,Color.Black,Color.Orange,Color.Aquamarine,Color.DarkGreen,Color.Purple,Color.Aqua,Color.BlueViolet,Color.Brown,Color.BurlyWood,Color.CadetBlue,Color.Chartreuse, Color.Chocolate,Color.Coral,Color.CornflowerBlue};

			if( tChart1.Series.Count <colors.Length)
			{
				series1.Color = colors[tChart1.Series.Count];
			}
			series1.Title = columnName;

			int sz = table.Rows.Count;
			for(int i=0; i<sz; i++)
			{
				DateTime date = (DateTime)table.Rows[i][0];
                
                bool plotPoint = true;
                if (m_db == TimeInterval.Irregular)
                {
                    string flag = " ";
                    int idx = table.Columns.IndexOf(columnName);
                    idx++; // flag column is next
                    if (!checkBoxShowBadData.Checked && table.Rows[i][idx] != DBNull.Value)
                    {
                        flag = table.Rows[i][idx].ToString().Trim();
                        plotPoint = IsGoodDayfileFlag(flag);
                    }
                }
				if( table.Rows[i][columnName] != System.DBNull.Value && plotPoint)
				{
					double val = (double)table.Rows[i][columnName];
					series1.Add((double)date.ToOADate(),val);
				}
				else
				{
					series1.Add((double)date.ToOADate(),avg,Color.Transparent);
				}
			}
			return series1;
		}

        private bool IsGoodDayfileFlag(string flag)
        {
            string f = flag.Trim();
            if (f == "" || f == " " || f == "e")
                return true;
            return false;
        }


		private double AverageOfColumn(DataTable table , string columnName)
		{
			int sz = table.Rows.Count;
			int counter =0;
			double rval =0;
			for(int i=0; i<sz; i++)
			{

                bool plotPoint = true;
                if (!checkBoxShowBadData.Checked && m_db == TimeInterval.Irregular)
                {
                    string flag = "";
                    int idx = table.Columns.IndexOf(columnName);
                    idx++; // flag column is next
                    if (table.Rows[i][idx] != DBNull.Value)
                    {
                        flag = table.Rows[i][idx].ToString().Trim();
                        plotPoint = IsGoodDayfileFlag(flag);
                    }
                }

				if( table.Rows[i][columnName] != System.DBNull.Value
                    && plotPoint)
				{
					double x =(double)table.Rows[i][columnName];
					rval += x;
					counter++;
				}
			}
			if(counter >0)
				return rval/counter;
			else return 0;
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

        
        
		private void tChart1_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{

            // point may have been moved with mouse.
            if (prevRowIndex >= 0)
            {
                timeSeriesSpreadsheet1.SetCellValue(prevRowIndex, prevColIndex, newvalue);
            }

			if( GraphDrawNeeded)
			{
				GraphDrawNeeded = false;
				Graph();
			}
		}

        //private void menuItemOpenSpreadsheet_Click(object sender, System.EventArgs e)
        //{
        //    if( this.dataTable == null || dataTable.Rows.Count <=0)
        //        return;
        //    string csvFile = Path.ChangeExtension(Path.GetTempFileName(),".csv");
        //    CsvFile.WriteToCSV(this.dataTable,csvFile,false);
        //    ProcessStartInfo psi  = new ProcessStartInfo(csvFile);
        //    Process p = Process.Start(psi);
        //    Process.Start(csvFile);
        //}

		private void comboBoxInputs_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			this.RefreshClick(sender,e);
		}

		private void comboBoxEditSeries_SelectedIndexChanged(object sender, System.EventArgs e)
		{
            //if (comboBoxEditSeries.SelectedIndex >0)
            //{
            //    dragPoint1.Series = tChart1[comboBoxEditSeries.SelectedIndex-1];
            //    dragPoint1.Active = true;
            //}
            //else
            //    dragPoint1.Active = false;

		}

		private void dataGrid1_CurrentCellChanged(object sender, System.EventArgs e)
		{
			Graph();
		}

		private void linkLabelChartDetails_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
            Steema.TeeChart.Editor.Show(tChart1);
		}

        int prevColIndex = -1;
        int prevRowIndex = -1;
        double newvalue = 0;
		private void dragPoint1_Drag(Steema.TeeChart.Tools.DragPoint sender, int Index)
		{
            int seriesIndex = 0;// comboBoxEditSeries.SelectedIndex;

            if (seriesIndex >= 0 && tChart1.Series.Count > 0)
            {
                newvalue = tChart1[seriesIndex ].YValues[Index];
                newvalue = System.Math.Round(newvalue, 2);
                tChart1[seriesIndex].YValues[Index] = newvalue;
                
                prevRowIndex = Index;
                prevColIndex = seriesIndex+1; // offset for date column
                // update spreasheet...
                GraphDrawNeeded = true;
            }
            else
            {
                prevColIndex = -1;
                prevRowIndex = -1;
            }
		}

		private DataTable GetTimeSeries()
		{
            HydrometHost svr = HydrometInfoUtility.HydrometServerFromPreferences();
          
            string query = comboBoxInputs.Text.Trim();
            if (m_db == TimeInterval.Daily)
            {
                if (CbttOnly(query))
                {
                  string[] pcodes =  HydrometInfoUtility.ArchiveParameters(query);
                    if( pcodes.Length >0)
                    {
                        query = query + " " + String.Join(",", pcodes);
                    }
                }
                string[] tokens = query.Split(' ');
                if( tokens.Length != 2)
                    return new DataTable();

                string cbtt = tokens[0];
                string pcode = tokens[1];
                Series s = new HydrometDailySeries(cbtt, pcode, HydrometInfoUtility.HydrometServerFromPreferences());
                var sl = new SeriesList();
                sl.Add(s);

                int beginningMonth = 1;
                if (checkBoxWaterYear.Checked)
                    beginningMonth = 10;

                var wyList = PiscesAnalysis.WaterYears(sl, this.yearSelector1.SelectedYears, false, beginningMonth, true);

                if (checkBoxCelsius.Checked)
                {
                    for (int i = 0; i < wyList.Count; i++)
                    {
                        s = wyList[i];
                        if (s.Units.ToLower() == "degrees f")
                        {
                            Reclamation.TimeSeries.Math.ConvertUnits(s, "degrees C");
                        }
                    }
                }

                // remove months outside selected range
                var list = FilterBySelectedRange(this.monthRangePicker1.MonthDayRange, wyList);


                return list.ToDataTable(true);
              // return HydrometUtility.ArchiveTable(svr,query, T1, T2);
            }
            //else
            //    if (m_db == HydrometDataBase.Dayfiles)
            //    {

            //        if (CbttOnly(query))
            //        {
            //            string[] pcodes = Hydromet.DayfileParameters(query);
            //            if (pcodes.Length > 0)
            //            {
            //                query = query + " " + String.Join(",", pcodes);
            //            }
            //        }
            //        return HydrometUtility.DayFilesTable(svr,query, T1, T2);
            //    }
            //    else
            //        if (m_db == HydrometDataBase.MPoll)
            //        {

            //            return HydrometUtility.MPollTable(svr,query, T1, T2);
            //        }

            return new DataTable();
		}

        public static SeriesList FilterBySelectedRange(MonthDayRange range, SeriesList wyList)
        {
            SeriesList list = new SeriesList();
            foreach (Series item in wyList)
            {
                list.Add(Reclamation.TimeSeries.Math.ShiftToYear(Reclamation.TimeSeries.Math.Subset(item, range), 2000));
            }

            return list;
        }

        private bool CbttOnly(string query)
        {
            string[] pairs = query.Split(',');
            if (pairs.Length ==1)
            {
                var tokens = pairs[0].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (tokens.Length == 1)
                    return true;
            }

            return false;
        }

        private void checkBoxShowBadData_CheckedChanged(object sender, EventArgs e)
        {
            Graph();
        }

        private void checkBoxShowPoints_CheckedChanged(object sender, EventArgs e)
        {
            Graph();
        }

        private void TimeSeriesEditor_Load(object sender, EventArgs e)
        {
            // setup default date range..
            Reset();
        }

        private void tChart1_Click(object sender, EventArgs e)
        {
            
        }

        private void Reset()
        {
            var t = DateTime.Now;
            int wy = t.Year;
            if (t.Month >= 10)
                wy++;
            var t2 = t.AddMonths(1);

            if (t.Month == 9)
            {
                this.monthRangePicker1.MonthDayRange = new MonthDayRange(10, 1, 9, 30);
            }
            else
            {
                this.monthRangePicker1.MonthDayRange = new MonthDayRange(10, 1, t2.Month, DateTime.DaysInMonth(t2.Year, t2.Month));
            }
        }

        private void checkBoxWaterYear_CheckedChanged(object sender, EventArgs e)
        {

            if (checkBoxWaterYear.Checked)
                monthRangePicker1.BeginningMonth = 10;
            else
                monthRangePicker1.BeginningMonth = 1;
        }

        private void buttonDownload_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                buttonDownload.PerformClick();
            }
        }

        private void comboBoxInputs_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                buttonDownload.PerformClick();
            }
        }

       

       
	}
}
using System;
using System.Drawing;
using System.Collections;
using System.Diagnostics;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using Reclamation.Core;
using Reclamation.TimeSeries.Hydromet;
using System.Web.Security;
using HydrometTools.Advanced;
using Reclamation.TimeSeries.Forms;
using Reclamation.TimeSeries;
using Reclamation.TimeSeries.Hydromet.Operations;
using HydrometTools.RecordWorkup;

namespace HydrometTools
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class FormMain : System.Windows.Forms.Form
	{
	   Control arcEditor;
       Control dayEditor;
       Control mpollEditor;
       //Control graphTabs;
       Control hydroEditor;

       private TabPage tabPageFcplot;

		private System.ComponentModel.IContainer components=null;
        private System.Windows.Forms.MainMenu mainMenu1;
        private TabControl tabControl1;
        private TabPage tabPageDay;
        private TabPage tabPageArc;
        private TabPage tabPageMPoll;
        private TabPage tabPageSnowGG;
        private TabPage tabPageSetup;
        private Settings setup1;
        private TabPage tabPageUpdater;
        private TabPage tabPageRating;
        private RatingTableDisplay ratingTable1;
        private TabPage tabPageAdvanced;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel toolStripStatusLabel1;
        private TabPage tabPageAgrimet;
        private AgriMet.AgriMetTab agriMetTab1;
        private TabPage tabPageHydrographEditor;
        private System.Windows.Forms.ToolTip toolTip1;
        private TabPage tabPageStats;
        private TabPage tabPageReports;
        private TabControl tabControl2;
        private TabPage tabPageDailyImport;
        //private ImportDaily importUI1;
        private TabPage tabPageFdrImport;
        //private Import.ImportFDRTemperature importFDRTemperature1;
        private TabPage tabPageRecords;
        private TabPage tabPageShifts;
        private FcPlot.FcPlotUI fcUi;

		public FormMain()
        {
            var host = HydrometInfoUtility.HydrometServerFromPreferences();
            if (host == HydrometHost.PN)
            {
                UserPreference.SetDefault("HydrometServer", HydrometHost.PNLinux.ToString(), false);
            }

            HydrometInfoUtility.SetDefaultHydrometServer();

            UserPreference.SetDefault("HideStatusDialog", "False", false);
            UserPreference.SetDefault("AutoFlagDayFiles", "True", false);


            InitializeComponent();

            
            this.Text += " " + Application.ProductVersion;

           // FileUtility.CleanTempPath();
            Application.EnableVisualStyles();
            TabPageManager tabManager = new TabPageManager(tabControl1);
            Logger.OnLogEvent += new StatusEventHandler(Logger_OnLogEvent);
            UpdateTabs();
            //MakeMinimalVersion();
        }

        private void MakeMinimalVersion()
        {
            //this.tabControl1.Controls.Remove(this.tabPageDay);
            //this.tabControl1.Controls.Remove(this.tabPageArc);
            this.tabControl1.Controls.Remove(this.tabPageMPoll);
            //this.tabControl1.Controls.Remove(this.tabPageSnowGG);
            //this.tabControl1.Controls.Remove(this.tabPageSetup);
            this.setup1.HideNotificationSettings();
            this.setup1.HideAlarms();
            this.tabControl1.Controls.Remove(this.tabPageUpdater);
            this.tabControl1.Controls.Remove(this.tabPageRating);
            this.tabControl1.Controls.Remove(this.tabPageAdvanced);
            this.tabControl1.Controls.Remove(this.tabPageAgrimet);
            this.tabControl1.Controls.Remove(this.tabPageHydrographEditor);
            this.tabControl1.Controls.Remove(this.tabPageStats);
            this.tabControl1.Controls.Remove(this.tabPageReports);
            this.tabControl1.Controls.Remove(this.tabPageFcplot);
            this.tabControl1.Controls.Remove(this.tabPageRecords);
        }

       

        void Logger_OnLogEvent(object sender, StatusEventArgs e)
        {
            if (e.Tag == "ui" && this.statusStrip1.Items.Count > 0)
            {
                this.statusStrip1.Items[0].Text = e.Message;
                Application.DoEvents();
            }
        }

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.mainMenu1 = new System.Windows.Forms.MainMenu(this.components);
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageDay = new System.Windows.Forms.TabPage();
            this.tabPageArc = new System.Windows.Forms.TabPage();
            this.tabPageMPoll = new System.Windows.Forms.TabPage();
            this.tabPageRecords = new System.Windows.Forms.TabPage();
            this.tabPageSnowGG = new System.Windows.Forms.TabPage();
            this.tabPageSetup = new System.Windows.Forms.TabPage();
            this.tabPageUpdater = new System.Windows.Forms.TabPage();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPageDailyImport = new System.Windows.Forms.TabPage();
            this.tabPageFdrImport = new System.Windows.Forms.TabPage();
            this.tabPageRating = new System.Windows.Forms.TabPage();
            this.ratingTable1 = new HydrometTools.RatingTableDisplay();
            this.tabPageAdvanced = new System.Windows.Forms.TabPage();
            this.tabPageAgrimet = new System.Windows.Forms.TabPage();
            this.agriMetTab1 = new HydrometTools.AgriMet.AgriMetTab();
            this.tabPageHydrographEditor = new System.Windows.Forms.TabPage();
            this.tabPageStats = new System.Windows.Forms.TabPage();
            this.tabPageReports = new System.Windows.Forms.TabPage();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tabPageShifts = new System.Windows.Forms.TabPage();
            this.tabControl1.SuspendLayout();
            this.tabPageUpdater.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabPageRating.SuspendLayout();
            this.tabPageAgrimet.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPageDay);
            this.tabControl1.Controls.Add(this.tabPageArc);
            this.tabControl1.Controls.Add(this.tabPageMPoll);
            this.tabControl1.Controls.Add(this.tabPageRecords);
            this.tabControl1.Controls.Add(this.tabPageSnowGG);
            this.tabControl1.Controls.Add(this.tabPageSetup);
            this.tabControl1.Controls.Add(this.tabPageUpdater);
            this.tabControl1.Controls.Add(this.tabPageRating);
            this.tabControl1.Controls.Add(this.tabPageAdvanced);
            this.tabControl1.Controls.Add(this.tabPageAgrimet);
            this.tabControl1.Controls.Add(this.tabPageHydrographEditor);
            this.tabControl1.Controls.Add(this.tabPageStats);
            this.tabControl1.Controls.Add(this.tabPageReports);
            this.tabControl1.Controls.Add(this.tabPageShifts);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(942, 522);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabPageDay
            // 
            this.tabPageDay.Location = new System.Drawing.Point(4, 22);
            this.tabPageDay.Name = "tabPageDay";
            this.tabPageDay.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageDay.Size = new System.Drawing.Size(934, 496);
            this.tabPageDay.TabIndex = 0;
            this.tabPageDay.Text = "Instant";
            this.tabPageDay.UseVisualStyleBackColor = true;
            // 
            // tabPageArc
            // 
            this.tabPageArc.Location = new System.Drawing.Point(4, 22);
            this.tabPageArc.Name = "tabPageArc";
            this.tabPageArc.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageArc.Size = new System.Drawing.Size(934, 496);
            this.tabPageArc.TabIndex = 1;
            this.tabPageArc.Text = "Daily";
            this.tabPageArc.UseVisualStyleBackColor = true;
            // 
            // tabPageMPoll
            // 
            this.tabPageMPoll.Location = new System.Drawing.Point(4, 22);
            this.tabPageMPoll.Name = "tabPageMPoll";
            this.tabPageMPoll.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageMPoll.Size = new System.Drawing.Size(934, 496);
            this.tabPageMPoll.TabIndex = 8;
            this.tabPageMPoll.Text = "Monthly";
            this.tabPageMPoll.UseVisualStyleBackColor = true;
            // 
            // tabPageRecords
            // 
            this.tabPageRecords.Location = new System.Drawing.Point(4, 22);
            this.tabPageRecords.Name = "tabPageRecords";
            this.tabPageRecords.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageRecords.Size = new System.Drawing.Size(934, 496);
            this.tabPageRecords.TabIndex = 17;
            this.tabPageRecords.Text = "Daily Records";
            this.tabPageRecords.UseVisualStyleBackColor = true;
            // 
            // tabPageSnowGG
            // 
            this.tabPageSnowGG.Location = new System.Drawing.Point(4, 22);
            this.tabPageSnowGG.Name = "tabPageSnowGG";
            this.tabPageSnowGG.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSnowGG.Size = new System.Drawing.Size(934, 496);
            this.tabPageSnowGG.TabIndex = 2;
            this.tabPageSnowGG.Text = "SnowGG";
            this.tabPageSnowGG.UseVisualStyleBackColor = true;
            // 
            // tabPageSetup
            // 
            this.tabPageSetup.Location = new System.Drawing.Point(4, 22);
            this.tabPageSetup.Name = "tabPageSetup";
            this.tabPageSetup.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSetup.Size = new System.Drawing.Size(934, 496);
            this.tabPageSetup.TabIndex = 3;
            this.tabPageSetup.Text = "settings";
            this.tabPageSetup.UseVisualStyleBackColor = true;
            // 
            // tabPageUpdater
            // 
            this.tabPageUpdater.Controls.Add(this.tabControl2);
            this.tabPageUpdater.Location = new System.Drawing.Point(4, 22);
            this.tabPageUpdater.Name = "tabPageUpdater";
            this.tabPageUpdater.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageUpdater.Size = new System.Drawing.Size(934, 496);
            this.tabPageUpdater.TabIndex = 6;
            this.tabPageUpdater.Text = "Data Import";
            this.tabPageUpdater.UseVisualStyleBackColor = true;
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tabPageDailyImport);
            this.tabControl2.Controls.Add(this.tabPageFdrImport);
            this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl2.Location = new System.Drawing.Point(3, 3);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(928, 490);
            this.tabControl2.TabIndex = 0;
            // 
            // tabPageDailyImport
            // 
            this.tabPageDailyImport.Location = new System.Drawing.Point(4, 22);
            this.tabPageDailyImport.Name = "tabPageDailyImport";
            this.tabPageDailyImport.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageDailyImport.Size = new System.Drawing.Size(920, 464);
            this.tabPageDailyImport.TabIndex = 0;
            this.tabPageDailyImport.Text = "Daily";
            this.tabPageDailyImport.UseVisualStyleBackColor = true;
            // 
            // tabPageFdrImport
            // 
            this.tabPageFdrImport.Location = new System.Drawing.Point(4, 22);
            this.tabPageFdrImport.Name = "tabPageFdrImport";
            this.tabPageFdrImport.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageFdrImport.Size = new System.Drawing.Size(920, 464);
            this.tabPageFdrImport.TabIndex = 1;
            this.tabPageFdrImport.Text = "FDR - Water Quality";
            this.tabPageFdrImport.UseVisualStyleBackColor = true;
            // 
            // tabPageRating
            // 
            this.tabPageRating.Controls.Add(this.ratingTable1);
            this.tabPageRating.Location = new System.Drawing.Point(4, 22);
            this.tabPageRating.Name = "tabPageRating";
            this.tabPageRating.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageRating.Size = new System.Drawing.Size(934, 496);
            this.tabPageRating.TabIndex = 7;
            this.tabPageRating.Text = "Rating Tables";
            this.tabPageRating.UseVisualStyleBackColor = true;
            // 
            // ratingTable1
            // 
            this.ratingTable1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ratingTable1.Location = new System.Drawing.Point(3, 3);
            this.ratingTable1.Margin = new System.Windows.Forms.Padding(4);
            this.ratingTable1.Name = "ratingTable1";
            this.ratingTable1.Size = new System.Drawing.Size(928, 490);
            this.ratingTable1.TabIndex = 0;
            // 
            // tabPageAdvanced
            // 
            this.tabPageAdvanced.Location = new System.Drawing.Point(4, 22);
            this.tabPageAdvanced.Name = "tabPageAdvanced";
            this.tabPageAdvanced.Size = new System.Drawing.Size(934, 496);
            this.tabPageAdvanced.TabIndex = 9;
            this.tabPageAdvanced.Text = "Advanced";
            this.tabPageAdvanced.UseVisualStyleBackColor = true;
            // 
            // tabPageAgrimet
            // 
            this.tabPageAgrimet.Controls.Add(this.agriMetTab1);
            this.tabPageAgrimet.Location = new System.Drawing.Point(4, 22);
            this.tabPageAgrimet.Name = "tabPageAgrimet";
            this.tabPageAgrimet.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageAgrimet.Size = new System.Drawing.Size(934, 496);
            this.tabPageAgrimet.TabIndex = 10;
            this.tabPageAgrimet.Text = "AgriMet";
            this.tabPageAgrimet.UseVisualStyleBackColor = true;
            // 
            // agriMetTab1
            // 
            this.agriMetTab1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.agriMetTab1.Location = new System.Drawing.Point(3, 3);
            this.agriMetTab1.Margin = new System.Windows.Forms.Padding(4);
            this.agriMetTab1.Name = "agriMetTab1";
            this.agriMetTab1.Size = new System.Drawing.Size(928, 490);
            this.agriMetTab1.TabIndex = 0;
            // 
            // tabPageHydrographEditor
            // 
            this.tabPageHydrographEditor.Location = new System.Drawing.Point(4, 22);
            this.tabPageHydrographEditor.Name = "tabPageHydrographEditor";
            this.tabPageHydrographEditor.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageHydrographEditor.Size = new System.Drawing.Size(934, 496);
            this.tabPageHydrographEditor.TabIndex = 12;
            this.tabPageHydrographEditor.Text = "Hydrograph Editor";
            this.tabPageHydrographEditor.UseVisualStyleBackColor = true;
            // 
            // tabPageStats
            // 
            this.tabPageStats.Location = new System.Drawing.Point(4, 22);
            this.tabPageStats.Name = "tabPageStats";
            this.tabPageStats.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageStats.Size = new System.Drawing.Size(934, 496);
            this.tabPageStats.TabIndex = 15;
            this.tabPageStats.Text = "Check";
            this.tabPageStats.UseVisualStyleBackColor = true;
            // 
            // tabPageReports
            // 
            this.tabPageReports.Location = new System.Drawing.Point(4, 22);
            this.tabPageReports.Name = "tabPageReports";
            this.tabPageReports.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageReports.Size = new System.Drawing.Size(934, 496);
            this.tabPageReports.TabIndex = 16;
            this.tabPageReports.Text = "Reports";
            this.tabPageReports.UseVisualStyleBackColor = true;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 522);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(942, 22);
            this.statusStrip1.TabIndex = 1;
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(118, 17);
            this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // tabPageShifts
            // 
            this.tabPageShifts.Location = new System.Drawing.Point(4, 22);
            this.tabPageShifts.Name = "tabPageShifts";
            this.tabPageShifts.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageShifts.Size = new System.Drawing.Size(934, 496);
            this.tabPageShifts.TabIndex = 18;
            this.tabPageShifts.Text = "Shifts";
            this.tabPageShifts.UseVisualStyleBackColor = true;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(942, 544);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.statusStrip1);
            this.HelpButton = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Menu = this.mainMenu1;
            this.MinimumSize = new System.Drawing.Size(723, 419);
            this.Name = "FormMain";
            this.Text = "Hydromet/AgriMet Tools";
            this.tabControl1.ResumeLayout(false);
            this.tabPageUpdater.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.tabPageRating.ResumeLayout(false);
            this.tabPageAgrimet.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
            Logger.EnableLogger(true);
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(CurrentDomain_ProcessExit);
            Application.Idle += new EventHandler(Application_Idle);
            Application.ApplicationExit += new EventHandler(Application_ApplicationExit);
			Application.Run(new FormMain());
		}

        static DateTime prevDateTime = DateTime.Now;
        static void Application_Idle(object sender, EventArgs e)
        { // close connection to Hydromet after 1 hour idle.
            try
            {
                if (DateTime.Now >= prevDateTime.AddHours(1.0))
                {
                    prevDateTime = DateTime.Now;
                    HydrometTools.ssh.Utility.Close();
                }
            }
            catch (Exception)
            {
                
               
            }
        }

        static void Application_ApplicationExit(object sender, EventArgs e)
        {
            Cleanup();
        }

        private static void Cleanup()
        {
            try
            {
                HydrometTools.ssh.Utility.Close();
                PostgreSQL.ClearAllPools();
            }
            catch (Exception)
            {
            }
        }

        static void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            Cleanup();
        }

	
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateTabs();

        }


        Control advanced=null;


        private Stats.StatsControl statsControl1;
        private Reports.Reports reportControl1;
        private SnowGG.SnowGG snowGG1;
        private ImportDaily import1;
        DailyRecordWorkup records;
        private Shift.ShiftInput shifts;
        private void UpdateTabs()
        {


            if (tabControl1.SelectedTab == tabPageAdvanced && advanced == null )
            {
                if (Login.AdminPasswordIsValid())
                {
                    advanced = new AdvancedControl();
                    advanced.Parent = tabPageAdvanced;
                    advanced.Dock = DockStyle.Fill;
                }
            }

            if (tabControl1.SelectedTab == tabPageArc && arcEditor == null)
            {
                arcEditor = new TimeSeriesEditor(TimeInterval.Daily);
                arcEditor.Parent = tabPageArc;
                arcEditor.Dock = DockStyle.Fill;
            }
            else
                if (tabControl1.SelectedTab == tabPageDay && dayEditor == null)
            {
                dayEditor = new TimeSeriesEditor(TimeInterval.Irregular);
                dayEditor.Parent = tabPageDay;
                dayEditor.Dock = DockStyle.Fill;
            }
            else
                    if (tabControl1.SelectedTab == tabPageMPoll && mpollEditor == null)
            {
                HydrometHost svr = HydrometInfoUtility.HydrometServerFromPreferences();

                mpollEditor = new TimeSeriesEditor(TimeInterval.Monthly);
                mpollEditor.Parent = tabPageMPoll;
                mpollEditor.Dock = DockStyle.Fill;
            }
            else if (tabControl1.SelectedTab == tabPageSetup)
            {
                if (setup1 == null)
                {
                    setup1 = new Settings();
                    setup1.Parent = tabPageSetup;
                    setup1.Dock = DockStyle.Fill;
                }


                        setup1.ReadUserPref();
               }
                    //else if( tabControl1.SelectedTab == tabPageGraphTool && graphTabs == null)
                    //{
                    //    graphTabs = new Graphing.GraphingTabs();
                    //    graphTabs.Parent = tabPageGraphTool;
                    //    graphTabs.Dock = DockStyle.Fill;
                    //}
                    else if (tabControl1.SelectedTab == tabPageHydrographEditor && hydroEditor == null)
                    {
                        hydroEditor = new TimeSeriesHydrographEditor(TimeInterval.Daily);
                        hydroEditor.Parent = tabPageHydrographEditor;
                        hydroEditor.Dock = DockStyle.Fill;
                    }

                    else if ( tabControl1.SelectedTab == tabPageFcplot && fcUi == null)
                    {
                        fcUi = new FcPlot.FcPlotUI();
                        fcUi.Parent = tabPageFcplot;
                        fcUi.Dock = DockStyle.Fill;
                    }
                    else if (tabControl1.SelectedTab == tabPageStats
                        && statsControl1 == null)
                    {
                        statsControl1 = new Stats.StatsControl();
                        statsControl1.Parent = tabPageStats;
                        statsControl1.Dock = DockStyle.Fill;
                    }
            else if( tabControl1.SelectedTab == tabPageReports
                        && reportControl1 == null)
                    {
                        reportControl1 = new Reports.Reports(); //new Reports.YakimaStatus();
                        reportControl1.Parent = tabPageReports;
                        reportControl1.Dock = DockStyle.Fill;
                    }
                    else if( tabControl1.SelectedTab  == tabPageSnowGG
                        && snowGG1 == null)
                    {
                        snowGG1 = new SnowGG.SnowGG();
                        snowGG1.Parent = tabPageSnowGG;
                        snowGG1.Dock = DockStyle.Fill;
                    }
                    else if( tabControl1.SelectedTab == tabPageUpdater
                        &&  import1 == null )
                    {
                        import1 = new ImportDaily();
                        import1.Parent = tabPageDailyImport;
                        import1.Dock = DockStyle.Fill;
                       
                        var fdr = new Import.ImportFDRTemperature();
                        fdr.Parent = tabPageFdrImport;
                        fdr.Dock = DockStyle.Fill;
                    }
            else if( tabControl1.SelectedTab == tabPageRecords
                && records == null)
            {
                records = new DailyRecordWorkup();
                records.Parent = tabPageRecords;
                records.Dock = DockStyle.Fill;

            }

            if( tabControl1.SelectedTab == tabPageShifts && shifts == null)
            {
                shifts = new Shift.ShiftInput();
                shifts.Parent = tabPageShifts;
                shifts.Dock = DockStyle.Fill;
            }

            if (tabPageFcplot == null 
                && FcPlotDataSet.HasRuleCurves())
            {
                tabPageFcplot = new TabPage("FCplot");
                tabControl1.TabPages.Add(tabPageFcplot);
            }

        }

       

        private void linkLabelHelp_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(FileUtility.GetFileReference( "Hydromet Tools User Manual.pdf"));
        }

        private void richTextBoxLog_TextChanged(object sender, EventArgs e)
        {

        }

       
	}
}
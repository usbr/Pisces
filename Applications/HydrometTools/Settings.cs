using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Reclamation.Core;
using Reclamation.TimeSeries.Hydromet;
using Reclamation.TimeSeries.Forms.Alarms;

namespace HydrometTools
{
	/// <summary>
	/// Summary description for Setup.
	/// </summary>
	public class Settings : System.Windows.Forms.UserControl	{
        private CheckBox checkBoxHideStatus;
        private CheckBox checkBoxCompute;
        private GroupBox groupBox2;
        private IContainer components;
        private CheckBox checkBoxMultipleYAxis;
        private GroupBox groupBox3;
        private ToolTip toolTip1;
        private CheckBox checkBoxAutoFlagDayfiles;
        private TabControl tabControl1;
        private TabPage tabPageGeneral;
        private TabPage tabPageNotifications;
        private CheckBox checkBoxAdmin;
        private Reclamation.TimeSeries.Forms.Hydromet.ServerSelection serverSelection1;
        private TabPage tabPageLog;
        private RichTextBox richTextBoxLog;
        private Panel panel1;
        private Button buttonClearLog;
        private TabPage tabPageAlarms;
        private TextBox textBoxDbPassword;
        private GroupBox groupBox1;
        private Label label1;
        private Button buttonShowPassword;
        private TextBox textBoxDbName;
        private Label labelDbName;
        bool Ready = false;
		public Settings()
		{
			InitializeComponent();
			ReadUserPref();
            Ready = true;
            var c = new HydrometNotifications.UserInterface.NotificationMain();
            c.Parent = tabPageNotifications;
            c.Dock = DockStyle.Fill;
            c.Visible = true;
		}

        internal void HideNotificationSettings()
        {
            if( tabControl1.TabPages.Contains(tabPageNotifications))
                 tabControl1.TabPages.Remove(tabPageNotifications);
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
            this.checkBoxHideStatus = new System.Windows.Forms.CheckBox();
            this.checkBoxCompute = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.checkBoxAdmin = new System.Windows.Forms.CheckBox();
            this.checkBoxMultipleYAxis = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.checkBoxAutoFlagDayfiles = new System.Windows.Forms.CheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageGeneral = new System.Windows.Forms.TabPage();
            this.textBoxDbName = new System.Windows.Forms.TextBox();
            this.labelDbName = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonShowPassword = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxDbPassword = new System.Windows.Forms.TextBox();
            this.serverSelection1 = new Reclamation.TimeSeries.Forms.Hydromet.ServerSelection();
            this.tabPageNotifications = new System.Windows.Forms.TabPage();
            this.tabPageLog = new System.Windows.Forms.TabPage();
            this.richTextBoxLog = new System.Windows.Forms.RichTextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.buttonClearLog = new System.Windows.Forms.Button();
            this.tabPageAlarms = new System.Windows.Forms.TabPage();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPageGeneral.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabPageLog.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // checkBoxHideStatus
            // 
            this.checkBoxHideStatus.AutoSize = true;
            this.checkBoxHideStatus.Location = new System.Drawing.Point(7, 227);
            this.checkBoxHideStatus.Name = "checkBoxHideStatus";
            this.checkBoxHideStatus.Size = new System.Drawing.Size(111, 17);
            this.checkBoxHideStatus.TabIndex = 25;
            this.checkBoxHideStatus.Text = "hide status dialog ";
            this.checkBoxHideStatus.UseVisualStyleBackColor = true;
            this.checkBoxHideStatus.CheckedChanged += new System.EventHandler(this.checkBoxHideStatus_CheckedChanged);
            // 
            // checkBoxCompute
            // 
            this.checkBoxCompute.AutoSize = true;
            this.checkBoxCompute.Location = new System.Drawing.Point(12, 57);
            this.checkBoxCompute.Name = "checkBoxCompute";
            this.checkBoxCompute.Size = new System.Drawing.Size(124, 17);
            this.checkBoxCompute.TabIndex = 26;
            this.checkBoxCompute.Text = "enable computations";
            this.checkBoxCompute.UseVisualStyleBackColor = true;
            this.checkBoxCompute.CheckedChanged += new System.EventHandler(this.checkBoxCompute_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.checkBoxAdmin);
            this.groupBox2.Controls.Add(this.checkBoxCompute);
            this.groupBox2.Location = new System.Drawing.Point(286, 16);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(299, 101);
            this.groupBox2.TabIndex = 28;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "advanced";
            // 
            // checkBoxAdmin
            // 
            this.checkBoxAdmin.AutoSize = true;
            this.checkBoxAdmin.Location = new System.Drawing.Point(12, 30);
            this.checkBoxAdmin.Name = "checkBoxAdmin";
            this.checkBoxAdmin.Size = new System.Drawing.Size(164, 17);
            this.checkBoxAdmin.TabIndex = 27;
            this.checkBoxAdmin.Text = "I\'m an expert show all options";
            this.checkBoxAdmin.UseVisualStyleBackColor = true;
            this.checkBoxAdmin.CheckedChanged += new System.EventHandler(this.checkBoxAdmin_CheckedChanged);
            // 
            // checkBoxMultipleYAxis
            // 
            this.checkBoxMultipleYAxis.AutoSize = true;
            this.checkBoxMultipleYAxis.Location = new System.Drawing.Point(12, 19);
            this.checkBoxMultipleYAxis.Name = "checkBoxMultipleYAxis";
            this.checkBoxMultipleYAxis.Size = new System.Drawing.Size(173, 17);
            this.checkBoxMultipleYAxis.TabIndex = 37;
            this.checkBoxMultipleYAxis.Text = "use multiple y axis  (on left side)";
            this.toolTip1.SetToolTip(this.checkBoxMultipleYAxis, "create multiple left vertical axes");
            this.checkBoxMultipleYAxis.UseVisualStyleBackColor = true;
            this.checkBoxMultipleYAxis.CheckedChanged += new System.EventHandler(this.checkBoxMultipleYAxis_CheckedChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.checkBoxAutoFlagDayfiles);
            this.groupBox3.Controls.Add(this.checkBoxMultipleYAxis);
            this.groupBox3.Location = new System.Drawing.Point(286, 144);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(383, 76);
            this.groupBox3.TabIndex = 38;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "other";
            // 
            // checkBoxAutoFlagDayfiles
            // 
            this.checkBoxAutoFlagDayfiles.AutoSize = true;
            this.checkBoxAutoFlagDayfiles.Location = new System.Drawing.Point(11, 42);
            this.checkBoxAutoFlagDayfiles.Name = "checkBoxAutoFlagDayfiles";
            this.checkBoxAutoFlagDayfiles.Size = new System.Drawing.Size(237, 17);
            this.checkBoxAutoFlagDayfiles.TabIndex = 38;
            this.checkBoxAutoFlagDayfiles.Text = "automatically flag dayfiles with \'e\' during edits";
            this.toolTip1.SetToolTip(this.checkBoxAutoFlagDayfiles, "create multiple left vertical axes");
            this.checkBoxAutoFlagDayfiles.UseVisualStyleBackColor = true;
            this.checkBoxAutoFlagDayfiles.CheckedChanged += new System.EventHandler(this.checkBoxAutoFlagDayfiles_CheckedChanged);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPageGeneral);
            this.tabControl1.Controls.Add(this.tabPageNotifications);
            this.tabControl1.Controls.Add(this.tabPageLog);
            this.tabControl1.Controls.Add(this.tabPageAlarms);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(711, 511);
            this.tabControl1.TabIndex = 40;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabPageGeneral
            // 
            this.tabPageGeneral.Controls.Add(this.textBoxDbName);
            this.tabPageGeneral.Controls.Add(this.labelDbName);
            this.tabPageGeneral.Controls.Add(this.groupBox1);
            this.tabPageGeneral.Controls.Add(this.serverSelection1);
            this.tabPageGeneral.Controls.Add(this.checkBoxHideStatus);
            this.tabPageGeneral.Controls.Add(this.groupBox3);
            this.tabPageGeneral.Controls.Add(this.groupBox2);
            this.tabPageGeneral.Location = new System.Drawing.Point(4, 22);
            this.tabPageGeneral.Name = "tabPageGeneral";
            this.tabPageGeneral.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageGeneral.Size = new System.Drawing.Size(703, 485);
            this.tabPageGeneral.TabIndex = 0;
            this.tabPageGeneral.Text = "general";
            this.tabPageGeneral.UseVisualStyleBackColor = true;
            // 
            // textBoxDbName
            // 
            this.textBoxDbName.Location = new System.Drawing.Point(26, 180);
            this.textBoxDbName.Name = "textBoxDbName";
            this.textBoxDbName.Size = new System.Drawing.Size(186, 20);
            this.textBoxDbName.TabIndex = 44;
            this.textBoxDbName.Text = "timeseries";
            this.textBoxDbName.TextChanged += new System.EventHandler(this.textBoxDbName_TextChanged);
            // 
            // labelDbName
            // 
            this.labelDbName.AutoSize = true;
            this.labelDbName.Location = new System.Drawing.Point(23, 163);
            this.labelDbName.Name = "labelDbName";
            this.labelDbName.Size = new System.Drawing.Size(83, 13);
            this.labelDbName.TabIndex = 43;
            this.labelDbName.Text = "database name:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.buttonShowPassword);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.textBoxDbPassword);
            this.groupBox1.Location = new System.Drawing.Point(286, 227);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(361, 100);
            this.groupBox1.TabIndex = 42;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "relational database";
            // 
            // buttonShowPassword
            // 
            this.buttonShowPassword.Location = new System.Drawing.Point(265, 71);
            this.buttonShowPassword.Name = "buttonShowPassword";
            this.buttonShowPassword.Size = new System.Drawing.Size(75, 23);
            this.buttonShowPassword.TabIndex = 42;
            this.buttonShowPassword.Text = "show password";
            this.buttonShowPassword.UseVisualStyleBackColor = true;
            this.buttonShowPassword.Click += new System.EventHandler(this.buttonShowPassword_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 13);
            this.label1.TabIndex = 41;
            this.label1.Text = "password";
            // 
            // textBoxDbPassword
            // 
            this.textBoxDbPassword.Location = new System.Drawing.Point(9, 45);
            this.textBoxDbPassword.Name = "textBoxDbPassword";
            this.textBoxDbPassword.PasswordChar = '*';
            this.textBoxDbPassword.Size = new System.Drawing.Size(275, 20);
            this.textBoxDbPassword.TabIndex = 40;
            this.textBoxDbPassword.TextChanged += new System.EventHandler(this.textBoxDbPassword_TextChanged);
            // 
            // serverSelection1
            // 
            this.serverSelection1.Location = new System.Drawing.Point(7, 16);
            this.serverSelection1.Margin = new System.Windows.Forms.Padding(2);
            this.serverSelection1.Name = "serverSelection1";
            this.serverSelection1.Size = new System.Drawing.Size(248, 206);
            this.serverSelection1.TabIndex = 39;
            // 
            // tabPageNotifications
            // 
            this.tabPageNotifications.Location = new System.Drawing.Point(4, 22);
            this.tabPageNotifications.Name = "tabPageNotifications";
            this.tabPageNotifications.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageNotifications.Size = new System.Drawing.Size(703, 485);
            this.tabPageNotifications.TabIndex = 1;
            this.tabPageNotifications.Text = "notifications";
            this.tabPageNotifications.UseVisualStyleBackColor = true;
            // 
            // tabPageLog
            // 
            this.tabPageLog.Controls.Add(this.richTextBoxLog);
            this.tabPageLog.Controls.Add(this.panel1);
            this.tabPageLog.Location = new System.Drawing.Point(4, 22);
            this.tabPageLog.Name = "tabPageLog";
            this.tabPageLog.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageLog.Size = new System.Drawing.Size(703, 485);
            this.tabPageLog.TabIndex = 2;
            this.tabPageLog.Text = "log details";
            this.tabPageLog.UseVisualStyleBackColor = true;
            // 
            // richTextBoxLog
            // 
            this.richTextBoxLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxLog.Location = new System.Drawing.Point(3, 44);
            this.richTextBoxLog.Name = "richTextBoxLog";
            this.richTextBoxLog.Size = new System.Drawing.Size(697, 438);
            this.richTextBoxLog.TabIndex = 2;
            this.richTextBoxLog.Text = "";
            this.richTextBoxLog.VisibleChanged += new System.EventHandler(this.richTextBoxLog_VisibleChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.buttonClearLog);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(697, 41);
            this.panel1.TabIndex = 3;
            // 
            // buttonClearLog
            // 
            this.buttonClearLog.Location = new System.Drawing.Point(19, 12);
            this.buttonClearLog.Name = "buttonClearLog";
            this.buttonClearLog.Size = new System.Drawing.Size(75, 23);
            this.buttonClearLog.TabIndex = 0;
            this.buttonClearLog.Text = "Clear";
            this.buttonClearLog.UseVisualStyleBackColor = true;
            this.buttonClearLog.Click += new System.EventHandler(this.buttonClearLog_Click);
            // 
            // tabPageAlarms
            // 
            this.tabPageAlarms.Location = new System.Drawing.Point(4, 22);
            this.tabPageAlarms.Name = "tabPageAlarms";
            this.tabPageAlarms.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageAlarms.Size = new System.Drawing.Size(703, 485);
            this.tabPageAlarms.TabIndex = 3;
            this.tabPageAlarms.Text = "alarms";
            this.tabPageAlarms.UseVisualStyleBackColor = true;
            // 
            // Settings
            // 
            this.Controls.Add(this.tabControl1);
            this.Name = "Settings";
            this.Size = new System.Drawing.Size(711, 511);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPageGeneral.ResumeLayout(false);
            this.tabPageGeneral.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabPageLog.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion


		public void ReadUserPref()
		{

            this.checkBoxAdmin.Checked = UserPreference.Lookup("Admin") == "True" ;
            this.checkBoxHideStatus.Checked = UserPreference.Lookup("HideStatusDialog") == "True";
            this.checkBoxCompute.Checked = UserPreference.Lookup("EnableComputations") == "True";
            //this.checkBoxAgrimet.Checked = UserPreference.Lookup("AgrimetCalculations") == "True";
            this.checkBoxMultipleYAxis.Checked = UserPreference.Lookup("MultipleYAxis") == "True";
            this.checkBoxAutoFlagDayfiles.Checked = UserPreference.Lookup("AutoFlagDayFiles") == "True";

            string pw = UserPreference.Lookup("timeseries_database_password"); ;
            if( pw != "")
              this.textBoxDbPassword.Text = StringCipher.Decrypt(pw, "");

            this.textBoxDbName.Text = UserPreference.Lookup("TimeSeriesDatabaseName","timeseries");
		}

		public void SaveUserPref()
        {
            if (!Ready)
                return;


            UserPreference.Save("HideStatusDialog", this.checkBoxHideStatus.Checked.ToString());
            UserPreference.Save("Admin", this.checkBoxAdmin.Checked.ToString());
            UserPreference.Save("EnableComputations", this.checkBoxCompute.Checked.ToString());
            //UserPreference.Save("AgrimetCalculations", this.checkBoxAgrimet.Checked.ToString());
            UserPreference.Save("MultipleYAxis", this.checkBoxMultipleYAxis.Checked.ToString());
            UserPreference.Save("AutoFlagDayFiles", this.checkBoxAutoFlagDayfiles.Checked.ToString());
            var pw = textBoxDbPassword.Text;
            UserPreference.Save("timeseries_database_password",StringCipher.Encrypt(pw,""));
            UserPreference.Save("TimeSeriesDatabaseName", this.textBoxDbName.Text);
        }

       
        private void radioButtonYakHydromet_CheckedChanged(object sender, System.EventArgs e)
		{
            SaveUserPref();
		}

        private void radioButtonPnHydromet_CheckedChanged(object sender, EventArgs e)
        {
            SaveUserPref();
        }

        private void checkBoxHideStatus_CheckedChanged(object sender, EventArgs e)
        {
            SaveUserPref();
        }

       

        
        private void checkBoxCompute_CheckedChanged(object sender, EventArgs e)
        {
            SaveUserPref();
        }

        private void checkBoxAgrimet_CheckedChanged(object sender, EventArgs e)
        {
            SaveUserPref();
        }

        private void radioButtonGP_CheckedChanged(object sender, EventArgs e)
        {
            SaveUserPref();
        }

        private void checkBoxMultipleYAxis_CheckedChanged(object sender, EventArgs e)
        {
            SaveUserPref();
        }

        private void checkBoxAutoFlagDayfiles_CheckedChanged(object sender, EventArgs e)
        {
            SaveUserPref();
        }

        //private void buttonAlarms_Click(object sender, EventArgs e)
        //{
        //    var db = HydrometNotifications.AlarmDataSet.DB;
        //    var names = new string[] { "alarm_definition", "alarm_group","alarm_sites","alarm_history" };
        //    SqlTableEditor s = new SqlTableEditor(db, names);
        //    s.Show();

        //}

        private void checkBoxAdmin_CheckedChanged(object sender, EventArgs e)
        {
            SaveUserPref();
        }

        private void buttonClearLog_Click(object sender, EventArgs e)
        {
            Logger.LogHistory.Clear();
            richTextBoxLog.Lines = new string[] { };
        }

        private void richTextBoxLog_VisibleChanged(object sender, EventArgs e)
        {
            	
            if( richTextBoxLog.Visible )
            this.richTextBoxLog.Lines = Logger.LogHistory.ToArray();
        }


        AlarmManagerControl alarmUI;
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

            if( tabControl1.SelectedTab == tabPageAlarms)
            {
                if ( alarmUI == null)
                {
                    var db = Database.DB();
                    if ( db == null)
                        throw new Exception(
                    "Alarms require a database password. Please enter that in the settings/general tab");

                    alarmUI = new AlarmManagerControl(db.Server);
                    alarmUI.Parent = tabPageAlarms;
                    alarmUI.Dock = DockStyle.Fill;
                    alarmUI.BringToFront();
                }

            }
        }

        private void textBoxDbPassword_TextChanged(object sender, EventArgs e)
        {
            SaveUserPref();
        }

        private void buttonShowPassword_Click(object sender, EventArgs e)
        {
            this.textBoxDbPassword.PasswordChar = '\0';
        }

        private void textBoxDbName_TextChanged(object sender, EventArgs e)
        {
            SaveUserPref();
        }

        

	}
}

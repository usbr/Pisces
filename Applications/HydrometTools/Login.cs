// $Id: Setup.cs,v 1.6 2003/03/04 00:02:46 ktarbet Exp $
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using System.Windows.Forms;
using Reclamation.Core;
using System.Web.Security;

namespace HydrometTools
{
	/// <summary>
	/// Summary description for Setup.
	/// </summary>
	public class Login : System.Windows.Forms.Form
	{
        static string s_password = "";
        static string mpoll_password = "";
		private System.Windows.Forms.Button buttonOk;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.TextBox textBoxUser;
		private System.Windows.Forms.TextBox textBoxPass;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button buttonCancel;
		private System.ComponentModel.IContainer components;
        private System.Windows.Forms.ToolTip toolTip1;
        private GroupBox groupBoxPassMpoll;
        private Label label3;
        private TextBox textBoxPassMpoll;
        private GroupBox groupBoxHydromet;
        private CheckBox checkBoxRatingTable;
        private CheckBox checkBoxArchiver;
        private CheckBox checkBoxACE;
		public bool CancelClicked = true;

        public Login()
        {
            InitializeComponent();
            this.Password = s_password;
            this.MpollPassword = mpoll_password;
            ReadPreferences();
        }

		public string Password
		{
			get { return this.textBoxPass.Text; }
            set { this.textBoxPass.Text = value; }
		}

        public bool CalculateDailyValues
        {
            get { return this.checkBoxArchiver.Checked; }
        }

        public bool RatingTableMath
        {
            get { return this.checkBoxRatingTable.Checked; }
        }
        public string Username
        {
            get { return this.textBoxUser.Text.ToLower(); }
        }

        public string MpollPassword
        {
            get { return this.textBoxPassMpoll.Text; }
            set { this.textBoxPassMpoll.Text = value; }
        }

        public bool AceCheckboxVisible { 
            get
        { 
               return this.checkBoxACE.Visible;
        }
            set{ this.checkBoxACE.Visible= value; }
      }
        public bool MpollPasswordGroupVisible
        {
            get { return this.groupBoxPassMpoll.Visible; }
            set { this.groupBoxPassMpoll.Visible = value; }
        }

        public bool AdvancedOptionsVisible
        {
          //  get { return this.groupBoxHydromet.Visible; }
            set { this.groupBoxHydromet.Visible = value; }
        }

        public bool RatingTableCalculationEnabled
        {
            get { return this.checkBoxRatingTable.Enabled; }
            set { this.checkBoxRatingTable.Enabled = value; }
        }

        //public bool AgrimetOptionsVisible
        //{
        //   // get { return this.groupBoxAgrimet.Visible; }
        //    set { this.groupBoxAgrimet.Visible = value; }
        //}
       

        //public bool ComputeET
        //{
        //    get { return checkBoxET.Checked; }
        //}
        ////public bool SendToWeb
        //{
        //    get { return checkBoxWeb.Checked; }
        //}
        //public bool CropCharts
        //{
        //    get { return checkBoxCropCharts.Checked; }
        //}


		void ReadPreferences()
		{
            this.textBoxUser.Text = UserPreference.Lookup("UserName");
            this.Text = "Save changes " + UserPreference.Lookup("HydrometServer");

          //  this.checkBoxCropCharts.Checked = (UserPreference.Lookup("BuildCropCharts") == "True");
            //this.checkBoxET.Checked = (UserPreference.Lookup("ComputeET") == "True");
            this.checkBoxArchiver.Checked = (UserPreference.Lookup("RunArchiver") == "True");
//            this.checkBoxWeb.Checked = (UserPreference.Lookup("SendToWeb") == "True");

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Login));
            this.buttonOk = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxPass = new System.Windows.Forms.TextBox();
            this.textBoxUser = new System.Windows.Forms.TextBox();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.groupBoxPassMpoll = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxPassMpoll = new System.Windows.Forms.TextBox();
            this.groupBoxHydromet = new System.Windows.Forms.GroupBox();
            this.checkBoxACE = new System.Windows.Forms.CheckBox();
            this.checkBoxRatingTable = new System.Windows.Forms.CheckBox();
            this.checkBoxArchiver = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBoxPassMpoll.SuspendLayout();
            this.groupBoxHydromet.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonOk
            // 
            this.buttonOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOk.Location = new System.Drawing.Point(320, 16);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(75, 23);
            this.buttonOk.TabIndex = 0;
            this.buttonOk.Text = "Ok";
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.textBoxPass);
            this.groupBox1.Controls.Add(this.textBoxUser);
            this.groupBox1.Location = new System.Drawing.Point(16, 24);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(298, 79);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "login";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(24, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 23);
            this.label2.TabIndex = 3;
            this.label2.Text = "password";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(24, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 23);
            this.label1.TabIndex = 2;
            this.label1.Text = "username";
            // 
            // textBoxPass
            // 
            this.textBoxPass.Location = new System.Drawing.Point(96, 44);
            this.textBoxPass.Name = "textBoxPass";
            this.textBoxPass.PasswordChar = '*';
            this.textBoxPass.Size = new System.Drawing.Size(100, 20);
            this.textBoxPass.TabIndex = 1;
            this.toolTip1.SetToolTip(this.textBoxPass, "pnhyd0 password");
            // 
            // textBoxUser
            // 
            this.textBoxUser.Location = new System.Drawing.Point(96, 16);
            this.textBoxUser.Name = "textBoxUser";
            this.textBoxUser.Size = new System.Drawing.Size(100, 20);
            this.textBoxUser.TabIndex = 0;
            this.toolTip1.SetToolTip(this.textBoxUser, "username for pnhyd0");
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(320, 48);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 2;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // groupBoxPassMpoll
            // 
            this.groupBoxPassMpoll.Controls.Add(this.label3);
            this.groupBoxPassMpoll.Controls.Add(this.textBoxPassMpoll);
            this.groupBoxPassMpoll.Location = new System.Drawing.Point(16, 109);
            this.groupBoxPassMpoll.Name = "groupBoxPassMpoll";
            this.groupBoxPassMpoll.Size = new System.Drawing.Size(298, 56);
            this.groupBoxPassMpoll.TabIndex = 3;
            this.groupBoxPassMpoll.TabStop = false;
            this.groupBoxPassMpoll.Text = "mpoll login";
            this.groupBoxPassMpoll.Visible = false;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(24, 23);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 23);
            this.label3.TabIndex = 4;
            this.label3.Text = "password";
            // 
            // textBoxPassMpoll
            // 
            this.textBoxPassMpoll.Location = new System.Drawing.Point(96, 20);
            this.textBoxPassMpoll.Name = "textBoxPassMpoll";
            this.textBoxPassMpoll.PasswordChar = '*';
            this.textBoxPassMpoll.Size = new System.Drawing.Size(100, 20);
            this.textBoxPassMpoll.TabIndex = 0;
            // 
            // groupBoxHydromet
            // 
            this.groupBoxHydromet.Controls.Add(this.checkBoxACE);
            this.groupBoxHydromet.Controls.Add(this.checkBoxRatingTable);
            this.groupBoxHydromet.Controls.Add(this.checkBoxArchiver);
            this.groupBoxHydromet.Location = new System.Drawing.Point(16, 109);
            this.groupBoxHydromet.Name = "groupBoxHydromet";
            this.groupBoxHydromet.Size = new System.Drawing.Size(248, 61);
            this.groupBoxHydromet.TabIndex = 4;
            this.groupBoxHydromet.TabStop = false;
            this.groupBoxHydromet.Text = "advanced options";
            this.groupBoxHydromet.Visible = false;
            // 
            // checkBoxACE
            // 
            this.checkBoxACE.AutoSize = true;
            this.checkBoxACE.Location = new System.Drawing.Point(170, 18);
            this.checkBoxACE.Name = "checkBoxACE";
            this.checkBoxACE.Size = new System.Drawing.Size(70, 17);
            this.checkBoxACE.TabIndex = 2;
            this.checkBoxACE.Text = "ace table";
            this.checkBoxACE.UseVisualStyleBackColor = true;
            // 
            // checkBoxRatingTable
            // 
            this.checkBoxRatingTable.AutoSize = true;
            this.checkBoxRatingTable.Location = new System.Drawing.Point(27, 18);
            this.checkBoxRatingTable.Name = "checkBoxRatingTable";
            this.checkBoxRatingTable.Size = new System.Drawing.Size(133, 17);
            this.checkBoxRatingTable.TabIndex = 1;
            this.checkBoxRatingTable.Text = "Apply rating table math";
            this.checkBoxRatingTable.UseVisualStyleBackColor = true;
            // 
            // checkBoxArchiver
            // 
            this.checkBoxArchiver.AutoSize = true;
            this.checkBoxArchiver.Location = new System.Drawing.Point(27, 38);
            this.checkBoxArchiver.Name = "checkBoxArchiver";
            this.checkBoxArchiver.Size = new System.Drawing.Size(125, 17);
            this.checkBoxArchiver.TabIndex = 0;
            this.checkBoxArchiver.Text = "compute daily values";
            this.checkBoxArchiver.UseVisualStyleBackColor = true;
            // 
            // Login
            // 
            this.AcceptButton = this.buttonOk;
            this.AccessibleRole = System.Windows.Forms.AccessibleRole.Dialog;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(504, 228);
            this.Controls.Add(this.groupBoxHydromet);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.groupBoxPassMpoll);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Login";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Upload data";
            this.Load += new System.EventHandler(this.Login_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBoxPassMpoll.ResumeLayout(false);
            this.groupBoxPassMpoll.PerformLayout();
            this.groupBoxHydromet.ResumeLayout(false);
            this.groupBoxHydromet.PerformLayout();
            this.ResumeLayout(false);

		}
		#endregion

		private void buttonOk_Click(object sender, System.EventArgs e)
		{
            s_password = this.textBoxPass.Text;
            mpoll_password = this.textBoxPassMpoll.Text;
            UserPreference.Save("UserName", this.textBoxUser.Text);
            //UserPreference.Save("BuildCropCharts", checkBoxCropCharts.Checked.ToString());
            //UserPreference.Save("ComputeET",checkBoxET.Checked.ToString());
            UserPreference.Save("RunArchiver", checkBoxArchiver.Checked.ToString());
            //UserPreference.Save("SendToWeb", checkBoxWeb.Checked.ToString());

		 Close();
		 this.CancelClicked = false;
		}

		private void buttonCancel_Click(object sender, System.EventArgs e)
		{
			CancelClicked = true;
			Close();
		}

		private void Login_Load(object sender, System.EventArgs e)
		{
		
		}


        public static bool AdminPasswordIsValid()
        {
            return UserPreference.Lookup("Admin") == "True";
        }
    }
}

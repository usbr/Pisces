using System;
using System.Data;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Configuration;

namespace Reclamation.Core
{
	/// <summary>
	/// Summary description for Login.
	/// </summary>
  public class Login : System.Windows.Forms.Form
  {
    string _username="";
    string _password="";
    static DateTime _PasswordChanged = DateTime.MinValue;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.TextBox textBoxPass;
    private System.Windows.Forms.TextBox textBoxUser;
    private System.Windows.Forms.Button buttonCancel;
    private System.Windows.Forms.Button buttonOk;
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.Container components = null;

    public Login()
    {
      InitializeComponent();
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
      System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(Login));
      this.label2 = new System.Windows.Forms.Label();
      this.label1 = new System.Windows.Forms.Label();
      this.textBoxPass = new System.Windows.Forms.TextBox();
      this.textBoxUser = new System.Windows.Forms.TextBox();
      this.buttonCancel = new System.Windows.Forms.Button();
      this.buttonOk = new System.Windows.Forms.Button();
      this.SuspendLayout();
      // 
      // label2
      // 
      this.label2.Location = new System.Drawing.Point(12, 68);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(144, 16);
      this.label2.TabIndex = 7;
      this.label2.Text = "User Database Password";
      // 
      // label1
      // 
      this.label1.Location = new System.Drawing.Point(12, 24);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(100, 16);
      this.label1.TabIndex = 6;
      this.label1.Text = "User Database ID";
      // 
      // textBoxPass
      // 
      this.textBoxPass.Location = new System.Drawing.Point(12, 92);
      this.textBoxPass.Name = "textBoxPass";
      this.textBoxPass.PasswordChar = '*';
      this.textBoxPass.Size = new System.Drawing.Size(112, 20);
      this.textBoxPass.TabIndex = 5;
      this.textBoxPass.Text = "";
      // 
      // textBoxUser
      // 
      this.textBoxUser.Location = new System.Drawing.Point(12, 44);
      this.textBoxUser.Name = "textBoxUser";
      this.textBoxUser.TabIndex = 4;
      this.textBoxUser.Text = "";
      // 
      // buttonCancel
      // 
      this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.buttonCancel.Location = new System.Drawing.Point(238, 40);
      this.buttonCancel.Name = "buttonCancel";
      this.buttonCancel.TabIndex = 9;
      this.buttonCancel.TabStop = false;
      this.buttonCancel.Text = "Cancel";
      this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
      // 
      // buttonOk
      // 
      this.buttonOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonOk.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.buttonOk.Location = new System.Drawing.Point(238, 8);
      this.buttonOk.Name = "buttonOk";
      this.buttonOk.TabIndex = 8;
      this.buttonOk.Text = "Ok";
      this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
      // 
      // Login
      // 
      this.AcceptButton = this.buttonOk;
      this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
      this.ClientSize = new System.Drawing.Size(330, 124);
      this.Controls.Add(this.buttonCancel);
      this.Controls.Add(this.buttonOk);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.textBoxPass);
      this.Controls.Add(this.textBoxUser);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "Login";
      this.Text = "Login";
      this.Load += new System.EventHandler(this.Login_Load);
      this.ResumeLayout(false);

    }
    #endregion

    private void buttonOk_Click(object sender, System.EventArgs e)
    {

      _password=this.textBoxPass.Text;
      _username = this.textBoxUser.Text;
    }

    private void buttonCancel_Click(object sender, System.EventArgs e)
    {
      Close();
    }

    private void Login_Load(object sender, System.EventArgs e)
    {
    }


   

    public  string Username
    {
      get {return  _username;}
      set {_username = value;}

     }

    public string Password
    {
      get { return _password;}
      set { _password=value;}
    }
	}
}

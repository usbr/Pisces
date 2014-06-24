using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Reclamation.TimeSeries.Forms
{
  /// <summary>
  /// Summary description for InputOffset.
  /// </summary>
  public class InputOffset : System.Windows.Forms.Form
  {
    private System.Windows.Forms.TextBox textBox1;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Button buttonCancel;
    private System.Windows.Forms.Button buttonOK;
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.Container components = null;

    public InputOffset()
    {
      InitializeComponent();
    }


    /// <summary>
    /// gets offset entered by user.
    /// </summary>
    public double Offset=0;
     
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
      this.textBox1 = new System.Windows.Forms.TextBox();
      this.label1 = new System.Windows.Forms.Label();
      this.buttonCancel = new System.Windows.Forms.Button();
      this.buttonOK = new System.Windows.Forms.Button();
      this.SuspendLayout();
      // 
      // textBox1
      // 
      this.textBox1.Location = new System.Drawing.Point(88, 24);
      this.textBox1.Name = "textBox1";
      this.textBox1.Size = new System.Drawing.Size(84, 20);
      this.textBox1.TabIndex = 0;
      this.textBox1.Text = "0";
      // 
      // label1
      // 
      this.label1.Location = new System.Drawing.Point(20, 28);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(56, 16);
      this.label1.TabIndex = 1;
      this.label1.Text = "offset";
      // 
      // buttonCancel
      // 
      this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.buttonCancel.Location = new System.Drawing.Point(244, 68);
      this.buttonCancel.Name = "buttonCancel";
      this.buttonCancel.TabIndex = 2;
      this.buttonCancel.Text = "Cancel";
      // 
      // buttonOK
      // 
      this.buttonOK.Location = new System.Drawing.Point(244, 36);
      this.buttonOK.Name = "buttonOK";
      this.buttonOK.TabIndex = 3;
      this.buttonOK.Text = "OK";
      this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
      // 
      // InputOffset
      // 
      this.AcceptButton = this.buttonOK;
      this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
      this.CancelButton = this.buttonCancel;
      this.ClientSize = new System.Drawing.Size(330, 100);
      this.Controls.Add(this.buttonOK);
      this.Controls.Add(this.buttonCancel);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.textBox1);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "InputOffset";
      this.Text = "Vertical offset for selection";
      this.ResumeLayout(false);

    }
    #endregion

    private void buttonOK_Click(object sender, System.EventArgs e)
    {
      bool anyError = false;
      Offset = 0;
      try
      {
        Offset = Convert.ToDouble(this.textBox1.Text);
      }
      catch(Exception ex)
      {
        anyError = true;
        this.DialogResult = DialogResult.Cancel;
        MessageBox.Show(ex.Message);
      }
        

      if( !anyError)
      {
        DialogResult = DialogResult.OK;
        this.Close();

      }
    }
  }
}

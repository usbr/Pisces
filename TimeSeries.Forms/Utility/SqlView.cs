using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.IO;
namespace Reclamation.Core
{
	/// <summary>
	/// SqlView is a popup window that shows all the sql commands that
	/// have been sent to the server. SqlView also allows sending sql
	/// commands to the dataaser server.  This can be activated by double 
	/// clicking the status bar in the main soi application. 
	/// </summary>
	public class SqlView : System.Windows.Forms.Form
	{
    private System.Windows.Forms.DataGrid dataGrid1;
    private System.Windows.Forms.Panel panel1;
    private System.Windows.Forms.Button buttonSql;
		private System.Windows.Forms.ListBox listBoxSql;
		private System.Windows.Forms.TextBox textBoxSql;
		private System.Windows.Forms.Button buttonClear;
        private System.Windows.Forms.Label labelRecordCount;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.RadioButton radioButtonTable;
		private System.Windows.Forms.RadioButton radioButtonCmd;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
        private Button buttonExcel;

        BasicDBServer m_server;

		public SqlView(BasicDBServer server)
		{
			InitializeComponent();
            m_server = server;
            this.Text = "SQL Viewer";

		  this.listBoxSql.Items.Clear();
	      this.listBoxSql.Items.AddRange(server.SqlCommands.ToArray());
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
            this.dataGrid1 = new System.Windows.Forms.DataGrid();
            this.panel1 = new System.Windows.Forms.Panel();
            this.buttonExcel = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButtonCmd = new System.Windows.Forms.RadioButton();
            this.radioButtonTable = new System.Windows.Forms.RadioButton();
            this.labelRecordCount = new System.Windows.Forms.Label();
            this.buttonClear = new System.Windows.Forms.Button();
            this.textBoxSql = new System.Windows.Forms.TextBox();
            this.listBoxSql = new System.Windows.Forms.ListBox();
            this.buttonSql = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid1)).BeginInit();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGrid1
            // 
            this.dataGrid1.DataMember = "";
            this.dataGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGrid1.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.dataGrid1.Location = new System.Drawing.Point(0, 312);
            this.dataGrid1.Name = "dataGrid1";
            this.dataGrid1.Size = new System.Drawing.Size(752, 134);
            this.dataGrid1.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.buttonExcel);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.labelRecordCount);
            this.panel1.Controls.Add(this.buttonClear);
            this.panel1.Controls.Add(this.textBoxSql);
            this.panel1.Controls.Add(this.listBoxSql);
            this.panel1.Controls.Add(this.buttonSql);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(752, 312);
            this.panel1.TabIndex = 0;
            // 
            // buttonExcel
            // 
            this.buttonExcel.Location = new System.Drawing.Point(139, 283);
            this.buttonExcel.Name = "buttonExcel";
            this.buttonExcel.Size = new System.Drawing.Size(64, 23);
            this.buttonExcel.TabIndex = 16;
            this.buttonExcel.Text = "excel";
            this.buttonExcel.Click += new System.EventHandler(this.buttonExcel_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButtonCmd);
            this.groupBox1.Controls.Add(this.radioButtonTable);
            this.groupBox1.Location = new System.Drawing.Point(232, 256);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(124, 56);
            this.groupBox1.TabIndex = 15;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "query type";
            // 
            // radioButtonCmd
            // 
            this.radioButtonCmd.Location = new System.Drawing.Point(12, 36);
            this.radioButtonCmd.Name = "radioButtonCmd";
            this.radioButtonCmd.Size = new System.Drawing.Size(104, 16);
            this.radioButtonCmd.TabIndex = 1;
            this.radioButtonCmd.Text = "command";
            // 
            // radioButtonTable
            // 
            this.radioButtonTable.Checked = true;
            this.radioButtonTable.Location = new System.Drawing.Point(12, 16);
            this.radioButtonTable.Name = "radioButtonTable";
            this.radioButtonTable.Size = new System.Drawing.Size(104, 16);
            this.radioButtonTable.TabIndex = 0;
            this.radioButtonTable.TabStop = true;
            this.radioButtonTable.Text = "table";
            // 
            // labelRecordCount
            // 
            this.labelRecordCount.Location = new System.Drawing.Point(16, 272);
            this.labelRecordCount.Name = "labelRecordCount";
            this.labelRecordCount.Size = new System.Drawing.Size(208, 16);
            this.labelRecordCount.TabIndex = 13;
            this.labelRecordCount.Text = "0 Records";
            // 
            // buttonClear
            // 
            this.buttonClear.Location = new System.Drawing.Point(496, 272);
            this.buttonClear.Name = "buttonClear";
            this.buttonClear.Size = new System.Drawing.Size(64, 23);
            this.buttonClear.TabIndex = 12;
            this.buttonClear.Text = "Clear";
            this.buttonClear.Click += new System.EventHandler(this.buttonClear_Click_1);
            // 
            // textBoxSql
            // 
            this.textBoxSql.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxSql.Location = new System.Drawing.Point(8, 184);
            this.textBoxSql.Multiline = true;
            this.textBoxSql.Name = "textBoxSql";
            this.textBoxSql.Size = new System.Drawing.Size(736, 72);
            this.textBoxSql.TabIndex = 10;
            // 
            // listBoxSql
            // 
            this.listBoxSql.Dock = System.Windows.Forms.DockStyle.Top;
            this.listBoxSql.Location = new System.Drawing.Point(0, 0);
            this.listBoxSql.Name = "listBoxSql";
            this.listBoxSql.Size = new System.Drawing.Size(752, 173);
            this.listBoxSql.TabIndex = 9;
            this.listBoxSql.SelectedIndexChanged += new System.EventHandler(this.listBoxSql_SelectedIndexChanged);
            // 
            // buttonSql
            // 
            this.buttonSql.Location = new System.Drawing.Point(576, 272);
            this.buttonSql.Name = "buttonSql";
            this.buttonSql.Size = new System.Drawing.Size(88, 23);
            this.buttonSql.TabIndex = 6;
            this.buttonSql.Text = "Execute SQL";
            this.buttonSql.Click += new System.EventHandler(this.buttonSql_Click);
            // 
            // SqlView
            // 
            this.AcceptButton = this.buttonSql;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(752, 446);
            this.Controls.Add(this.dataGrid1);
            this.Controls.Add(this.panel1);
            this.Name = "SqlView";
            this.Text = "SOI SQL Viewer ";
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

    private void buttonSql_Click(object sender, System.EventArgs e)
    {
      if(this.textBoxSql.Text.Length>0)
      {
		Cursor = Cursors.WaitCursor;
		  try
		  {
		DataTable tbl ;
			  string sql =this.textBoxSql.Text;
//			  if( sql.ToLower().IndexOf("update") >=0
//				  || sql.ToLower().IndexOf("insert") >=0
//				  || sql.ToLower().IndexOf("drop ") >=0
//				  || sql.ToLower().IndexOf("truncate") >=0
//				  || sql.ToLower().IndexOf("get") ==0
//				  || sql.ToLower().IndexOf("set") ==0
//				  || sql.ToLower().IndexOf("create") ==0
//				  )
			  if( this.radioButtonCmd.Checked)
			  {
				  m_server.RunSqlCommand(sql);
				  tbl = new DataTable();
			  }
			  else
			  {
				  tbl = m_server.Table("test",this.textBoxSql.Text);
          if(tbl == null)
          {
		  this.dataGrid1.DataSource=null;
          return;
          }
			  }
			  this.dataGrid1.DataSource = tbl;
			  this.labelRecordCount.Text = tbl.Rows.Count.ToString()+" Records";

			  //this.dataGrid1.ReadOnly = true;
			  this.listBoxSql.Items.Add(this.textBoxSql.Text);
		  }
		  catch(Exception ex)
		  {
              string msg = "Error: " + ex.Message;
			  MessageBox.Show (msg, "Error ", 
				  MessageBoxButtons.OK, MessageBoxIcon.Error);

		  }
		  finally
		  {
			  Cursor = Cursors.Default;
		  }
        
      }
    }

    private void buttonClear_Click(object sender, System.EventArgs e)
    {
    this.dataGrid1.DataSource = null;
    }

    private void buttonSaveResults_Click(object sender, System.EventArgs e)
    {
      
    }


		private void listBoxSql_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			this.textBoxSql.Text = listBoxSql.Text;
		}

	
		private void buttonClear_Click_1(object sender, System.EventArgs e)
		{
            this.m_server.SqlCommands.Clear();
			this.listBoxSql.Items.Clear();
		}

        private void buttonExcel_Click(object sender, EventArgs e)
        {
            DataTable tbl = this.dataGrid1.DataSource as DataTable;
            string fn = FileUtility.GetTempFileName(".csv");
            DataTableOutput.Write(tbl, fn, true);

            System.Diagnostics.Process.Start(fn);

        }

    

	}
}

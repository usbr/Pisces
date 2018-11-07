using HydrometTools;
using Reclamation.TimeSeries;
using Reclamation.TimeSeries.Hydromet;

namespace Shift
{
    public class ShiftInput : System.Windows.Forms.UserControl
    {
        private System.Data.DataTable table = new System.Data.DataTable();

        private System.DateTime today = System.DateTime.Today;

        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.TextBox textBoxCbtt;

        private System.Windows.Forms.Label DateMeasuredLabel;

        private System.Windows.Forms.Label CBTTLabel;

        private System.Windows.Forms.TextBox DateMeasured;

        private System.Windows.Forms.Label HeaderLabel;

        private System.Windows.Forms.TextBox textBoxNewShift;

        private System.Windows.Forms.Label NewShiftLabel;

        private System.Windows.Forms.TextBox textBoxGageHeight;

        private System.Windows.Forms.Label DischargeLabel;

        private System.Windows.Forms.Label GageLabel;

        private System.Windows.Forms.TextBox textBoxDischarge;

        private System.Windows.Forms.Label CommentsLabel;

        private System.Windows.Forms.TextBox textBoxComments;

        private System.Windows.Forms.Button buttonSaveShift;

        private System.Windows.Forms.DataGridView DataView;

        private System.Windows.Forms.Label PCODELabel;

        private System.Windows.Forms.TextBox textBoxPcode;

        private System.Windows.Forms.Timer timer1;

        private System.Windows.Forms.GroupBox groupBox1;

        private System.Windows.Forms.LinkLabel linkLabelDailyQuery;

        private System.Windows.Forms.LinkLabel linkLabelShowAll;

        private System.Windows.Forms.LinkLabel linkLabelShowInExcel;

        private string cbtt
        {
            get
            {
                return this.textBoxCbtt.Text.ToUpper().Trim();
            }
        }

        public ShiftInput()
        {
            this.InitializeComponent();
            this.Defaults();
        }

        internal void Defaults()
        {
            this.DateMeasured.Text = this.today.ToString("d");
        }

        private void CBTT_Leave(object sender, System.EventArgs e)
        {
            if (!(this.cbtt == ""))
            {
                this.ReadShiftTable();
            }
        }

        private void SaveShift_Click(object sender, System.EventArgs e)
        {
            float? num = null;
            if (this.textBoxDischarge.Text.Trim() != "")
            {
                num = new float?(System.Convert.ToSingle(this.textBoxDischarge.Text));
            }
            float? num2 = null;
            if (this.textBoxGageHeight.Text.Trim() != "")
            {
                num2 = new float?(System.Convert.ToSingle(this.textBoxGageHeight.Text));
            }
            HydrometTools.Login login = new HydrometTools.Login();
            if (login.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    string text = HydrometEditsVMS.UpdateShift(login.Username, login.Password, this.cbtt.ToUpper().Trim(), this.textBoxPcode.Text.ToUpper().Trim(), (double)System.Convert.ToSingle(this.textBoxNewShift.Text));
                    if (text.ToLower().Contains("error"))
                    {
                        FormStatus.ShowStatus(text, true);
                    }
                    else
                    {
                        FormStatus.ShowStatus(text, false);
                        System.Console.WriteLine(text);
                        double shift = System.Convert.ToDouble(this.textBoxNewShift.Text);
                        string arg_1D9_1 = this.cbtt.ToUpper().Trim();
                        string arg_1D9_2 = this.textBoxPcode.Text.ToUpper().Trim();
                        System.DateTime arg_1D9_3 = System.Convert.ToDateTime(this.DateMeasured.Text);
                        float? num3 = num;
                        double? arg_1D9_4 = num3.HasValue ? new double?((double)num3.GetValueOrDefault()) : null;
                        num3 = num2;
                        Database.InsertShift(arg_1D9_1, arg_1D9_2, arg_1D9_3, arg_1D9_4, num3.HasValue ? new double?((double)num3.GetValueOrDefault()) : null, shift, this.textBoxComments.Text, System.DateTime.Now);
                        Reclamation.Core.BasicDBServer postgresServer = Reclamation.Core.PostgreSQL.GetPostgresServer("", "", "");
                        TimeSeriesName tn = new TimeSeriesName(this.cbtt.ToLower() + "_" + this.textBoxPcode.Text.ToLower(), "instant");
                        TimeSeriesDatabaseDataSet.seriespropertiesDataTable.Set("shift", shift.ToString(), tn, postgresServer);
                        this.ReadShiftTable();
                        this.textBoxPcode.Text = "";
                        this.textBoxCbtt.Text = "";
                        this.textBoxDischarge.Text = "";
                        this.textBoxComments.Text = "";
                        this.textBoxNewShift.Text = "";
                        this.textBoxGageHeight.Text = "";
                    }
                }
                catch (System.Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show(ex.Message);
                }
            }
        }

        private void ReadShiftTable()
        {
            this.table = Database.GetShiftsTable(this.cbtt.Trim());
            this.DataView.DataSource = this.table;
            this.DataView.Columns["id"].Visible = false;
            this.DataView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.DataView.Columns["stage"].DefaultCellStyle.Format = "F2";
            this.DataView.Columns["discharge"].DefaultCellStyle.Format = "F2";
            this.DataView.Columns["shift"].DefaultCellStyle.Format = "F2";
        }

        private void timer1_Tick(object sender, System.EventArgs e)
        {
            if (this.textBoxPcode.Text.Trim().Length != 0 && this.textBoxCbtt.Text.Trim().Length != 0 && this.textBoxNewShift.Text.Trim().Length != 0)
            {
                if (!this.buttonSaveShift.Enabled)
                {
                    this.buttonSaveShift.Enabled = true;
                }
            }
            else if (this.buttonSaveShift.Enabled)
            {
                this.buttonSaveShift.Enabled = false;
            }
        }

        private void linkLabelDailyQuery_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
        {
            this.table =  Database.GetDailyShifts(this.today.AddDays(30.0));
            this.DataView.DataSource = this.table;
            this.DataView.Columns["id"].Visible = false;
            this.DataView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.DataView.Columns["stage"].DefaultCellStyle.Format = "F2";
            this.DataView.Columns["discharge"].DefaultCellStyle.Format = "F2";
            this.DataView.Columns["shift"].DefaultCellStyle.Format = "F2";
        }

        private void linkLabelShowAll_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
        {
            this.table = Database.GetAllShifts();
            this.DataView.DataSource = this.table;
            this.DataView.Columns["id"].Visible = false;
            this.DataView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.DataView.Columns["stage"].DefaultCellStyle.Format = "F2";
            this.DataView.Columns["discharge"].DefaultCellStyle.Format = "F2";
            this.DataView.Columns["shift"].DefaultCellStyle.Format = "F2";
        }

        private void linkLabelShowInExcel_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
        {
            string tempFileName = Reclamation.Core.FileUtility.GetTempFileName(".csv");
            System.Data.DataTable dataTable = this.table.Copy();
            dataTable.PrimaryKey = new System.Data.DataColumn[0];
            dataTable.Columns.Remove("id");
            Reclamation.Core.CsvFile.WriteToCSV(dataTable, tempFileName, false, true);
            System.Diagnostics.Process.Start(tempFileName);
        }

        private void textBoxNewShift_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == System.Windows.Forms.Keys.Return)
            {
                this.buttonSaveShift.PerformClick();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && this.components != null)
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.textBoxCbtt = new System.Windows.Forms.TextBox();
            this.DateMeasuredLabel = new System.Windows.Forms.Label();
            this.CBTTLabel = new System.Windows.Forms.Label();
            this.DateMeasured = new System.Windows.Forms.TextBox();
            this.HeaderLabel = new System.Windows.Forms.Label();
            this.textBoxNewShift = new System.Windows.Forms.TextBox();
            this.NewShiftLabel = new System.Windows.Forms.Label();
            this.textBoxGageHeight = new System.Windows.Forms.TextBox();
            this.DischargeLabel = new System.Windows.Forms.Label();
            this.GageLabel = new System.Windows.Forms.Label();
            this.textBoxDischarge = new System.Windows.Forms.TextBox();
            this.CommentsLabel = new System.Windows.Forms.Label();
            this.textBoxComments = new System.Windows.Forms.TextBox();
            this.buttonSaveShift = new System.Windows.Forms.Button();
            this.DataView = new System.Windows.Forms.DataGridView();
            this.PCODELabel = new System.Windows.Forms.Label();
            this.textBoxPcode = new System.Windows.Forms.TextBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.linkLabelDailyQuery = new System.Windows.Forms.LinkLabel();
            this.linkLabelShowAll = new System.Windows.Forms.LinkLabel();
            this.linkLabelShowInExcel = new System.Windows.Forms.LinkLabel();
            ((System.ComponentModel.ISupportInitialize)(this.DataView)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBoxCbtt
            // 
            this.textBoxCbtt.Location = new System.Drawing.Point(75, 47);
            this.textBoxCbtt.Name = "textBoxCbtt";
            this.textBoxCbtt.Size = new System.Drawing.Size(100, 20);
            this.textBoxCbtt.TabIndex = 1;
            this.textBoxCbtt.Leave += new System.EventHandler(this.CBTT_Leave);
            // 
            // DateMeasuredLabel
            // 
            this.DateMeasuredLabel.AutoSize = true;
            this.DateMeasuredLabel.Location = new System.Drawing.Point(21, 31);
            this.DateMeasuredLabel.Name = "DateMeasuredLabel";
            this.DateMeasuredLabel.Size = new System.Drawing.Size(83, 13);
            this.DateMeasuredLabel.TabIndex = 2;
            this.DateMeasuredLabel.Text = "Date Measured:";
            // 
            // CBTTLabel
            // 
            this.CBTTLabel.AutoSize = true;
            this.CBTTLabel.Location = new System.Drawing.Point(22, 50);
            this.CBTTLabel.Name = "CBTTLabel";
            this.CBTTLabel.Size = new System.Drawing.Size(38, 13);
            this.CBTTLabel.TabIndex = 3;
            this.CBTTLabel.Text = "CBTT:";
            // 
            // DateMeasured
            // 
            this.DateMeasured.Location = new System.Drawing.Point(110, 28);
            this.DateMeasured.Name = "DateMeasured";
            this.DateMeasured.Size = new System.Drawing.Size(100, 20);
            this.DateMeasured.TabIndex = 4;
            // 
            // HeaderLabel
            // 
            this.HeaderLabel.AutoSize = true;
            this.HeaderLabel.Location = new System.Drawing.Point(5, 14);
            this.HeaderLabel.Name = "HeaderLabel";
            this.HeaderLabel.Size = new System.Drawing.Size(55, 13);
            this.HeaderLabel.TabIndex = 7;
            this.HeaderLabel.Text = "Shift Input";
            // 
            // textBoxNewShift
            // 
            this.textBoxNewShift.Location = new System.Drawing.Point(75, 98);
            this.textBoxNewShift.Name = "textBoxNewShift";
            this.textBoxNewShift.Size = new System.Drawing.Size(100, 20);
            this.textBoxNewShift.TabIndex = 3;
            this.textBoxNewShift.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxNewShift_KeyDown);
            // 
            // NewShiftLabel
            // 
            this.NewShiftLabel.AutoSize = true;
            this.NewShiftLabel.Location = new System.Drawing.Point(13, 98);
            this.NewShiftLabel.Name = "NewShiftLabel";
            this.NewShiftLabel.Size = new System.Drawing.Size(56, 13);
            this.NewShiftLabel.TabIndex = 12;
            this.NewShiftLabel.Text = "New Shift:";
            // 
            // textBoxGageHeight
            // 
            this.textBoxGageHeight.Location = new System.Drawing.Point(258, 28);
            this.textBoxGageHeight.Name = "textBoxGageHeight";
            this.textBoxGageHeight.Size = new System.Drawing.Size(100, 20);
            this.textBoxGageHeight.TabIndex = 5;
            // 
            // DischargeLabel
            // 
            this.DischargeLabel.AutoSize = true;
            this.DischargeLabel.Location = new System.Drawing.Point(367, 32);
            this.DischargeLabel.Name = "DischargeLabel";
            this.DischargeLabel.Size = new System.Drawing.Size(58, 13);
            this.DischargeLabel.TabIndex = 10;
            this.DischargeLabel.Text = "Discharge:";
            // 
            // GageLabel
            // 
            this.GageLabel.AutoSize = true;
            this.GageLabel.Location = new System.Drawing.Point(214, 31);
            this.GageLabel.Name = "GageLabel";
            this.GageLabel.Size = new System.Drawing.Size(38, 13);
            this.GageLabel.TabIndex = 9;
            this.GageLabel.Text = "Stage:";
            // 
            // textBoxDischarge
            // 
            this.textBoxDischarge.Location = new System.Drawing.Point(431, 28);
            this.textBoxDischarge.Name = "textBoxDischarge";
            this.textBoxDischarge.Size = new System.Drawing.Size(100, 20);
            this.textBoxDischarge.TabIndex = 6;
            // 
            // CommentsLabel
            // 
            this.CommentsLabel.AutoSize = true;
            this.CommentsLabel.Location = new System.Drawing.Point(11, 61);
            this.CommentsLabel.Name = "CommentsLabel";
            this.CommentsLabel.Size = new System.Drawing.Size(59, 13);
            this.CommentsLabel.TabIndex = 14;
            this.CommentsLabel.Text = "Comments:";
            // 
            // textBoxComments
            // 
            this.textBoxComments.Location = new System.Drawing.Point(6, 77);
            this.textBoxComments.Multiline = true;
            this.textBoxComments.Name = "textBoxComments";
            this.textBoxComments.Size = new System.Drawing.Size(559, 92);
            this.textBoxComments.TabIndex = 7;
            // 
            // buttonSaveShift
            // 
            this.buttonSaveShift.Enabled = false;
            this.buttonSaveShift.Location = new System.Drawing.Point(75, 133);
            this.buttonSaveShift.Name = "buttonSaveShift";
            this.buttonSaveShift.Size = new System.Drawing.Size(75, 23);
            this.buttonSaveShift.TabIndex = 8;
            this.buttonSaveShift.Text = "Save Shift";
            this.buttonSaveShift.UseVisualStyleBackColor = true;
            this.buttonSaveShift.Click += new System.EventHandler(this.SaveShift_Click);
            // 
            // DataView
            // 
            this.DataView.AllowUserToAddRows = false;
            this.DataView.AllowUserToDeleteRows = false;
            this.DataView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DataView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DataView.Location = new System.Drawing.Point(3, 236);
            this.DataView.Name = "DataView";
            this.DataView.ReadOnly = true;
            this.DataView.Size = new System.Drawing.Size(843, 364);
            this.DataView.TabIndex = 17;
            // 
            // PCODELabel
            // 
            this.PCODELabel.AutoSize = true;
            this.PCODELabel.Location = new System.Drawing.Point(13, 74);
            this.PCODELabel.Name = "PCODELabel";
            this.PCODELabel.Size = new System.Drawing.Size(47, 13);
            this.PCODELabel.TabIndex = 18;
            this.PCODELabel.Text = "PCODE:";
            // 
            // textBoxPcode
            // 
            this.textBoxPcode.Location = new System.Drawing.Point(75, 71);
            this.textBoxPcode.Name = "textBoxPcode";
            this.textBoxPcode.Size = new System.Drawing.Size(100, 20);
            this.textBoxPcode.TabIndex = 2;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.GageLabel);
            this.groupBox1.Controls.Add(this.DateMeasuredLabel);
            this.groupBox1.Controls.Add(this.DateMeasured);
            this.groupBox1.Controls.Add(this.textBoxDischarge);
            this.groupBox1.Controls.Add(this.DischargeLabel);
            this.groupBox1.Controls.Add(this.textBoxComments);
            this.groupBox1.Controls.Add(this.CommentsLabel);
            this.groupBox1.Controls.Add(this.textBoxGageHeight);
            this.groupBox1.Location = new System.Drawing.Point(181, 40);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(587, 175);
            this.groupBox1.TabIndex = 19;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "optional";
            // 
            // linkLabelDailyQuery
            // 
            this.linkLabelDailyQuery.AutoSize = true;
            this.linkLabelDailyQuery.Location = new System.Drawing.Point(22, 159);
            this.linkLabelDailyQuery.Name = "linkLabelDailyQuery";
            this.linkLabelDailyQuery.Size = new System.Drawing.Size(92, 13);
            this.linkLabelDailyQuery.TabIndex = 21;
            this.linkLabelDailyQuery.TabStop = true;
            this.linkLabelDailyQuery.Text = "show recent shifts";
            this.linkLabelDailyQuery.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelDailyQuery_LinkClicked);
            // 
            // linkLabelShowAll
            // 
            this.linkLabelShowAll.AutoSize = true;
            this.linkLabelShowAll.Location = new System.Drawing.Point(22, 180);
            this.linkLabelShowAll.Name = "linkLabelShowAll";
            this.linkLabelShowAll.Size = new System.Drawing.Size(72, 13);
            this.linkLabelShowAll.TabIndex = 20;
            this.linkLabelShowAll.TabStop = true;
            this.linkLabelShowAll.Text = "show all shifts";
            this.linkLabelShowAll.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelShowAll_LinkClicked);
            // 
            // linkLabelShowInExcel
            // 
            this.linkLabelShowInExcel.AutoSize = true;
            this.linkLabelShowInExcel.Location = new System.Drawing.Point(22, 201);
            this.linkLabelShowInExcel.Name = "linkLabelShowInExcel";
            this.linkLabelShowInExcel.Size = new System.Drawing.Size(70, 13);
            this.linkLabelShowInExcel.TabIndex = 22;
            this.linkLabelShowInExcel.TabStop = true;
            this.linkLabelShowInExcel.Text = "open in excel";
            this.linkLabelShowInExcel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelShowInExcel_LinkClicked);
            // 
            // ShiftInput
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.linkLabelShowInExcel);
            this.Controls.Add(this.linkLabelShowAll);
            this.Controls.Add(this.linkLabelDailyQuery);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.textBoxPcode);
            this.Controls.Add(this.PCODELabel);
            this.Controls.Add(this.DataView);
            this.Controls.Add(this.buttonSaveShift);
            this.Controls.Add(this.textBoxNewShift);
            this.Controls.Add(this.NewShiftLabel);
            this.Controls.Add(this.HeaderLabel);
            this.Controls.Add(this.CBTTLabel);
            this.Controls.Add(this.textBoxCbtt);
            this.Name = "ShiftInput";
            this.Size = new System.Drawing.Size(904, 629);
            ((System.ComponentModel.ISupportInitialize)(this.DataView)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}

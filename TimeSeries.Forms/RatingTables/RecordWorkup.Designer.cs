namespace Reclamation.TimeSeries.Forms.RatingTables
{
    partial class RecordWorkup
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RecordWorkup));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.xyZedGraph1 = new Reclamation.TimeSeries.Forms.Graphing.XYZedGraph();
            this.shiftFileEditor1 = new Reclamation.TimeSeries.Forms.RatingTables.ShiftFileEditor();
            this.recordSelection1 = new Reclamation.TimeSeries.Forms.RatingTables.RecordSelection();
            this.recordNotes1 = new Reclamation.TimeSeries.Forms.RatingTables.RecordNotes();
            this.graphExplorerView1 = new Reclamation.TimeSeries.Graphing.GraphExplorerView();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 8.676755F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 34.47097F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35.16039F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 21.69189F));
            this.tableLayoutPanel1.Controls.Add(this.xyZedGraph1, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.shiftFileEditor1, 3, 2);
            this.tableLayoutPanel1.Controls.Add(this.recordSelection1, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.recordNotes1, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.graphExplorerView1, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1137, 645);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // xyZedGraph1
            // 
            this.xyZedGraph1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xyZedGraph1.Location = new System.Drawing.Point(891, 218);
            this.xyZedGraph1.Name = "xyZedGraph1";
            this.xyZedGraph1.Size = new System.Drawing.Size(243, 209);
            this.xyZedGraph1.TabIndex = 1;
            // 
            // shiftFileEditor1
            // 
            this.shiftFileEditor1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.shiftFileEditor1.Location = new System.Drawing.Point(891, 433);
            this.shiftFileEditor1.Name = "shiftFileEditor1";
            this.shiftFileEditor1.Size = new System.Drawing.Size(243, 209);
            this.shiftFileEditor1.TabIndex = 2;
            // 
            // recordSelection1
            // 
            this.recordSelection1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.recordSelection1.Location = new System.Drawing.Point(891, 3);
            this.recordSelection1.Name = "recordSelection1";
            this.recordSelection1.Size = new System.Drawing.Size(243, 209);
            this.recordSelection1.TabIndex = 3;
            // 
            // recordNotes1
            // 
            this.recordNotes1.Location = new System.Drawing.Point(101, 433);
            this.recordNotes1.Name = "recordNotes1";
            this.recordNotes1.Size = new System.Drawing.Size(378, 209);
            this.recordNotes1.TabIndex = 4;
            // 
            // graphExplorerView1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.graphExplorerView1, 2);
            this.graphExplorerView1.DataTable = null;
            this.graphExplorerView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.graphExplorerView1.Location = new System.Drawing.Point(101, 3);
            this.graphExplorerView1.Messages = ((System.Collections.Generic.List<string>)(resources.GetObject("graphExplorerView1.Messages")));
            this.graphExplorerView1.Name = "graphExplorerView1";
            this.tableLayoutPanel1.SetRowSpan(this.graphExplorerView1, 2);
            this.graphExplorerView1.Size = new System.Drawing.Size(784, 424);
            this.graphExplorerView1.TabIndex = 5;
            this.graphExplorerView1.UndoZoom = false;
            // 
            // RecordWorkup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "RecordWorkup";
            this.Size = new System.Drawing.Size(1137, 645);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private Graphing.XYZedGraph xyZedGraph1;
        private ShiftFileEditor shiftFileEditor1;
        private RecordSelection recordSelection1;
        private RecordNotes recordNotes1;
        private TimeSeries.Graphing.GraphExplorerView graphExplorerView1;
    }
}

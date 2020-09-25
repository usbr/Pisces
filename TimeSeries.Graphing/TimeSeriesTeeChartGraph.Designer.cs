 
namespace Reclamation.TimeSeries.Graphing
{

    partial class TimeSeriesTeeChartGraph
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TimeSeriesTeeChartGraph));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonPrin = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonEditGraph = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripComboBoxZoomType = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripButtonZoomOut = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonZoomIn = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonUndoZoom = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripComboBoxDragPoints = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripButtonSelect = new System.Windows.Forms.ToolStripButton();
            this.tChart1 = new Steema.TeeChart.TChart();
            this.line1 = new Steema.TeeChart.Styles.Line();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonPrin,
            this.toolStripSeparator1,
            this.toolStripButtonEditGraph,
            this.toolStripSeparator2,
            this.toolStripLabel1,
            this.toolStripComboBoxZoomType,
            this.toolStripButtonZoomOut,
            this.toolStripButtonZoomIn,
            this.toolStripButtonUndoZoom,
            this.toolStripSeparator3,
            this.toolStripLabel2,
            this.toolStripComboBoxDragPoints,
            this.toolStripButtonSelect});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(865, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButtonPrin
            // 
            this.toolStripButtonPrin.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonPrin.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Underline);
            this.toolStripButtonPrin.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonPrin.Image")));
            this.toolStripButtonPrin.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonPrin.Name = "toolStripButtonPrin";
            this.toolStripButtonPrin.Size = new System.Drawing.Size(36, 22);
            this.toolStripButtonPrin.Text = "Print";
            this.toolStripButtonPrin.ToolTipText = "print";
            this.toolStripButtonPrin.Click += new System.EventHandler(this.toolStripButtonPrin_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButtonEditGraph
            // 
            this.toolStripButtonEditGraph.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonEditGraph.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Underline);
            this.toolStripButtonEditGraph.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonEditGraph.Name = "toolStripButtonEditGraph";
            this.toolStripButtonEditGraph.Size = new System.Drawing.Size(66, 22);
            this.toolStripButtonEditGraph.Text = "Edit Graph";
            this.toolStripButtonEditGraph.Click += new System.EventHandler(this.toolStripButtonEditGraph_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(45, 22);
            this.toolStripLabel1.Text = "Zoom: ";
            // 
            // toolStripComboBoxZoomType
            // 
            this.toolStripComboBoxZoomType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.toolStripComboBoxZoomType.Items.AddRange(new object[] {
            "Zoom Both",
            "Zoom Horizontal",
            "Zoom Vertical"});
            this.toolStripComboBoxZoomType.Name = "toolStripComboBoxZoomType";
            this.toolStripComboBoxZoomType.Size = new System.Drawing.Size(121, 25);
            this.toolStripComboBoxZoomType.SelectedIndexChanged += new System.EventHandler(this.toolStripComboBoxZoomType_SelectedIndexChanged);
            // 
            // toolStripButtonZoomOut
            // 
            this.toolStripButtonZoomOut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonZoomOut.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonZoomOut.Image")));
            this.toolStripButtonZoomOut.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonZoomOut.Name = "toolStripButtonZoomOut";
            this.toolStripButtonZoomOut.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonZoomOut.Text = "-";
            this.toolStripButtonZoomOut.ToolTipText = "zoom out";
            this.toolStripButtonZoomOut.Click += new System.EventHandler(this.toolStripButtonZoomOut_Click);
            // 
            // toolStripButtonZoomIn
            // 
            this.toolStripButtonZoomIn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonZoomIn.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonZoomIn.Image")));
            this.toolStripButtonZoomIn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonZoomIn.Name = "toolStripButtonZoomIn";
            this.toolStripButtonZoomIn.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonZoomIn.Text = "+";
            this.toolStripButtonZoomIn.ToolTipText = "zoom in";
            this.toolStripButtonZoomIn.Click += new System.EventHandler(this.toolStripButtonZoomIn_Click);
            // 
            // toolStripButtonUndoZoom
            // 
            this.toolStripButtonUndoZoom.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonUndoZoom.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Underline);
            this.toolStripButtonUndoZoom.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonUndoZoom.Image")));
            this.toolStripButtonUndoZoom.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonUndoZoom.Name = "toolStripButtonUndoZoom";
            this.toolStripButtonUndoZoom.Size = new System.Drawing.Size(39, 22);
            this.toolStripButtonUndoZoom.Text = "Reset";
            this.toolStripButtonUndoZoom.Click += new System.EventHandler(this.toolStripButtonUndoZoom_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(74, 22);
            this.toolStripLabel2.Text = "Drag Points: ";
            // 
            // toolStripComboBoxDragPoints
            // 
            this.toolStripComboBoxDragPoints.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.toolStripComboBoxDragPoints.Name = "toolStripComboBoxDragPoints";
            this.toolStripComboBoxDragPoints.Size = new System.Drawing.Size(121, 25);
            this.toolStripComboBoxDragPoints.ToolTipText = "drag points for selected series";
            this.toolStripComboBoxDragPoints.SelectedIndexChanged += new System.EventHandler(this.toolStripComboBoxDragPoints_SelectedIndexChanged);
            // 
            // toolStripButtonSelect
            // 
            this.toolStripButtonSelect.CheckOnClick = true;
            this.toolStripButtonSelect.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonSelect.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Underline);
            this.toolStripButtonSelect.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonSelect.Image")));
            this.toolStripButtonSelect.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSelect.Name = "toolStripButtonSelect";
            this.toolStripButtonSelect.Size = new System.Drawing.Size(42, 22);
            this.toolStripButtonSelect.Text = "Select";
            this.toolStripButtonSelect.Click += new System.EventHandler(this.toolStripButtonSelect_Click);
            // 
            // tChart1
            // 
            // 
            // 
            // 
            this.tChart1.Aspect.ColorPaletteIndex = 20;
            this.tChart1.Aspect.View3D = false;
            // 
            // 
            // 
            this.tChart1.Axes.Automatic = true;
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
            this.tChart1.Axes.Bottom.Grid.Visible = false;
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChart1.Axes.Bottom.Labels.Font.Brush.Color = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            // 
            // 
            // 
            this.tChart1.Axes.Bottom.Ticks.Transparency = 50;
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChart1.Axes.Bottom.Title.Font.Brush.Color = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChart1.Axes.Left.AxisPen.Visible = false;
            this.tChart1.Axes.Left.AxisPen.Width = 1;
            this.tChart1.Axes.Left.EndPosition = 99D;
            // 
            // 
            // 
            this.tChart1.Axes.Left.Grid.Style = System.Drawing.Drawing2D.DashStyle.Solid;
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChart1.Axes.Left.Labels.Font.Brush.Color = System.Drawing.Color.Gray;
            // 
            // 
            // 
            this.tChart1.Axes.Left.MinorTicks.Visible = false;
            this.tChart1.Axes.Left.StartPosition = 1D;
            // 
            // 
            // 
            this.tChart1.Axes.Left.Ticks.Transparency = 70;
            this.tChart1.Axes.Left.Ticks.Visible = false;
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChart1.Axes.Left.Title.Font.Brush.Color = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChart1.Axes.Right.AxisPen.Visible = false;
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChart1.Axes.Right.Labels.Font.Brush.Color = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tChart1.Axes.Right.Visible = false;
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChart1.Axes.Top.Labels.Font.Brush.Color = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tChart1.Axes.Top.Visible = false;
            this.tChart1.CurrentTheme = Steema.TeeChart.ThemeType.Report;
            this.tChart1.Cursor = System.Windows.Forms.Cursors.Default;
            this.tChart1.Dock = System.Windows.Forms.DockStyle.Fill;
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChart1.Header.Font.Brush.Color = System.Drawing.Color.Gray;
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
            this.tChart1.Legend.Font.Brush.Color = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tChart1.Legend.LegendStyle = Steema.TeeChart.LegendStyles.Series;
            // 
            // 
            // 
            this.tChart1.Legend.Pen.Visible = false;
            // 
            // 
            // 
            this.tChart1.Legend.Shadow.Visible = false;
            this.tChart1.Legend.Transparent = true;
            this.tChart1.Location = new System.Drawing.Point(0, 25);
            this.tChart1.Name = "tChart1";
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChart1.Panel.Bevel.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.tChart1.Panel.Bevel.StringColorTwo = "FFE0E0E0";
            // 
            // 
            // 
            this.tChart1.Panel.Brush.Color = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            // 
            // 
            // 
            this.tChart1.Panel.Brush.Gradient.Visible = false;
            // 
            // 
            // 
            this.tChart1.Panel.ImageBevel.Width = 1;
            this.tChart1.Series.Add(this.line1);
            this.tChart1.Size = new System.Drawing.Size(865, 569);
            this.tChart1.TabIndex = 2;
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChart1.Walls.Back.Brush.Visible = false;
            this.tChart1.Walls.Back.Transparent = true;
            this.tChart1.Walls.Back.Visible = false;
            this.tChart1.Walls.Visible = false;
            this.tChart1.ClickLegend += new System.Windows.Forms.MouseEventHandler(this.tChart1_ClickLegend);
            this.tChart1.ClickSeries += new Steema.TeeChart.SeriesEventHandler(this.tChart1_ClickSeries);
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
            this.line1.Marks.Brush.Color = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            // 
            // 
            // 
            this.line1.Marks.Brush.Gradient.EndColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.line1.Marks.Brush.Gradient.MiddleColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.line1.Marks.Brush.Gradient.StartColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(215)))), ((int)(((byte)(0)))));
            this.line1.Marks.Brush.Gradient.UseMiddle = true;
            // 
            // 
            // 
            // 
            // 
            // 
            this.line1.Marks.Font.Brush.Color = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            // 
            // 
            // 
            this.line1.Marks.Pen.Color = System.Drawing.Color.FromArgb(((int)(((byte)(61)))), ((int)(((byte)(92)))), ((int)(((byte)(147)))));
            // 
            // 
            // 
            this.line1.Marks.Shadow.Visible = false;
            // 
            // 
            // 
            this.line1.Marks.TailParams.CustomPointPos = ((System.Drawing.PointF)(resources.GetObject("resource.CustomPointPos")));
            this.line1.Marks.TailParams.Margin = 0F;
            this.line1.Marks.TailParams.PointerHeight = 8D;
            this.line1.Marks.TailParams.PointerWidth = 8D;
            this.line1.Marks.TailStyle = Steema.TeeChart.Styles.MarksTail.WithPointer;
            this.line1.OriginalCursor = null;
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
            this.line1.Pointer.SizeDouble = 0D;
            this.line1.Pointer.SizeUnits = Steema.TeeChart.Styles.PointerSizeUnits.Pixels;
            this.line1.Title = "line1";
            this.line1.UseExtendedNumRange = false;
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
            // TimeSeriesTeeChartGraph
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tChart1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "TimeSeriesTeeChartGraph";
            this.Size = new System.Drawing.Size(865, 594);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        //private Steema.TeeChart.Editor editor1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButtonEditGraph;
        public Steema.TeeChart.TChart tChart1;
        private System.Windows.Forms.ToolStripButton toolStripButtonUndoZoom;
        private System.Windows.Forms.ToolStripButton toolStripButtonZoomOut;
        private System.Windows.Forms.ToolStripButton toolStripButtonZoomIn;
        private System.Windows.Forms.ToolStripButton toolStripButtonPrin;
        private Steema.TeeChart.Styles.Line line1;
        private System.Windows.Forms.ToolStripComboBox toolStripComboBoxDragPoints;
        private System.Windows.Forms.ToolStripButton toolStripButtonSelect;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripComboBox toolStripComboBoxZoomType;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
    }

}
 

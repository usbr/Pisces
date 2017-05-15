namespace HydrometTools.AgriMet
{
    partial class AgriMetTab
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
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPageCropDates = new System.Windows.Forms.TabPage();
            this.cropDatesDataSet1 = new Reclamation.TimeSeries.AgriMet.CropDatesDataSet();
            this.tabControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cropDatesDataSet1)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tabPageCropDates);
            this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl2.Location = new System.Drawing.Point(0, 0);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(617, 447);
            this.tabControl2.TabIndex = 1;
            // 
            // tabPageCropDates
            // 
            this.tabPageCropDates.Location = new System.Drawing.Point(4, 22);
            this.tabPageCropDates.Name = "tabPageCropDates";
            this.tabPageCropDates.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageCropDates.Size = new System.Drawing.Size(609, 421);
            this.tabPageCropDates.TabIndex = 0;
            this.tabPageCropDates.Text = "Crop Dates";
            this.tabPageCropDates.UseVisualStyleBackColor = true;
            // 
            // cropDatesDataSet1
            // 
            this.cropDatesDataSet1.DataSetName = "CropDatesDataSet";
            this.cropDatesDataSet1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // AgriMetTab
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl2);
            this.Name = "AgriMetTab";
            this.Size = new System.Drawing.Size(617, 447);
            this.tabControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cropDatesDataSet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPageCropDates;
        private Reclamation.TimeSeries.AgriMet.CropDatesDataSet cropDatesDataSet1;
    }
}

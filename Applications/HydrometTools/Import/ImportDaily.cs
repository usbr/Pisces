using System;
using System.Windows.Forms;
using Reclamation.TimeSeries.Usgs;
using Reclamation.TimeSeries;
using Reclamation.TimeSeries.Hydromet;
using Reclamation.Core;
using System.IO;
using Reclamation.TimeSeries.Owrd;
using System.Configuration;
using Reclamation.TimeSeries.Nrcs;
using System.Web.Security;
using Reclamation.TimeSeries.Graphing;
using Reclamation.TimeSeries.IDWR;

namespace HydrometTools
{

    /// <summary>
    /// User interface to import data from USGS, OWRD, etc..
    /// </summary>
    public partial class ImportDaily : UserControl
    {
        enum ExternalSource { USGS, OWRD, Excel, IDACORP,nrcs, Idwr };
        CsvFile csv;
        int rowIndex = 0;
        Series externalSeries;
        HydrometDailySeries hmet;

        public ImportDaily()
        {
            InitializeComponent();
            HydrometInfoUtility.WebOnly = true;

            Reclamation.TimeSeries.Point.MissingValueFlag = 998877;

            this.timeSelectorBeginEnd1.T1 = new DateTime(2011, 10, 1);
            this.timeSelectorBeginEnd1.T2 = DateTime.Now.Date;

            teeChartExplorerView1 = new Reclamation.TimeSeries.Graphing.GraphExplorerView(new TimeSeriesTeeChartGraph());
            this.teeChartExplorerView1.Parent = this.panelChart;
            this.teeChartExplorerView1.Dock = System.Windows.Forms.DockStyle.Fill;

            var fn = FileUtility.GetFileReference("data_import_sites.csv");
            if(File.Exists(fn))
            {
                OpenFile(fn);
            }
        }

       

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                labelStatus.Text = "Reading";
                
                Cursor = Cursors.WaitCursor;
                Sync();
                Application.DoEvents();
                DateTime t1 = this.timeSelectorBeginEnd1.T1;
                DateTime t2 = this.timeSelectorBeginEnd1.T2;

                ReadExternalData( t1, t2);

                labelStatus.Text = "found " + externalSeries.Count + " records in "+GetSourceType().ToString();
                Application.DoEvents();


                HydrometHost svr = HydrometInfoUtility.HydrometServerFromPreferences();

                hmet = new HydrometDailySeries(textBoxcbtt.Text.Trim(),
                    textBoxPcode.Text.Trim(), svr);
                hmet.Read(t1, t2);


                int hmetCount = hmet.Count - hmet.CountMissing();
                labelStatus.Text += " and " + hmetCount + " records in hydromet";
                Application.DoEvents();

                SeriesList list = new SeriesList();
                list.Add(externalSeries);
                list.Add(hmet);

                this.teeChartExplorerView1.SeriesList = list;
                //this.timeSeriesGraph1.Series = list;
                this.teeChartExplorerView1.Draw();
                //this.timeSeriesGraph1.Draw(true);


                Series diff = hmet - externalSeries;

                SeriesList list2 = new SeriesList();
                list2.Add(diff);
                timeSeriesGraph2.Series = list2;

                timeSeriesGraph2.Draw(true);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message+" \n"+ex.StackTrace);
                

            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }


        ExternalSource GetSourceType()
        {
            if (textBoxUsgs.Text.Trim().Length > 0)
                return ExternalSource.USGS;

            if (textBoxOwrd.Text.Trim().Length > 0)
                return ExternalSource.OWRD;

            //if (textBoxExcel.Text.Trim().Length > 0)
            //    return ExternalSource.Excel;

            if (textBoxIdaCorp.Text.Trim().Length > 0)
                return ExternalSource.IDACORP;

            if (textBoxSnotel.Text.Trim().Length > 0)
                return ExternalSource.nrcs;
            if (textBoxIdwr.Text.Trim().Length > 0)
                return ExternalSource.Idwr;


            throw new NotImplementedException("Invalid type of Data Source.  Missing data?");


        }

        private void ReadExternalData(DateTime t1, DateTime t2)
        {
            var pc = textBoxPcode.Text.Trim().ToLower();
            if (GetSourceType() == ExternalSource.USGS)
            {
                UsgsDailyParameter usgsParm = FindUsgsParameter(pc);

                     externalSeries = new UsgsDailyValueSeries(this.textBoxUsgs.Text.Trim(), usgsParm);
            }
            else
                if (GetSourceType() == ExternalSource.OWRD)
                {
                    if (pc == "qd" || pc == "qj")
                        externalSeries = new OwrdSeries(this.textBoxOwrd.Text.Trim(), OwrdSeries.OwrdDataSet.MDF,this.checkBoxProvisional.Checked);
                    else
                        if (pc == "af")
                            externalSeries = new OwrdSeries(this.textBoxOwrd.Text.Trim(), OwrdSeries.OwrdDataSet.Midnight_Volume, this.checkBoxProvisional.Checked);
                        else
                            externalSeries = new Series();
                }
                else
                        if (GetSourceType() == ExternalSource.IDACORP)
                        {
                            externalSeries = GetIdahoPowerSeries();
                            externalSeries.Units = "cfs";
                        }
                        else if (GetSourceType() == ExternalSource.nrcs)
                        {
                            var snotelSiteID = NrcsSnotelSeries.LookupSiteID(textBoxSnotel.Text);
                            string snotelParameter = NrcsSnotelSeries.SnotelParameterFromHydrometPcode(pc);
                            externalSeries = new NrcsSnotelSeries(snotelSiteID, snotelParameter);
                        }
                        else if (GetSourceType() == ExternalSource.Idwr)
                        {
                            externalSeries = new IDWRDailySeries(textBoxIdwr.Text.Trim());

                        }

            externalSeries.Read(t1, t2);
            // convert units if needed

            if( externalSeries.Units == "degrees Celsius")
                externalSeries.Units = "degrees C" ;

            if (externalSeries.Units  =="degrees C" )
            {
                Reclamation.TimeSeries.Math.ConvertUnits(externalSeries, "degrees F");
            }


            externalSeries.Appearance.LegendText =  GetSourceType().ToString() + " " + externalSeries.Name;


        }

        private static UsgsDailyParameter FindUsgsParameter(string pc)
        {
            UsgsDailyParameter rval = UsgsDailyParameter.DailyMeanDischarge;

            if (pc == "qd")
                rval = UsgsDailyParameter.DailyMeanDischarge;

            if (pc == "wi")
                rval = UsgsDailyParameter.DailyMinTemperature;
            if (pc == "wk")
                rval = UsgsDailyParameter.DailyMaxTemperature;
            if (pc == "wz")
                rval = UsgsDailyParameter.DailyMeanTemperature;


            return rval;
        }


       
        private Series GetIdahoPowerSeries()
        {
            var tokens = this.textBoxIdaCorp.Text.Split('/');
            if (tokens.Length != 2)
            {
                MessageBox.Show("Error:  site id should look  like this '13245000/Q'");
                return new Series();
            }
            string id = tokens[0];
            string ipcoType = tokens[1];
            var s = new Reclamation.TimeSeries.IdahoPower.IdahoPowerSeries(id,ipcoType, TimeInterval.Daily);
            return s;
            
        }

        public void Sync()
        {
            teeChartExplorerView1.Clear();

            this.textBoxcbtt.Text = csv.Rows[rowIndex]["cbtt"].ToString();
            this.textBoxPcode.Text = csv.Rows[rowIndex]["pcode"].ToString();
            this.textBoxUsgs.Text = csv.Rows[rowIndex]["usgs"].ToString();
            this.textBoxOwrd.Text = csv.Rows[rowIndex]["owrd"].ToString();
            this.textBoxIdwr.Text = csv.Rows[rowIndex]["idwr"].ToString();
            this.textBoxNotes.Text = csv.Rows[rowIndex]["notes"].ToString();
            this.textBoxIdaCorp.Text = csv.Rows[rowIndex]["IdahoPower"].ToString();
            this.textBoxSnotel.Text = csv.Rows[rowIndex]["nrcs"].ToString();
            this.textBoxLastUpdate.Text = csv.Rows[rowIndex]["Updated"].ToString();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //OpenFile("cata);
        }

        private void OpenFile(string filename)
        {
            csv = new CsvFile(filename, CsvFile.FieldTypes.AllText);

            if (csv.Columns.IndexOf("Updated") < 0)
            {
                csv.Columns.Add("Updated", typeof(string));
            }

            this.comboBoxSite.DataSource = csv;
            this.comboBoxSite.ValueMember = "Name";
            this.comboBoxSite.DisplayMember = "Name";
            this.comboBoxSite.SelectedIndex = 0;
            Sync();
        }

        private void comboBoxSite_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxSite.SelectedIndex != rowIndex)
            {
                rowIndex = comboBoxSite.SelectedIndex;
                Sync();
            }
        }
        private void buttonNext_Click(object sender, EventArgs e)
        {
            rowIndex++;
            if (rowIndex >= csv.Rows.Count)
            {
                rowIndex = 0;
               // MessageBox.Show("back to the beginning");
            }
            Sync();
            comboBoxSite.SelectedIndex = rowIndex;
            if (checkBoxAutoRefresh.Checked)
                buttonRefresh_Click(this, EventArgs.Empty);
        }

        private void buttonPrevious_Click(object sender, EventArgs e)
        {
            rowIndex--;
            if (rowIndex <0)
            {
                rowIndex = 0;
            }
            Sync();
            comboBoxSite.SelectedIndex = rowIndex;
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (externalSeries == null || hmet == null)
            {
                MessageBox.Show("can not save. please try loading data");
                return;
            }

            string cbtt = textBoxcbtt.Text.Trim().ToUpper();
            string pcode = textBoxPcode.Text.Trim().ToUpper();



            string fileName = FileUtility.GetTempFileName(".txt"); //"update" + DateTime.Now.ToString("yyyyMMMdd") + ".txt";

            int counter = WriteArchivesImportFile(cbtt, pcode, fileName,GetSourceType());
            labelStatus.Text = "Saved  "+counter+" records to file " + fileName;
            Application.DoEvents();


            if(counter == 0)
                return;

            Login login = new Login();

            bool admin = Login.AdminPasswordIsValid();
            if( !admin)
                MessageBox.Show("You must enter the administrator password in the setup tab for this feature to work");

            HydrometHost svr = HydrometInfoUtility.HydrometServerFromPreferences();

            if ( svr == HydrometHost.GreatPlains &&
                admin && login.ShowDialog() == DialogResult.OK)
            {
                SaveToVMS(fileName, login);
            }
            else if( admin)
            {
                SaveToLinux(fileName);
            }
        }

        private void SaveToVMS(string fileName, Login login)
        {
            try
            {
                var un = System.Security.Principal.WindowsIdentity.GetCurrent().Name.ToString();
                Cursor = Cursors.WaitCursor;
                Application.DoEvents();
                var results = HydrometEditsVMS.SaveDailyData(login.Username, login.Password, fileName, HydrometDataUtility.CreateRemoteFileName(login.Username, TimeInterval.Daily), false, false);
                TimeSeriesEditor.ShowVmsStatus(results);
                textBoxLastUpdate.Text = DateTime.Now.ToShortDateString() + " " + un;
                this.buttonSaveCsv_Click(this, EventArgs.Empty);
            }
            finally
            {
                Cursor = Cursors.Default;
                Logger.WriteLine("done.", "ui");
            }
        }

        private void SaveToLinux(string filename)
        {
            HydrometHost svr = HydrometInfoUtility.HydrometServerFromPreferences();

            if (svr == HydrometHost.PNLinux )
            { // saving to Postgresql/Linux

                if (Database.IsPasswordBlank())
                {
                    MessageBox.Show("Warning: the database password is blank.");
                    return;
                }

                SaveOptions o = new SaveOptions(TimeInterval.Daily);
                if (o.ShowDialog() == DialogResult.OK)
                {
                    Logger.WriteLine("Pisces import: " + filename, "ui");
                    Database.ImportVMSTextFile(filename, o.ComputeDependencies);
                }
            }
        }


        private int WriteArchivesImportFile(string cbtt, string pcode, string fileName , ExternalSource src)
        {
            StreamWriter output = new StreamWriter(fileName, true);

            output.WriteLine("MM/dd/yyyy cbtt         PC        NewValue      OldValue ");

            int counter = 0;
            for (int i = 0; i < externalSeries.Count; i++)
            {
                DateTime t = externalSeries[i].DateTime;
                string flag = externalSeries[i].Flag;

                
                if (ValidData(pcode, src, flag) )
                {
                    if (
                          hmet.IndexOf(t) >= 0 &&
                           System.Math.Abs(hmet[t].Value - externalSeries[i].Value) > 0.01 
                          && System.Math.Abs(externalSeries[i].Value - 998877) > .1
                        )
                    {
                        // save to output file
                        output.WriteLine(t.ToString("MM/dd/yyyy")
                            + " " + cbtt.PadRight(12)
                            + " " + pcode.PadRight(9)
                            + " " + externalSeries[i].Value.ToString("F2").PadRight(13)
                            + " " + hmet[t].Value.ToString("F2").PadRight(13));
                        counter++;
                    }
                }
            }
            output.Close();
            return counter;
        }

        private bool ValidData(string pcode, ExternalSource src, string flag)
        {
            return (src == ExternalSource.USGS && flag.IndexOf("A") == 0)
                                || (src == ExternalSource.USGS && checkBoxProvisional.Checked)
                                || pcode.Trim().ToUpper() == "WI"
                                || pcode.Trim().ToUpper() == "WK"
                                || pcode.Trim().ToUpper() == "WZ"
                                || src == ExternalSource.Idwr && pcode.ToLower() == "qj"
                                || src == ExternalSource.OWRD && flag == "Published"
                                || src == ExternalSource.OWRD &&  checkBoxProvisional.Checked
                                || src == ExternalSource.nrcs
                                || src == ExternalSource.IDACORP;
        }

        private void buttonSaveCsv_Click(object sender, EventArgs e)
        {
            csv.Rows[rowIndex]["cbtt"] = this.textBoxcbtt.Text;
            csv.Rows[rowIndex]["pcode"] = this.textBoxPcode.Text;
            csv.Rows[rowIndex]["usgs"] = this.textBoxUsgs.Text ;
            csv.Rows[rowIndex]["owrd"] = this.textBoxOwrd.Text;
            csv.Rows[rowIndex]["idwr"] = this.textBoxIdwr.Text;
            csv.Rows[rowIndex]["notes"] = this.textBoxNotes.Text;
            csv.Rows[rowIndex]["idahopower"] = this.textBoxIdaCorp.Text;
            csv.Rows[rowIndex]["nrcs"] = this.textBoxSnotel.Text;
            csv.Rows[rowIndex]["Updated"] = this.textBoxLastUpdate.Text;
            
            DataTableOutput.Write(this.csv, labelFileName.Text, false);
        }

       

        private void buttonOpenFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Csv Files|*.csv";
            dlg.FileName = this.labelFileName.Text;

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                OpenFile(dlg.FileName);
                this.labelFileName.Text = dlg.FileName;
            }
        }

       
    }
}

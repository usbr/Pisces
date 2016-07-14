using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using Reclamation.Core;
using Reclamation.TimeSeries;

namespace Rwis.Sync
{
    public partial class rwisForm : Form
    {
        // Connect to DB Server
        private static string dbname = ConfigurationManager.AppSettings["MySqlDatabase"];
        private static string server = ConfigurationManager.AppSettings["MySqlServer"];
        private static string user = ConfigurationManager.AppSettings["MySqlUser"];
        private static BasicDBServer svr = MySqlServer.GetMySqlServer(server, dbname, user);
        private static TimeSeriesDatabase db = new TimeSeriesDatabase(svr);
        private static DataTable siteCat = db.GetSiteCatalog();
        private static DataTable parCat = db.GetParameterCatalog();

        /// <summary>
        /// Main entry form
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            // Starts the application.
            Application.Run(new rwisForm());
        }

        /// <summary>
        /// Generate form
        /// </summary>
        public rwisForm()
        {
            InitializeComponent();            
        }

        /// <summary>
        /// Gets Region option selected for use by other options
        /// </summary>
        /// <returns></returns>
        public string GetRegion()
        {
            if (this.pnRadioButton.Checked)
            { return "PN"; }
            else if (this.gpRadioButton.Checked)
            { return "GP"; }
            else if (this.lcRadioButton.Checked)
            { return "LC"; }
            else if (this.ucRadioButton.Checked)
            { return "UC"; }
            else if (this.mpRadioButton.Checked)
            { return "MP"; }
            else
            { return "PN"; }
        }

        /// <summary>
        /// Controls visibility of Required Regional Connection Information based on Region selection
        /// </summary>
        private void setRegion(object sender, EventArgs e)
        {
            var region = GetRegion();
            if (region == "LC" || region == "UC")
            {
                this.siteCodeTextbox.Enabled = false;
                this.parameterCodeTextBox.Enabled = false;
                this.sdiTextBox.Enabled = true;
            }
            else
            {
                this.siteCodeTextbox.Enabled = true;
                this.parameterCodeTextBox.Enabled = true;
                this.sdiTextBox.Enabled = false;
            }
            GetDataProvider();
        }

        /// <summary>
        ///  Gets Parameter Type options
        /// </summary>
        public void getParTypes(object sender, EventArgs e)
        {
            DataTable distinctPars = parCat.DefaultView.ToTable(true, "name");
            DataRow[] distinctParRows = distinctPars.Select("", "name ASC");
            var outVals = new List<string>(); ;
            foreach (DataRow row in distinctParRows)
            { this.parameterTypeComboBox.Items.Add(row[0].ToString()); }
        }

        /// <summary>
        /// Gets Site Type options
        /// </summary>
        public void getSiteTypes(object sender, EventArgs e)
        {
            this.siteTypeComboBox.Items.Clear();
            DataTable distinctSites = siteCat.DefaultView.ToTable(true, "type");
            var outVals = new List<string>(); ;
            foreach (DataRow row in distinctSites.Rows)
            { this.siteTypeComboBox.Items.Add(row[0].ToString()); }
        }

        /// <summary>
        /// Gets Sites given option selections
        /// </summary>
        private void getSitesByType(object sender, EventArgs e)
        {
            string typeVal;
            try
            { typeVal = this.siteTypeComboBox.SelectedItem.ToString(); }
            catch
            { typeVal = ""; }
            var region = GetRegion();
            this.siteComboBox.Items.Clear();
            if (typeVal != "")
            {
                DataRow[] distinctSites = siteCat.Select("type='" + typeVal + "' AND agency_region='" + region + "'",
                    "description ASC");
                if (distinctSites.Count() < 1)
                {
                    ComboboxItem item = new ComboboxItem();
                    item.Text = "No sites found in " + region + " for SiteType=" + typeVal + "...";
                    item.Value = "";
                    this.siteComboBox.Items.Add(item);
                }
                else
                {
                    foreach (DataRow row in distinctSites)
                    {
                        ComboboxItem item = new ComboboxItem();
                        item.Text = row["description"].ToString();
                        item.Value = row["siteid"].ToString();
                        this.siteComboBox.Items.Add(item);
                    }
                }
            }
            else
            { this.siteComboBox.Items.Add("SELECT A SITE TYPE..."); }
        }

        /// <summary>
        /// Clear contents for labels on option changes
        /// </summary>
        private void clearSiteComboBox(object sender, EventArgs e)
        {
            this.siteComboBox.SelectedIndex = -1;
        }

        /// <summary>
        /// Gets Parameters given option selections
        /// </summary>
        private void getParametersByTypeTimeStep(object sender, EventArgs e)
        {
            string typeVal, tStep;
            try
            {
                typeVal = this.parameterTypeComboBox.SelectedItem.ToString();
                tStep = this.tstepComboBox.SelectedItem.ToString();
            }
            catch
            {
                typeVal = "";
                tStep = "";
            }
            this.parameterComboBox.Items.Clear();
            if (typeVal != "" && tStep != "")
            {
                DataRow[] distinctSites = parCat.Select("name='" + typeVal + "' AND timeinterval='" + tStep + "'",
                    "id ASC");
                foreach (DataRow row in distinctSites)
                {
                    ComboboxItem item = new ComboboxItem();
                    item.Text = row["id"].ToString();
                    item.Value = row["id"].ToString();
                    this.parameterComboBox.Items.Add(item);
                }
            }
            else
            { this.parameterComboBox.Items.Add("SELECT A TIME-STEP AND PARAMETER TYPE..."); }
        }

        /// <summary>
        /// Clear contents for labels on option changes
        /// </summary>
        private void clearParameterComboBox(object sender, EventArgs e)
        {
            this.parameterComboBox.SelectedIndex = -1;
            GetDataProvider();
        }

        /// <summary>
        /// ComboBoxItem class to add text-value pairs to ComboBoxes
        /// http://stackoverflow.com/questions/3063320/combobox-adding-text-and-value-to-an-item-no-binding-source
        /// </summary>
        public class ComboboxItem
        {
            public string Text { get; set; }
            public object Value { get; set; }

            public override string ToString()
            {
                return Text;
            }
        }

        /// <summary>
        /// Sets text for the Site Info labels
        /// </summary>
        private void GetSiteInfo(object sender, EventArgs e)
        {
            string siteCode;
            try
            { siteCode = (this.siteComboBox.SelectedItem as ComboboxItem).Value.ToString(); }
            catch
            { siteCode = ""; }
            if (siteCode != "")
            {
                DataRow[] siteInfo = siteCat.Select("siteid='" + siteCode + "'");
                var info = siteInfo[0];
                string info1 = "", info2 = "";
                info1 = "Site Info: Region: " + info["agency_region"] +
                    ", Site ID: " + info["siteid"] +
                    ", Time Zone: " + info["timezone"];
                info2 += "State: " + info["state"] +
                    ", Lat: " + info["latitude"] +
                    ", Lon: " + info["longitude"] +
                    ", Elev: " + info["elevation"];
                this.siteInfoLabel1.Text = info1;
                this.siteInfoLabel2.Text = info2;
            }
            else
            {
                this.siteInfoLabel1.Text = "Site Info : N/A";
                this.siteInfoLabel2.Text = "N/A";
            }
            GetPiscesName();
        }

        /// <summary>
        /// Sets text for Parameter Info labels
        /// </summary>
        private void GetParameterInfo(object sender, EventArgs e)
        {
            string parCode;
            try
            { parCode = (this.parameterComboBox.SelectedItem as ComboboxItem).Value.ToString(); }
            catch
            { parCode = ""; }
            if (parCode != "")
            {
                DataRow[] siteInfo = parCat.Select("id='" + parCode + "'");
                var info = siteInfo[0];
                string info1 = "", info2 = "";
                info1 = "Parameter Info: Parameter ID: " + info["id"];
                info2 += "Time Interval: " + info["timeinterval"] +
                    ", Physical Units: " + info["units"] +
                    ", Data Statistic: " + info["statistic"];
                this.parameterInfoLabel1.Text = info1;
                this.parameterInfoLabel2.Text = info2;
            }
            else
            {
                this.parameterInfoLabel1.Text = "Parameter Info : N/A";
                this.parameterInfoLabel2.Text = "N/A";
            }
            GetPiscesName();
        }

        /// <summary>
        /// Builds the Data Provider string
        /// </summary>
        private string GetDataProvider()
        {
            var region = GetRegion();
            string intervalCode = intervalCode = this.tstepComboBox.SelectedItem.ToString();
            intervalCode = GetIntervalCode(intervalCode);

            string provider = "";
            switch (region)
            {
                case "PN": case "GP": 
                    provider += "Hydromet" + intervalCode + "Series";
                    break;
                case "LC": case "UC":
                    provider += "HDBSeries";
                    break;
                case "MP":
                    provider += "SFTPSeries";
                    break;
            }
            this.dataProviderLabel.Text = "Data Provider: " + provider;
            return provider;
        }

        /// <summary>
        /// Resolves the table and series name for storage into RWIS
        /// </summary>
        private void GetPiscesName()
        {
            string siteId, parId;
            try
            {
                siteId = (this.siteComboBox.SelectedItem as ComboboxItem).Value.ToString();
                parId = (this.parameterComboBox.SelectedItem as ComboboxItem).Value.ToString();
                this.rwisNameLabel.Text = "RWIS Name: " + siteId + "_" + parId;
            }
            catch
            {
                this.rwisNameLabel.Text  = "RWIS Name: N/A";
            }            
        }

        /// <summary>
        /// Get Connection String for GUI label
        /// </summary>
        private void BuildConnectionString(object sender, EventArgs e)
        {
            var conx = GetConnectionString();
            this.conxnStringLabel.Text = "RWIS Connection: " + conx;
        }

        /// <summary>
        /// Build Connection String
        /// </summary>
        /// <returns></returns>
        private string GetConnectionString()
        {
            string conx = "";
            var region = GetRegion();
            var siteCode = this.siteCodeTextbox.Text.ToString();
            var parCode = this.parameterCodeTextBox.Text.ToString();
            var sdiCode = this.sdiTextBox.Text.ToString();
            var intervalCode = this.tstepComboBox.SelectedItem.ToString();

            intervalCode = GetIntervalCode(intervalCode);

            switch (region)
            {
                case "PN":
                    conx += "server=PN;cbtt=" + siteCode + ";pcode=" + parCode;
                    break;
                case "GP":
                    conx += "server=GreatPlains;cbtt=" + siteCode + ";pcode=" + parCode;
                    break;
                case "LC":
                    conx += "server=LCHDB2;sdi=" + sdiCode + ";timeinterval=" + intervalCode;
                    break;
                case "UC":
                    conx += "server=UCHDB2;sdi=" + sdiCode + ";timeinterval=" + intervalCode;
                    break;
                case "MP":
                    conx += "server=MPSFTP;cbtt=" + siteCode + ";pcode=" + parCode;
                    break;

            }
            return conx;
        }

        /// <summary>
        /// Match RWIS Interval Code in Tables
        /// </summary>
        /// <param name="intervalCode"></param>
        /// <returns></returns>
        private string GetIntervalCode(string intervalCode)
        {
            switch (intervalCode)
            {
                case "Day":
                    intervalCode = "Daily";
                    break;
                case "Month":
                    intervalCode = "Monthly";
                    break;
                default:
                    intervalCode = "Daily";
                    break;
            }
            return intervalCode;
        }

        /// <summary>
        /// Close Form
        /// </summary>
        private void CloseForm(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Opens Form with sitecatalog Data
        /// </summary>
        private void viewSiteCat(object sender, EventArgs e)
        {
            catalogViewer form = new catalogViewer(siteCat);
            form.Show();
        }

        /// <summary>
        /// Opens Form with parametercatalog Data
        /// </summary>
        private void viewParCat(object sender, EventArgs e)
        {
            catalogViewer form = new catalogViewer(parCat);
            form.Show();
        }

        /// <summary>
        /// Add dataset to RWIS DB
        /// </summary>
        private void addDatasetToRWIS(object sender, EventArgs e)
        {
            //////////////////////////////////////////////////////////////////////////
            // Get required variables to add dataset from GUI
            //////////////////////////////////////////////////////////////////////////
            // Get Site Info
            string siteCode;
            try
            { siteCode = (this.siteComboBox.SelectedItem as ComboboxItem).Value.ToString(); }
            catch
            {
                MessageBox.Show("Select a Site...");
                return;
            }
            var site = siteCat.Select("siteid='" + siteCode + "'")[0];
            // Get Parameter Info
            string parCode;
            try
            { parCode = (this.parameterComboBox.SelectedItem as ComboboxItem).Value.ToString(); }
            catch
            {
                MessageBox.Show("Select a Parameter...");
                return;
            }
            var par= parCat.Select("id='" + parCode + "'")[0];
            // Get Connection Information
            var connectionstring = GetConnectionString() + ";LastUpdate=" + DateTime.Now.ToString("MM/dd/yyyy HH:mm");
            var provider = GetDataProvider();
            // Get Parameter information from parametercatalog
            var timeinterval = GetIntervalCode(par["timeinterval"].ToString());
            var parameter = par["id"].ToString();
            var units = par["units"].ToString();
            // Get Site Information from sitecatalog
            var siteid = site["siteid"].ToString();
            // Get Parent ID from seriescatalog
            var region = GetRegion();
            DataTable serCatFolders = db.GetSeriesCatalog("isfolder=1");
            var regionFolderId = serCatFolders.Select("name='" + region + "'")[0]["id"];
            var intervalFolderId = serCatFolders.Select("parentid=" +regionFolderId + " AND name='" + timeinterval + "'")[0]["id"];
            var parentid = serCatFolders.Select("parentid=" + intervalFolderId + " AND name='" + site["type"] + "'")[0]["id"];
            // Get Sort Order from seriescatalog
            DataTable serCatMembers = db.GetSeriesCatalog("parentid=" + parentid);
            int sortOrder;
            Int32.TryParse(serCatMembers.Compute("max(sortorder)", string.Empty).ToString(), out sortOrder);
            sortOrder = System.Math.Max(sortOrder, 1);
            // Set other standard input variables
            string name = (siteid + "_" + parameter).ToLower();
            string tablename = (region + "_" + RemoveSpecialCharacters(name)).ToLower();
            int isFolder = 0;
            int enabled = 1;
            string expression = "";
            string notes = "";
            string iconname = GetIconName();

            //////////////////////////////////////////////////////////////////////////
            // Add data to DB
            //////////////////////////////////////////////////////////////////////////
            // Check for duplicates
            DataTable duplicateCheckTable = db.GetSeriesCatalog("tablename='"+ tablename + "'");
            if (duplicateCheckTable.Rows.Count != 0)
            {
                MessageBox.Show("Dataset for " + site["description"].ToString().ToUpper() + " " + 
                    par["statistic"].ToString().ToUpper() + " " + par["timeinterval"].ToString().ToUpper() + 
                    " " + par["name"].ToString().ToUpper() +" already exists in the RWIS DB. " + 
                    "Select a different Site and Parameter...");
            }
            else
            {
                // Add to series catalog
                showMessage("Adding metadata fields to RWIS DB...");
                string sqlInsertSeriesCatalog = "INSERT INTO seriescatalog (parentid, " + "isfolder," +
                                        "sortorder, " + "iconname, " + "name, " + "siteid, " + "units, " +
                                        "timeinterval, " + "parameter, " + "tablename, " + "provider, " +
                                        "connectionstring, " + "expression, " + "notes, " + "enabled) " +
                             "VALUES (" + parentid + "," + isFolder + ", " + sortOrder + ", " + "'" + 
                                        iconname + "', " + "'" + name + "', " + "'" + siteid + "', " +
                                        "'" + units + "', " + "'" + timeinterval + "', " + "'" + parameter + 
                                        "', " + "'" + tablename + "', " + "'" + provider + "', " + "'" + 
                                        connectionstring + "', " + "'" + expression + "', " + "'" + notes + 
                                        "', " + "" + enabled + "); ";
                svr.RunSqlCommand(sqlInsertSeriesCatalog);
                // Add timeseries table
                showMessage("Adding new table to RWIS DB...");
                string sqlCreateTable = "Create Table " + tablename;
                sqlCreateTable += " (datetime datetime primary key, value float, flag varchar(50)" + " );";
                svr.RunSqlCommand(sqlCreateTable);
                // Add entries to seriesproperties table
                showMessage("Downloading data from regional DB...");
                // Update data
                var s = db.GetSeriesFromTableName(tablename);
                s.Update(this.t1Date.Value, this.t2Date.Value);
                var newSeriesId = db.GetSeriesCatalog("tablename='" + tablename + "'")[0]["id"];
                string sqlInsertSeriesProperties = "INSERT INTO seriesproperties (seriesid, name, value) " +
                    "VALUES (" + newSeriesId + ", 't1', '" + this.t1Date.Value.ToShortDateString() + "'), (" + newSeriesId + ", 't2', '" + this.t2Date.Value.ToShortDateString() + "')";
                svr.RunSqlCommand(sqlInsertSeriesProperties);

                showMessage("Success!");
            }

        }

        private void showMessage(string msg)
        {
            this.toolStripStatusMessage.Text = msg;
            statusStrip1.Refresh();
        }

        /// <summary>
        /// Removes special characters from string
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string RemoveSpecialCharacters(string str)
        {
            return System.Text.RegularExpressions.Regex.Replace(str, "[^a-zA-Z0-9_]+", "", System.Text.RegularExpressions.RegexOptions.Compiled);
        }

        /// <summary>
        /// Sets Icon Name for seriescatalog
        /// </summary>
        private string GetIconName()
        {
            var region = GetRegion();
            string iconName = "";
            switch (region)
            {
                case "PN":case "GP":
                    iconName += "hydromet";
                    break;
                case "LC":case "UC":
                    iconName += "hdb";
                    break;
                case "MP":
                    iconName += "har";
                    break;
            }
            return iconName;
        }

        /// <summary>
        /// Test connection to source DB given connection information
        /// </summary>
        private void testConnection(object sender, EventArgs e)
        {
            var s = new Series();
            s.ConnectionString = GetConnectionString();
            s.Provider = GetDataProvider();
            s.Update(this.t1Date.Value, this.t2Date.Value);
        }


    }
}

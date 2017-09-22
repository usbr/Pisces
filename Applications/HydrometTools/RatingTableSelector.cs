using System;
using System.Data;
using System.Windows.Forms;
using Reclamation.Core;
using System.IO;
using Reclamation.TimeSeries.Hydromet;
using System.Text.RegularExpressions;
using System.Configuration;

namespace HydrometTools
{
    public partial class RatingTableSelector : Form
    {
        public RatingTableSelector()
        {
            InitializeComponent();


            DataTable tbl = new DataTable();
            if (HydrometInfoUtility.HydrometServerFromPreferences() == HydrometHost.GreatPlains)
            {
                string fn = Path.Combine("gp", "ratingtables.csv");
                fn = FileUtility.GetFileReference(fn);

                tbl = new CsvFile(fn);
                var c = tbl.Columns.Add("name");
                c.DefaultValue = "";
            }
            else
            {
                tbl = WebTable();
            }


            dataGridView1.DataSource = tbl;
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
            buttonOk.Enabled = false;
        }

        private DataTable WebTable()
        {
            string rt = ConfigurationManager.AppSettings["RatingTablePath"];
            if (String.IsNullOrEmpty(rt))
            {
                MessageBox.Show("Error: RatingTablePath Not defined in config file");
                rt = "http://lrgs1.pn.usbr.gov/rating_tables/";
            }
            var data = Web.GetPage(rt);

            DataTable rval = new DataTable();
            rval.Columns.Add("Site");
            rval.Columns.Add("Yparm");
            rval.Columns.Add("name");
           rval.Columns.Add("date_modified");
            /* <a href="acao.csv">acao.csv</a>  */
            // "<tr><td valign=\"top\"><img src=\"/icons/text.gif\" alt=\"[TXT]\"></td><td><a href=\"acao_shift.csv\">acao_shift.csv</a>         </td><td align=\"right\">2016-01-15 07:02  </td><td align=\"right\">1.9K</td><td>&nbsp;</td></tr>"
             Regex re = new Regex(
      "<a href=\\\".*\\\">(?<name>.*)\\.(csv)</a>\\s*</td><td a"+
      "lign=\\\"right\\\">(?<date_modified>[\\-0-9\\s\\:]*)",
    RegexOptions.Multiline
    | RegexOptions.CultureInvariant
    | RegexOptions.Compiled
    );

            for (int i = 0; i < data.Length; i++)
            {
                if (re.IsMatch(data[i]))
                {
                    var m = re.Match(data[i]);
                    string n = m.Groups["name"].Value;
                    var tokens = n.Split('_');
                    var cbtt = tokens[0];
                    var pcode = "q";
                    if (tokens.Length == 2)
                        pcode = tokens[1];

                    if (pcode == "shift")
                        continue;

                    var date_modified = m.Groups["date_modified"].Value;

                    rval.Rows.Add(cbtt, pcode,n,date_modified);
                }
            }
            return rval;
        }


        public string cbtt
        {
            get { return GetData("site"); }
        }
        public string pcode
        {
            get { return GetData("Yparm"); }
        }

        public string RatingName
        {
            get { return GetData("name"); }
        }

        public string DateModified
        {
            get { return GetData("date_modified"); }
        }


        private string GetData(string colName)
        {

                if (this.dataGridView1.SelectedRows.Count > 0
                    && dataGridView1.Columns.Contains(colName))
                {
                    DataRowView drv = (DataRowView)dataGridView1.SelectedRows[0].DataBoundItem;
                if( drv != null)
                  return drv.Row[colName].ToString();
                }

            return "";
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            buttonOk.Enabled = true;
        }


        private void textBoxSiteFilter_TextChanged(object sender, EventArgs e)
        {
            try { ((DataTable)dataGridView1.DataSource).DefaultView.RowFilter = "site like '%" + textBoxSiteFilter.Text.Trim() + "%' "; }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}

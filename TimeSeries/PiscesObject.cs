using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Text.RegularExpressions;

namespace Reclamation.TimeSeries
{

    /// <summary>
    /// Base Class for Pisces Objects PiscesObject
    /// </summary>
    public abstract class PiscesObject
    {

        TimeSeriesDatabaseDataSet.SeriesCatalogRow row;
        TimeSeriesDatabaseDataSet.SeriesCatalogDataTable catalog;


        protected PiscesObject()
        {
            catalog = new TimeSeriesDatabaseDataSet.SeriesCatalogDataTable();
            row = catalog.NewSeriesCatalogRow();
            row.id = -1;
            row.enabled = 1;
            // Catalog.Rows.Add(row);
            // Console.WriteLine("PiscesObject()");
        }


        protected PiscesObject(TimeSeriesDatabaseDataSet.SeriesCatalogRow row)
        {
            if (row.RowState == System.Data.DataRowState.Detached)
                catalog = new TimeSeriesDatabaseDataSet.SeriesCatalogDataTable();
            else
                catalog = (TimeSeriesDatabaseDataSet.SeriesCatalogDataTable)row.Table;// 
            this.row = row;
        }

        public TimeSeriesDatabaseDataSet.SeriesCatalogRow SeriesCatalogRow
        {
            get { return row; }
        }


        ///// <summary>
        ///// Name displayed in Pisces Tree.
        ///// </summary>
        public string Name
        {
            set
            {

                row.Name = value;// CleanTextForTreeName(value);
            }
            get
            {
                return row.Name;
            }
        }

        /// <summary>
        /// removes invalid characters for use with calculation series.
        /// </summary>
        private string CleanTextForTreeName(string txt)
        {
            if (string.IsNullOrEmpty(txt))
                return txt;

            if (txt.Length > 0 && char.IsDigit(txt, 0))
                txt = "_" + txt;
            
            // replace all non-alphanumeric characters except underscore with
            // underscore ignoring whitespace
            txt = Regex.Replace(txt, @"[^a-zA-Z0-9_\s]", "_");

            return txt;
        }


        public string SiteID  
        {
            get { return row.siteid; }
            set { row.siteid = value; }
        }


        private PiscesFolder m_parent;

        public PiscesFolder Parent
        {
            get { return m_parent; }
            set
            {
                m_parent = value;
                row.ParentID = value.ID;
            }
        }

        /// <summary>
        ///  parameter identifier 
        /// </summary>
        public string Parameter
        {
            get { return row.Parameter; }
            set { row.Parameter = value; }
        }

        public int SortOrder
        {
            get { return row.SortOrder; }
            set { row.SortOrder = value; }
        }


        public int ID
        {
            get { return row.id; }

            set
            {
                row.id = value;
                if ( catalog.Rows.Count == 0 )
                    catalog.AddSeriesCatalogRow(row);

            }
        }

        public Image Icon
        {
            get { return row.Icon; }
            set { row.Icon = value; }
        }

        /// <summary>
        ///  USGS, Hydromet , Text file, etc...
        ///  Used to define the Icon for display
        /// </summary>
        public string Source
        {
            get { return row.iconname; }
            set { row.iconname = value; }
        }

        public string Provider
        {
            get { return row.Provider; }
            set { row.Provider = value; }
        }
        public string Units
        {
            set { row.Units = value; }
            get
            {
                return row.Units;
            }
        }

        //public int FileIndex
        //{
        //    get { return row.FileIndex; }
        //    set { row.FileIndex = value; }
        //}
        public string ConnectionString
        {
            get { return row.ConnectionString; }
            set { this.row.ConnectionString = value; }
        }

        public string ConnectionStringToken(string name, string defaultValue = "")
        {
            return Reclamation.Core.ConnectionStringUtility.GetToken(ConnectionString, name, defaultValue);
        }

        public string Expression
        {
            get { return row.Expression; }
            set { row.Expression = value; }
        }
        public short Enabled
        {
            get { return row.enabled; }
            set { this.row.enabled = value; }
        }
        //public string Alias
        //{
        //    get { return row.Alias; }
        //    set { row.Alias = value; }
        //}
        public string Notes
        {
            get { return row.Notes; }
            set { this.row.Notes = value; }
        }

        public short IsFolder
        {
            get { return row.IsFolder; }
            set { row.IsFolder = value; }
        }

        public int ParentID
        {
            get { return row.ParentID; }
            set { row.ParentID = value; }
        }
    }
}

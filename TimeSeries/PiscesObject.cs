using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

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

                row.Name = value;
            }
            get
            {
                return row.Name;
            }
        }

        [Obsolete("Use SiteID, also see sitecatalog for site name")]
        public string SiteName   // name/id for site:  example: usgs site number, or hydromet cbtt, or modsim node name
        {
            get { return SiteID; }
            set { SiteID = value; }
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
        ///  Flow, Volume, Temperature, etc.. (descriptive)
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
        public bool Enabled
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

        public bool IsFolder
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

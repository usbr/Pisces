using System;
using System.Collections.Generic;
using Reclamation.Core;
using System.Data;
using System.Linq;

namespace Reclamation.TimeSeries
{
    public partial class TimeSeriesDatabase
    {
       
        /// <summary>
        /// Filter used when generating the tree structure 
        /// with GetChildren() and GetRootObjects
        /// </summary>
        public string Filter { get; set; }
       
        public event EventHandler<ArrayEventArgs> DatabaseChanged;

        protected void OnDatabaseChanged(object[] args)
        {
            if (DatabaseChanged != null)
                DatabaseChanged(this,new ArrayEventArgs(args));
        }

        public void RefreshFolder(PiscesFolder parent)
        {
            if( DatabaseChanged != null)
              OnDatabaseChanged(GetPathArray(parent));
        }


        object[] GetPathArray(PiscesObject node)
        {

           if (node == null)
               return new object[] { };// empty

           if (node.ID == m_root.ID)
           {
               return new object[] {node};
           } 
           else
           {
               Stack<object> stack = new Stack<object>();
               var rootObjectsID = GetRootObjects().Select(x => x.ID).ToList();
               while (!rootObjectsID.Contains(node.ID))
               {
                   stack.Push(node);
                   if (node.Parent == null)
                   {
                       System.Windows.Forms.MessageBox.Show("oops node.Parent == null");
                       Console.WriteLine("oops node.Parent == null");
                       break;
                   }
                   node = node.Parent;
               }
               stack.Push(node); //root object
               return stack.ToArray();
           }
        }

        
        public void ChangeParent(PiscesObject piscesObject, PiscesFolder piscesFolder)
        {
            if (piscesObject.Parent.ID == piscesFolder.ID)
                return; // nothing to do
            TimeSeriesDatabaseDataSet.SeriesCatalogRow r = GetSeriesRow(piscesObject.ID);
            r.ParentID = piscesFolder.ID;
            SaveSeriesRow(r);
            OnDatabaseChanged(GetPathArray(piscesObject.Parent));
            OnDatabaseChanged(GetPathArray(piscesFolder));
        }

        public void ChangeSortOrder(PiscesObject piscesObject, int sortOrder)
        {

          if (piscesObject.SortOrder == sortOrder)
          {
              return;// nothing to do
          }

          string sql = "Update seriescatalog set sortorder = sortorder+1 where parentid = " + piscesObject.Parent.ID  
              + " and  sortorder > " + sortOrder; 
          int i = m_server.RunSqlCommand(sql);

          int newSortOrder = sortOrder + 1;
          sql = "Update seriescatalog set sortorder = " + newSortOrder + " Where id = " + piscesObject.ID;
           i = m_server.RunSqlCommand(sql);
          Logger.WriteLine("SortOrder changed for " + i + " items");

          OnDatabaseChanged(GetPathArray(piscesObject.Parent));
        }

        private PiscesObject m_root;
        public PiscesFolder RootFolder
        {
            get {
                if (m_root == null)
                    m_root = GetRootObjects()[0];
                return m_root as PiscesFolder;
            }
            set
            {
                m_root = value;
            }
        }


        public PiscesObject[] GetChildren(PiscesObject parent)
        {
            var tbl = GetChildrenRows(parent);

            List<PiscesObject> rval = new List<PiscesObject>();
            for (int i = 0; i < tbl.Rows.Count; i++)
            {
                PiscesObject o = this.Factory.CreateObject(tbl[i]);
                o.Parent = parent as PiscesFolder;
                rval.Add(o);
            }
            return rval.ToArray();
        }

        private TimeSeriesDatabaseDataSet.SeriesCatalogDataTable GetChildrenRows(PiscesObject parent)
        {
            if (Filter.Trim() == "")
            {
                string sql = "select * from seriescatalog where parentid = " + parent.ID + " and  id <> " + parent.ID + " order by sortorder";
                var tbl = new TimeSeriesDatabaseDataSet.SeriesCatalogDataTable();
                m_server.FillTable(tbl, sql);
                return tbl;
            }
            else
            {// filtered catalog

                var sc = GetFilteredCatalog();
                var tbl = new TimeSeriesDatabaseDataSet.SeriesCatalogDataTable();
                var rows = sc.Select("parentid=" + parent.ID+ " and id <> parentid","sortorder");

                foreach (var item in rows)
                {
                    var newRow = tbl.NewSeriesCatalogRow();
                    newRow.ItemArray = item.ItemArray;
                    tbl.Rows.Add(newRow);
                }
                tbl.AcceptChanges();
                return tbl;
            }
        }


        public List<PiscesObject> GetRootObjects()
        {
            var tbl = new TimeSeriesDatabaseDataSet.SeriesCatalogDataTable();
            string sql = "select * from seriescatalog where id = parentid ";
            m_server.FillTable(tbl, sql);

            if (tbl.Rows.Count == 0)
            {
                Reclamation.Core.Logger.WriteLine("Tree Requires at least one root Node");
                Logger.WriteLine("AutoCreation of New Root Folder");
                CreateRootFolder();
                m_server.FillTable(tbl, sql);
            }

            List<PiscesObject> rval = new List<PiscesObject>();
            for (int i = 0; i < tbl.Rows.Count; i++)
            {
                rval.Add(Factory.CreateObject(tbl[i]));
            }

            if (rval.Count > 0)    
                m_root = rval[0];
            return rval;
        }


        private string m_prevFilter = "";

        private TimeSeriesDatabaseDataSet.SeriesCatalogDataTable m_SeriesCatalog;


        /// <summary>
        /// Get a filtered view of the SeriesCatalog.  
        /// Include all parent folders with children that match the filter.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        internal TimeSeriesDatabaseDataSet.SeriesCatalogDataTable GetFilteredCatalog( )
        {
            if (m_prevFilter == Filter && m_SeriesCatalog != null)
                return m_SeriesCatalog;

            m_SeriesCatalog = new TimeSeriesDatabaseDataSet.SeriesCatalogDataTable();
            m_server.FillTable(m_SeriesCatalog); // get all rows.

            if( Filter.Trim() == "")
            {
                m_prevFilter = Filter;
                return m_SeriesCatalog;
            }

            var newColumn = new DataColumn("keep", typeof(System.Boolean));
            newColumn.DefaultValue = false;
            m_SeriesCatalog.Columns.Add(newColumn);

            List<int> parentsIncluded = new List<int>();

            var sql = "";
            // Search with an AND filter
            if (Filter.Contains('&'))
            {
                var andFilters = Filter.Split('&');
                for (int i = 0; i < andFilters.Length; i++)
                {
                    if (i > 0)
                        sql += " and ";

                    sql += "name like '%" + BasicDBServer.SafeSqlLikeClauseLiteral(andFilters[i].Trim()) + "%'";
                }
            }
            else
            {
                // Search with an OR filter
                var filters = Filter.Split(',');
                
                for (int i = 0; i < filters.Length; i++)
                {
                    if (i > 0)
                        sql += " or ";

                    sql += "name like '%" + BasicDBServer.SafeSqlLikeClauseLiteral(filters[i].Trim()) + "%'";
                }
            }
            var rows = m_SeriesCatalog.Select(sql);
            foreach (var item in rows)
            {
                item["keep"] = true;
                int parentID = Convert.ToInt32(item["parentid"]);
                if (!parentsIncluded.Contains(parentID))
                {
                    MarkParentsAsPartOfFilter(parentID, m_SeriesCatalog);
                    parentsIncluded.Add(parentID);
                }
            }

            var rowsToDelete = m_SeriesCatalog.Select("keep = false");
            for (int i = 0; i < rowsToDelete.Length; i++)
            {
                m_SeriesCatalog.Rows.Remove(rowsToDelete[i]);
            }
            m_SeriesCatalog.Columns.Remove("keep");

            m_SeriesCatalog.AcceptChanges();

            m_prevFilter = Filter;
            return m_SeriesCatalog;
        }

        private void MarkParentsAsPartOfFilter(int id, TimeSeriesDatabaseDataSet.SeriesCatalogDataTable tbl)
        {
            var row = tbl.FindByid(id);

            if( row == null)
            {
                Logger.WriteLine("Row does not exist.  It may have been deleted.");
                return;
            }
            row["keep"] = true;

            if (row.ParentID != row.id) // skip root level folders where parentid=id
            {
                MarkParentsAsPartOfFilter(row.ParentID, tbl);
            }

        }


    }
}

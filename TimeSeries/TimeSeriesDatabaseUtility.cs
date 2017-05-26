using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reclamation.TimeSeries
{
    public class TimeSeriesDatabaseUtility
    {
        TimeSeriesDatabase m_db;
        TimeSeriesDatabaseDataSet.SeriesCatalogDataTable m_seriesCatalog;
        TimeSeriesDatabaseDataSet.sitecatalogDataTable m_siteCatalog;
        

        public TimeSeriesDatabaseUtility(TimeSeriesDatabase db)
        {
           m_db = db;
           m_seriesCatalog = m_db.GetSeriesCatalog();
           m_siteCatalog = m_db.GetSiteCatalog();
        }

        public void SortByName(PiscesFolder parent)
        {
            var sc = m_db.GetSeriesCatalog("parentid = " + parent.ID + " and isfolder = 1 ", "", " order by name");
            int sortOrder = 10;
            for (int i = 0; i < sc.Rows.Count; i++)
            {
                if (sc[i].siteid == "")
                    sc[i].siteid = sc[i].Name; // fix... siteid might be handy on a folder
                sc[i].SortOrder = sortOrder;
                sortOrder += 10;
            }
            m_db.Server.SaveTable(sc);
        }


        /// <summary>
        /// define path based on  root/sitecatalog.type/siteid/interval
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private List<string> GetNewPath(string root,TimeSeriesDatabaseDataSet.SeriesCatalogRow row)
        {
            var selectedPath = new List<string>();

            selectedPath.Add(root);
            
            var siteRow = m_siteCatalog.FindBysiteid(row.siteid);
            if (siteRow != null)
                selectedPath.Add(siteRow.type);
            else
            {
                Console.WriteLine("no site defined... in sitecatalog.");
                selectedPath.Add("unknown");
            }
            selectedPath.Add(row.siteid);
            selectedPath.Add(GetIntervalPath(row));
            return selectedPath;

        }

        /// <summary>
        /// Put all series into a consistent folder structure
        /// </summary>
        public void OrganizeSeriesCatalog(PiscesFolder folder)
        {
            
            if( folder.ID != folder.ParentID)
            {
                return; // must start from root folder.
            }

            var selectedPath = new List<string>();
            selectedPath.AddRange(m_seriesCatalog.GetPath(folder.ID));
            selectedPath.Add(folder.Name);


            for (int i = 0; i < m_seriesCatalog.Count; i++)
            {
                var row = m_seriesCatalog[i];
                if (row.IsFolder == 1 )
                    continue;

                var existingPath = new List<string>();
                existingPath.AddRange(m_seriesCatalog.GetPath(row.id));


                var newPath = GetNewPath(folder.Name,row);

                 

                if (!SamePath(newPath,existingPath))
                {
                    Console.WriteLine("Moving "+row.TableName);
                    Console.WriteLine("From :"+ String.Join("/",existingPath.ToArray()));
                    Console.WriteLine("To   :"+ String.Join("/", newPath.ToArray()));
                    var id = m_seriesCatalog.GetOrCreateFolder(newPath.ToArray());
                    row.ParentID = id;
                }
            }

            m_db.Server.SaveTable(m_seriesCatalog);

        }

         private bool SamePath(List<string> path1, List<string> path2)
         {
             if (path1.Count != path2.Count)
                 return false;

             for (int i = 0; i < path1.Count; i++)
             {
                 if (path1[i] != path2[i])
                     return false;
             }
             return true;
         }

        ///// <summary>
        ///// determines if the child is inside the root
        ///// </summary>
        ///// <param name="selectedPath">root path </param>
        ///// <param name="rowPath"></param>
        ///// <returns></returns>
        // private bool IsChildInRoot(List<string> root, List<string> child)
        // {
        //     if (root.Count > child.Count)
        //         return false;

        //     for (int i = 0; i < root.Count; i++)
        //     {
        //         if (root[i] != child[i])
        //             return false;
        //     }
        //     return true;
        // }

         private static string GetIntervalPath(TimeSeriesDatabaseDataSet.SeriesCatalogRow row)
         {
             TimeSeriesName tn = new TimeSeriesName(row.TableName);
             var interval = "instant";
             if (row.TimeInterval == "Irregular" || row.TimeInterval == "Hourly")
                 interval = "instant";
             else
                 interval = row.TimeInterval.ToLower();

             if (TimeSeriesDatabase.IsQuality(tn.pcode))
                 interval = "quality";

             return interval;
         }


    }
}

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
        public TimeSeriesDatabaseUtility(TimeSeriesDatabase db)
        {
            m_db = db;
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
        ///  put each series in folder ../siteid/interval/
        /// </summary>
        /// <param name="root"></param>
        /// <param name="folder"></param>
         public void OrganizeCatalogBySiteInterval(PiscesFolder folder)
        {
          

            TimeSeriesDatabaseDataSet.SeriesCatalogDataTable sc = m_db.GetSeriesCatalog();
            var siteCatalog = m_db.GetSiteCatalog();

            var selectedPath = new List<string>();
            selectedPath.AddRange(sc.GetPath(folder.ID));
            selectedPath.Add(folder.Name);

            for (int i = 0; i < sc.Count; i++)
            {
                var row = sc[i];
                if (row.IsFolder == 1 || row.Provider != "Series")
                    continue;
                TimeSeriesName tn = new TimeSeriesName(row.TableName);

                var rowPath = new List<string>();

                rowPath.AddRange(sc.GetPath(row.id));

                if( ! IsChildInRoot(selectedPath,rowPath))
                    continue;

                var expectedPath = new List<string>();
                expectedPath.AddRange(selectedPath);
                expectedPath.Add(tn.siteid);
                expectedPath.Add(GetIntervalPath(row, tn.pcode));
             
                if (!SamePath(expectedPath,rowPath))
                {
                    Console.WriteLine("Moving "+row.TableName);
                    Console.WriteLine("From :"+ String.Join("/",rowPath.ToArray()));
                    Console.WriteLine("To   :"+ String.Join("/", expectedPath.ToArray()));
                    var id = sc.GetOrCreateFolder(expectedPath.ToArray());
                    row.ParentID = id;
                }
            }

            m_db.Server.SaveTable(sc);

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

        /// <summary>
        /// determines if the child is inside the root
        /// </summary>
        /// <param name="selectedPath">root path </param>
        /// <param name="rowPath"></param>
        /// <returns></returns>
         private bool IsChildInRoot(List<string> root, List<string> child)
         {
             if (root.Count > child.Count)
                 return false;

             for (int i = 0; i < root.Count; i++)
             {
                 if (root[i] != child[i])
                     return false;
             }
             return true;
         }

         private static string GetIntervalPath(TimeSeriesDatabaseDataSet.SeriesCatalogRow row, string pcode)
         {
             var interval = "instant";
             if (row.TimeInterval == "Irregular" || row.TimeInterval == "Hourly")
                 interval = "instant";
             else
                 interval = row.TimeInterval.ToLower();

             if (TimeSeriesDatabase.IsQuality(pcode))
                 interval = "quality";

             return interval;
         }


    }
}

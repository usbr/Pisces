using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Reclamation.Core;

namespace Reclamation.TimeSeries
{
    /// <summary>
    /// provides method to delete from database tree recursivly 
    /// </summary>
    public class TimeSeriesDatabaseDelete
    {

        TimeSeriesDatabase m_db;
        TimeSeriesDatabaseDataSet.SeriesCatalogDataTable catalog;
        int sdi;
        public TimeSeriesDatabaseDelete(TimeSeriesDatabase db, PiscesObject o)
        {
            this.m_db = db;
            sdi = o.ID;
            catalog = m_db.GetSeriesCatalog();
            //catalog.PrimaryKey = new DataColumn[] { catalog.Columns["id"]};
        }

        
        public void Delete()
        {
            MarkForDeletion(sdi);
            m_db.Server.SaveTable(catalog);

        }
        void MarkForDeletion(int id)
        {
            Console.WriteLine("MarkForDeletion("+id+")");
            var sr = catalog.FindByid(id);
            //DataRow row = catalog.Rows.Find(id);

            //SeriesCatalogRow sr = new SeriesCatalogRow(row);

            if (sr.IsFolder)
            {
                DataRow[] children = catalog.Select("ParentID = " + id);
                for (int i = 0; i < children.Length; i++)
                {
                    int childSdi = Convert.ToInt32(children[i]["id"]);
                    if( childSdi != id)
                      MarkForDeletion(childSdi);
                }
            }

            if (sr.Provider == "ModsimSeries"
               || sr.Provider == "RiverWareSeries"
               || sr.Provider == "HecDssSeries" || sr.TableName == "" 
           
                )
            {// no table.  Delete catalog row
                sr.Delete();
            }
            else
            {// drop table then delete catalog row
                m_db.DropTable(sr.TableName);
                sr.Delete();
            }
        }
    }
}

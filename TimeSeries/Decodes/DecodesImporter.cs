using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Reclamation.Core;
using System.IO;

namespace Reclamation.TimeSeries.Decodes
{
    public class DecodesImporter
    {
        TimeSeriesDatabase m_db;
        public DecodesImporter(TimeSeriesDatabase db)
        {
            m_db = db;
        }

        internal void Import(string path)
        {
            string attic = Path.Combine(path,"attic");
            if( !Directory.Exists(attic))
                Directory.CreateDirectory(attic);

            
            string[] fileEntries = Directory.GetFiles(path);
            foreach (string fileName in fileEntries)
            {

                DecodesParser p = new DecodesParser(fileName);
                Series[] items =  p.GetSeries();
                foreach (var s in items)
                {
                   var  sr = m_db.GetSeriesRow("TableName = '" + s.Table.TableName + "'");
                   if (sr == null)
                   {
                       // need to create a new entry/table
                       sr = m_db.GetNewSeriesRow();
                       sr.TableName = s.Table.TableName;
                       m_db.AddSeries(s);
                   }
                   else
                   {
                       m_db.SaveTimeSeriesTable(sr.id, s, DatabaseSaveOptions.UpdateExisting);
                   }
                }

                // move into the attic after processing is completed.
                // if allready exits...create new filename
                //File.Move(fileName, Path.Combine(attic, Path.GetFileName(fileName)));
            }


        }
    }
}

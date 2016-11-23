using System;
using System.Collections.Generic;
using Reclamation.TimeSeries;
using Aga.Controls.Tree;

namespace Reclamation.TimeSeries.Forms
{
    public class TimeSeriesTreeModel:Aga.Controls.Tree.TreeModelBase
    {
        TimeSeriesDatabase db;
        public TimeSeriesTreeModel(TimeSeriesDatabase db)
        {
            this.db = db;
            db.DatabaseChanged += new EventHandler<Reclamation.Core.ArrayEventArgs>(db_DatabaseChanged);
        }

        void db_DatabaseChanged(object sender, Reclamation.Core.ArrayEventArgs e)
        {
         this.OnStructureChanged(new TreePathEventArgs(new TreePath( e.Args)));     
        }

        public override System.Collections.IEnumerable  GetChildren(Aga.Controls.Tree.TreePath treePath)
        {
            List<PiscesObject> items = new List<PiscesObject>();

            if (treePath.IsEmpty())
            {
                var roots = db.GetRootObjects();
                
                foreach (PiscesObject o in roots)
                {
                    items.Add(o);
                }
            }
            else
            {
                PiscesObject s = treePath.LastNode as PiscesObject;
                PiscesObject[] children = db.GetChildren(s);

                foreach (PiscesObject si in children)
                {
                    items.Add(si);
                }
            }
            return items;
        }

        public override bool IsLeaf(Aga.Controls.Tree.TreePath treePath)
        {
            PiscesObject o = treePath.LastNode as PiscesObject;
            return !(o is PiscesFolder);
        }

       
    }
}

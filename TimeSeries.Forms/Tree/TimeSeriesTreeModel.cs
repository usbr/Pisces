using System;
using System.Collections.Generic;
using Reclamation.TimeSeries;
using Aga.Controls.Tree;
using System.Drawing;
using Reclamation.Core;
using System.IO;

namespace Reclamation.TimeSeries.Forms
{
    public class TimeSeriesTreeModel : Aga.Controls.Tree.TreeModelBase
    {
        TimeSeriesDatabase db;
        public TimeSeriesTreeModel(TimeSeriesDatabase db)
        {
            this.db = db;
            db.DatabaseChanged += new EventHandler<Reclamation.Core.ArrayEventArgs>(db_DatabaseChanged);
        }

        void db_DatabaseChanged(object sender, Reclamation.Core.ArrayEventArgs e)
        {
            this.OnStructureChanged(new TreePathEventArgs(new TreePath(e.Args)));
        }

        public override System.Collections.IEnumerable GetChildren(Aga.Controls.Tree.TreePath treePath)
        {
            List<PiscesObject> items = new List<PiscesObject>();

            if (treePath.IsEmpty())
            {
                var roots = db.GetRootObjects();

                foreach (PiscesObject o in roots)
                {
                    AssignImage(o);
                    items.Add(o);
                }
            }
            else
            {
                PiscesObject s = treePath.LastNode as PiscesObject;
                PiscesObject[] children = db.GetChildren(s);

                foreach (PiscesObject si in children)
                {
                    AssignImage(si);
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

        private void AssignImage(PiscesObject o)
        {
            o.Icon = AssignIcon(o.SeriesCatalogRow.iconname);
        }

        static private Image AssignIcon(string source)
        {
            if (source == "")
                return null;
            for (int i = 0; i < IconNames.Length; i++)
            {
                string s = IconNames[i];
                int idx = source.IndexOf(s, StringComparison.CurrentCultureIgnoreCase);
                if (idx >= 0)
                {
                    return m_images[i];
                }
            }
            return null;
        }

        static string[] m_iconNames = null;
        static Image[] m_images = null;

        /// <summary>
        /// A list of image names without an extension.
        /// </summary>
        private static string[] IconNames
        {
            get
            {
                if (m_iconNames == null)
                {
                    //string dir = Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath);
                    string dir = FileUtility.GetExecutableDirectory();
                    dir = Path.Combine(dir, "images");

                    if (!Directory.Exists(dir))
                        return new string[] { };

                    var lst = new List<string>();
                    var imgList = new List<Image>();
                    DirectoryInfo di = new DirectoryInfo(dir);
                    FileInfo[] files = di.GetFiles();

                    foreach (var f in files)
                    {
                        var ext = f.Extension.ToLower();
                        if (ext == ".ico"
                            || ext == ".bmp"
                            || ext == ".gif")
                        {
                            Bitmap b = null;
                            try
                            {
                                Logger.WriteLine("reading " + f.FullName);
                                Bitmap b1;
                                if (ext == ".gif")
                                {
                                    byte[] ir = File.ReadAllBytes(f.FullName);
                                    Image i = Image.FromStream(new MemoryStream(ir));
                                    b1 = new Bitmap(new Bitmap(i));
                                }
                                else
                                {
                                    b1 = new Bitmap(f.FullName);
                                }
                                b = new Bitmap(b1, new Size(16, 16));

                            }
                            catch (Exception ex)
                            {
                                //System.Windows.Forms.MessageBox.Show(ex.Message);
                                Logger.WriteLine(ex.Message);
                                //m_iconNames = new string[]{};
                                //return m_iconNames;
                                continue;
                            }
                            lst.Add(Path.GetFileNameWithoutExtension(f.FullName));
                            imgList.Add(b);
                        }
                    }
                    m_iconNames = lst.ToArray();
                    m_images = imgList.ToArray();

                }
                return m_iconNames;
            }
        }


    }
}

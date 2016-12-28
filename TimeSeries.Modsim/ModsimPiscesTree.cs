using System;
using System.Data;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using Csu.Modsim.ModsimIO;
using Csu.Modsim.ModsimModel;
using Reclamation.TimeSeries;
using Reclamation.Core;
using Aga.Controls.Tree;
namespace Reclamation.TimeSeries.Modsim
{
    public class PiscesTree
    {
        static string m_databaseName = "";
        static AccessDB m_db;
        static string m_xyFilename;
        static string modsimName;
        static Csu.Modsim.ModsimModel.Model mi;// = new Model();
        static TimeSeriesDatabaseDataSet.SeriesCatalogDataTable seriesCatalog;
        static int sdi;
        static TimeSeriesDatabase s_db;

        static int studyFolderID;
        static string dir = "";


        public static void CreatePiscesTree(string fileName, PiscesFolder root,
            TimeSeriesDatabase db)
        {
            mi = new Model();
            s_db = db;
            sdi = db.NextSDI();
            studyFolderID = sdi;
            int parentID = root.ID;
            
            seriesCatalog = new TimeSeriesDatabaseDataSet.SeriesCatalogDataTable();
            if (File.Exists(fileName))
            {
                XYFileReader.Read(mi, fileName);
                m_xyFilename = Path.GetFileNameWithoutExtension(fileName);
            }
            else
            {
                throw new FileNotFoundException("Modsim xy file is not found " + fileName);
            }

            string mdbJetName = Path.Combine(Path.GetDirectoryName(fileName), m_xyFilename + "OUTPUT.mdb");
            string mdbAceName = Path.Combine(Path.GetDirectoryName(fileName), m_xyFilename + "OUTPUT.accdb");
            if (File.Exists(mdbAceName))
            {
                m_databaseName = mdbAceName;
            }
            else
                m_databaseName = mdbJetName;

            if (File.Exists(m_databaseName))
            {
                m_db = new AccessDB(m_databaseName);
                dir = Path.GetDirectoryName(Path.GetFullPath(m_databaseName));
                //AddNewRow(sdi,parentID,true, "", mi.name, "");
                AddNewRow(sdi, parentID, true, "", Path.GetFileNameWithoutExtension(fileName), "");

                ReservoirsTree();
                DemandsTree();
                RiverLinksTree();
                TotalsTree();

            }
            else
            {
                throw new FileNotFoundException(" MODSIM output not found " + m_databaseName);
            }

            //DataTableOutput.Write(seriesCatalog, @"C:\temp\a.csv",false);
            db.Server.SaveTable(seriesCatalog);
            db.RefreshFolder(root);
        }

        private static void TotalsTree()
        {
            var fnT = Path.Combine(dir, "ModsimTotals.txt");
            if (File.Exists(fnT))
            {

                int totalFolderID = ++sdi;
                AddNewRow(totalFolderID, studyFolderID, true, "", "Totals", "");

                string[] lines = File.ReadAllLines(fnT);

                for (int i = 0; i < lines.Length; i++)
                {
                    if (lines[i].IndexOf("#") == 0)
                    {
                        continue;
                    }

                    AddTotals(lines[i], totalFolderID);
                }
            }

        }

        private static void AddTotals(string line, int parentID)
        {
            int idx = line.IndexOf("=");
            if (idx <= 0)
            {
                Logger.WriteLine("could not parse line in ModsimTotals.txt:  missing '='");
                Logger.WriteLine(line);
                return;
            }
            string name = line.Substring(0, idx).Trim();
            string right = line.Substring(idx + 1).Trim();

            idx = right.IndexOf(":");
            if (idx <= 0)
            {
                Logger.WriteLine("could not parse line in ModsimTotals.txt: missing ':'");
                Logger.WriteLine(line);
                return;
            }
            string columnName = right.Substring(0, idx);
            string modsimName = right.Substring(idx + 1).Trim();

            if (columnName == "Demand" || columnName == "Shortage" || columnName == "Surf_In"
                || columnName == "Stor_End")
            {
                if (!NodesExist(modsimName.Split(',')))
                {
                    Logger.WriteLine("end of Warning");
                }
            }
            else
            {
                if (columnName == "Flow")
                {
                    if (!LinksExist(modsimName.Split(',')))
                    {
                        Logger.WriteLine("end of Warning");
                    }
                }
            }

            AddNewRow(++sdi, parentID, false, modsimName, name, columnName);

        }

        /// <summary>
        /// check if all the links exist in Modsim output mdb file
        /// </summary>
        /// <param name="links"></param>
        /// <returns></returns>
        private static bool LinksExist(string[] links)
        {
            string sql = "Select LName from LinksInfo where "
                + " LName in ('" + String.Join("','", links) + "')";

            DataTable tbl = m_db.Table("LinksInfo", sql);

            bool rval = tbl.Rows.Count == links.Length;

            if (!rval)
            {
                Logger.WriteLine("WARNING: some Modsim Link Names do not exist in the output");
                Logger.WriteLine("-- from output -- " + tbl.Rows.Count + " links");
                for (int i = 0; i < tbl.Rows.Count; i++)
                {
                    Logger.WriteLine(tbl.Rows[i][0].ToString());
                }
                Logger.WriteLine("-- from ModsimTotals.txt --- " + links.Length + " links ");
                for (int i = 0; i < links.Length; i++)
                {
                    Logger.WriteLine(links[i]);
                }

            }

            return rval;
        }

        /// <summary>
        /// check if all the nodes exist in Modsim output mdb file
        /// </summary>
        /// <param name="nodes"></param>
        /// <returns></returns>
        private static bool NodesExist(string[] nodes)
        {
            string sql = "Select NName from NodesInfo where "
                + " NName in ('" + String.Join("','", nodes) + "')";

            DataTable tbl = m_db.Table("NodesInfo", sql);

            bool rval = tbl.Rows.Count == nodes.Length;

            if (!rval)
            {
                Logger.WriteLine("WARNING: some Modsim Node Names do not exist in the output");
                Logger.WriteLine("-- from output -- " + tbl.Rows.Count + " nodes");
                for (int i = 0; i < tbl.Rows.Count; i++)
                {
                    Logger.WriteLine(tbl.Rows[i][0].ToString());
                }
                Logger.WriteLine("-- from ModsimTotals.txt --- " + nodes.Length + " nodes ");
                for (int i = 0; i < nodes.Length; i++)
                {
                    Logger.WriteLine(nodes[i]);
                }

            }

            return rval;

        }


        static void ReservoirsTree()
        {
            DataTable resTable = m_db.Table("test", "Select * from NodesInfo where NType = 'Reservoir'");
            if (resTable.Rows.Count <= 0)
                return;

            string whereClause = "WHERE NodesInfo.NType='Reservoir' ";

            string fnRes = Path.Combine(dir, "Reservoirs.txt");

            if (File.Exists(fnRes))
            {
                StreamReader sr = new StreamReader(fnRes);
                string[] names = sr.ReadToEnd().Replace("\r", "").Split('\n');
                string inClause = "('" + String.Join("','", names) + "')";
                whereClause = "WHERE NodesInfo.NName In " + inClause + " ";
            }

            string sql = "SELECT NodesInfo.NName , " +
                        "Max(RESOutput.Spills) AS MaxOfSpills, " +
                        "Max(RESOutput.Head_Avg) AS MaxOfHead_Avg, " +
                        "Max(RESOutput.Evap_Loss) AS MaxOfEvap_Loss " +
                        "FROM NodesInfo INNER JOIN RESOutput ON  NodesInfo.NNumber = RESOutput.NNo  " +
                        whereClause
                        + "Group by NodesInfo.NName";

            resTable = m_db.Table("ResOuput", sql);


            if (resTable.Rows.Count > 0)
            {
                modsimName = "";
                int reservoirsFolder = ++sdi;
                AddNewRow(sdi, studyFolderID, true, "", "Reservoirs", "");

                for (int i = 0; i < resTable.Rows.Count; i++)
                {
                    DataRow row = resTable.Rows[i];
                    modsimName = row["NName"].ToString();
                    int resNameFolder = ++sdi;
                    AddNewRow(resNameFolder, reservoirsFolder, true, "", modsimName, "");
                    AddDefaultResRows(sdi);
                    //if (Convert.ToInt32(row["MaxOfSpills"]) > 0)
                    //    AddSpillRow();
                    //if (Convert.ToInt32(row["MaxOfHead_Avg"]) > 0)
                    //    AddPowerRows();
                    //if (Convert.ToInt32(row["MaxOfEvap_Loss"]) > 0)
                    //    AddEvapRow();
                    if (mi.ExtStorageRightActive)
                        AddStorageRights(resNameFolder);
                    //parentID = savedParent;
                }
            }
        }
        static void AddStorageRights(int resSDI)
        {
            int i;
            Link l;
            LinkList ll;
            LinkList inLinks = mi.FindNode(modsimName).InflowLinks;
            List<Link> accLinkList = new List<Link>();
            for (ll = inLinks; ll != null; ll = ll.next)
            {
                l = ll.link;
                if (l.IsAccrualLink)
                {
                    accLinkList.Add(l);
                }
            }
            if (accLinkList.Count > 0)
            {
                int savedParent = ++sdi;
                //AddNewRow(sdi, studyFolderID, true, "", "Accrual Links", "");
                AddNewRow(sdi, resSDI, true, "", "Accrual Links", "");
                for (i = 0; i < accLinkList.Count; i++)
                {
                    modsimName = accLinkList[i].name;
                    int saved2 = ++sdi;
                    AddNewRow(saved2, savedParent, true, "", modsimName, "");
                    AddNewRow(++sdi, saved2, false, modsimName, "Time Step Accrual", "NaturalFlow");
                    AddNewRow(++sdi, saved2, false, modsimName, "Storage Left", "StorLeft");
                    AddNewRow(++sdi, saved2, false, modsimName, "Group Storage Left", "GroupStorLeft");
                    AddNewRow(++sdi, saved2, false, modsimName, "Season Accrual", "Accrual");
                }
            }
        }
        static void AddDefaultResRows(int parentID)
        {
            AddNewRow(++sdi, parentID, false, modsimName, "Ending Storage Content", "Stor_End");
            AddNewRow(++sdi, parentID, false, modsimName, "Target Storage Content", "Stor_Trg");
            AddNewRow(++sdi, parentID, false, modsimName, "Ending Forebay Elevation", "Elev_End");//, "feet");
            AddNewRow(++sdi, parentID, false, modsimName, "Hydrologic State Index", "Hydro_State_Res");//, "");
        }
        //static void AddSpillRow()
        //{
        //    AddNewRow(false, modsimName, "Reservoir Spill", "Spill");
        //}
        //static void AddPowerRows()
        //{
        //    PiscesFolder savedID = parent;
        //    AddNewRow(true, "", "Power Output", "");

        //    AddNewRow(false, modsimName, "Average KW", "Powr_Avg", "Avg KW");
        //    AddNewRow(false, modsimName, "Average Head Feet", "Head_Avg", "Feet");
        //    parent = savedID;
        //}
        //static void AddEvapRow()
        //{
        //    AddNewRow(false, modsimName, "Evaporation Loss", "Evap_Loss");
        //}

        static void DemandsTree()
        {
            List<string> demList = new List<string>();
            List<string> ftList = new List<string>();
            string[] demnodes;
            int i;

            string dfn = Path.Combine(dir, "Demands.txt");
            if (File.Exists(dfn))
            {
                StreamReader sr = new StreamReader(dfn);
                demnodes = sr.ReadToEnd().Replace("\r", "").Split('\n');
                if (demnodes.Length == 1 && demnodes[0].Trim() == "")
                    demnodes = new string[] { };
            }
            else
            {
                string sql = "SELECT NName from NodesInfo where Ntype='Demand' order by NName ";
                DataTable demTable = m_db.Table("NodesInfo", sql);
                demnodes = new string[demTable.Rows.Count];
                for (i = 0; i < demTable.Rows.Count; i++)
                {
                    demnodes[i] = Convert.ToString(demTable.Rows[i][0]);
                }
            }
            for (i = 0; i < demnodes.Length; i++)
            {
                Csu.Modsim.ModsimModel.Node node = mi.FindNode(demnodes[i]);
                if (node == null)
                {
                    System.Windows.Forms.MessageBox.Show("Error:  A node in the output file was not found in the xy file. (run the model?)");
                    Logger.WriteLine("Error: could not find node '" + demnodes[i] + "'");
                }

                if (node != null && node.m.idstrmx[0] != null)
                {
                    ftList.Add(demnodes[i]);
                }
                else
                {
                    //if (demnodes[i].Substring(0, 3) != "neg" && demnodes[i].Substring(0, 3) != "nat") 
                    demList.Add(demnodes[i]);
                }
            }

            if (demList.Count > 0)
                ComsumptiveDemandsTree(demList.ToArray());
            if (ftList.Count > 0)
                FlowThruDemandsTree(ftList.ToArray());
        }
        static void ComsumptiveDemandsTree(string[] demands)
        {
            int demandHead = ++sdi;
            AddNewRow(sdi, studyFolderID, true, "", "Consumptive Demands", "");
            for (int i = 0; i < demands.Length; i++)
            {
                modsimName = demands[i];
                int dNode = ++sdi;
                AddNewRow(dNode, demandHead, true, "", modsimName, "");
                AddNewRow(++sdi, dNode, false, modsimName, "Demand", "Demand");
                AddNewRow(++sdi, dNode, false, modsimName, "Surface Delivered", "Surf_In");
                AddNewRow(++sdi, dNode, false, modsimName, "Shortage", "Shortage");
                AddNewRow(++sdi, dNode, false, modsimName, "Hydrologic State Index", "Hydro_State_Dem");
                string saveModsimName = modsimName;
                if (mi.ExtWaterRightsActive)
                    AddNaturalFlowRights(dNode);
                modsimName = saveModsimName;
                if (mi.ExtStorageRightActive)
                    AddStorageContracts(dNode);
            }
        }
        static void AddNaturalFlowRights(int dNode)
        {
            int i;
            Link l;
            LinkList ll;
            LinkList inLinks = mi.FindNode(modsimName).InflowLinks;
            List<Link> accLinkList = new List<Link>();
            for (ll = inLinks; ll != null; ll = ll.next)
            {
                l = ll.link;
                if (l.IsNaturalFlowLink())
                {
                    accLinkList.Add(l);
                }
            }
            if (accLinkList.Count > 0)
            {
                int natural = ++sdi;
                AddNewRow(sdi, dNode, true, "", "Natural Flow Links", "");

                for (i = 0; i < accLinkList.Count; i++)
                {
                    modsimName = accLinkList[i].name;
                    int aNode = ++sdi;
                    AddNewRow(aNode, natural, true, "", modsimName, "");
                    AddNewRow(++sdi, aNode, false, modsimName, "Time Step Flow", "Flow");
                }
            }
        }
        static void AddStorageContracts(int parentID)
        {
            int i;
            Link l;
            LinkList ll;
            LinkList inLinks = mi.FindNode(modsimName).InflowLinks;
            List<Link> accLinkList = new List<Link>();
            for (ll = inLinks; ll != null; ll = ll.next)
            {
                l = ll.link;
                if (l.IsOwnerLink())
                {
                    accLinkList.Add(l);
                }
            }
            if (accLinkList.Count > 0)
            {
                int contracts = ++sdi;
                AddNewRow(contracts, parentID, true, "", "Storage Contract Links", "");

                for (i = 0; i < accLinkList.Count; i++)
                {
                    int aNode = ++sdi;
                    modsimName = accLinkList[i].name;
                    AddNewRow(aNode, contracts, true, "", modsimName, "");
                    AddNewRow(++sdi, aNode, false, modsimName, "Time Step Flow", "Flow");
                    AddNewRow(++sdi, aNode, false, modsimName, "Storage Left", "StorLeft");
                    AddNewRow(++sdi, aNode, false, modsimName, "Group Storage Left", "GroupStorLeft");
                    AddNewRow(++sdi, aNode, false, modsimName, "Season Accrual", "Accrual");
                }
            }
        }
        static void FlowThruDemandsTree(string[] ftnodes)
        {
            int ftDemands = ++sdi;
            AddNewRow(sdi, studyFolderID, true, "", "FlowThru Demands", "");

            for (int i = 0; i < ftnodes.Length; i++)
            {
                int aNode = ++sdi;
                modsimName = ftnodes[i];
                AddNewRow(aNode, ftDemands, true, "", modsimName, "");

                AddNewRow(++sdi, aNode, false, modsimName, "Demand", "Demand");
                AddNewRow(++sdi, aNode, false, modsimName, "Surface Delivered", "Surf_In");
                AddNewRow(++sdi, aNode, false, modsimName, "Shortage", "Shortage");
                AddNewRow(++sdi, aNode, false, modsimName, "Hydrologic State Index", "Hydro_State_Dem");
                string saveModsimName = modsimName;
                if (mi.ExtWaterRightsActive)
                    AddNaturalFlowRights(aNode);
                modsimName = saveModsimName;
                if (mi.ExtStorageRightActive)
                    AddStorageContracts(aNode);
            }
        }
        static void RiverLinksTree()
        {
            string[] linkNames = new string[0];


            var fnLinks = Path.Combine(dir, "RiverLinks.txt");

            if (File.Exists(fnLinks))
            {
                linkNames = File.ReadAllLines(fnLinks);
            }
            else
            {
                linkNames = RiverLinksFromModelFile();
            }

            if (linkNames.Length == 0) return;
            int rLinks = ++sdi;
            AddNewRow(rLinks, studyFolderID, true, "", "River Links", "");

            for (int i = 0; i < linkNames.Length; i++)
            {
                int aNode = ++sdi;
                modsimName = linkNames[i];
                AddNewRow(aNode, rLinks, true, "", modsimName, "");

                AddNewRow(++sdi, aNode, false, modsimName, "Time Step Flow", "Flow");
                AddNewRow(++sdi, aNode, false, modsimName, "Natural Flow Step Flow", "NaturalFlow");
                AddNewRow(++sdi, aNode, false, modsimName, "Time Step Capacity", "LMax");
            }

        }
        static string[] RiverLinksFromModelFile()
        {
            if (mi.timeStep.TSType != ModsimTimeStepType.Daily && mi.timeStep.TSType != ModsimTimeStepType.Monthly)
            {
                Console.WriteLine(" autoGenerate is set up for monthly and daily time steps only");
            }

            string[] links = new string[0];
            {
                List<string> list = new List<string>();

                Link l;
                var realList = mi.Links_Real;

                //for (l = mi.firstLink; l.next != null; l = l.next)
                for (int i = 0; i < realList.Length; i++)
                {
                    l = realList[i];
                    
                    if (l.from.nodeType.Equals(NodeType.NonStorage) || l.from.nodeType.Equals(NodeType.Reservoir))
                    {
                        //if (l.name == "60K_nat" || l.name == "skyline")
                        //{
                        //    list.Add(l.name);
                        //}
                        if (l.description.Trim().Length == 0 &&
                            l.to.nodeType.Equals(NodeType.Demand) && mi.ExtWaterRightsActive == true)
                        {
                            continue;
                        }
                        list.Add(l.name);
                    }
                }

                list.Sort();
                links = new string[list.Count];
                list.CopyTo(links);
            }

            return links;
        }

        /// <summary>
        /// insert new tree row
        /// </summary>
        static void AddNewRow(int sdi, int parentID, bool isFolder, string modsimName, string displayName, string timeSeriesName)
        {
            var row = seriesCatalog.NewSeriesCatalogRow();
            row.id = sdi;
            seriesCatalog.Rows.Add(row);
            row.ParentID = parentID;

            if (isFolder)
            {
                row.IsFolder = 1;
                row.Name = displayName;
                row.TableName = "";
            }
            else
            {
                ModsimSeries s = new ModsimSeries(mi, modsimName, timeSeriesName);
                row.IsFolder = 0;
                row.ConnectionString = ConnectionStringUtility.MakeFileNameRelative(s.ConnectionString, s_db.DataSource);
                row.iconname = "Modsim";
                row.Provider = s.Provider;
                row.Name = displayName;
                row.TimeInterval = s.TimeInterval.ToString();
                //row.TableName = "empty" + Guid.NewGuid().ToString();
                row.siteid = s.SiteID;
                row.Parameter = s.Parameter;
                row.Units = s.Units;
                //row.DisplayUnits = s.DisplayUnits;
                row.Expression = s.Expression;
                row.Notes = s.Notes;
            }

        }
    }
}

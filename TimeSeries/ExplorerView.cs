using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using Reclamation.TimeSeries.RatingTables;

namespace Reclamation.TimeSeries
{
    /// <summary>
    /// ExplorerView class is a text/console version of pisces for testing 
    /// purposes
    /// </summary>
    public class ExplorerView: IExplorerView
    {
        private DataTable table;
        private SeriesList list;
        private List<string> messages;
        public ExplorerView()
        {
            messages = new List<string>();
        }

        bool m_undoZoom = true;

        public bool UndoZoom
        {
            get { return m_undoZoom; }
            set { m_undoZoom = value; }
        }

        bool m_multiYAxis =false;

        public bool MultipleYAxis{
            //get { return m_multiYAxis; }
            set { m_multiYAxis = value; }

        }

        bool m_monthlySummaryMultiYear = true;
        public bool MonthlySummaryMultiYear 
        {
            set { m_monthlySummaryMultiYear = value; } 
        }

        public void Clear()
        {
            Console.WriteLine();
        }

        public List<string> Messages
        {
            get { return messages; }
            set { this.messages = value; }
        }


        public bool ReadOnly
        {
            get { return true; }
            set { ; }
        }
	
        

        public SeriesList SeriesList
        {
            set {this.list = value ; }
            get { return this.list; }
        }

        public string Title
        {
            set { ; }
        }

        public string SubTitle
        {
            set { ; }
        }

        public System.Data.DataTable DataTable
        {
            set { this.table = value; }
            get { return this.SeriesList.ToDataTable(SeriesList.Count>1); }//.table; }
        }

        public AnalysisType AnalysisType
        {
            set { ; }
        }

        public void Draw()
        {
            Console.WriteLine("Table "+table.TableName);
            if (table != null)
            {
                for (int c = 0; c < table.Columns.Count; c++)
                {
                    Console.Write(table.Columns[c].ColumnName);
                    if (c == table.Columns.Count - 1)
                    {
                        Console.WriteLine();
                    }
                    else
                    {
                        Console.Write(", ");
                    }
                }
                for (int r = 0; r < table.Rows.Count; r++)
                {
                    for (int c = 0; c < table.Columns.Count; c++)
                    {
                        Console.Write(table.Rows[r][c]);
                        if (c == table.Columns.Count - 1)
                        {
                            Console.WriteLine();
                        }
                        else
                        {
                            Console.Write(", ");
                        }
                    }
                    if (r > 3)
                        break;
                }
            }
        }


       public BasicMeasurement Measurement { get; set; }


       

    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace HydrometServer.CommandLine
{
    /// <summary>
    /// class for printing out a data table
    /// </summary>
    public static class TablePrinter
    {
        /// <summary>
        /// data table print method
        /// </summary>
        /// <param name="table"></param>
        public static void Print(DataTable table, int columnsPerSection)
        {
            //Make a fuction that loops through the tbl until it has finished printing 
            //TODO write set number of columns at a time (amf for size)
            //with the columns started with a formatted time and followed by 
            //formatted decimal values

            int startIndex = 1;
            int endIndex = Math.Min(startIndex + columnsPerSection - 1, table.Columns.Count - 1);

            do
            {
                formattedTable(table, startIndex, endIndex);
                startIndex = endIndex + 1;
                endIndex = startIndex + columnsPerSection - 1;
                if (endIndex >= table.Columns.Count)
                {
                    endIndex = table.Columns.Count - 1;
                }

            }
            while (startIndex < table.Columns.Count);

          
       
              
               
          
           




            //int x = columnsToPrint;
            //    if (currentColumn + x >= dataTable.Columns.Count)
            //        x = dataTable.Columns.Count - currentColumn;

            //    formattedTable(dataTable, currentColumn, x);
            //    currentColumn += x;
            
        }

        private static void formattedTable(DataTable dataTable, int startIndex, int endIndex)
        {
            String title = "";
            String output = "";

           

            title += dataTable.Columns[0].ColumnName.PadLeft(10) + " | ";
            for (int i = startIndex; i <= endIndex; i++)
                {
                    
                    title += dataTable.Columns[i].ColumnName.PadLeft(10) + " | ";
                }
            for (int rowIndex = 0; rowIndex < dataTable.Rows.Count; rowIndex++)
                {
                    output += "\n";
                    output += Convert.ToDateTime(dataTable.Rows[rowIndex][0]).ToString("hh:mm");

                    for (int columnIndex = startIndex; columnIndex <= endIndex; columnIndex++)
                    {
                        output += dataTable.Rows[rowIndex][columnIndex].ToString().PadLeft(15);
                    }
                }
                Console.Write(title);
                Console.Write(output);
                Console.WriteLine();

        }

    }
}

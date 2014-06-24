using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Reclamation.Core
{
    /// <summary>
    /// Manage a fixed with file (often FORTRAN type output)
    /// </summary>
    public class FixedWidthTextFile
    {

         List<string[]> fileData;

        public int RowCount
        {
            get { return fileData.Count; }
        }
        
        public string this[int rowIndex, int columnIndex]
        {
            get
            {
                var line = fileData[rowIndex];
                if( columnIndex >= line.Length )
                    return "";

                return line[columnIndex];
            }
        }

        public FixedWidthTextFile(string fileName, params int[] widths)
        {
            TextFile tf = new TextFile(fileName);
            fileData = new List<string[]>();

            for (int i = 0; i < tf.Length; i++)
            {
                var x = TextFile.Split(tf[i], widths);
                fileData.Add(x);
            }
        }


        /// <summary>
        /// Finds the column and row index to a single column range of data
        /// </summary>
        /// <param name="columnRange">search for this data in a column</param>
        public void FindIndexToRange(string[] columnRange, out int rowIndex, out int columnIndex)
        {
            rowIndex = columnIndex = -1;

            for (int r = 0; r < RowCount-columnRange.Length; r++)
            {
                for (int c = 0; c < fileData[r].Length; c++)
                {
                    for (int j = 0; j < columnRange.Length; j++)
                    {
                        if (this[r+j, c].Trim() != columnRange[j].Trim())
                            break;
                        if (j == columnRange.Length - 1)
                        { // we have a match 
                            rowIndex = r;
                            columnIndex = c;
                            return;
                        }
                    }    
                    

                }
            }

            
        }
    }
}

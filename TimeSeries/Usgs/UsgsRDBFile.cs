using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Reclamation.Core;

namespace Reclamation.TimeSeries.Usgs
{
    /// <summary>
    /// Parses a USGS rdb text file into a DataTable
    /// </summary>
    public class UsgsRDBFile: DataTable
    {
        TextFile tf;

        public TextFile TextFile
        {
            get { return tf; }
            set { tf = value; }
        }
        int m_dataIndex = -1;

        public int DataIndex
        {
            get { return m_dataIndex; }
            set { m_dataIndex = value; }
        }
        public UsgsRDBFile(string filename)
        {
            tf = new TextFile(filename);

            ParseFile();
        }
        public UsgsRDBFile(string[] data)
        {
            tf = new TextFile(data);
            ParseFile();
        }

        private void ParseFile()
        {
            FindIndexToData(); 

            AddColumns();
            AddData();

        }

        private void AddData()
        {
            int errorCount = 0;
            for (int i = DataIndex+2; i < TextFile.Length; i++)
            {
                DataRow newRow = this.NewRow();
                if (TextFile[i].Trim() == "")
                    continue;
                string[] tokens = TextFile[i].Split('\t');
                if (tokens.Length != this.Columns.Count)
                {
                    throw new Exception("not enough data at line:" + (int)(i + 1));
                }

                for (int c = 0; c < Columns.Count; c++)
                {
                    if (Columns[c].DataType == typeof(string))
                    {
                        newRow[c] = tokens[c];
                    }
                    else
                        if (Columns[c].DataType == typeof(double))
                        {
                            double d = 0;
                            if (double.TryParse(tokens[c], out d))
                            {
                                newRow[c] = d;
                            }
                            else
                            {
                                errorCount++;
                                if (errorCount < 100)
                                {
                                    Logger.WriteLine("Error: could not convert string to double '" + tokens[c] + "'");
                                }
                                
                                newRow[c] = Point.MissingValueFlag;
                            }
                        }
                        else
                            if (Columns[c].DataType == typeof(DateTime))
                            {
                                newRow[c] = Convert.ToDateTime(tokens[c]);
                            }
                            else
                            {
                                string msg = "error: data type at line:" + (int)(i + 1) + "";
                                throw new Exception(msg);
                            }
                }
                this.Rows.Add(newRow);
            }

            if (errorCount > 100)
            {
                Logger.WriteLine("..."+ (errorCount -100).ToString()+" messages skipped");
            }
        }

        private void FindIndexToData()
        {
            // find first non-comment line.


            for (int i = 0; i < TextFile.Length; i++)
            {
                if (TextFile[i].IndexOf("#") != 0)
                {
                    DataIndex = i;
                    break;
                }
            }

            if (DataIndex < 0)
                throw new Exception("No data was found in the rdb file '" + TextFile.FileName + "'");

        }

        private void AddColumns()
        {
            string[] columnNames = TextFile[DataIndex].Split('\t');
            string[] types = TextFile[DataIndex + 1].Split('\t');

            if (columnNames.Length != types.Length)
            {
                throw new Exception("the number data types did not match the number of columns in the file '" + TextFile.FileName + "'");
            }

            for (int i = 0; i < columnNames.Length; i++)
            {
                string t = "";
                if (types[i].Length > 0)
                {
                    t = types[i].Substring(types[i].Length - 1, 1);
                }
                if (t == "n")
                {
                    Columns.Add(columnNames[i], typeof(double));
                }
                else
                    if (t == "d")
                    {
                        Columns.Add(columnNames[i], typeof(DateTime));
                    }
                    else if (t == "s")
                    {
                        Columns.Add(columnNames[i]).DefaultValue = "";
                    }
                    else
                    {
                        Columns.Add(columnNames[i]); //
                        //   throw new Exception("invalid column type '" + types[i] + "' in file '" + tf.Filename + "'");
                    }
            }
        }



    }

}

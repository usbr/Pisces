/*********************************
 * 
 * Csv.cs 
 *
 * Karl Tarbet  May 30, 2002
 * April 2007 (csv bug fixes)
 * May 2007 data type estimation
 * A DataTable class that reads and writes
 * to comma seperated files.
 * 
 ********************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Reclamation.Core
{
    /// <summary>
    /// Class for reading flat text files. (comma seperated)
    /// </summary>
    public class CsvFile : DataTable
    {
        static bool debugOutput = false;
        private string[] lines;
        private string[] dataTypes;
        private string[] fieldNames;
        string m_filename;

        public string Filename
        {
            get { return m_filename; }
            //set { m_filename = value; }
        }
        int[] widths; // widths for dbase support
        int[] decimals; // decimals for dbase support


        public CsvFile()
        {
            this.m_filename = "";
        }
        /// <summary>
        /// Construct a CsvFile : DataTable
        /// first line of filename contains column names
        /// </summary>
        /// <param name="filename">comma seperated filename</param>
        /// <param name="dataTypes">array of System data types</param>
        public CsvFile(string filename, string[] dTypes, string[] columnNames=null)
        {
            dataTypes = new string[dTypes.Length];
            for (int i = 0; i < dTypes.Length; i++)
            {
                dataTypes[i] = dTypes[i];
            }
            ReadFile(filename);
            char[] delim ={ ',' };
            if (lines.Length > 0 && columnNames == null)
            {
                fieldNames = lines[0].Split(delim);
                ParseFile(dataTypes, fieldNames, 1, "");
            }
            else
            {
                fieldNames = columnNames;
                ParseFile(dataTypes, fieldNames, 1, "");
            }
        }

        /// <summary>
        /// Constructs a CsvFile.  First row in the input text file
        /// contains data. (use the other constructors if the first line has the column names)
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="dataTypes"></param>
        /// <param name="fieldNames"></param>
        public CsvFile(string filename, string[] dTypes, string[] fNames, string pattern)
        {
            dataTypes = new string[dTypes.Length];
            fieldNames = new string[fNames.Length];
            for (int i = 0; i < dataTypes.Length; i++)
            {
                dataTypes[i] = dTypes[i];
                fieldNames[i] = fNames[i];
            }
            ReadFile(filename);
            ParseFile(dataTypes, fieldNames, 0, pattern);
        }

        public enum FieldTypes {AutoDetect,AllText};
        /// <summary>
        /// Construct a CsvFile, based on a DataTable
        /// Second line of input file may contain the data types,
        /// and optionally widths.
        /// </summary>
        /// <param name="filename">comma seperated filename</param>
        public CsvFile(string filename, FieldTypes fieldType = FieldTypes.AutoDetect)
        {
            char[] delim = { ',' };
            ReadFile(filename);

            fieldNames = ParseCSV(lines[0]);

            if (fieldType == FieldTypes.AllText)
            {
                dataTypes = new string[fieldNames.Length];
                for (int i = 0; i < dataTypes.Length; i++)
                {
                    dataTypes[i] = "String";
                }
                ParseFile(dataTypes, fieldNames, 1, "");
                return;
            }
            else
            {// auto detect
                dataTypes = lines[1].Split(delim);

                if (!ValidDataTypes(dataTypes))
                {

                    dataTypes = EstimateDataTypes();
                    ParseFile(dataTypes, fieldNames, 1, "");
                }
                else
                {
                    ParseFile(dataTypes, fieldNames, 2, "");
                }
            }
        }

        private string[] EstimateDataTypes()
        {
            string[] line0 = ParseCSV(lines[0]);
            string[] dataTypes = new string[line0.Length];
            
            for (int i = 0; i < line0.Length; i++)
            {
                string rval = EstimateType(i);
                dataTypes[i] = rval;
            }
            return dataTypes;
        }

        private string EstimateType(int columnIndex)
        {
            int r = 0;
            double d = 0;
            Boolean b = true;
            DateTime t = DateTime.Now;
            int intCount = 0;
            int doubleCount = 0;
            int boolCount = 0;
            int dateCount = 0;
            int stringCount = 0;

            for (int i = 1; i < lines.Length; i++)
            {
                string[] line = ParseCSV(this.lines[i]);
                if (columnIndex >= line.Length)
                    continue;
                string strData = line[columnIndex];
                if (strData.Trim() == "")
                    continue;
                if (Int32.TryParse(strData, out r))
                    intCount++;
                else
                    if (Double.TryParse(strData, out d))
                        doubleCount++;
                    else
                        if (DateTime.TryParse(strData, out t))
                            dateCount++;
                        else
                            if (Boolean.TryParse(strData, out b))
                                boolCount++;
                            else
                                stringCount++;
            }
            if (stringCount > 0)
                return "System.String";
            if (intCount > 0 && doubleCount == 0)
                return "System.Int32";
            if (doubleCount > 0)
                return "System.Double";
            if (dateCount > 0)
                return "System.DateTime";
            if (boolCount > 0)
                return "System.Boolean";

            return "System.String";
        }

        private bool ValidDataTypes(string[] dataTypes)
        {
            for (int i = 0; i < dataTypes.Length; i++)
            {
                string type = AdjustTypeString(dataTypes[i]);
                Type t = System.Type.GetType(type, false, true);
                if (t == null)
                    return false;
            }

            return true;
        }



        private void ParseFile(string[] dataTypes, string[] fieldNames, 
            int lineIndexToBeginRead, string pattern)
        {
            if (debugOutput)
                Console.WriteLine("reading " + this.Filename);

            if (lines.Length <= 0 || lineIndexToBeginRead >= lines.Length)
            {
                return;
            }
            
            
            DataRow myDataRow;

            widths = new int[dataTypes.Length];
            decimals = new int[dataTypes.Length];

            Debug.Assert(fieldNames.Length == dataTypes.Length,
                       "Error: in " + "'" + Filename + "'",
                       "line 1:" + lines[0] + "\n" + "line 2:" + lines[1]);
            int patternMatchFailureCount = 0;
            //int i;
            CreateColumns(dataTypes, fieldNames);

            int col = 0;
            int row = lineIndexToBeginRead;
            DataRowIO io = new DataRowIO(true);
            try
            {
                for (; row < lines.Length; row++)
                {

                    if (lines[row].Trim() == "")
                    {
                        continue;
                    }
                    if (pattern != "" && !Regex.IsMatch(lines[row], pattern))
                    {
                        patternMatchFailureCount++;
                        continue;
                    }


                    string[] val = CsvFile.ParseCSV(lines[row]);

                    myDataRow = this.NewRow();


                    for (col = 0; col < fieldNames.Length; col++)
                    {
                        if (col >= val.Length)
                        {
                            myDataRow[col] = DBNull.Value;
                            continue;
                        }

                        switch (dataTypes[col])
                        {
                            case "System.Int32":
                                io.SaveInt(myDataRow, fieldNames[col], val[col]);
                                break;
                            case "System.Int16":
                                io.SaveInt(myDataRow, fieldNames[col], val[col]);
                                break;
                            case "System.Integer":
                                io.SaveInt(myDataRow, fieldNames[col], val[col]);
                                break;
                            case "System.Double":
                                io.SaveDouble(myDataRow, fieldNames[col], val[col]);
                                break;
                            case "System.Single":
                                io.SaveFloat(myDataRow, fieldNames[col], val[col]);
                                break;
                            case "System.String":
                                myDataRow[col] = val[col];
                                break;
                            case "System.DateTime":
                                myDataRow[col] = Convert.ToDateTime(val[col]);
                                break;
                            case "System.Boolean":
                                if (val[col].ToString() == "")
                                    myDataRow[col] = DBNull.Value;
                                else
                                {
                                    if (val[col].ToString() == "0")
                                        myDataRow[col] = false;
                                    else if (val[col].ToString() == "1")
                                        myDataRow[col] = true;
                                    else
                                    myDataRow[col] = Convert.ToBoolean(val[col]);
                                }
                                break;
                            default:
                                Console.WriteLine("Invalid selection : '" + dataTypes[col] + "'");
                                break;
                        }
                    }
                    this.Rows.Add(myDataRow);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                if (lines.Length < row)
                {
                    Console.WriteLine("'" + lines[row] + "'");
                }
                throw new Exception("Error reading data at row " + row + "  and column " + col + "  file name =  " + this.Filename);
            }

            if (patternMatchFailureCount > 0)
            {
                Logger.WriteLine(patternMatchFailureCount + " lines skipped that did not match pattern " + pattern);
            }
            if( io.Messages.Trim().Length >0)
              Logger.WriteLine(io.Messages);
        }

        private void CreateColumns(string[] dataTypes, string[] fieldNames)
        {
            DataColumn myDataColumn; 
            int i = 0;
            try
            {
                for (i = 0; i < fieldNames.Length; i++)
                {
                    fieldNames[i] = fieldNames[i].Trim();
                    dataTypes[i] = dataTypes[i].Trim();

                    myDataColumn = new DataColumn();

                    string type = dataTypes[i];
                    int idxWidth = type.IndexOf(":");
                    int idxDecimal = type.LastIndexOf(".");

                    this.widths[i] = 0;
                    this.decimals[i] = 0;
                    if (idxWidth > 0)
                    {
                        //type = type.Substring(0,idxWidth);
                        if (idxDecimal > 0 && idxDecimal > idxWidth)
                        {
                            string strWidth = type.Substring(idxWidth + 1, idxDecimal - idxWidth - 1);
                            this.widths[i] = Convert.ToInt32(strWidth);
                            string strDecimals = type.Substring(idxDecimal + 1);
                            this.decimals[i] = Convert.ToInt32(strDecimals);
                        }
                        type = type.Substring(0, idxWidth);
                    }

                    type = AdjustTypeString(type);

                    dataTypes[i] = type;
                    myDataColumn.DataType = System.Type.GetType(type);
                    myDataColumn.ColumnName = fieldNames[i].Trim();
                    //myDataColumn.ReadOnly = true;
                    myDataColumn.Unique = false;

                    if (this.Columns.Contains(myDataColumn.ColumnName))
                    {
                        int j = 1;
                        string name ="";
                        do
                        {
                            name = myDataColumn.ColumnName + j;
                            j++;
                        }
                        while (Columns.Contains(name));

                        name = myDataColumn.ColumnName+j;
                        Logger.WriteLine("Warning: Column allready exists... renamed to "+name);
                        myDataColumn.ColumnName = name;
                    }
                    this.Columns.Add(myDataColumn);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception("Error parsing data types: " + dataTypes[i] + " column = '" + fieldNames[i] + "' filename = " + this.Filename);
            }

        }

        private static string AdjustTypeString(string type)
        {
            if (type == "Integer")
                type = "Int32";
            if (type.IndexOf("System") < 0)
            {
                type = "System." + type;
            }

            if (type == "System.")
            {
                type = "System.String";

            }
            return type;
        }

        bool IsNull(string s)
        {
            if (s.Trim() == "")
            {
                return true;
            }
            return false;
        }


        /// <summary>
        /// reads contents of a text file into an 
        /// array of strings, one for each line.
        /// </summary>
        /// <param name="filename"></param>
        void ReadFile(string filename)
        {
            m_filename = filename;
            string tmpFile = FileUtility.GetTempFileName(".txt");
            File.Copy(filename, tmpFile, true);// solves locked file issues with excel.

            StreamReader srFromFile =
                new StreamReader(tmpFile);
            ArrayList lineList = new ArrayList();
            string strLine;
            while ((strLine = srFromFile.ReadLine()) != null)
            {
                lineList.Add(strLine);
            }
            lines = new String[lineList.Count];
            lineList.CopyTo(lines);
            srFromFile.Close();

            File.Delete(tmpFile);
        }



        public static void Write(CsvFile table, string filename)
        {
            bool sizeInfo = true;
            for (int i = 0; i < table.widths.Length; i++)
            {
                if (table.widths[i] == 0)
                    sizeInfo = false;
            }
            Write(table, filename, sizeInfo);
        }

        /// <summary>
        /// Saves contents of CsvFile to a comma seperated file.
        /// TO DO:  check for internal commas, in string types.
        /// </summary>
        /// <param name="filename"></param>
        private static void Write(CsvFile table, string filename, bool sizeInfo)
        {
            if (debugOutput)
                Console.Write("Writing to " + filename);
            StreamWriter sr = new StreamWriter(filename, false);
            int sz = table.Rows.Count;
            int cols = table.Columns.Count;
            bool[] IsStringColumn = new bool[cols];

            int c;
            for (c = 0; c < cols; c++)
            {
                if (c < cols - 1)
                    sr.Write(table.Columns[c].ColumnName.Trim() + ",");
                else
                    sr.WriteLine(table.Columns[c].ColumnName.Trim()); // no comma on last

                if (table.Columns[c].DataType.ToString() == "System.String")
                    IsStringColumn[c] = true;
            }

            for (c = 0; c < cols - 1; c++)
            {
                sr.Write(table.Columns[c].DataType);
                if (sizeInfo)
                {
                    sr.Write(":" + table.widths[c] + "." + table.decimals[c]);
                }
                sr.Write(",");

            }

            sr.Write(table.Columns[c].DataType); // no comma on last
            if (sizeInfo)
            {
                sr.Write(":" + table.widths[c] + "." + table.decimals[c]);
            }
            sr.WriteLine();


            for (int r = 0; r < sz; r++)
            {
                for (c = 0; c < cols; c++)
                {
                    if (IsStringColumn[c])
                    {
                        sr.Write("\"" + table.Rows[r][c] + "\"");
                    }
                    else
                    {
                        sr.Write(table.Rows[r][c]);
                    }
                    if (c < cols - 1)
                        sr.Write(",");
                }
                sr.WriteLine();
            }
            sr.Close();
            Console.WriteLine(" done.");
        }


        static StreamWriter s_sw = null;

        private static void Write(string s)
        {
            if (s_sw != null)
                s_sw.Write(s);
            else
                Console.Write(s);
        }
        private static void WriteLine(string s)
        {
            if (s_sw != null)
                s_sw.Write(s);
            else
                Console.Write(s);
        }
        /// <summary>
        /// Saves contents of DataTable to comma seperated file.
        /// if filename is empty print to console instead of a file
        /// </summary>
        /// <param name="filename"></param>
        public static void WriteToCSV(DataTable table, string filename, bool WriteSchema=true, bool printHeader=true)
        {
            if (debugOutput)
                Console.Write("Writing to " + filename);
            if (filename != "")
                s_sw = new StreamWriter(filename, false);
            else
                s_sw = null;

            int sz = table.Rows.Count;
            int cols = table.Columns.Count;
            bool[] IsStringColumn = new Boolean[cols];
            int c;

            if (printHeader)
            {
                for (c = 0; c < cols; c++)
                {
                    if (c < cols - 1)
                        Write(EncodeCSVCell(table.Columns[c].ColumnName.Trim()) + ",");
                    else
                        WriteLine(EncodeCSVCell(table.Columns[c].ColumnName.Trim())); // no comma on last
                    if (table.Columns[c].DataType.ToString() == "System.String")
                        IsStringColumn[c] = true;
                }
            }

            if (WriteSchema && cols > 0)
            {
                for (c = 0; c < cols - 1; c++)
                {
                    Write(table.Columns[c].DataType.ToString());
                    Write(",");
                }

                Write(table.Columns[c].DataType.ToString()); // no comma on last
                WriteLine("");
            }

            for (int r = 0; r < sz; r++)
            {
                for (c = 0; c < cols; c++)
                {
                    if (IsStringColumn[c])
                    {
                        string s = table.Rows[r][c].ToString();
                        // s = s.Replace("\"", "\"\"");
                        //sr.Write("\""+s+"\"");
                        Write(EncodeCSVCell(s));
                    }
                    else
                    {
                        Write(table.Rows[r][c].ToString());
                    }
                    if (c < cols - 1)
                        Write(",");
                }
                WriteLine("");

            }
            if( s_sw != null)
              s_sw.Close();
            if (debugOutput)
                Console.WriteLine(" done.");
        }


        public int[] Widths
        {
            get
            {
                return widths;
            }
            set
            {
                this.widths = value;
            }
        }
        public int[] Decimals
        {
            get
            {
                return decimals;
            }
            set
            {
                this.decimals = value;
            }
        }
        /// <summary>
        /// Guesss on widths and decimals attributes
        /// for preserving some *.dbf info
        /// </summary>
        public void EstimateDBFAttributes()
        {
            if (this.widths == null || this.widths.Length < this.Columns.Count)
            {
                this.widths = new int[this.Columns.Count];
                this.decimals = new int[this.Columns.Count];
            }

            for (int i = 0; i < this.Columns.Count; i++)
            {

                switch (this.Columns[i].DataType.ToString())
                {
                    case "System.Int32":
                        this.widths[i] = 10;
                        this.decimals[i] = 0;
                        break;
                    case "System.Integer":
                        this.widths[i] = 10;
                        this.decimals[i] = 0;
                        break;
                    case "System.Double":
                        this.widths[i] = 19;
                        this.decimals[i] = 11;
                        break;
                    case "System.String":
                        int maxWidth = 80;
                        for (int r = 0; r < this.Rows.Count; r++)
                        {
                            if (this.Rows[r][i].GetType() == System.Type.GetType("System.DBNull"))
                                continue;
                            string data = (string)this.Rows[r][i];
                            if (data.Length > maxWidth)
                                maxWidth = data.Length + 5;
                        }
                        this.widths[i] = maxWidth;
                        this.decimals[i] = 0;
                        break;
                    default:
                        Console.WriteLine("Warning .. width=0, decimals = 0 in TextDB");
                        this.widths[i] = 256;
                        this.decimals[i] = 0;
                        break;
                }
            }
        }


        public void AddColumn(string columnName, string type)
        {
            if (this.Columns.Contains(columnName))
                throw new Exception("Allready contains column " + columnName);

            if (type == "Double")
            {
                this.Columns.Add(columnName, System.Type.GetType("System.Double"));
                int[] w = new int[this.Widths.Length + 1];
                int[] d = new int[this.Decimals.Length + 1];
                this.Widths.CopyTo(w, 0);
                this.Decimals.CopyTo(d, 0);
                w[this.Widths.Length] = 19;//width for double
                d[this.Decimals.Length] = 11;//decimals for double
                this.Widths = w;
                this.Decimals = d;
            }
            else if (type == "String")
            {
                this.Columns.Add(columnName, System.Type.GetType("System.String"));
                int[] w = new int[this.Widths.Length + 1];
                int[] d = new int[this.Decimals.Length + 1];
                this.Widths.CopyTo(w, 0);
                this.Decimals.CopyTo(d, 0);
                w[this.Widths.Length] = 80;//width for string
                d[this.Decimals.Length] = 0;
                this.Widths = w;
                this.Decimals = d;
            }
            else if (type == "Int32" || type == "Integer" || type == "System.Int32")
            {
                this.Columns.Add(columnName, System.Type.GetType("System.Int32"));
                //this.Columns[columnName].DefaultValue = 0;

                int[] w = new int[this.Widths.Length + 1];
                int[] d = new int[this.Decimals.Length + 1];
                this.Widths.CopyTo(w, 0);
                this.Decimals.CopyTo(d, 0);
                w[this.Widths.Length] = 10;
                d[this.Decimals.Length] = 0;
                this.Widths = w;
                this.Decimals = d;
            }


        }

        /// <summary>
        /// encode csv data for a single
        /// Excel cell. 
        /// </summary>
        public static string EncodeCSVCell(string data)
        {
            string rval = data;
            if (data.IndexOf('"') >= 0)
            {
                rval = data.Replace("\"", "\"\"");
            }

            if (rval.IndexOfAny(new char[] { ',', '\"' }) >= 0)
            {
                rval = "\"" + rval + "\"";
            }
            return rval;
        }



        /// <summary>
        /// From Mastering Regular Expressions by Jeffrey E.F. Friedl 2nd Edition
        /// </summary>
        static string csvPattern =
                "\\G(?:^|,)                                " +
                "(?:                                       " +
                "(?# Either a double-quoted field... )     " +
                "\"(?# field's opening quote )               " +
                "(?<QuotedField>  (?> [^\"]+ | \"\" )*   ) " +
                "  \"(?# field's closing quote )             " +
                " (?# ...or... )                           " +
                "|                                         " +
                "(?# ...some non-quote/non-comma text... ) " +
                "(?<UnquotedField> [^\",]* )               )"
                + "| "
                + "(?# empty field )  "
                + "(?<EmptyField>) ";


        public static string[] ParseJeffrey(string oneLine)
        {
            Regex re = new Regex(csvPattern, RegexOptions.IgnorePatternWhitespace);
            Match fieldMatch = re.Match(oneLine);
            List<string> rval = new List<string>();
            while (fieldMatch.Success)
            {
                string field = "";
                if (fieldMatch.Groups[1].Success)
                {
                    field = fieldMatch.Groups["QuotedField"].Value;
                    field = Regex.Replace(field, "\"\"", "\"");
                }
                else
                {
                    field = fieldMatch.Groups["UnquotedField"].Value;
                }
                rval.Add(field);
                fieldMatch = fieldMatch.NextMatch();
            }

            return rval.ToArray();
        }


        public static string[] ParseCSV(string line)
        {
            return DecodeCSV(line);
        }


        static Regex s_csvRegex = null;
        private static Regex CsvRegex
        {
            get
            {
                if (s_csvRegex == null)
                {
                    string strPattern = ("^" + "(?:\"(?<value>(?:\"\"|[^\"\\f\\r])*)\"|(?<value>[^,\\f\\r\"]*))") + "(?:,(?:[ \\t]*\"(?<value>(?:\"\"|[^\"\\f\\r])*)\"|(?<value>[^,\\f\\r\"]*)))*" + "$";
            //Match objMatch = Regex.Match(strLine, strPattern);
                    s_csvRegex = new Regex(strPattern,RegexOptions.Compiled);
                }
                return s_csvRegex;
            }
        }

        /// <summary>
        /// DEcodes CSV string
        /// </summary>
        public static string[] DecodeCSV(string strLine)
        {
            /// from http://www.nonhostile.com/page000029.asp
            Match objMatch = CsvRegex.Match(strLine);
            if (!objMatch.Success)
            {
                Logger.WriteLine("Bad CSV line: " + strLine);
                return new string[] { };
            }
            Group objGroup = objMatch.Groups["value"];
            int intCount = objGroup.Captures.Count;
            string[] arrOutput = new string[(intCount - 1) + 1];
            for (int i = 0; i < intCount; i++)
            {
                Capture objCapture = objGroup.Captures[i];
                arrOutput[i] = objCapture.Value;
                arrOutput[i] = arrOutput[i].Replace("\"\"", "\"").Trim();
            }
            //Console.WriteLine(strLine);
            //Console.WriteLine("length = " + Conversions.ToString(arrOutput.Length));
            //Console.WriteLine(string.Join(";", arrOutput));
            return arrOutput;
        }











        /// <summary>
        /// Parse comma seperated string
        /// Original (cpp) code From Brian Winters  of solin corp
        /// </summary>
        private static string[] ParseCSVBrianWinters(string oneLine)
        {
            bool quoted = false;
            string str;
            int beginIndex = 0;
            int endIndex = 0;
            int quoteIndex = 0;
            string delimeter = ",";

            if (oneLine.Trim() == "")
            {
                return new string[] { };
            }
            ArrayList rval = new ArrayList();
            rval.Clear();
            try
            {
                while (true)
                {
                    endIndex = oneLine.IndexOf(delimeter, beginIndex);

                    if (endIndex == -1)
                    {
                        endIndex = oneLine.Length;
                    }


                    if (oneLine[beginIndex] == '\"')
                    {
                        quoted = true;

                        beginIndex++;

                        quoteIndex = oneLine.IndexOf("\"", beginIndex); // initilize

                        for (; ((quoteIndex + 1) < oneLine.Length
                          && oneLine[quoteIndex + 1] == '\"');
                          quoteIndex = oneLine.IndexOf("\"", quoteIndex)) // increment
                        { // find next quote character pairs
                            // until the closing quota character
                            quoteIndex += 2;
                        }

                        endIndex = quoteIndex;
                    }
                    else
                    {
                        quoted = false;
                    }

                    // beginIndex points to the first character of the field
                    // endIndex points to the character after the last character of the field
                    if (quoted)
                        // k.t. 9/25/01 str = "\"" +  oneLine.substr(beginIndex, endIndex - beginIndex)  + "\"";
                        str = oneLine.Substring(beginIndex, endIndex - beginIndex);
                    else
                        str = oneLine.Substring(beginIndex, endIndex - beginIndex);

                    str = str.Trim();

                    // index of first character to be copied,  second number is number of characters to copy
                    if (quoted)
                    {
                        ///		string.Replace(CString("\"\""), CString("\"")); // replace doubles with singles
                        //		replaceAll(str,"\"\"", "\""); // replace doubles with singles
                    }

                    //push_back(str);
                    rval.Add(str);

                    endIndex += (quoted ? 2 : 1);

                    beginIndex = endIndex;

                    if (beginIndex >= (long)oneLine.Length)
                        break;
                }


            }
            catch (Exception e)
            {
                Console.WriteLine("error parsing csv values '" + oneLine + "'");
                throw e;
            }
            string[] srval;
            srval = new string[rval.Count];
            rval.CopyTo(srval);
            return srval;
        }


        /// <summary>
        /// Reads data into a strongly typed DataTable
        /// </summary>
        /// <param name="table"></param>
        public static void ReadIntoTable(string filename, DataTable table)
        {

            string[] dataTypes = table.Columns.Cast<DataColumn>().Select(x => x.DataType.ToString()).ToArray();

            CsvFile csv = new CsvFile(filename,dataTypes);

            foreach (DataRow csvRow in csv.Rows)
            {
                var myDataRow = table.NewRow();
                myDataRow.ItemArray = csvRow.ItemArray;
                table.Rows.Add(myDataRow);
            }
        }

       
    }
}

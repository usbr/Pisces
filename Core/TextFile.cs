using System;
using System.Text.RegularExpressions;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;

namespace Reclamation.Core
{
    /// <summary>
    /// TextFile is used to manage flat text files
    /// </summary>
    public class TextFile
    {
        List<string> lines;
        string filename;
        DateTime m_lastWriteTime;


        public DateTime LastWriteTime
        {
            get { return m_lastWriteTime; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="filename"></param>
        public TextFile(string filename)
        {
            this.filename = filename;
            FileInfo fi = new FileInfo(filename);
            m_lastWriteTime = fi.LastWriteTime;
            ReadFile(filename);
        }

        public void Append(string[] data)
        {
            lines.AddRange(data);
        }

        public void Append(string filename)
        {
            TextFile tf = new TextFile(filename);
            lines.AddRange(tf.lines);
        }


        /// <summary>
        /// Compares 2 text files and returns lines where differences occur.
        /// </summary>
        /// <param name="f1"></param>
        /// <param name="f2"></param>
        /// <returns></returns>
        public static string[] Compare(TextFile f1, TextFile f2)
        {
            int maxIndex = Math.Min(f1.lines.Count, f2.lines.Count);
            var rval = new List<string>();

            if (f1.lines.Count != f2.lines.Count)
                rval.Add("Length of files is different");

            
            for (int i = 0; i < maxIndex; i++)
            {
                string l1 = f1.lines[i];
                string l2 = f2.lines[i];
                if (l1 != l2)
                {
                    rval.Add("File 1: <" + l1+">  File 2:<"+l2+">");
                }
            }
            return rval.ToArray();
        }


        /// <summary>
        /// add data to end of file
        /// </summary>
        public void Add(string line)
        {
            lines.Add(line);
        }

        
        /// <summary>
        /// Reads text file. keeps lines that match the expression
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="expression"></param>
        public TextFile(string filename, string expression)
        {
            this.filename = filename;
            ReadFile(filename, expression);
        }
        /// <summary>
        /// retruns file data as array of strings
        /// </summary>
        public string[] FileData
        {
            get { return lines.ToArray(); }
        }
        /// <summary>
        /// number of lines in file
        /// </summary>
        public int Length
        {
            get { return lines.Count; }
        }
        /// <summary>
        /// constructor using array of string as input
        /// </summary>
        /// <param name="input"></param>
        public TextFile(string[] input)
        {
            this.filename = "untitled";
            this.lines = new List<string>();
            lines.AddRange(input);
            //this.lines = input;
        }

        public TextFile()
        {
            this.filename = "untitled";
            this.lines = new List<string>();
        }

        /// <summary>
        /// returns file as a single string
        /// </summary>
        public string FileContents
        {
            get
            {
                StreamReader srFromFile =
                    new StreamReader(filename);
                string rval = srFromFile.ReadToEnd();
                return rval;
            }

        }
        /// <summary>
        /// Splits line, at index, as space separated values
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public string[] Split(int index)
        {
            return Split(this[index]);
        }

        /// <summary>
        /// Splits string data as space separated
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string[] Split(string data)
        {
            return Regex.Split(data.Trim(), @"\s+");
        }

        /// <summary>
        /// Splits data by fixed widths 
        /// </summary>
        /// <returns></returns>
        public static string[] Split(string data,params int[] widths )
        {
            var rval = new List<string>();
            int idx =0;
            for (int i = 0; i < widths.Length; i++)
            {
                if (idx + widths[i] > data.Length)
                    break;
                rval.Add(data.Substring(idx,widths[i]));
                idx += widths[i];
            }

            return rval.ToArray();
        }



        /// <summary>
        /// filename provided to constructor
        /// </summary>
        public string FileName
        {
            get { return this.filename; }
        }
        /// <summary>
        /// indexer for line by index
        /// </summary>
        public string this[int index]
        {
            get { return lines[index]; }
        }

        void ReadFile(string filename)
        {
            ReadFile(filename, "");
        }
        void ReadFile(string filename, string expression)
        {

            lines = new List<string>();
            if (expression == "")
            {
                lines.AddRange(File.ReadAllLines(filename));
                return;
            }

            Regex re = new Regex(expression, RegexOptions.Compiled);

            this.filename = filename;
            StreamReader srFromFile =
                new StreamReader(filename);
            string strLine;
            while ((strLine = srFromFile.ReadLine()) != null)
            {

                if (re.IsMatch(strLine))
                {
                    lines.Add(strLine);
                }
            }
            srFromFile.Close();
        }


        /// <summary>
        /// searches for string starting 
        /// at the end of the file
        /// </summary>
        /// <param name="search"></param>
        /// <returns>index to matched data or -1 for failure</returns>
        public int LastIndexOf(string search)
        {
            return LastIndexOf(search, 0);
        }
        /// <summary>
        /// search for text starting at the end
        /// </summary>
        /// <param name="search"></param>
        /// <param name="startIndex"></param>
        /// <returns></returns>
        public int LastIndexOf(string search, int startIndex)
        {
            Debug.Assert(startIndex >= 0);
            Debug.Assert(startIndex < lines.Count);
            int sz = lines.Count;
            for (int i = sz - 1; i >= startIndex; i--) // search backwards through file.
            {
                if (lines[i].IndexOf(search) >= 0)
                    return i;
            }
            return -1;
        }

        /// <summary>
        /// Returns index in file to search string
        /// </summary>
        public int IndexOf(string search)
        {
            return IndexOf(search, 0);
        }

        /// <summary>
        /// Finds index to the count copy of search string.  
        /// for example: find the 12th line that contains 'ABC'
        ///       NthIndex("ABC",12);
        /// </summary>
        /// <param name="search">search string</param>
        /// <param name="count">instance of search string</param>
        /// <returns></returns>
        public int NthIndexOf(string search, int count)
        {
            int idx = -1;
            for (int i = 1; i <= count; i++)
            {
                idx = IndexOf(search, ++idx);
                if (idx == -1)
                    return -1;
                
            }
            return idx;

        }

        /// <summary>
        /// Returns index in file to search string
        /// </summary>
        /// <param name="search"></param>
        /// <param name="startIndex"></param>
        /// <returns></returns>
        public int IndexOfAny(string[] search, int startIndex)
        {
            Debug.Assert(startIndex >= 0);
            Debug.Assert(startIndex < lines.Count);
            int sz = lines.Count;
            for (int i = startIndex; i < sz; i++)
            {
                for (int j = 0; j < search.Length; j++)
                {
                    if (lines[i].IndexOf(search[j]) >= 0)
                        return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// Returns index in file to search string
        /// </summary>
        /// <param name="search"></param>
        /// <param name="startIndex"></param>
        /// <returns></returns>
        public int IndexOf(string search, int startIndex)
        {
            Debug.Assert(startIndex >= 0);
            int sz = lines.Count;
            for (int i = startIndex; i < sz; i++)
            {
                if (lines[i].IndexOf(search) >= 0)
                    return i;
            }
            return -1;
        }


        /// <summary>
        /// Returns index in file using regular expression 
        /// </summary>
        public int IndexOfRegex(string pattern)
        {
            return IndexOfRegex(pattern, 0);
        }

        /// <summary>
        /// Returns index in file using regular expression 
        /// </summary>
        public int IndexOfRegex(string pattern, int startIndex)
        {
            Regex re = new Regex(pattern, RegexOptions.Compiled);//,RegexOptions.Multiline);
            return IndexOfRegex(re, startIndex);
        }

        
        private int IndexOfRegex(Regex re, int startIndex)
        {
            Debug.Assert(startIndex >= 0);
            Debug.Assert(startIndex < lines.Count);
            int sz = lines.Count;
            for (int i = startIndex; i < sz; i++)
            {
                if (re.IsMatch(lines[i]))
                    return i;
            }
            return -1;
        }

        /// <summary>
        /// search for two consecetive lines that match search Regex
        /// </summary>
        public int IndexOfBothRegex(string expression1,
            string expression2)
        {
            return IndexOfBothRegex(expression1, expression2, 0);
        }
        /// <summary>
        /// search for two consecetive lines that match search Regex
        /// </summary>
        public int IndexOfBothRegex(string expression1, 
            string expression2, int startingIndex)
        {
            Regex re1 = new Regex(expression1, RegexOptions.Compiled);
            Regex re2 = new Regex(expression2, RegexOptions.Compiled);
            int i = startingIndex;
            while (i < Length)
            {
                i = IndexOfRegex(re1, i);
                if (i < 0 || i == Length-1)
                {
                    return -1;
                }
                else
                {
                    if ( re2.IsMatch( this[i + 1]))
                    {
                        return i;
                    }
                }

                i++;
            }
            return -1;
        }





        /// <summary>
        /// search for two consecetive lines that match search strings
        /// </summary>
        public int IndexOfBoth(string line1, string line2, int startingIndex)
        {
            int i = startingIndex;
            while (i < Length)
            {
                i = IndexOf(line1, i);
                if (i < 0)
                {
                    return -1;
                }
                else
                {
                    if (this[i + 1].IndexOf(line2) >= 0)
                    {
                        return i;
                    }
                }

                i++;
            }
            return -1;
        }
        /// <summary>
        /// Reads a DateTime from the file
        /// example:  ReadDate("LaunchTime:")
        /// example file below
        /// --------------
        /// LaunchTime:1/2/2003
        /// </summary>
        /// <param name="label"></param>
        /// <returns></returns>
        public DateTime ReadDate(string label)
        {
            DateTime rval;
            string s = ParseLine(label);
            try
            {
                rval = Convert.ToDateTime(s);
            }
            catch (Exception ex)
            {
                Console.WriteLine("TextFile.ReadDate() Error converting '" + label + "' to a DateTime ");
                throw ex;
            }
            return rval;
        }

        /// <summary>
        /// Reads a integer from the file
        /// example:  ReadInt("SecondsElapsed:")
        /// example file below
        /// --------------
        /// SecondsElapsed:863
        /// </summary>
        /// <param name="label"></param>
        /// <returns></returns>
        public int ReadInt(string label)
        {
            int rval;
            string s = ParseLine(label);
            try
            {
                rval = Convert.ToInt32(s);
            }
            catch (Exception ex)
            {
                Console.WriteLine("TextFile.ReadInt() Error converting '" + label + "' to a integer ");
                throw ex;
            }
            return rval;
        }
        /// <summary>
        /// Reads float from file
        /// example:  ReadSingle("SecondsElapsed:")
        /// example file below
        /// --------------
        /// SecondsElapsed:863.5
        /// </summary>
        /// <param name="label"></param>
        /// <returns></returns>
        public float ReadSingle(string label)
        {
            float rval;
            string s = ParseLine(label);
            try
            {
                rval = Convert.ToSingle(s);
            }
            catch (Exception ex)
            {
                Console.WriteLine("TextFile.ReadSingle() Error converting '" + label + "' to a integer ");
                throw ex;
            }
            return rval;
        }



        public string ParseLine(string label)
        {
            return ParseLine(label, 0, this.Length - 1);
        }
        /// <summary>
        /// Extracts data from AquatCalc text file.
        /// Example:  ParseLine("GAGE ID#")
        /// 
        /// looks for line that contains the label "GAGE ID#"
        /// then returns the value to the right of the label "GAGE ID#"
        /// </summary>
        /// <param name="label">text prefix to data. search for this label</param>
        /// <returns></returns>
        public string ParseLine(string label, int startIndex, int endIndex)
        {
            TextFile txtFile = this;
            int rowIndex = txtFile.IndexOf(label,startIndex);
            if (rowIndex < 0)
                return "";

            string line = txtFile[rowIndex];
            int dataIndex = line.IndexOf(label);
            Debug.Assert(dataIndex >= 0, "Error parsing file " + txtFile.FileName + " at  line " + rowIndex);

            dataIndex += label.Length;
            string data = line.Substring(dataIndex).Trim();
            return data;
        }

        public void SaveAsVms(string filename)
        {
            
            StreamWriter sw = new StreamWriter(filename);
            //sw.NewLine = "\r";
            for (int i = 0; i < lines.Count; i++)
            {
                sw.Write(lines[i]);
                sw.Write("\n");
            }
            sw.Close();
        }

        public void SaveAs(string filename)
        {
            StreamWriter sw = new StreamWriter(filename);
            for (int i = 0; i < lines.Count; i++)
            {
                sw.WriteLine(lines[i]);
            }
            sw.Close();
        }


        public void Replace(string pattern, string replace)
        {
            for (int i = 0; i < lines.Count; i++)
            {
                if (Regex.IsMatch(lines[i], pattern))
                {
                    lines[i] = Regex.Replace(lines[i], pattern, replace);
                    break;
                }
            }
        }

        public void ReplaceAll(string pattern, string replace)
        {
            for (int i = 0; i < lines.Count; i++)
            {
               lines[i] = Regex.Replace(lines[i], pattern, replace);
            }       
        }

        public void DeleteLines(int idx1, int idx2)
        {
            lines.RemoveRange(idx1, idx2 - idx1+1);
        }

        public void DeleteLine(int idx1)
        {
            lines.RemoveAt(idx1);
        }
    }
}

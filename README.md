Pisces   June 24, 2014
======

Pisces is a desktop application that graphs and analyzes time series data. Pisces is designed to organize, graph, and analyze natural resource data that varies with time: gauge height, river flow, water temperature, etc. 

Download Pisces and user manual here: http://www.usbr.gov/pn/hydromet/pisces
 
The Pisces time series database is designed to be fast and simple.  The default database engine is Sqlite http://www.sqlite.com/
However, Pisces also supports postgresql, SqlServer, and SqlCompact.

The key libraries  (Reclamation.Core.dll and Reclamation.TimeSeries.dll) work under Windows or Linux/mono.  
 
Hydrologist, Engineers (especially modelers), and programmers have used these Pisces libraries to manage large amounts of time series data with ease. The main componet in the library called Series can be used without any database if desired.

Here is an example in C# that finds the minimum and maximum temperature each day using spreadsheet data collected at a 30 minute interval

     [Test]
        public void MaxMin()
        { 
            string fn = TestData.DataPath + "\\temp example 7 day max.xls";
            var s = new ExcelDataReaderSeries(fn, "457373", "C", "D");
            s.Read();

            Series max = Math.DailyMax(s);
            max.WriteCsv(@"c:\temp\a.csv");
            Assert.AreEqual(7, max[0].DateTime.Day);
            Assert.AreEqual(14.68, max[0].Value, 0.01);
            Assert.AreEqual(17.21, max["8/5/2004"].Value, 0.01);
            Assert.AreEqual(1965.0, max[max.Count - 1].Value, .001);

            Series min = Math.DailyMin(s);

            Assert.AreEqual(7, min[0].DateTime.Day);
            Assert.AreEqual(12.98, min[0].Value, 0.01);
            Assert.AreEqual(15.31, min["8/5/2004"].Value, 0.01);

            Assert.AreEqual(1965.0, min[min.Count - 1].Value, 0.001);
        }

-- Programmer Help Wanted --
The directory Pisces_open compiles with just open source libraries.  Howerver, some functionality is limited.
* ZedGraph support is very limited. Several graphs don't yet work with ZedGraph.  
* HEC-DSS files load slow using the command line DSSUTIL.exe.  A more tightly integrated method would likely be faster.
* An open source Spreadsheet component is needed. (excelreader, or npoi)?

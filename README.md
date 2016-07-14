Linux
[![Pisces Build](https://api.travis-ci.org/usbr/Pisces.svg)](https://travis-ci.org/usbr/Pisces)
Windows
[![Build status](https://ci.appveyor.com/api/projects/status/vrtk5m141gfrb6gt?svg=true)](https://ci.appveyor.com/project/ktarbet/pisces)

Pisces   
======

Pisces is a time series database including a desktop application that graphs and analyzes time series data. Pisces is designed to organize, graph, and analyze natural resource data that varies with time: gauge height, river flow, water temperature, etc. 

![Pisces Example](https://github.com/usbr/Pisces/blob/master/Doc/pisces.png)

Download Pisces and user manual here: http://www.usbr.gov/pn/hydromet/pisces
See a bulletin here https://www.usbr.gov/research/docs/updates/pre-2012/27-pisces.pdf
 
The Pisces time series database is designed to be fast and simple.  The default database engine is Sqlite http://www.sqlite.com/
However, Pisces also supports postgresql, MySql, SqlServer, and ~~SqlCompact~~.

The key programs and assemblies  (HydrometServer.exe, Reclamation.Core.dll and Reclamation.TimeSeries.dll) work under Windows or Linux/mono.  
 
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


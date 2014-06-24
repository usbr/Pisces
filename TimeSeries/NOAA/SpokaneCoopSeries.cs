using System;
using System.Collections.Generic;
using System.Text;
using Reclamation.Core;
using System.Text.RegularExpressions;

namespace Reclamation.TimeSeries.NOAA
{

    public class SpokaneCoopSeries:Series
    {
        //http://www.wrh.noaa.gov/otx/climate/coop/get_coop.php?form_test=true&station=Ritzville&monthname=December&ob_norm=Observed+Data&year=2007&.submit=Submit+Query
        //http://www.wrh.noaa.gov/otx/climate/coop/get_coop.php?form_test=true&station=Ephrata&monthname=November&ob_norm=Observed+Data&year=2007&.submit=Submit+Query
        //http://www.wrh.noaa.gov/otx/climate/coop/get_coop.php?form_test=true&station=Moses_Lake&monthname=April&ob_norm=Observed+Data&year=2007&.submit=Submit+Query
        //http://www.wrh.noaa.gov/otx/climate/coop/get_coop.php?form_test=true&station=Davenport&monthname=April&ob_norm=Observed+Data&year=2007&.submit=Submit+Query

        public SpokaneCoopSeries()
        {

        }
        string m_station;
        string m_parameter;
        public SpokaneCoopSeries(string station, string parameter)
        {
            m_parameter = parameter;
            m_station = station;
            Name = m_station + " " + m_parameter;
        }

        protected override void ReadCore(DateTime t1, DateTime t2)
        {
            DateTime t = t1;
            while (t <= t2)
            {
                Series s = ReadMonth(t);
                Logger.WriteLine("found " + s.Count + " values ");
                for (int i = 0; i < s.Count; i++)
                {
                    Point pt = s[i];
                    if( pt.BoundedBy(t1,t2))
                        Add(pt);
                }
                t = t.AddMonths(1);
            }
 
        }

        private Series ReadMonth(DateTime t)
        {
            t = new DateTime(t.Year, t.Month, 1);
            string url = "http://www.wrh.noaa.gov/otx/climate/coop/get_coop.php?form_test=true&station=Davenport&monthname=April&ob_norm=Observed+Data&year=2007&.submit=Submit+Query";
            url = url.Replace("&station=Davenport&", "&station=" + m_station + "&");
                url = url.Replace("&monthname=April&", "&monthname=" + t.ToString("MMMM") + "&");
                url = url.Replace("&year=2007&", "&year="+t.Year+"&");
          
            Series s = new Series();
            
            TextFile tf = new TextFile(Web.GetPage(url,true));
          //  tf.SaveAs(@"C:\temp\test.txt");
            //[472] = "<font face=\"Arial, Helvetica, sans-serif\"><h3><b>Observed Data for Davenport November 2005</b></h3></font>"

            string spaceSeparatedStation = m_station.Replace("_", " ");
            string findMe = "Observed Data for " + spaceSeparatedStation + " " + t.ToString("MMMM") + " " + t.Year;
           int idx = tf.IndexOf(findMe);
           if( idx <0)
           {
               Logger.WriteLine("No data found matching '" + findMe + "'");
                   return s;
           }
           string header = tf[idx + 1];
           header = header.Replace("<b>", "");
           header = header.Replace("</b>", "");
           string[] columnNames = Regex.Split(header, @"\s+");

           int idxParameter = Array.IndexOf(columnNames, m_parameter);
           if (idxParameter < 0)
           {
               throw new Exception("could not find parameter '" + m_parameter+"'"
                   + "\nin "+tf[idx+1]);
           }

           for (int i = idx + 2; i < tf.Length; i++)
           {
               if (tf[i].IndexOf("___________") == 0)
               {
                   break; // done
               }
             string[] tokens = Regex.Split(tf[i].Replace("<b>", "").Trim().Replace("</b>", ""), @"\s+");
             if (tokens.Length <= idxParameter)
             {
                 Logger.WriteLine("Error: wrong number of columns?");
                 break;

             }
             int day = -1;
             if (! int.TryParse(tokens[0], out day))
             {
                 Logger.WriteLine("Error:invalid day at "+tf[i]);
                 break;
             }

             if (day != t.Day)
             {
                 Logger.WriteLine("Dates out of sync..");
                 break;
             }
             string strValue = tokens[idxParameter];
             double value = Point.MissingValueFlag;

             if (strValue.IndexOf("T") >= 0)
             {
                 s.Add(t, 0);
             }

             else if (double.TryParse(strValue, out value))
             {
                 s.Add(t, value);
             }
             else
             {
                 s.Add(t, Point.MissingValueFlag);
             }
             t = t.AddDays(1);
           
           }


           return s;
        }
        /* interesting result... shows underlying program.
        <pre>
<font face="Arial, Helvetica, sans-serif"><h3><b>Observed Data for  October 2005</b></h3></font>
<b>Day	Max	Min	Precip	Snow	Depth</b>
<b> 1</b>	Usage:	climRead	   -p	PARAM1	[PARAM2
<b> 2</b>	[-b	MN/DY/YEAR]	     	    	    
<b> 3</b>	[-e	MN/DY/YEAR]	     	    	    
<b> 4</b>	[-f	FLDSEP1	[FLDSEP2	..]]	    
<b> 5</b>	[-F	FMT1	[FMT2	..]]	    
<b> 6</b>	[-M	MSG1	[MSG2	..]]	    
<b> 7</b>	[-a	FACTOR1	[FACTOR2	..]]	    
<b> 8</b>	[-m	FACTOR1	[FACTOR2	..]]	    
<b> 9</b>	[-yr]	   	     	    	    
<b>10</b>	[-T]	   	     	    	    
<b>11</b>	[-total]	   	     	    	    
<b>12</b>	[-missings	[N]]	     	    	    
<b>13</b>	[-nonl]	   	     	    	    
<b>14</b>	[-q]	   	     	    	  
*/
        /*
         <font face="Arial, Helvetica, sans-serif"><h3><b>Observed Data for Ritzville December 2007</b></h3></font>
<b>Day	Max	Min	Precip	Snow	Depth</b>
<b> 1</b>	 27	 18	 0.00	 0.0	   1
<b> 2</b>	 34	 20	 0.04	 1.0	   2
<b> 3</b>	 45	 32	 0.48	 0.0	   0
<b> 4</b>	 51	 44	 0.14	 0.0	   0
<b> 5</b>	 55	 34	 0.00	 0.0	   0
<b> 6</b>	 44	 25	 0.00	 0.0	   0
<b> 7</b>	 34	 26	 0.00	 0.0	   0
<b> 8</b>	 38	 25	 0.00	 0.0	   0
<b> 9</b>	 35	 17	 0.00	 0.0	   0
<b>10</b>	 26	 18	 0.03	 0.5	   1
<b>11</b>	 26	 15	 0.00	 0.0	   1
<b>12</b>	  M	  M	    M	   M	   M
<b>13</b>	  M	  M	    M	   M	   M
<b>14</b>	  M	  M	    M	   M	   M
<b>15</b>	  M	  M	    M	   M	   M
<b>16</b>	  M	  M	    M	   M	   M
<b>17</b>	  M	  M	    M	   M	   M
<b>18</b>	  M	  M	    M	   M	   M
<b>19</b>	  M	  M	    M	   M	   M
<b>20</b>	  M	  M	    M	   M	   M
<b>21</b>	  M	  M	    M	   M	   M
<b>22</b>	  M	  M	    M	   M	   M
<b>23</b>	  M	  M	    M	   M	   M
<b>24</b>	  M	  M	    M	   M	   M
<b>25</b>	  M	  M	    M	   M	   M
<b>26</b>	  M	  M	    M	   M	   M
<b>27</b>	  M	  M	    M	   M	   M
<b>28</b>	  M	  M	    M	   M	   M
<b>29</b>	  M	  M	    M	   M	   M
<b>30</b>	  M	  M	    M	   M	   M
<b>31</b>	  M	  M	    M	   M	   M
______________________________________________________
<b>Avg/Tot</b>	37.7	24.9	 0.69	 1.5
         */
    }
}

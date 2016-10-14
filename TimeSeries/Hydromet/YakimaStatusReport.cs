using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Reclamation.TimeSeries.Hydromet
{
    public class YakimaStatusReport
    {
        TimeSeriesDatabase m_db;
        public YakimaStatusReport(TimeSeriesDatabase db)
        {
            m_db = db;
        }

        /// <summary>
        /// Creates a text file report and returns path to it.
        /// </summary>
        /// <returns></returns>
        public string Create(DateTime t, int hour=8) // default 8am.
        {
            string rval = "";
            HydrometInstantSeries.Cache.Add(this.yakima_data, t.AddDays(-1), t, HydrometHost.Yakima, TimeInterval.Irregular);


            return rval;

        }
        //DATA CAPS/157800.,239000.,436900.,33970.,198000.,        11065400./
        /*
         * AF(1)=KEE STORAGE,AF(2)=KAC STORAGE,AF(3)=CLE STORAGE
C               AF(4)=BUM STORAGE,AF(5)=RIM STORAGE
         */
        private string template = @"
            13-OCT-2016  09:12:35    US BUREAU OF RECLAMATION
                                   YAKIMA PROJECT
                               SYSTEM STATUS AT 06:00

                 FOREBAY                TOTAL     PERCENT   RESERVOIR RESERVOIR
     RESERVOIR  ELEVATION   CONTENT    CAPACITY   CAPACITY     INFLOW  RELEASES
     --------------------------------------------------------------------------
                      FB         AF          AF         %         CFS       CFS
     Keechelus   2448.25     29925.     157800.        19.       142.      102.
     Kachess     2223.12     85518.     239000.        36.       159.      847.
     Cle Elum    2140.38     65109.     436900.        15.       341.      223.
     Bumping     3403.84     10232.      33970.        30.       147.      138.
     Rimrock     2849.14     53502.     198000.        27.       230.      368.
     Clear Cr    3011.31
     TOTALS                 244286.    1065400.        23.      1019.     1677.

     IRRIGATION DIVERSIONS                    RIVER FLOWS
     ---------------------    CFS             -----------                  CFS
     Kittitas                712.
     Roza                    430.             Yakima River at Easton       245.
     Yakima-Tieton            20.             Teanaway River at Forks       46.
     Wapato                  717.             Yakima River near Horlick    796.
     Sunnyside               575.             Yakima River near Umtanum   1463.
     MAJOR USERS TOTAL      2453.             Yakima River blw Roza Dam   1012.
                                              Naches River nr. Clf'Dell    264.
     Westside                 33.             Tieton Rvr belw Cnl Hdwks    420.
     Naches-Selah            113.             Naches River near Naches     585.
     OTHERS ABOVE PARKER     584.             Yakima River near Parker     478.
                                              Yakima River near Prosser    792.
     TOTAL ABOVE PARKER     3037.
     ----------------------------
     Kennewick               174.

     OTHER CANAL DIVERSIONS
     Wapatox                  57.
     Roza at Headworks       438.  Gate tucked, 1.5 panels down.
     Chandler               1258.  Generating 6.2 MW.

     UNREGULATED TRIBUTARY & RETURN FLOW ABOVE PARKER     -  -   1837. CFS
            ";

        private string[] yakima_data =
            new string[]{
"KEE FB",
"KAC FB",
"CLE FB",
"BUM FB",
"RIM FB",
"CLR FB",
"KEE AF",
"KAC AF",
"CLE AF",
"BUM AF",
"RIM AF",
"EASW Q",
"YUMW Q",
"UMTW Q",
"NACW Q",
"PARW Q",
"YRPW Q",
"KEE Q",
"KAC Q",
"CLE Q",
"BUM Q",
"RIM Q",
"KTCW QC",
"WOPW QC",
"RZCW QC",
"NSCW QC",
"SNCW QC",
"RSCW QC",
"TIEW QC",
"ROZW QC",
"WESW QC",
"CHCW QC",
"KNCW QC",
"TNAW Q",
"YRWW Q",
"CLFW Q",
"TICW Q",
"RBDW Q",
"YRCW Q"            };

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using Reclamation.Core;
using Reclamation.TimeSeries.Hydromet;
using System.Data;
namespace Reclamation.TimeSeries.Decodes
{
    class McfDecodesConverter
    {

//        static string s_datatypeStandard = "SHEF-PE"; 
        static string s_datatypeStandard = "CBTT"; 

        PostgreSQL svr;
        McfDataSet mcf;
        DecodesDataSet decodes;
        UnitConversion m_converter;
        int platformID;
     public McfDecodesConverter(PostgreSQL svr, McfDataSet mcf, DecodesDataSet decodes)
        {
            this.svr = svr;
            this.mcf = mcf;
            this.decodes = decodes;
            m_converter = new UnitConversion(mcf, decodes,svr);
        }


    public void importMcf(string[] siteFilter,string networkListName){
      //  DecodesUtility.TruncateTables(svr);
        //DecodesUtility.AddEquipment(decodes);
        
        int networklistid = SetupNetworkList(networkListName);
        InsertSite(siteFilter);
        platformID = NextID(svr, "platform");
        InsertGoesPlatform(siteFilter, networklistid);
        DecodesUtility.UpdateServer(svr, decodes);
        
        svr.Vacuum();
    }


   
    private void InsertGoesPlatform(string[] siteFilter,int networklistid)
    {

        for (int i = 0; i < mcf.sitemcf.Rows.Count; i++)
        {
           McfDataSet.sitemcfRow s = mcf.sitemcf[i];

           if (siteFilter.Length >0  &&  Array.IndexOf(siteFilter, s.SITE.Trim()) <0)
               continue;


            string nessid = s.NESSID.Trim();

           if (s.TTYPE != "SAT" || s.MASTERSW == 0 || nessid == "" || nessid == "0" 
               ||  decodes.transportmedium.Select("mediumid='"+nessid+"'").Length>0  || s.PCHAN == 199) 
           {
               Console.WriteLine("Skipping adding existing platform "+s.SITE+" " +s.NESSID);
               continue;
           }

               string cfgName = "";//
               string platformDesignator = "";
               string platformDescription = "";
               string parameterFilter = "";


               List<McfDataSet.sitemcfRow> sitesAtTransmitter =
                   new List<McfDataSet.sitemcfRow>(from row in mcf.sitemcf.AsEnumerable()
                                                where row.NESSID == nessid
                                                //&& row.NESSID != ""
                                                //&& row.NESSID != "0" 
                                                orderby row.SITE
                                                select row);

               
                   for (int j = 0; j < sitesAtTransmitter.Count; j++)
                   {
                       //if (transmitters.Count > 1)
                         //  Logger.WriteLine(transmitters[j].SITE);
                       if( cfgName.Length >0)
                           cfgName +="-";
                       cfgName += "" + sitesAtTransmitter[j].SITE.Trim();

                       if (platformDesignator.Length > 0)
                           platformDesignator += "-";
                       platformDesignator += sitesAtTransmitter[j].SITE.Trim();

                       if (platformDescription.Length > 0)
                           platformDescription += "\n   ";
                       platformDescription += sitesAtTransmitter[j].DESCR;

                       if (parameterFilter.Length > 0)
                           parameterFilter += " or ";
                       parameterFilter += " GOESDEC like '"+sitesAtTransmitter[j].SITE.PadRight(8)+"%' ";
                   }
                   //cfgName +="-"+ConfigName(s);


                var pc = decodes.platformconfig;
                var p = decodes.platform;
               // build Platform and Platform Config together.

                int siteID = LookupSiteIDFromCBTT(s.SITE.Trim());

                 
                pc.AddplatformconfigRow(platformID, cfgName, s.DESCR, GetManufactureCode( s.DCPMFG ));
                p.AddplatformRow(platformID, s.GetAgencyName(), "TRUE", siteID, platformID, platformDescription, DateTime.Now,
                       DateTime.Now.AddYears(100).Date, platformDesignator);

                decodes.transportmedium.AddtransportmediumRow(platformID, "goes-self-timed", nessid, "ST",
                            s.PCHAN,s.REPTIM,s.TSSIZ,s.REPINT*60,GetManufactureCode(s.DCPMFG),
                             0,"S", "UTC");

                decodes.networklistentry.AddnetworklistentryRow(networklistid, nessid);

                decodes.platformproperty.AddplatformpropertyRow(platformID, "HydrometGroup", getHydrometCatagoryName(s));

                if( s.IsIdahoPower)
                    decodes.platformproperty.AddplatformpropertyRow(platformID, "IdahoPower", "True");
                if (s.IsUsgs)
                    decodes.platformproperty.AddplatformpropertyRow(platformID, "USGS", "True");
                if( s.IsOwrd)
                    decodes.platformproperty.AddplatformpropertyRow(platformID, "OWRD", "True");

                var parameters = (McfDataSet.goesmcfRow[])mcf.goesmcf.Select(parameterFilter+" and ACTIVE = 1");

                //Logger.WriteLine(parameters.Length.ToString() + " parameters at " + transmitters.Count + " SITE");

                int configId = platformID;
                int decodesScriptID = configId;
                string dataOrder = "D";
               if( s.DCPMFG == "A")
                   dataOrder = "A";

                decodes.decodesscript.AdddecodesscriptRow(decodesScriptID, configId, "ST", "Decodes", dataOrder);

                BuildDecodesScripts(sitesAtTransmitter.ToArray(), siteID, parameters, configId, decodesScriptID);
                InsertBatteryVoltScript(configId, decodesScriptID,  parameters.Length + 1, s);
                m_converter.AddBatteryVoltConverter(decodesScriptID, parameters.Length + 1, s);
                platformID++;

           }
        }

    private void BuildDecodesScripts(McfDataSet.sitemcfRow[] sitesAtTransmitter, int siteID, McfDataSet.goesmcfRow[] parameters, int configId, int decodesScriptID)
    {
        var s = sitesAtTransmitter[0];
        for (int idx = 0; idx < parameters.Length; idx++)
        {
            var goes = parameters[idx];


            string parameterName = goes.GOESDEC.Substring(7).Trim();
            string siteName = goes.GOESDEC.Substring(0, 8).Trim();
            double max, min;
            string recordingMode = "F";
            int sensorNumber = idx + 1;
            int recordingInterval = System.Math.Abs(parameters[idx].S_TBV * 60);
            int datatypeID = GetDataTypeID(goes);

            if (goes.ACTIVE == 0)
                continue;

            m_converter.GetMaxMin(goes, out min, out max);
            var senosrName = parameterName;

            if (sitesAtTransmitter.Length > 1)
                senosrName = siteName + "_" + parameterName;

            decodes.configsensor.AddconfigsensorRow(configId, sensorNumber, senosrName, recordingMode,
                         recordingInterval, 0, 0, min, max, "");
            decodes.configsensordatatype.AddconfigsensordatatypeRow(configId, sensorNumber, datatypeID);
            // if actual site is different than platform create entry in platformsensor
            int actualSiteID = LookupSiteIDFromCBTT(siteName);
            
            DataRow row = decodes.platformsensor.NewplatformsensorRow();
            row["platformid"] = platformID;
            row["sensornumber"] = sensorNumber;

            if (actualSiteID != siteID)
                row["siteid"] = actualSiteID;

            //if (s.IsIdahoPower && parameterName == "GH" || parameterName == "CH")
            //{
            //    decodes.platformsensor.Rows.Add(row);
            //    decodes.platformsensorproperty.AddplatformsensorpropertyRow(platformID, sensorNumber, "TabShef", "Q");
            //    decodes.platformsensorproperty.AddplatformsensorpropertyRow(platformID, sensorNumber, "TabName", siteName.ToLower());
            //    decodes.platformsensorproperty.AddplatformsensorpropertyRow(platformID, sensorNumber, "TabFile", siteName.ToLower() + ".csv");
            //    decodes.platformsensorproperty.AddplatformsensorpropertyRow(platformID, sensorNumber, "TabEU", "cfs");
            //}else  if (s.IsUsgs && parameterName == "GH")
            //{
            //    decodes.platformsensor.Rows.Add(row);
            //    decodes.platformsensorproperty.AddplatformsensorpropertyRow(platformID, sensorNumber, "RdbShef", "Q");
            //    decodes.platformsensorproperty.AddplatformsensorpropertyRow(platformID, sensorNumber, "RdbFile", siteName.ToLower() + ".rdb");
            //}
            //else
            if( actualSiteID != siteID)
                decodes.platformsensor.Rows.Add(row);
            
            
            string dataType = LookupGoesDataFormat(goes.S_DTFMT, s);



            string script = "";
            if (goes.S_NV <= 0)
            {
                Console.WriteLine("Warning: zero values to decode.");
            }
            else
            {

                script = CreateScript(goes, sensorNumber, dataType);
            }

            // we tried using a different decoding label besides ST and It didn't seem to work.
            decodes.formatstatement.AddformatstatementRow(decodesScriptID, sensorNumber, "ST", script);

            m_converter.AddUnitConverter(decodesScriptID, sensorNumber, goes, s);

        }
    }

        /// <summary>
        /// Create Decoding Script 
        /// </summary>
        /// <param name="goes"></param>
        /// <param name="sensorNumber"></param>
        /// <param name="dataType"></param>
        /// <param name="script"></param>
        /// <returns></returns>
    private static string CreateScript(McfDataSet.goesmcfRow goes, int sensorNumber,  string dataType)
    {
        string script = "1p," + goes.S_OFV + "x,";

        int spaceBetween = goes.S_ONV - goes.S_DTSIZE;

        if (spaceBetween == 0)
        {
            script += goes.S_NV.ToString() + "(f(s," + dataType + "," + goes.S_DTSIZE + "," + sensorNumber + "))";
        }
        else
        {
            if (goes.S_NV > 1)
                script += (goes.S_NV - 1).ToString() + "(f(s," + dataType + "," + goes.S_DTSIZE + "," + sensorNumber + ")," + spaceBetween + "x),";

            script += "f(s," + dataType + "," + goes.S_DTSIZE + "," + sensorNumber + ")";

            if (goes.S_NV == 1)
                script = "1p," + goes.S_OFV + "x,f(s," + dataType + "," + goes.S_DTSIZE + "," + sensorNumber + ")";
        }
        return script;
    }

   
    private void InsertBatteryVoltScript(int configId, int decodesScriptID, int sensorNumber, McfDataSet.sitemcfRow s)
    {
        int size = 1;
        int batteryOffset = 0;
        string dataType = "b";
        if (s.DCPMFG == "A")
        {
            size = 3;
            batteryOffset = s.SMSGLEN - 4;
            dataType = "bc";
        }
        else
        {
            batteryOffset = s.SMSGLEN - 2;
        }
        string script = "1p," + batteryOffset + "x,f(s," + dataType + "," + size + "," + sensorNumber + ")";
        decodes.formatstatement.AddformatstatementRow(decodesScriptID, sensorNumber, "ST", script);
        decodes.configsensor.AddconfigsensorRow(configId, sensorNumber,"BATVOLT", "F", 60 * 60,
                     0, 0, -.1, 99999999, "");
        decodes.configsensordatatype.AddconfigsensordatatypeRow(configId, sensorNumber, GetDataTypeID("BATVOLT"));

    }

    private string LookupGoesDataFormat(string fmt, McfDataSet.sitemcfRow s)
    {
        string rval = "I";
        

        //s.DCPMFG

        if( fmt ==   "X")
            rval = "bc";

        if( fmt == "S")
                rval = "bd"; // design analysis
        if (fmt == "T")
            rval = "I";

          if( fmt == "B")
             rval = "b";
         
        if (fmt == "F")
            rval = "A";

        return rval;
    }




        /// <summary>
        /// Gets id from datatype table for this pcode.
        /// if necesssary create entry in datatype table
        /// </summary>
        /// <param name="goes"></param>
        /// <returns></returns>
    private int GetDataTypeID(McfDataSet.goesmcfRow goes)
    {
       string pcode = goes.GOESDEC.Substring(7).Trim();

       return GetDataTypeID(pcode);
    }

    private int GetDataTypeID(string pcode)
    {
        var datatypes = (DecodesDataSet.datatypeRow[])decodes.datatype.Select("standard = '" + s_datatypeStandard + "' and code = '" + pcode + "'");

        if (datatypes.Length > 0)
        {
            return datatypes[0].id;
        }
        else
        {// create new datatype, with corresponding id.
            int id;
            if (decodes.datatype.Rows.Count == 0)
                id = 1;
            else
            {
                id = decodes.datatype.Max(u => u.id) + 1;
            }
            decodes.datatype.AdddatatypeRow(id, s_datatypeStandard, pcode);
            return id;
        }
    }


    /// <summary>
    /// find site name based on cbtt
    /// </summary>
    /// <param name="p"></param>
    /// <returns></returns>
    private int LookupSiteIDFromCBTT(string cbtt)
    {
     DecodesDataSet.sitenameRow[] rows =
         (DecodesDataSet.sitenameRow[] )
        decodes.sitename.Select("nametype ='cbtt' and sitename = '"+cbtt +"'");

     if (rows.Length == 1)
         return rows[0].siteid;

     if (rows.Length > 1)
     {
         Logger.WriteLine(rows.Length.ToString() +" matches for cbtt site name '" + cbtt + "'");
         return 0;
     }

     Logger.WriteLine("Error: could not find site '" + cbtt + "'");
     return 0;
    }

    private string ConfigName(McfDataSet.sitemcfRow s)
    {
        string rval = "unknown";

        if (s.DCPMFG == "S" && s.REPINT == 60
            && s.SCHAN != 42 && s.SCHAN != 54)
            rval = "SutronSatlink";

        if (s.DCPMFG == "S")
            rval = "Sutron8xxx";
        if (s.DCPMFG == "D")
            rval = "DesignAnalysis";
        
        if (s.DCPMFG == "V")
            rval = "VitelVX1004";

        if (s.DCPMFG == "A")
            rval = "FTSTX312";


        return rval + "-pnusbr";
    }


//       SetupNetworkList();



    private void InsertSite(string[] siteFilter)
    {
        int id = NextID(svr, "site");
        int count = 0;
        for (int i = 0; i < mcf.sitemcf.Rows.Count; i++)
        {

            Reclamation.TimeSeries.Hydromet.McfDataSet.sitemcfRow s = mcf.sitemcf[i];


            if (siteFilter.Length > 0 && Array.IndexOf(siteFilter, s.SITE.Trim()) < 0)
                continue;

            var existing = decodes.site.Select("description = '" + s.DESCR + "'");
            if (existing.Length > 0)
            {
                string msg = "Site "+s.SITE+" " +s.DESCR+ " Allready in DECODES ";
                Logger.WriteLine(msg) ;
                throw new Exception(msg);
                //continue;
            }
            
            decodes.site.AddsiteRow(id, s.LAT.ToString(), s.LONG.ToString(), "",
                s.STATE, "", s.TimeZone(s.TZONE.ToString()),
                "USA", s.ELEV, "feet", s.DESCR);

            decodes.site_property.Addsite_propertyRow(id, "accessControlList",s.ACL);
            decodes.site_property.Addsite_propertyRow(id, "alternateID", s.ALTID);
            decodes.site_property.Addsite_propertyRow(id, "catagoryID", s.CATID);
            decodes.site_property.Addsite_propertyRow(id, "stationType", s.STATYPE);
            decodes.site_property.Addsite_propertyRow(id, "group", s.GRP.ToString()) ;
            decodes.site_property.Addsite_propertyRow(id, "telemetryType", s.TTYPE);
            decodes.site_property.Addsite_propertyRow(id, "timeZone", s.TTYPE);
            decodes.site_property.Addsite_propertyRow(id, "codingType", s.CTYPE);
            decodes.site_property.Addsite_propertyRow(id, "masterSwitch", s.MASTERSW.ToString());
            decodes.site_property.Addsite_propertyRow(id, "dayfileSwitch", s.DFMSW.ToString());
            decodes.site_property.Addsite_propertyRow(id, "archiverSwitch", s.ACMSW.ToString());


            decodes.sitename.AddsitenameRow(id, "cbtt", s.SITE.Trim(), "", "");
            if (s.IsUsgs)
            {
                decodes.sitename.AddsitenameRow(id, "usgs", s.ALTID.Trim(), "", "");
            }

            if (s.IsSnotel)
            {
                decodes.sitename.AddsitenameRow(id, "snotel", s.ALTID.Trim(), "", "");
            }

            count++;
            id++;
        }
        if (count == 0)
        {
            throw new Exception("CBTT/Site not found in MCF "+ String.Join(",",siteFilter));
        }
    }


    private String getHydrometCatagoryName(McfDataSet.sitemcfRow s)
    {

        if (s.CATID == "WMCO" || s.ACL == "WMCO" )
            return "AgriMet";
        if (s.CATID == "BURL")
            return "Burley";
        if (s.CATID == "KLAM")
            return "Klamath";
        if (s.CATID == "ROGU")
            return "Rogue";
        if (s.CATID == "UMAT")
            return "Umatilla";
        if (s.CATID == "SNOT")
            return "snotel";
        if (s.CATID == "YAKR")
            return "Yakima";
        if (s.CATID == "IDWD")
            return "IDWR";
        if (s.CATID == "DESU")
            return "Deschutes";
        if (s.CATID == "BOIA")
            return "Boise-Payette";

        return s.CATID;
    }


  




    public static int NextID(PostgreSQL svr, string tableName)
    {

        var tbl = svr.Table("Site", "SELECT count(*) as count, max(id) from "+tableName);

        int count = Convert.ToInt32(tbl.Rows[0]["count"]);
        if (count == 0)
        {
            return 1;
        }
        int max = Convert.ToInt32(tbl.Rows[0][1]);
        return (max + 1);
    }

    private int SetupNetworkList(string name)
    {
        var rows = decodes.networklist.Select("name = '" + name + "'");
        if (rows.Length == 1)
            return Convert.ToInt32(rows[0]["id"]);

        int id = NextID(svr, "networklist");
        var today = DateTime.Now;

        svr.RunSqlCommand("insert into networklist values("+id +",'"+name+"','goes','cbtt','" + today.ToString() + "')");

        return id;

//        // create group all.
//        svr.RunSqlCommand("insert into networklistentry select 1,mediumid from transportmedium");

//        String sql = "insert into networklistentry  select  "
//        + "CASE when B.Value = 'AgriMet' then 2 "
//        + "     when B.Value = 'Yakima' then 3 "
//        + "     when B.Value = 'IDWR' then 4 "
//        + "     when B.Value = 'Burley' then  5 "
//        + "     when B.Value = 'Deschutes' then 6 "
//        + "     when B.Value = 'Umatilla' then 7 "
//        + "     when B.Value ='Boise-Payette' then 8 "
//        + "           else 99  "
//        + " END, mediumid "
//        + "from transportmedium a, platformproperty b "
//        + "where a.platformid = b.platformid and b.name ='HydrometGroup' ;";
//        svr.RunSqlCommand(sql);

//        //IdahoPower
//        sql = "insert into networklistentry  select  9, mediumid "
//+ "from transportmedium a, platformproperty b "
//+ "where a.platformid = b.platformid and b.name ='IdahoPower' ;";
//        svr.RunSqlCommand(sql);

//        //usgs
//        sql = "insert into networklistentry  select  10, mediumid "
//        + "from transportmedium a, platformproperty b "
//        + "where a.platformid = b.platformid and b.name ='USGS' ;";
//        svr.RunSqlCommand(sql);

//        //owrd
//        sql = "insert into networklistentry  select  11, mediumid "
//        + "from transportmedium a, platformproperty b "
//        + "where a.platformid = b.platformid and b.name ='OWRD' ;";
//        svr.RunSqlCommand(sql);

    }

    static int GetManufactureCode(String m)
    {
        if (m.Trim() == "S")
            return 1;
        if (m.Trim() == "A")
            return 2;
        if (m.Trim() == "D")
            return 3;
        if (m.Trim() == "V")
            return 4;
        if (m.Trim() == "cr1000")
            return 5;

        return 0;
    }



    }

}

using System;
using Reclamation.TimeSeries.Hydromet;
using Reclamation.Core;
using System.Data;

namespace Reclamation.TimeSeries.Decodes
{
    class UnitConversion
    {
        private Reclamation.TimeSeries.Hydromet.McfDataSet mcf;
        private HydrometTools.Decodes.DecodesDataSet decodes;
        PostgreSQL svr;
        public UnitConversion(Reclamation.TimeSeries.Hydromet.McfDataSet mcf, 
            HydrometTools.Decodes.DecodesDataSet decodes,
             PostgreSQL svr)
        {
            this.mcf = mcf;
            this.decodes = decodes;
            this.svr = svr;
        }


        /// <summary>
        /// Scaling, offset, shift, etc.
        /// Done by adding appropirate row to scriptsensor
        /// </summary>
        /// <param name="decodesScriptID"></param>
        /// <param name="sensorNumber"></param>
        /// <param name="goes"></param>
        /// <param name="s"></param>
        internal void AddUnitConverter(int decodesScriptID, int sensorNumber, McfDataSet.goesmcfRow goes, McfDataSet.sitemcfRow s)
        {
            int unitconverterid = GetUnitConverterID(goes, s);
            if (unitconverterid >= 0)
            {
                decodes.scriptsensor.AddscriptsensorRow(decodesScriptID, sensorNumber, unitconverterid);
            }
            else
            {
                Console.WriteLine("did not find unit converter "+goes.GOESDEC);
            }
        }

        internal void AddBatteryVoltConverter(int decodesScriptID, int sensorNumber, McfDataSet.sitemcfRow s)
        {
            int id = -1;
            if (s.DCPMFG == "V") // vitel
            {
                id = LinearEquation(.1, 9.5,"v");
                
            }
            else
                if (s.DCPMFG == "S")
                {
                    id = LinearEquation(.234, 10.6, "v");
                }
                else
                    if (s.DCPMFG == "A")
                    {
                        id = LinearEquation(1, 0, "v");
                    }
                    else if( s.DCPMFG == "D" || s.DCPMFG == "O")
                    {
                        id = UsgsEquation(.313, -32, 1, 10.3, "v");
                    }    

            if (id >= 0)
            {
                decodes.scriptsensor.AddscriptsensorRow(decodesScriptID, sensorNumber, id);
            }

        }

        internal void GetMaxMin(McfDataSet.goesmcfRow goes, out double min, out double max)
        {
            max = 9999999;
            min = 0.0001;
            var rows = (McfDataSet.pcodemcfRow[])mcf.pcodemcf.Select("PCODE ='" + goes.GOESDEC + "'");

            if (rows.Length == 0)
            {
                Logger.WriteLine("Error: could not find '" + goes.GOESDEC + "' in pcode table");
                return;
            }

            if (rows[0].QCSW == 1)
            {
                if( System.Math.Abs(rows[0].QHILIM -998877) > 0.01)
                   max = rows[0].QHILIM;
                if (System.Math.Abs(rows[0].QHILIM - 998877) > 0.01)
                     min = rows[0].QLOLIM;
            }

        }

        /// <summary>
        /// Find appropriate method for to scale, offset, etc
        /// If necessary create entry in unitconverter. Otherwise use 
        /// existing unitconverter.
        /// </summary>
        /// <param name="goes"></param>
        /// <returns></returns>
        internal int GetUnitConverterID(McfDataSet.goesmcfRow goes, McfDataSet.sitemcfRow s)
        {
            var rows = (McfDataSet.pcodemcfRow[])mcf.pcodemcf.Select("PCODE ='" + goes.GOESDEC + "'");

            //if( s.SITE.IndexOf("ARNO") >=0 )
            //     Console.WriteLine("");

            if (rows.Length == 0)
            {
                Logger.WriteLine("Error: could not find '" + goes.GOESDEC + "' in pcode table");
                return -1;
            }


            if (s.CTYPE == "S")
            {
                Logger.WriteLine("Warning < check unit conversion >:  Site based processing '"+s.SITE+"'");
                //return -1;
            }

            var pcode = rows[0];

            

            if (pcode.ACTIVE == 0 || goes.ACTIVE == 0)
            {
                Logger.WriteLine("parameter not active :" + goes.GOESDEC);
                return -1;
            }

            // DONT' look at goes.RTCNAME ( pcode.RTCPROC is used instead)
            if (!pcode.IsRTCPROCNull()  && pcode.RTCPROC.Trim().Replace("\0","") != "") 
            {
                // TO DO  GH_WEIR, GH_WEIRX,  
                if (pcode.RTCPROC.ToLower().Trim() == "ch_weir")
                {
                    //Y  =  A*( B*x +C + D )^E
                    //return Weir(pcode.SCALE, 0.01, pcode.OFFSET, pcode.SHIFT,pcode.BASE);
                    return LinearEquation(0.01, 0,"ft");
                }
                if (pcode.RTCPROC.ToLower().Trim() == "gh_vshift" || pcode.RTCPROC.ToLower().Trim() == "gh_lim")
                {
                    return LinearEquation(pcode.SCALE, pcode.OFFSET, "ft");
                }
                
                Logger.WriteLine("not implemented Rtcm routine :" + pcode.RTCPROC+" " +goes.GOESDEC);
                return -1;
            }
               string parameterName = goes.GOESDEC.Substring(7).Trim();
                   // string siteName = goes.GOESDEC.Substring(0, 8).Trim();

               if (parameterName == "FB" || parameterName == "FB2")
                    {
                        return LinearEquation(pcode.SCALE, pcode.OFFSET,"ft");
                    }
               else if (parameterName == "PC")
               {
                   return LinearEquation(pcode.SCALE, pcode.OFFSET, "in");
               }
               else if (parameterName.IndexOf("GH") == 0 ||
                        parameterName.IndexOf("CH") == 0) // || parameterName == "GH2", GH-L2, GH-L3 )
               {
                   return LinearEquation(pcode.SCALE, pcode.OFFSET, "ft");
                   //return UsgsEquation(pcode.SCALE, pcode.OFFSET, 1.0, 0.0);
               }
               else if (parameterName.Trim() == "OB" || parameterName.Trim() == "TV" || parameterName == "WF")
               {
                   if (System.Math.Abs(pcode.SCALE) < 0.000001) // basically zero
                       return LinearEquation(0.07862, -99.02, "degF");
                   else
                       return LinearEquation(pcode.SCALE, pcode.OFFSET, "degF");
               }
               else if (parameterName.Trim() == "WC")
               {
                   if (System.Math.Abs(pcode.SCALE) < 0.000001) // basically zero
                       return LinearEquation(0.1221, pcode.OFFSET, "degC");
                   else
                       return LinearEquation(pcode.SCALE, pcode.OFFSET, "degC");
               }

               string units = GetUnits(parameterName);

               return LinearEquation(pcode.SCALE, pcode.OFFSET,units);


            //return -1;
        }


        DataTable m_units;
        private string GetUnits(string parameterName)
        {
            // get units from the presentation group.

            string sql = " SELECT t.standard, t.code, p.datatypeid, p.unitabbr "
   + " FROM datapresentation p "
   + " JOIN datatype t ON p.datatypeid = t.id; ";


            if( m_units == null)
            {
                m_units = svr.Table("pnhydromet_units",sql);
            }


            var rows = m_units.Select("standard = 'CBTT' and code ='" + parameterName + "'");

            if (rows.Length > 0)
            {
                return rows[0]["unitabbr"].ToString();
            }

            Console.WriteLine("Warning: no units defined for: " + parameterName);
            return "";

        }



        /// <summary>
        /// Y = A * (B + x)^C + D
        /// </summary>
        private int UsgsEquation(double a, double b, double c, double d, string units)
        {
            var uc = decodes.unitconverter.NewunitconverterRow();
            uc.id = decodes.unitconverter.GetNextID();
            uc.fromunitsabbr = "raw";
            uc.tounitsabbr = units;
            uc.algorithm = "usgs-standard";
            if (System.Math.Abs(a) < 0.000001)
                uc.a = 0.01;
            else
                uc.a = System.Math.Round(a, 5);

            uc.b = b;
            uc.c = c;
            uc.d = d;

            decodes.unitconverter.AddunitconverterRow(uc);
            return uc.id;
        }

      
        /// <summary>
        /// Y = A(x) + b
        /// </summary>
        private int LinearEquation(double a, double b, string units)
        {

            var uc = decodes.unitconverter.NewunitconverterRow();
            uc.id = decodes.unitconverter.GetNextID();
            uc.fromunitsabbr = "raw";
            uc.tounitsabbr = units;
            uc.algorithm = "linear";//"Y = Ax + B"
            if (System.Math.Abs(a) < 0.000001)
                uc.a = 0.01;
            else
                uc.a = System.Math.Round(a, 5);

            uc.b = b;
            decodes.unitconverter.AddunitconverterRow(uc);
            return uc.id;
        }

      

    }
}

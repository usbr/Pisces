using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ReclamationTesting.RiverWareDmiTest
{
    public class TestInjectingToRiverwareMMR_DMI
    {
        public void CreateDMIList()
        {

            string data = "\"$mgr\" prog {1929}\n"
              + "set dmi \"$mgr.prog.1929\"\n"
            + "\"$dmi\" type {Input}\n"
            + "\"$dmi\" confirmWarnings 1\n"
            + "\"$dmi\" ctlFile {C:/Documents and Settings/ktarbet/My Documents/project/UpperSnake/RiverWare/operations model/adjusted_locals_dmi_control.txt}\n"
            + "\"$dmi\" execFile {C:/Documents and Settings/ktarbet/My Documents/project/UpperSnake/RiverWare/operations model/traces_dmi.bat}\n"
            + "\"$dmi\" allowSpaces 1\n"
            + "\"$dmi\" value  {ImportDate} {Text} {yyyy-MM-dd}"
                        ;

            StreamWriter sw = new StreamWriter(@"c:\temp\dmi_code.txt");
            for (int i = 1928; i <= 2006; i++)
            {
                string s = data.Replace("1929", i.ToString());
                string dateStr = ((int)(i - 1)).ToString() + "-10-01";
                s = s.Replace("yyyy-MM-dd", dateStr);
                sw.WriteLine(s);
            }
            sw.Close();

        }
        public void CreateMRMList()
        {

            string rval = "$cfg dmi 1 {1928} 1 {1929} 1 {1930} ";
            for (int i = 1931; i <= 2006; i++)
            {
                rval = rval + " 1 {" + i.ToString() + "}";
            }

            File.WriteAllText(@"c:\temp\mrm_code.txt", rval);
        }
      
    }
}

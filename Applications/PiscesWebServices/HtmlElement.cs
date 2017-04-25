using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiscesWebServices
{
    static class HtmlElement
    {

        public static string SelectIntRange(string name,int first,int last, int selected)
        {
            string rval = "<select name=\""+name+"\">";
            for (int i = first; i <= last; i++)
			{
                if( i == selected)
                    rval +="<option selected value=\""+i+"\">"+i+"</option>\n";
                else
			        rval += "<option value=\""+i+"\">"+i+"</option>\n";
			}
            

            rval += "</select>";
                return rval;
        }

        public static string SelectMonth(string name, int currentMonth)
        {
            var rval = "<select name=\"" + name + "\">";
            DateTime now = new DateTime(2000, currentMonth, 1);
            for (int m = 1; m <= 12; m++)
            {
                if (m == currentMonth)
                    rval += "<option selected value=\"" + m + "\">" + now.ToString("MMMM") + "</option>";
                else
                    rval += "<option value=\"" + m + "\">" + now.ToString("MMMM") + "</option>";
            }
            rval += "</select>";
            return rval;
        }
    }
}

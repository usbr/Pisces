using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HdbApi.Models
{
    class ReadModels
    {
        public class DataQueryInputObject
        {
            // Legacy HDB CGI program call
            // http://ibr3lcrsrv01.bor.doi.net:8080/HDB_CGI.com?svr=lchdb2&sdi=1863,1930,2166,2146&tstp=DY&t1=1/1/1980&t2=1/1/2016&table=R&mrid=-1&format=9
            public string svr { get; set; }
            public string sdi { get; set; }
            public string tstp { get; set; }
            public string t1 { get; set; }
            public string t2 { get; set; }
            public string table { get; set; }
            public string mrid { get; set; }
            public string format { get; set; }
        }

    }
}

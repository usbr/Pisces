using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Reclamation.TimeSeries.Hydromet
{

    /* // from Fortran.
     * 
      common block for storage for one mpoll file record

      COMMON /RECORD1/ REC_AD,REC_STA,REC_APC,REC_YR,REC_MARK(12)
         CHARACTER REC_AD*25,REC_STA*12,REC_APC*9,REC_YR*4
         CHARACTER REC_MARK*1
      COMMON /RECORD2/ IYR_REC,REC_DATA(12)

     * // from webmpollcsv.c
     * 
     * #define MPOLL_SIZE 121
     * 
     * // from jms.c
    for(i=0;i<12&&station[i]!='\0';i++)
                record[i]=station[i];
        for(j=i;j<12;j++)
                record[j]=' ';
        for(i=0;i<9&&*(pcode+i)!='\0';i++)
                record[12+i]=*(pcode+i);
        for(j=i;j<9;j++)
                record[12+j]=' ';
        for(i=0;i<4;i++)
                record[21+i]=cyear[i];
 
     */
    /// <summary>
    /// 
    /// </summary>
    internal class  MpollRecord
    {
        public string Site="";
        public string Cbtt="";
        public int Year=2014;


    }
}

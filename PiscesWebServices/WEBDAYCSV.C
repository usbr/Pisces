// webdaycsv.c      
//  12/11/2000  john schidler
// gets hydromet dayfile data for web requests
// sep 2008 ktarbet using jms library
// oct 2008 bug fix. duplicate html for missing data (format=5)
// November 2008 K.Tarbet allow Post and Get.
//                         don't print flags(html) (format=7)
// January 8, 2009 -- remove headings in format =5
// feb 17, 2009  -- allow quality (diagnostic query)
// jan 3, 2011 -- added print_hourly option
// feb 6, 2012 -- cma support day3a.c API, so we can depricate day3a.exe 
// http://www.usbr.gov/pn-bin/webdaycsv.pl?station=jck&year=2010&month=1&day=1&year=2010&month=1&day=5&pcode=fb&pcode=af
// Apr 3, 2012 -- cma Added back option ...back=[number of hours] max is 192
// June 14, 2012 -- K.Tarbet support back with day3a API
// formats  &format=n
// 0-tab delimited (day3a.c compatibility mode)
// 1-tab delimited 
// 2-comma separated values 
// 3-html table (includes Quality column)
// 4-reserved: riverware?
// 5-html table quality is in with number)
// 6-reserved: used for no-date option like in in webarccsv
// 7-html without quality flags
//
#include <stdio.h>
#include <time.h>
#include "cgiutils.h"
#include "jms.h"


#define PCODES_PER_YEAR 366
#define MAX_PARAMS 500
#define PCODE_SIZE 10
#define CODESPERDAY 1440
#define STATION_SIZE 8
#define MAX_HOURS_BACK 360

extern int iHrTest=1,iMinTest=1;
struct param {
	int num;
	char station[MAX_PARAMS][STATION_SIZE];
	char pcode[MAX_PARAMS][PCODE_SIZE];
};
struct prec {
	int hasval[MAX_PARAMS][CODESPERDAY];
	float val[MAX_PARAMS][CODESPERDAY];
	char flag[MAX_PARAMS][CODESPERDAY];
};


void main(void)
{
	void get_request(struct datetime *,struct datetime *,struct param *,int *format, int *diagnostic,int *hourly);
	void get_records(struct datetime *,struct param *,struct prec *,int diagnostic);
	void print_records(struct datetime *,struct param *,struct prec *,int format,int hourly);
	void printDebugInfo(struct datetime *,struct datetime *,struct param *,int *format, int *diagnostic,int *hourly);
	void print_header(struct param *,int);
	void printwarning(void);
	void starthtml(void);
	void printtrailer(void);
	int retval,i,numpcodes,j,emptyrecord,format,hourly;
	int diagnostic; // 0=Data (default), 1=diagnostic/quality data
      char station[13],year[5];
	struct datetime startdate,enddate,date;
	double startjd,endjd;
	long day;
	int numrecs;
	struct param params;
	struct prec records;

	starthtml();

	get_request(&startdate,&enddate,&params,&format,&diagnostic,&hourly);
        if(! (format == 0 || format == 3 || format == 5 || format == 7) )
		printwarning();
//       printDebugInfo(&startdate,&enddate,&params,&format,&diagnostic,&hourly);
	 
	startjd=date_to_jd(&startdate);
	endjd=date_to_jd(&enddate);
      	// format 1-tab delimited , 2-comma separated values , 3-html table 
      
	if (format == 0)
	{
		fprintf(stdout,"<PRE>\n");
	}
	else if (format == 3 || format == 5 || format == 7)
	{   
		fprintf(stdout,"<TABLE BORDER=1 BGCOLOR=#FFFFFF>\n");
	}
	else
	{
		fprintf(stdout,"<PRE>\n");
		fprintf(stdout,"BEGIN DATA\n");
	} 

	print_header(&params,format);             

	while(startjd<endjd)
	{
		for(i=0;i<MAX_PARAMS;i++)           //init rec struct
			for(j=0;j<CODESPERDAY;j++)
				records.hasval[i][j] = 0;

		get_records(&startdate,&params,&records,diagnostic);
		print_records(&startdate,&params,&records,format,hourly);

		startjd = startjd + 1.0;

		jd_to_date(startjd,&startdate);
	}

	if (format==0)
           fprintf(stdout,"</pre>\n");
        else if (! ( format ==3 || format == 5 || format == 7 ))
	{
		fprintf(stdout,"END DATA\n");
		fprintf(stdout,"</pre>\n");
	}
	else
	{
		fprintf(stdout,"</TABLE>");
	}

	fprintf(stdout,"</body></html>\n\n\n\n");

}	//end main ****************************************************


void starthtml(void)
{
	fprintf(stdout,"Content-type: text/html\n\n"
		"<HTML>\n"
		"<HEAD><TITLE>Hydromet/AgriMet Data Access</title></head>\n"
		"<BODY BGCOLOR=#FFFFFF>\n"
		"<p>"
		);
}	//end starthtml*******************************************************

void printtrailer(void)
{
	fprintf(stdout,"</pre></body></html>\n");
}	//end printtrailer****************************************************

void printwarning(void)
{
	fprintf(stdout,
		"<PRE>\n"
		"<B>USBR Pacific Northwest Region\n"
		"Hydromet/AgriMet Data Access</B><BR>\n"
		"Although the US Bureau of Reclamation makes efforts to maintain the accuracy\n" 
		"of data found in the Hydromet system databases, the data is largely unverified\n" 
		"and should be considered preliminary and subject to change.  Data and services\n" 
		"are provided with the express understanding that the United States Government\n" 
		"makes no warranties, expressed or implied, concerning the accuracy, complete-\n" 
		"ness, usability or suitability for any particular purpose of the information\n" 
		"or data obtained by access to this computer system, and the United States\n" 
		"shall be under no liability whatsoever to any individual or group entity by\n" 
		"reason of any use made thereof. \n"
		"</PRE>\n"
		"<p>\n"
		);
}



int IsDiagnostic(char *pcode)
{
if( 
		 strncmp("BAY"    ,pcode,3) == 0
	  || strncmp("DEMOD"  ,pcode,4) == 0
      || strncmp("CHANNEL",pcode,7) == 0
	  || strncmp("FREQ"   ,pcode,4) == 0
      || strncmp("POWER"  ,pcode,5) == 0
	  || strncmp("SIGNAL" ,pcode,6) == 0         
      || strncmp("MODUL"  ,pcode,5) == 0
	  || strncmp("PARITY" ,pcode,6) == 0
      || strncmp("ILLCHRS",pcode,6) == 0
	  || strncmp("MAXPOS" ,pcode,6) == 0
      || strncmp("CHARCNT",pcode,7) == 0
	  || strncmp("LENERR" ,pcode,6) == 0
      || strncmp("TIMESEC",pcode,7) == 0
	  || strncmp("TIMEERR",pcode,7) == 0
      || strncmp("BATVOLT",pcode,7) == 0
	  || strncmp("RMSGCNT",pcode,7) == 0
      || strncmp("EOTCHAR",pcode,7) == 0
	  )
	  return 1;

return 0;
}

char * findlast(const char *source, const char *target)
{ 
    const char *current; 
    char *found = NULL; 
 
    size_t target_length = strlen(target); 
    current = source + strlen(source) - target_length; 
 
    while ( current >= source ) { 
        if ( (found = strstr(current, target)) ) { 
            break; 
        } 
        current -= 1; 
    } 
 
    return found; 
} 

void CalcDates(int dHours, struct datetime *startdate,struct datetime *enddate)
{
   //Calculate start date based upon new option [back=numhours]
   //
   struct tm *timep;
   time_t tn;
   int dSeconds;
   //get the current date and time
   dSeconds = 3600 * dHours;
   time(&tn);   
   timep = localtime(&tn);
   enddate->yr = timep->tm_year+1900;
   enddate->mn = timep->tm_mon+1;
   enddate->dy = timep->tm_mday;
  
   tn -= dSeconds;         //Our start time in seconds since Jan 1, 1970
   timep = localtime(&tn);

   startdate->mn=timep->tm_mon+1;
   startdate->dy=timep->tm_mday;
   startdate->yr=timep->tm_year+1900;
   
   startdate->hr=timep->tm_hour;
   startdate->mi=timep->tm_min;
   startdate->sc=0;


}


void get_day3_request(char* cb, struct datetime *startdate,
   struct datetime *enddate,
  struct param *params,int *format,int *hourly)
 {
	 char station[STATION_SIZE];
	 char year[5],month[3],day[3],formatc[2],tmpchar[2];
	 char* cb2 = findlast(cb,"year");
	 char pcode[PCODE_SIZE];
	 char sHours[3];
	 int iHours;
	 int j,k =0,l;
         
	 iHours = 0;
         
	 //http://www.usbr.gov/pn-bin/webdaycsv.pl?station=jck&year=2010&month=1&day=1&year=2010&month=1&day=5&pcode=fb&pcode=af

	
	 if (strstr(cb,"back"))  
	 {
		 cgi_parameter(cb,"back",sHours);
		 iHours=atoi(sHours);
		 if (iHours > MAX_HOURS_BACK  || iHours <=0 || !strstr(cb,"pcode"))
		 {
			 html_errorexit("invalid query 2");
			 return;
		 }
	 }
	 else
	 {
		 if( !strstr(cb,"year") || !strstr(cb,"month") ||!strstr(cb,"day") ||!strstr(cb,"pcode") )
		 {
			 html_errorexit("invalid query 1");
		 }
	 }

          cgi_parameter(cb,"station",station);
	 if(iHours > 0)
	 {
 	     CalcDates(iHours,startdate,enddate);
             enddate->hr = 23,enddate->mi = 59,enddate->sc =59;
             cb2 = cb;
	 }
	 else
	 {
		 cgi_parameter(cb,"year",year);    
		 cgi_parameter(cb,"month",month);
		 cgi_parameter(cb,"day",day);

		 startdate->yr = atoi(year);
		 startdate->mn = atoi(month);
		 startdate->dy = atoi(day);
		 startdate->hr = 0,startdate->mi = 0,startdate->sc =0;  

		 cgi_parameter(cb2,"year",year);    
		 cgi_parameter(cb2,"month",month);
		 cgi_parameter(cb2,"day",day);

		 enddate->yr = atoi(year);
		 enddate->mn = atoi(month);
		 enddate->dy = atoi(day);
		 enddate->hr = 23,enddate->mi = 59,enddate->sc =59;  
	 }


     j=0;
	cb2 = strstr(cb2,"pcode");
	while(cb2)
	{
//         printf("\n#Debug cb2 = %s",cb2);
		cgi_parameter(cb2,"pcode",pcode);
		//strncpy(params->station[j],station,STATION_SIZE);
		for(k=0;k<STATION_SIZE-1 && station[k]!='\0';k++)
			params->station[j][k] = station[k];

		for(l=k;l<STATION_SIZE-1;l++)
			params->station[j][l] = ' ';
		params->station[j][l]='\0';

		for(k=0;k<PCODE_SIZE-1 && pcode[k]!='\0';k++)
			params->pcode[j][k] = pcode[k];
		for(l=k;l<PCODE_SIZE-1;l++)
			params->pcode[j][l] = ' ';
		params->pcode[j][l]='\0';


		//strncpy(params->pcode[j],pcode,PCODE_SIZE);
		j++;

		if( cb2++ != '\0')
			cb2 = strstr(cb2,"pcode");
		else
			cb2 = 0;
	}
	params->num = j;
//        *format=0;   

        cgi_parameter(cb,"print_hourly",tmpchar);
        *hourly = atoi(tmpchar);

        cgi_parameter(cb,"format",tmpchar); 
        *format = atoi(tmpchar);
        if(*format<1 || *format > 7) 
            *format=0; // default=0 for day3.pl (not specified)

 }//end get_day3_request


 void printDebugInfo(struct datetime *startdate,struct datetime *enddate,struct param *params,int *format, int *diagnostic,int *hourly)
 {
int j; 
  for(j=0; j<params->num; j++)
   {
   printf("\n %s :  %s ",params->station[j],params->pcode[j]);

   }
  printf("\nformat = %d",*format);
  printf("\nhourly = %d",*hourly);
  printf("\ndiagnostic = %d",*diagnostic);
  fprintf(stdout,"\n start: %d %d %d\n",startdate->yr,startdate->mn,startdate->dy);
  fprintf(stdout,"\n start: hr min %d %d \n",startdate->hr,startdate->mi);
  fprintf(stdout,"\n end: %d %d %d\n",enddate->yr,enddate->mn,enddate->dy); 
  fprintf(stdout,"\n start hr mi: %d %d \n",enddate->hr,enddate->mi);

 }
 
//get the request from the web server
//content format: fieldname=fieldvalue&
void get_request(struct datetime *startdate,struct datetime *enddate,
struct param *params,int *format, int *diagnostic,int *hourly)
{
	void errorexit(char *);
	char *rqm,*ctlc,cb[MAX_CONTENTLENGTH];
	int  iHours,ctl,t,i,j,k,l,retval;
	char year[5],month[3],day[3],tmpchar[2];
	char pair[50],station[50],pcode[50];
        char sHours[3];

        *diagnostic = 0;
	ctl = get_content(cb);
	unescape_url(&cb[0]);
	if( strstr(cb,"station")) // day3 API
	{
		//fprintf(stdout,"\nUsing day3 API");
		get_day3_request(cb,startdate,enddate,params,format,hourly);
		return;
	}
	else if( !strstr(cb,"parameter")) // original API
	{
		html_errorexit("invalid query");
		return;
	}
	else if (strstr(cb,"back"))  
        {
	        cgi_parameter(cb,"back",sHours);
                iHours=atoi(sHours);
                if (iHours > MAX_HOURS_BACK  || iHours <=0 )
                   {
                       html_errorexit("invalid query");
                       return;
                    }
         }
  *diagnostic = 0;
	t=10;
	j=0;
	while(cb[t]!='&' && t < ctl)
	{
		for(i=0;i<50 && cb[t]!='&' && cb[t]!=',' && t < ctl;i++,t++)
			pair[i]=toupper(cb[t]);
		pair[i]='\0';
		
		//fprintf(stdout,"%s\n",pair);
		if(i>0 && j < MAX_PARAMS)
		{
			if((retval=sscanf(pair,"%s %s",station,pcode))==2)
			{
				*diagnostic = IsDiagnostic(&pcode[0]);

				for(k=0;k<STATION_SIZE-1 && station[k]!='\0';k++)
					params->station[j][k] = station[k];
				for(l=k;l<STATION_SIZE-1;l++)
					params->station[j][l] = ' ';
				params->station[j][l]='\0';

				for(k=0;k<PCODE_SIZE-1 && pcode[k]!='\0';k++)
					params->pcode[j][k] = pcode[k];
				for(l=k;l<PCODE_SIZE-1;l++)
					params->pcode[j][l] = ' ';
				params->pcode[j][l]='\0';
				j++;
			}                                    
		}
		if(cb[t]==',')
			t++;
	}
	params->num = j;

	//&syer=2007&smnth=3&sdy=25&eyer=2007&emnth=3&edy=26"
	//fprintf(stdout,"%s\n",&cb[t]);
        //fprintf(stdout,"iHours: %s\n",&sHours[0]);
        if (strstr(cb,"back"))  
           CalcDates(iHours,startdate,enddate);
        else 
          {

             cgi_parameter(cb,"syer",year);
	     cgi_parameter(cb,"smnth",month);
	     cgi_parameter(cb,"sdy",day);

	     startdate->yr = atoi(year);
	     startdate->mn = atoi(month);
	     startdate->dy = atoi(day);
	     startdate->hr = 0,startdate->mi = 0,startdate->sc =0;  
       
	     cgi_parameter(cb,"eyer",year);      
	     cgi_parameter(cb,"emnth",month); 
	     cgi_parameter(cb,"edy",day);

	     enddate->yr = atoi(year);
	     enddate->mn = atoi(month);
	     enddate->dy = atoi(day);
          }

	enddate->hr = 23,enddate->mi = 59,enddate->sc =59;  

	cgi_parameter(cb,"format",tmpchar);
	*format = atoi(tmpchar);
	if(*format<1 || *format > 7)
		*format=2;   

  cgi_parameter(cb,"print_hourly",tmpchar);
  *hourly = atoi(tmpchar);

        
  //fprintf(stdout,"\n start: %d %d %d\n",startdate->yr,startdate->mn,startdate->dy);
  //fprintf(stdout,"\n start: %d %d \n",startdate->hr,startdate->mi);
  //fprintf(stdout,"\n end: %d %d %d\n",enddate->yr,enddate->mn,enddate->dy);

}


#define DAYFILE_SIZE 1500
void get_records(struct datetime *day,struct param *params,struct prec *recs,
				 int diagnostic)
{
	struct FAB infab;
	struct RAB inrab;
	char filename[100],filedir[255]="sutron_dayfiles",month[4],dayofmonth[3];
	char record[DAYFILE_SIZE*289];
 	int i,j,k,r,retval,keysize=15,numcodes,minute;
	//int iread(struct RAB *,char *,int);
	char valchar[4],codes[10],pchar,ichar[3];
	unsigned char flag;
	float *val;

	val=(float *)&valchar[0];

	sprintf(filename,"%04d",day->yr);

	date_to_monthstr(day,month);
	strcat(filename,month);

	sprintf(dayofmonth,"%02d",day->dy);
	strcat(filename,dayofmonth);
	strcat(filename,".day");
        if((retval=openfab(&infab,&filename[0],&filedir[0]))!=0)
	{
		fprintf(stdout,"\n%s %s",filedir,filename);
		fprintf(stdout,"\nError: file access opening fab");

	}
        
        if((retval=connectrab(&infab,&inrab,'r',&record[0],DAYFILE_SIZE))!=0)
	{
		fprintf(stdout,"\nError: file access connect rab");
        }

        for(r=0;r<params->num;r++)
	{
	        record[0] =0x40;
		record[1] =0x00;
		record[2] ='D';
		if( diagnostic ==1)
		{
			//            printf("\ndiagnostic");
			record[2] = 'Q';
		}

		for(i=0;i<8&&params->station[r][i]!='\0';i++)
			record[3+i]=params->station[r][i];
		for(j=i;j<8;j++)
			record[3+j]=' ';
		for(i=0;i<4;i++)                    //time, start at 0000
			record[11+i]='0';
               if((retval=iread_dayfiles(&inrab,record,keysize))<0)
		{
		        keysize=11;//check any time any random or selftimed
			if((retval=iread_dayfiles(&inrab,record,keysize))<0)
			{
			        continue;
			}
			keysize=15;
		}
     
               j=0;
		while(strncmp((record+3),params->station[r],STATION_SIZE-1)==0 && retval > 0 && j<CODESPERDAY)
		{
			numcodes = (int)record[21];
                        ichar[0] = record[11];
			ichar[1] = record[12];
			ichar[2] ='\0';

			minute = atoi(ichar);
			minute *= 60;
			ichar[0] = record[13];
			ichar[1] = record[14];
			ichar[2] ='\0';
			minute += atoi(ichar);

			if(minute < 0 || minute > CODESPERDAY)
				printf("Time error\n");               //should never happen

			for(i=0;i<numcodes;i++)
			{
				for(j=0;j<8;j++)
					codes[j]=record[22+i*9+j];
				codes[j]='\0';

				if(strncmp(codes,params->pcode[r],7)==0 )   
				{
					flag=record[22+i*9+8];
					switch (flag) 
					{
					case 0xff : pchar=' ';break;
					case 0xfd : case 0xfb : case 0xf9: pchar='e';break;
					case 0x00 : pchar='u';break;
					case 0xfe : pchar='n';break;
					case 0xfc : pchar='m';break;
					case 0xfa : pchar='p';break;
					case 0xf8 : pchar='i';break;
					case 0xf6 : pchar='f';break;
					case 0xf4 : pchar='r';break;
					case 0xf2 : pchar='?';break;
					case 0xf0 : pchar='b';break;
					case 0xee : pchar='a';break;
					case 0xec : pchar='-';break;
					case 0xea : pchar='+';break;
					case 0xe8 : pchar='^';break;
					case 0xe6 : pchar='~';break;
					case 0xe4 : pchar='|';break;
					default  : pchar=' ';break;
					}

					valchar[0]=record[22+numcodes*9+i*4];
					valchar[1]=record[23+numcodes*9+i*4];
					valchar[2]=record[24+numcodes*9+i*4];
					valchar[3]=record[25+numcodes*9+i*4];

					recs->val[r][minute] = *val;
					recs->flag[r][minute] = pchar;
					recs->hasval[r][minute]=1;
				}
			}

			j++;
			retval=sread_dayfiles(&inrab,record); 
		}

	}
	closefab(infab);
}


void print_header(struct param *params,int format)
{
	int i;
	if(format==3)	//html table
	{
		fprintf(stdout,"<TR><TH>DATE      TIME</TH>");
		for(i=0;i<params->num;i++)
			fprintf(stdout,"<TH>%s %s</TH><TH>Quality</TH>",
			params->station[i],params->pcode[i]);
		fprintf(stdout,"</TR>\n");
	}     
	else if( format == 5 || format == 7) //html table no header
	{
		//    fprintf(stdout,"<TR><TH>DATE      TIME</TH>");        
		//   for(i=0;i<params->num;i++)         
		//   fprintf(stdout,"<TH>%s %s</TH>",
		//          params->station[i],params->pcode[i]);        
		//fprintf(stdout,"</TR>\n");
	} 
	else if(format==2)	//comma-delimited, assumes station name max 5 char
	{
		fprintf(stdout,"DATE       TIME ");
		for(i=0;i<params->num;i++)
			fprintf(stdout,",  %4.8s %-4.8s",params->station[i],params->pcode[i]);
		
		fprintf(stdout,"\n");
	}
	else if(format==1)
	{			//tab-delimited, assumes station name max 5 char
		fprintf(stdout,"DATE       TIME ");
		for(i=0;i<params->num;i++)
			fprintf(stdout,"\t   %4.8s %-4.8s",params->station[i],params->pcode[i]);
		fprintf(stdout,"\n");
	}
        else if (format==0)
        {     
              i=0;
              fprintf(stdout,"%s\n",params->station[i]);
              fprintf(stdout,"      DATE  TIME       ");
              for(i=0;i<params->num;i++)
                      fprintf(stdout,"%10s",params->pcode[i]);
              fprintf(stdout,"\n");
        }      
 }

void print_records(struct datetime *day,struct param *params,
                   struct prec *recs,int format,int hourly)
{

	int r,p,found,hour,min,startinMin,tMinutes;
        startinMin=day->hr*60+day->mi;
        for(r=0;r<CODESPERDAY;r++)
	{
                found=0;
		for(p=0;p<params->num;p++)
			if(recs->hasval[p][r])
				found=1;
               	if(found)
		{
		    hour = r/60;
		    min = r - hour*60;
                    
                    tMinutes=hour*60+min;

                    if (iHrTest && tMinutes < startinMin)
                       continue;
                    else
                       iHrTest=0;

                   if( hourly && min != 0)
                       continue;
                    if( format==0)
                        fprintf(stdout,"%02d/%02d/%4d %02d:%02d ",day->mn,day->dy,day->yr,hour,min);
		    else if(format==3 || format == 5 || format == 7)	//html table
		         fprintf(stdout,"<TR><TD>%02d/%02d/%04d %02d:%02d</TD>",
				day->mn,day->dy,day->yr,hour,min);
			else if(format==2)		//comma-delimited
				fprintf(stdout,"%02d/%02d/%04d %02d:%02d",day->mn,day->dy,day->yr,hour,min);
			else if(format==1)		//tab--delimited
				fprintf(stdout,"%02d/%02d/%04d %02d:%02d",day->mn,day->dy,day->yr,hour,min);

			for(p=0;p<params->num;p++)
			{
				if(recs->hasval[p][r])
				{
					if(format==0)
                                                fprintf(stdout,"%10.2f ",recs->val[p][r]); 
                                        else if(format==3)		//html table
						fprintf(stdout,"<TD>%10.2f</TD><TD>%c</TD>",
						recs->val[p][r],recs->flag[p][r]);
					else if(format == 5)
						fprintf(stdout,"<TD>%10.2f%c</TD>",
						recs->val[p][r],recs->flag[p][r]);
					else if( format == 7) // html no flags
						fprintf(stdout,"<TD>%10.2f</TD>",
						recs->val[p][r]);

					else if(format==2)	//comma-delimited
						fprintf(stdout,", %10.2f%c",recs->val[p][r],recs->flag[p][r]);
					else if(format==1)	//tab-delimited
						fprintf(stdout,"\t%10.2f%c",recs->val[p][r],recs->flag[p][r]);
				}
				else
				{
					if(format ==3)	//html table
						fprintf(stdout,"<TD></TD><TD></TD>");
					if( format ==5 || format == 7)
						fprintf(stdout,"<TD></TD>");
					else if(format==2)	//comma-delimited
						fprintf(stdout,",");
					else if(format==1)	//tab-delimited
						fprintf(stdout,"\t          ");
				}
			}
			if(format==3 || format == 5 || format == 7)	//html table
				fprintf(stdout,"</TR>\n");
			else if(format==2)		//comma-delimited
				fprintf(stdout,"\n");
			else if(format==1 || format==0)		//tab-delimited
				fprintf(stdout,"\n");
		}
	}
}	//end******************************************************************


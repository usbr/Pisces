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

## Motivation

The ability to write simple to understand time series equations is a motivation to create Pisces.

Example simple equation involving three different time series and two different time steps.
```
(pal_af-pal_af[t-1])/1.98347+(jck_af[t-1]-jck_af[t-2])/1.98347+heii_qd
```

The Legacy system requires writing the equation above like this:

```
CL
CREATE JCK AF
CREATE PAL AF
CREATE HEII QD
CREATE HEII QU
G JCK/AF
G PAL/AF
MATH
LINE1*1 +1
MS=TOTAL
MC=AF
LINE1+LINE2
E 2
E 1
SP=LINE1
MS=TOTAL
MC=CS
LINE1-LINE2 +1
LINE3/1.98347
E 2
 
G HEII /QD
MATH
MS=HEII
MC=TQU
LINE2+LINE3
MS=SPAC
MC=AF
SP=LINE1
1484450-LINE5
 
G HEII/QU
MATH
RANGE=OCT03,SEP30
MS=HEII
MC=QU
LINE6=LINE4
MARK HEII QU
 
REPLACE
SHOW TOTAL,SPAC,HEII /QD,QU,AF
```



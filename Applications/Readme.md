
# Hydromet Web Services

## Daily – Water Year Report

[http://www.usbr.gov/pn-bin/wyreport.pl?site=ABEI&amp;parameter=PP&amp;start=2016&amp;end=2016&amp;head=no](http://www.usbr.gov/pn-bin/wyreport.pl?site=ABEI&amp;parameter=PP&amp;start=2016&amp;end=2016&amp;head=no)


## Daily  

[http://www.usbr.gov/pn-bin/daily?list=luc fb,sco fb&amp;start=2016-04-01&amp;end=2016-04-20](http://www.usbr.gov/pn-bin/daily?list=luc%20fb,sco%20fb&amp;start=2016-04-01&amp;end=2016-04-20)

[http://www.usbr.gov/pn-bin/daily?site=luc&amp;start=2016-04-01&amp;end=2016-04-20](http://www.usbr.gov/pn-bin/daily?site=luc&amp;start=2016-04-01&amp;end=2016-04-20)



| options | Default value |   |
| --- | --- | --- |
| List=site1 par1,site2 par2 | empty | A list of station parameter pairs separated by a commas. |
| back=3   | empty | number of days back from current day |
| format=csv,html   | csv | output format |
| flags=true     | false | include quality flag for each data point. Note default is different than for instant data. |
| start=2016-04-01 | empty | Starting date |
| end=2016-04-20 | empty | Ending date |
| Site | Empty | Gives all parameters for a particular site |

## Instant 

http://www.usbr.gov/pn-bin/instant.pl?list=boii ob,boii obx&amp;start=2016-04-15&amp;end=2016-04-20



| options | Default value |   |
| --- | --- | --- |
| list=site1 par1,site2 par2 | empty | list of station parameter pairs separated by a commas. |
| back=3   | empty | number of days back from current day |
| format=csv|html|zrxp | csv | output format.  csv = comma separatedhtml = hypertextzrxp = kisters zrxp format |
| flags=true     | true | include quality flag for each data point. |
| header=false | true | When true include description/codes for each column (when format= html) |
| start=2016-04-01 | empty | Starting date |
| end=2016-04-20 | empty | Ending date |
|   |   |   |



## AgriMet Weather Data

AgriMet has a special program &quot;agrimet.pl&quot; that &#39;knows&#39; all the parameters for a site.   You ask for the data for a site and all parameters are returned.

Below are examples for 15-minute and daily data.

&#39;instant data&#39;

[http://www.usbr.gov/pn-bin/](http://www.usbr.gov/pn-bin/agrimet.pl?cbtt=CHAW&amp;back=192&amp;interval=instant&amp;format=2) [agrimet](http://www.usbr.gov/pn-bin/agrimet.pl?cbtt=CHAW&amp;back=192&amp;interval=instant&amp;format=2) [.](http://www.usbr.gov/pn-bin/agrimet.pl?cbtt=CHAW&amp;back=192&amp;interval=instant&amp;format=2) [pl](http://www.usbr.gov/pn-bin/agrimet.pl?cbtt=CHAW&amp;back=192&amp;interval=instant&amp;format=2) [?cbtt=CHAW&amp;back=192&amp;interval=instant&amp;format=2](http://www.usbr.gov/pn-bin/agrimet.pl?cbtt=CHAW&amp;back=192&amp;interval=instant&amp;format=2)

&#39;daily data&#39;

[http://www.usbr.gov/pn-bin/](http://www.usbr.gov/pn-bin/agrimet.pl?cbtt=CHAW&amp;back=10&amp;interval=daily&amp;format=3) [agrimet](http://www.usbr.gov/pn-bin/agrimet.pl?cbtt=CHAW&amp;back=10&amp;interval=daily&amp;format=3) [.](http://www.usbr.gov/pn-bin/agrimet.pl?cbtt=CHAW&amp;back=10&amp;interval=daily&amp;format=3) [pl](http://www.usbr.gov/pn-bin/agrimet.pl?cbtt=CHAW&amp;back=10&amp;interval=daily&amp;format=3) [?cbtt=CHAW&amp;back=10&amp;interval=daily&amp;format=3](http://www.usbr.gov/pn-bin/agrimet.pl?cbtt=CHAW&amp;back=10&amp;interval=daily&amp;format=3)


 



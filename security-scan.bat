path=%path%;\C:\Program Files\HP_Fortify\HP_Fortify_SCA_and_Apps_4.31\bin
c:
cd C:\Users\KTarbet\Documents\project\Pisces\Core\bin\Debug
sourceanalyzer -b cs-pisces -clean
sourceanalyzer -b cs-pisces -vsversion 14.0 .
sourceanalyzer -b cs-pisces -scan -debug -logfile scanlog.txt -f results.fpr
pause
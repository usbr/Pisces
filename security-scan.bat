path=%path%;C:\Program Files\HP_Fortify\HP_Fortify_SCA_and_Apps_4.31\bin
c:
cd %~dp0
cd Core\bin\Debug\
::C:\Program Files\HP_Fortify\HP_Fortify_SCA_and_Apps_4.31\bin\fortifyupdate.cmd
sourceanalyzer -b cs-pisces2 -clean
sourceanalyzer -b cs-pisces2 -vsversion 12.0 -libdirs . Reclamation.Core.dll
sourceanalyzer -b cs-pisces2 -scan -debug -logfile scanlog.txt -f results.fpr
pause
path=%path%;C:\Program Files\HP_Fortify\HP_Fortify_SCA_and_Apps_4.31\bin
c:
cd %~dp0
cd PiscesUI\bin\Debug\
::C:\Program Files\HP_Fortify\HP_Fortify_SCA_and_Apps_4.31\bin\fortifyupdate.cmd
sourceanalyzer -b cs-pisces2 -clean
sourceanalyzer -b cs-pisces2 -vsversion 12.0 -libdirs . Reclamation.TimeSeries.Modsim.dll Reclamation.TimeSeries.Forms.dll Reclamation.TimeSeries.ArmyCorps.dllReclamation.Core.dll Reclamation.TimeSeries.dll Reclamation.TimeSeries.Excel.dll Pisces.exe
sourceanalyzer -b cs-pisces2 -scan -debug -logfile scanlog.txt -f results.fpr
pause
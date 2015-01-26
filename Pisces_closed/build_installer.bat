cd /d %~dp0
set inoo="%ProgramFiles(x86)%\Inno Setup 5\iscc.exe"
if NOT EXIST %inoo% set inoo="%ProgramFiles%\Inno Setup 5\iscc.exe"
echo %inoo%

%inoo% pisces.iss
::svn  --xml --limit 20 log file://ibr1pnrfp002.bor.doi.net/common/PN6200/svn/pisces
pause


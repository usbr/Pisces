:: copy configuration data to output folder for debugging
:: build the installer.

cd %1
set inoo="%ProgramFiles(x86)%\Inno Setup 5\iscc.exe"
if NOT EXIST %inoo% set inoo="%ProgramFiles%\\Inno Setup 5\iscc.exe"

%inoo% HydrometTools.iss
pause
#!/bin/bash
# linux build script
RWIS=./Rwis.Sync/bin/debug
HYDROMET=./HydrometServer/bin/debug
if [ -e ./bin ]; then
   echo "using existing bin"
else
   echo "creating bin directory"
   mkdir bin
fi 
echo "copying core output to bin" 
cp $HYDROMET/Reclamation.TimeSeries.dll Reclamation.Core.dll ./bin
cp $HYDROMET/HydrometServer.exe HydrometServer.exe.config ./bin
cp $RWIS/Rwis.Sync.exe Rwis.Sync.exe.config ./bin
echo "core output copied to bin"
if [ -f ./.nuget/NuGet.exe ]; then
   echo "using existing nuget.exe"
else
   echo "downloading nuget"
wget http://nuget.org/nuget.exe
cp nuget.exe ./.nuget/NuGet.exe
chmod a+x ./.nuget/NuGet.exe
fi 
export EnableNuGetPackageRestore=true
./.nuget/NuGet.exe install NUnit.Runners -Version 2.6.4 -OutputDirectory testrunner
xbuild Pisces.sln /p:PostBuildEvent="" 

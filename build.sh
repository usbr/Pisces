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
cp /usr/lib/nuget/NuGet.exe ./.nuget/NuGet.exe
cp /usr/lib/nuget/NuGet.Core.dll ./.nuget/
export EnableNuGetPackageRestore=true
#./.nuget/NuGet.exe install NUnit.Runners -Version 2.6.4 -OutputDirectory testrunner
xbuild Pisces.sln /p:PostBuildEvent="" 

echo "copying core output to bin" 
cp $HYDROMET/Reclamation.TimeSeries.dll Reclamation.Core.dll ./bin
cp $HYDROMET/HydrometServer.exe HydrometServer.exe.config ./bin
cp $RWIS/Rwis.Sync.exe Rwis.Sync.exe.config ./bin
echo "core output copied to bin"
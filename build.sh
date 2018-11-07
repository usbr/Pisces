#!/bin/bash
# linux build script
RWIS=./Applications/Rwis.Sync/bin/Debug
HYDROMET=./Applications/HydrometServer/bin/Debug
if [ -e ./bin ]; then
   echo "using existing bin"
else
   echo "creating bin directory"
   mkdir bin
fi 

if [ -f ./.nuget/nuget.exe ]; then
   echo " nuget exists"
else
wget -O .nuget/nuget.exe https://dist.nuget.org/win-x86-commandline/latest/nuget.exe
fi


mono .nuget/nuget.exe restore Pisces.sln

export EnableNuGetPackageRestore=true
#./.nuget/NuGet.exe install NUnit.Runners -Version 2.6.4 -OutputDirectory testrunner
xbuild Pisces.sln /p:PostBuildEvent=""  /p:DefineConstants="__MonoCS__"

echo "copying core output to bin" 
cp $HYDROMET/Reclamation.TimeSeries.dll $HYDROMET/Reclamation.Core.dll ./bin
cp $HYDROMET/HydrometServer.exe $HYDROMET/HydrometServer.exe.config ./bin
cp $RWIS/Rwis.Sync.exe $RWIS/Rwis.Sync.exe.config ./bin
echo "core output copied to bin"

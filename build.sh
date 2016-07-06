#!/bin/bash
# linux build script
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

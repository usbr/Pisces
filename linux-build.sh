#!/bin/bash
. /opt/mono/env.sh
cd /data/apps/pisces/src/Pisces

if [ -f ./.nuget/NuGet.exe ]; then
   echo "using existing NuGet.exe"
else
wget http://nuget.org/nuget.exe -O ./.nuget/NuGet.exe
chmod a+x ./.nuget/NuGet.exe
sudo mozroots --import --machine --sync
sudo certmgr -ssl -m https://go.microsoft.com
sudo certmgr -ssl -m https://nugetgallery.blob.core.windows.net
sudo certmgr -ssl -m https://nuget.org   
fi


X=xbuild
#echo "checking https://github.com/usbr/Pisces"
#git pull
export EnableNuGetPackageRestore=true
$X ./Core/Reclamation.Core.csproj
$X ./TimeSeries/Reclamation.TimeSeries.csproj
$X ./HydrometServer/HydrometServer.csproj
$S ./PiscesWebServices/PiscesWebServices.csproj

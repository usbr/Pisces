#!/bin/bash
export MONO1=/opt/mono/bin
. $MONO1/env.sh
cd /data/apps/pisces/src/Pisces

if [ -f ./.nuget/NuGet.exe ]; then
   echo "using existing NuGet.exe"
else
wget http://nuget.org/nuget.exe -O ./.nuget/NuGet.exe
chmod a+x ./.nuget/NuGet.exe
sudo $MONO1/mozroots --import --machine --sync
sudo $MONO1/certmgr -ssl -m https://go.microsoft.com
sudo $MONO1/certmgr -ssl -m https://nugetgallery.blob.core.windows.net
sudo $MONO1/certmgr -ssl -m https://nuget.org   
fi

export PATH=$PATH:$MONO1
X=xbuild
#echo "checking https://github.com/usbr/Pisces"
#git pull
export EnableNuGetPackageRestore=true
$X ./Core/Reclamation.Core.csproj
$X ./TimeSeries/Reclamation.TimeSeries.csproj
$X ./HydrometServer/HydrometServer.csproj
$X ./PiscesWebServices/PiscesWebServices.csproj

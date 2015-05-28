#!/bin/bash
cd /home/pisces/src/Pisces
X=xbuild
echo "checking https://github.com/usbr/Pisces"
git pull
export EnableNuGetPackageRestore=true
$X ./Core/Reclamation.Core.csproj
$X ./TimeSeries/Reclamation.TimeSeries.csproj
$X ./HydrometServer/HydrometServer.csproj
$S ./PiscesWebServices/PiscesWebServices.csproj

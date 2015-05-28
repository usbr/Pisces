#!/bin/bash
S=/home/pisces/src/Pisces/HydrometServer/bin/x86/Debug/
D=/home/pisces/bin/

cp -v ${S}Reclamation.Core.dll ${D}
cp -v ${S}Reclamation.TimeSeries.dll ${D}
cp -v ${S}HydrometServer.exe ${D}

S=/home/pisces/src/Pisces/PiscesWebServices/bin/Debug/
D=/home/pisces/web/

cp -v ${S}Reclamation.Core.dll ${D}
cp -v ${S}Reclamation.TimeSeries.dll ${D}
cp -v ${S}PiscesWebServices.exe ${D}

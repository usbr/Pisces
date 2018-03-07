#!/bin/bash
# linux install script
HYDROMET=./Applications/HydrometServer/bin/Debug
cp $HYDROMET/Reclamation.TimeSeries.dll $HYDROMET/Reclamation.Core.dll $HYDROMET/HydrometServer.exe /home/hydromet/bin/HydrometServer/
